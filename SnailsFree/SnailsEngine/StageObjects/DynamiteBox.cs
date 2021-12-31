using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.SpacePartitioning;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class DynamiteBox : Box, ISnailsDataFileSerializable
    {
        protected enum DynamiteBoxStatus
        {
            BoxDrop,
            Counting,
            Explode,
            Deployed,
            ExplosionDelay,
            Idle
        }

        #region Constants
        new public const string ID = "DYNAMITE_BOX";
        public const int EXPLOSION_TIME = 9000;
        public const string SPRITE_BOX = "CommonTiles";
        private string RES_DYNAMITE_BOX = "spriteset/stage-objects";
        private string SPRITE_BOX_TIMER_FONT = "BoxTimeCounterFont";
        #endregion

        #region Members
        protected Sprite _spriteCounter;
        protected DynamiteBoxStatus _status;
        protected double _elapsedTimeToExplode;
        protected bool _isActive;
        protected bool _counterVisible;
        protected Sample _tickSample;

        // I want the drawing of the counter string to be as fast as possible
        // Only 2 digit counters are suported
        private string _counterString; // String that holds the counter. It's only update when a second ellapses
        protected int _currentSecond; // Current second of the counter
        private int _char1; // 1st digit of the counter
        private int _char2; // 2nd digit of the counter
        private Vector2 _counterPosition; // The number will be centered on the box. The position is only updated when a second ellapses
        private Vector2 _counterChar2Position;
        
        private string _implosionSpriteRes;
        private string _implosionEndSpriteRes;

        private Sprite _implosionSprite;
        private Sprite _implosionEndSprite;

        private  Explosion _explosion;
        #endregion

        public new Sprite Sprite
        {
            get { return base.Sprite; }
            set
            {
                base.Sprite = value;
            }
        }
        public DynamiteBox()
            : this(StageObjectType.DynamiteBox)
        {
            
        }

        public DynamiteBox(StageObjectType type)
            : base(type)
        {
            _status = DynamiteBoxStatus.BoxDrop;
            _elapsedTimeToExplode = 0;
            _isActive = true;
            _counterVisible = true;
            _currentSecond = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);

            DynamiteBox obj = other as DynamiteBox;
            this._currentSecond = obj._currentSecond;
            this._spriteCounter = obj._spriteCounter;
            this._counterVisible = obj._counterVisible;
            this._tickSample = obj._tickSample;
            this._implosionSpriteRes = obj._implosionSpriteRes;
            this._implosionEndSpriteRes = obj._implosionEndSpriteRes;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // load sprite to use by the counter
            this._spriteCounter = BrainGame.ResourceManager.GetSpriteTemporary(RES_DYNAMITE_BOX, SPRITE_BOX_TIMER_FONT);
			this._tickSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.CRATE_TIMER_TICK);
            if (!string.IsNullOrEmpty(this._implosionSpriteRes))
            {
                this._implosionSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._implosionSpriteRes);
            }
            if (!string.IsNullOrEmpty(this._implosionEndSpriteRes))
            {
                this._implosionEndSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._implosionEndSpriteRes);
            }

        }

        /// <summary>
        ///
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.PreComputeCounterData((int)(EXPLOSION_TIME / 1000));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void HintInitialize()
        {
            base.HintInitialize();
            this._counterVisible = false;
        }

        /// <summary>
        ///
        /// </summary>
        public override void StageInitialize()
        {
            base.StageInitialize();
            this.BoxDeployed(Stage.LoadingContext == Stage.StageLoadingContext.Gameplay, false, false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            if (_status == DynamiteBoxStatus.BoxDrop)
            {
                this.BoxDeployed(true, true, true);
                this.SwitchObjects();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
        {
            base.BoxDeployed(addTile, addPaths, checkCollisions);
            this._status = DynamiteBoxStatus.Counting;
            this.SetSpriteWhenIdle();
            this.PreComputeCounterData((int)(EXPLOSION_TIME / 1000));
            this._deployStatus = BoxDeployStatus.Deployed;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SnailCollided(Snail snail)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            switch (_status)
            {
                case DynamiteBoxStatus.Counting:
                    _elapsedTimeToExplode += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (_elapsedTimeToExplode > EXPLOSION_TIME)
                    {
                        _elapsedTimeToExplode = 0;
                        this.Explode();
                        //_status = DynamiteBoxStatus.Explode;
                    }
                    else
                    {
                        // Round ellapsed seconds up or else we will loose 1 second
                        int seconds = (int)(Math.Ceiling((double)((EXPLOSION_TIME - _elapsedTimeToExplode) / 1000f)));
                        // Update the counter string if the second changed
                        if (seconds != this._currentSecond)
                        {
                            _tickSample.Play();
                            this._currentSecond = seconds;
                            this.PreComputeCounterData(seconds);
                        }
                    }
                    break;

                case DynamiteBoxStatus.Explode:
                    this.Explode();
                    break;


            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);

            if (this._counterVisible)
            {
                // We have all the counter string crap pre-computed, so all we need is to draw
                // But the position depends if we have a 1 digit or 2 digit number
                if (this._counterString.Length == 1)
                {
                    this._spriteCounter.Draw(this._counterPosition, this._char1,Stage.CurrentStage.SpriteBatch);
                }
                else
                {
                    this._spriteCounter.Draw(this._counterPosition, this._char1, Stage.CurrentStage.SpriteBatch);
                    this._spriteCounter.Draw(this._counterChar2Position, this._char2, Stage.CurrentStage.SpriteBatch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            // If box deployng, call the collide on the base, the collide on the base kills the snail when
            // the box is deployed on top of a snail. After deploying, a collision with this box will trigger the timer
            if (this._deployStatus == Box.BoxDeployStatus.Deploying)
            {
                base.OnCollide(obj, listIdx);
                return;
            }

            if (this._status == DynamiteBoxStatus.Explode)
            {
                Snail snail = (Snail)obj;
                snail.KillByExplosion(this._explosion);
                return;
            }
            this.SnailCollided((Snail)obj);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void KillByExplosion(Explosion exp)
        {
            this.Explode();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Explode()
        {
            this._status = DynamiteBoxStatus.Explode;
            TileCellCoords coords = Stage.CurrentStage.Board.GetCoordsFromPosition(this.Position);
            TileCell cell = Stage.CurrentStage.Board.Tiles[coords.RowIndex, coords.ColIndex];
            if (cell != null)
            {
                if (!this.IsUnderLiquid)
                {
                    Stage.CurrentStage.Board.Tiles[coords.RowIndex, coords.ColIndex].Break();
                }
                Stage.CurrentStage.Board.RemoveTileAt(coords.ColIndex, coords.RowIndex);
            }

            // check the level of liquid the object is under to 
            // determinate if we should explode or implode
            bool canImplode = false;
            if (this.IsUnderLiquid)
            {
                float wY = this._inLiquidRef.QuadtreeCollisionBB.Top;
                float Y = this.Y;
                if (wY <= Y ||
                   (wY <= Y + (this.AABoundingBox.Height / 2))) // check liquid level
                { 
                    canImplode = true;
                }
            }

            // Havia um bug com a explosão
            // Os caracóis que estivessem a andar por baixo podiam não morrer por causa da gravidade (ficavam fora do alcance da explosão)
            // Para resolver, cria-se um BB igual à da explosão no caixote 
            // e testa-se colisões com os snails para os matar. Desta forma garantimos que todos os que estiverem a andar no caixote vão morrer
            this._explosion = this.Explode(Explosion.ExplosionSize.Small,
                        Explosion.ExplosionSize.Medium,
                        Explosion.ExplosionRadiusType.Square,
                        Explosion.ObjectTypeAffected.Snails,
                        new Vector2(this.Position.X + this.Sprite.Width / 2, this.Position.Y + this.Sprite.Height / 2),
                        false,
                        (canImplode ? this._implosionSprite : null),
                        (canImplode ? this._implosionEndSprite : null), false, this.IsUnderLiquid);

            this.AABoundingBox = this._explosion.GetObjCollisionBB();
            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            Stage.CurrentStage.DisposeObject(this);
        }

        /// <summary>
        /// Pre-computes all stuf needed to draw the counter
        /// </summary>
        public void PreComputeCounterData(int seconds)
        {
            this._counterString = seconds.ToString();
            float counterLength = 0;
            if (this._counterString.Length == 1)
            {
                this._char1 = (int) (this._counterString[0] - '0');
                this._char2 = -1;
                counterLength = this._spriteCounter.Frames[this._char1].Width;
            }
            else
            {
                this._char1 = (int)(this._counterString[0] - '0');
                this._char2 = (int)(this._counterString[1] - '0');
                counterLength = this._spriteCounter.Frames[this._char1].Width +
                                this._spriteCounter.Frames[this._char2].Width;
            }
            this._counterPosition = this.Position + new Vector2((this.Sprite.Width / 2) - (counterLength / 2),
                                                                (this.Sprite.Height / 2) - (this._spriteCounter.Height / 2));
            this._counterChar2Position =  this._counterPosition + new Vector2(this._spriteCounter.Frames[this._char1].Width, 0.0f);

        }



        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._implosionSpriteRes = record.GetFieldValue<string>("implosionSpriteRes", this._implosionSpriteRes);
            this._implosionEndSpriteRes = record.GetFieldValue<string>("implosionEndSpriteRes", this._implosionEndSpriteRes);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("implosionSpriteRes", this._implosionSpriteRes);
            record.AddField("implosionEndSpriteRes", this._implosionEndSpriteRes);
            return record;
        }
        #endregion

    }
}
