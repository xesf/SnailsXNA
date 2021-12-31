using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Liquid : StageObject
    {
        #region Constants
        const int MARGINS = 10;
        public const float LIQUID_LEVEL_FULL = 1f;
        public const float LIQUID_LEVEL_EMPTY = 0f;
        #endregion

        #region Members
        protected Vector2 _size;
        protected int _signal = 1;
        protected double _elapsedTime;
        protected bool _isWaving;
        protected BoundingSquare _liquidAABB;
        protected Vector2 _sizeInPixels;
        protected float _liquidLevel; // Liquid level 1.0 (full) to 0.0 ( empty)
        protected Rectangle _drawRectangle;
        private Color _liquidColor;
        private Color [] _colorsByTheme;
        private string _liquidColorString; // This string holds liquid color for each team (delimited by ;)

        // Waves support
        private string _waveSpriteResId;
        private SpriteAnimation _waveAnimation;
        private Sprite _waveSprite;
        private Vector2 _wavesPosition;
        private Color _wavesBlendColor;

        // Splash effect support
   	    private List<StageProp> _liquidSplashes;
        private bool _withSplashes;
        private string _splashPropId; // Id for the prop for the splash sprite
        private bool _killObjectsOnTouch;
        protected bool _allowCratesOnTop;
        #endregion

        #region Properties
        private bool WithWaves { get { return this._waveAnimation != null;  } }
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; Resize(); }
        }

        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this._liquidAABB;
            }
        }

        public bool IsEmpty { get { return this._liquidLevel == LIQUID_LEVEL_EMPTY; } }
        public bool IsFull { get { return this._liquidLevel == LIQUID_LEVEL_FULL; } }

        public float LiquidLevel
        {
            get { return this._liquidLevel; }
            set { this._liquidLevel = value; }
        }

        #endregion

        public Liquid(StageObjectType type)
            : base(type)
        {
            _size = Vector2.One;
            _liquidLevel = LIQUID_LEVEL_FULL;
            this._wavesBlendColor = Color.White;
            this._liquidSplashes = new List<StageProp>();
        }

        public Liquid(Liquid other)
            : base(other)
        {
            Copy(other);
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
            Liquid otherL = other as Liquid;
            this._waveSpriteResId = otherL._waveSpriteResId;
            this._withSplashes = otherL._withSplashes;
            this._splashPropId = otherL._splashPropId;
            this._size = otherL._size;
            this.BlendColor = otherL.BlendColor;
            this._wavesBlendColor = otherL._wavesBlendColor;
            this._liquidColorString = otherL._liquidColorString;
            this._killObjectsOnTouch = otherL._killObjectsOnTouch;
            this._allowCratesOnTop = otherL._allowCratesOnTop;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._colorsByTheme = new Color[(int)ThemeType.ThemeD + 1];
            if (this._liquidColorString != null)
            {
                string[] colorsByThemeStr = this._liquidColorString.Split(';');
                for (int i = 0; i < this._colorsByTheme.Length; i++)
                {
                    if (i < colorsByThemeStr.Length)
                    {
                        this._colorsByTheme[i] = Parsers.ParseColor(colorsByThemeStr[i]);
                    }
                }
            }
            this._liquidColor = this._colorsByTheme[(int)Stage.CurrentStage.LevelStage.ThemeId];

            // Create list for liquid splashes
            if (this._withSplashes)
            {
               this._liquidSplashes = new List<StageProp>();
            }
            Resize();
            Stage.CurrentStage.OnBeforeObjectsDraw += new Stage.StageDrawEventHandler(CurrentStage_OnBeforeObjectsDraw);
        }


        public override void LoadContent()
        {
            base.LoadContent();
            if (!string.IsNullOrEmpty(this._waveSpriteResId))
            {
                _waveSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._waveSpriteResId);
                _waveAnimation = new SpriteAnimation(_waveSprite);
                _waveAnimation.BlendColor = this._wavesBlendColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Resize()
        {
            int offset = 0;
            if (this.WithWaves)
            {
                offset = (int)this._waveSprite.Frames[0].Height;
            }
            float liquidHeight = (this._liquidLevel * this._sizeInPixels.Y);
            this._liquidAABB = new BoundingSquare(new Vector2(this.X - MARGINS,
                                                              this.Y + offset + this._sizeInPixels.Y - liquidHeight),
                                                    this._sizeInPixels.X + MARGINS * 2,
                                                    liquidHeight - offset + MARGINS);
            this._drawRectangle = this._liquidAABB.ToRect();
            this._wavesPosition = new Vector2(this.Position.X, this._drawRectangle.Top - offset);

           // Adjust any splash effect position to fit liquid level
            if (this._withSplashes)
            {
              foreach(StageProp prop in this._liquidSplashes)
              {
                if (prop.IsVisible)
                {
                    prop.Position = new Vector2(prop.Position.X, this._liquidAABB.Top);
                }
              }
           }

            if (!this._allowCratesOnTop)
            {
                this._crateCollisionBB = this._liquidAABB;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void KillObject(StageObject stageObject)
        {
            stageObject.KillByTouchingDeadlyLiquid();
        }

        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            StageObject stageObj = (StageObject)obj;
            if (stageObj.IgnoreLiquidCollisions)
            {
                return;
            }

            if (stageObj.CheckCollisionWithLiquid(this) == false)
            {
                return;
            }

            if (!stageObj.IsUnderLiquid)
            {
                if (!this._killObjectsOnTouch)
                {
                    stageObj.OnEnterLiquid(this);
                    if (stageObj.IsFalling && this._withSplashes)
                    {
                        this.AddSplashEffect(stageObj.Position);
                    }
                }
                else
                {
                    if (stageObj.CanDie)
                    {
                        this.KillObject(stageObj);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddSplashEffect(Vector2 position)
        {
           StageProp splashProp = null;
           // First step, look for an available StageProp
           foreach(StageProp prop in this._liquidSplashes)
           {
             if (prop.IsVisible == false)
             {
                splashProp = prop;
                break;
             }
           }

           if (splashProp == null)
           {
              splashProp = StageProp.Create(this._splashPropId);
              Stage.CurrentStage.AddObjectInRuntime(splashProp);
           }
           splashProp.Position = new Vector2(position.X, this._liquidAABB.Top);
           splashProp.Show();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this.IsEmpty)
            {
                return;
            }

            if (this.WithWaves)
            {
                _waveAnimation.Update(gameTime);
            }
            // check collisions with Snails and other objects
            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            this.DoQuadtreeCollisions(Stage.QUADTREE_STAGEOBJ_LIST_IDX);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Draw(bool shadow, Color blendColor, SpriteBatch spriteBatch)
        {

            if (this.WithWaves && !this.IsEmpty)
            {
                for (int i = 0; i < this.Size.X; i++)
                {
                    Vector2 pos = this._wavesPosition;
                    pos.X += i * Stage.CurrentStage.Board.TileWidth;
                    _waveAnimation.BlendColor = blendColor;
                    _waveAnimation.Draw(pos, Stage.CurrentStage.SpriteBatch);
                }
            }

            SnailsGame.DrawRectangleFilled(spriteBatch, _drawRectangle, blendColor);
#if DEBUG
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._crateCollisionBB.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        void CurrentStage_OnBeforeObjectsDraw(bool shadow, SpriteBatch spriteBatch)
        {
            if (this._liquidColor.A != 0)
            {
         //       this.Draw(shadow, this._liquidColor, spriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.Draw(shadow, BlendColor, Stage.CurrentStage.SpriteBatch);
            
#if DEBUG
            if (BrainGame.Settings.ShowBoundingBoxes)
            {
                this.QuadtreeCollisionBB.Draw(Color.Red, SnailsGame.Instance.ActiveCamera.Position);
            }
#endif

        }

        /// <summary>
        /// Pumps liquid into the liquid pool
        /// </summary>
        public void PumpLiquid(float quantity)
        {
            this._liquidLevel += quantity;
            if (this._liquidLevel > LIQUID_LEVEL_FULL)
            {
                this._liquidLevel = LIQUID_LEVEL_FULL;
            }
            if (this._liquidLevel < LIQUID_LEVEL_EMPTY)
            {
                this._liquidLevel = LIQUID_LEVEL_EMPTY;
            }

            this.Resize();
        }

        /// <summary>
        /// Returns the depth in pixels of the point
        /// </summary>
        public float GetDepth(Vector2 point)
        {
            return point.Y - this._liquidAABB.Top;
        }


        /// <summary>
        /// 
        /// </summary>
        public override bool CrateToolIsValid(BoundingSquare crateBs)
        {
            if (!this._allowCratesOnTop)
            {
                return !(crateBs.Collides(this._crateCollisionBB));
            }
            return true;
        }

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);

            int sx = record.GetFieldValue<int>("sizeX", (int)this._size.X);
            int sy = record.GetFieldValue<int>("sizeY", (int)this._size.Y);
            this._waveSpriteResId = record.GetFieldValue<string>("waveSpriteResId", this._waveSpriteResId);
            this._liquidLevel = record.GetFieldValue<float>("liquidLevel", this._liquidLevel);
            this._size = new Vector2(sx, sy);
            this._sizeInPixels = new Vector2(sx * Stage.TILE_WIDTH, sy * Stage.TILE_HEIGHT);
            this._splashPropId = record.GetFieldValue<string>("splashPropId", this._splashPropId);
	        this._withSplashes = !string.IsNullOrEmpty(this._splashPropId);
            this._wavesBlendColor = record.GetFieldValue<Color>("wavesBlendColor", this._wavesBlendColor);
            this._killObjectsOnTouch = record.GetFieldValue<bool>("killObjectsOnTouch", this._killObjectsOnTouch);
            this._liquidColorString = record.GetFieldValue<string>("liquidColor", this._liquidColorString);
            this._allowCratesOnTop = record.GetFieldValue<bool>("allowCrates", this._allowCratesOnTop);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            switch (context)
            {
                case ToDataFileRecordContext.StageDataSave:
                    record.AddField("waveSpriteResId", this._waveSpriteResId);
		            record.AddField("splashPropId", this._splashPropId);
                    record.AddField("wavesBlendColor", this._wavesBlendColor);
                    record.AddField("liquidColor", this._liquidColorString);
                    record.AddField("killObjectsOnTouch", this._killObjectsOnTouch);
                    record.AddField("allowCrates", this._allowCratesOnTop);
                    break;
                case ToDataFileRecordContext.StageSave:
                    record.RemoveField("color");
                    break;
            }

            record.AddField("sizeX", (int)this._size.X);
            record.AddField("sizeY", (int)this._size.Y);
            record.AddField("liquidLevel", this._liquidLevel);
            return record;
        }
        #endregion
    }
}
