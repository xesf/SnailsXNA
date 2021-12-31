using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.Snails.Player;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class PickableObject : MovingObject, ISnailsDataFileSerializable 
	{
        const double  QUANTITY_ANIMATION_TIME = 800.0f;
        
        public enum PickableType
        {
            Proxy,
            GoldCoin,
            SilverCoin,
            CopperCoin,
            Dynamite,
            Vitamin,
            Apple,
            Box,
            Copper,
            DynamiteBoxTriggered,
            DynamiteBox,
            Trampoline,
            Salt
        }

        public PickableType _pickableType;
        bool _PickedUp;
        Sprite _toolSprite;
        Sprite _PickedSprite;
        Sprite _BubbleSprite;
        SpriteAnimation _BubbleAnimation;

        // pre calculate this flags for speed
        bool _IsTool; 
        bool _IsCoin;
        bool _animateQuantity;

        TextFont _font;
        Vector2 _fontPosition;
        string _quantityString;
        double _quantityAnimationTime;

        Sample _toolFoundSample;
        Sample _coinFoundSample;

        public int Quantity { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public PickableObject()
            : base(StageObjectType.PickableObject)
        {
            this.Quantity = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            PickableObject pickObj = other as PickableObject;
            this._pickableType = pickObj._pickableType;
            this.Quantity = pickObj.Quantity;
            this._BubbleSprite = pickObj._BubbleSprite;
            this._IsCoin = pickObj._IsCoin;
            this._IsTool = pickObj._IsTool;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._IsTool = this.QueryIsTool();
            this._IsCoin = this.QueryIsCoin();
            this._animateQuantity = this._IsTool;
            this.InitSpriteSet();
            if (this._IsTool)
            {
                this._BubbleSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Bubble");
                this._BubbleAnimation = new SpriteAnimation(this._BubbleSprite);
            }
            this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-small", ResourceManager.ResourceManagerCacheType.Static);
            this._quantityString = this.Quantity.ToString();
            // This centers the quantity text in the sprite BB
            this._fontPosition = new Vector2(this.AABoundingBox.Left + (this.AABoundingBox.Width / 2) - (this._font.MeasureString(this._quantityString, new Vector2(1.0f, 1.0f)) / 2),
                                             this.AABoundingBox.Top + (this.AABoundingBox.Height / 2) - (this._font.MeasureStringHeight(this._quantityString, new Vector2(1.0f, 1.0f))));

            this._toolFoundSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TOOL_FOUND, this);
            this._coinFoundSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.COIN_FOUND, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.CurrentFrame = BrainGame.Rand.Next(this.Sprite.FrameCount);
        }


        /// <summary>
        /// Needed is by the editor
        /// </summary>
        public bool QueryIsTool()
        {
            return IsTool(this._pickableType);
        }

        /// <summary>
        /// used in stage to know if the object is a tool
        /// </summary>
        public static bool IsTool(PickableType type)
        {
            switch (type)
            {
                case PickableType.Apple:
                case PickableType.Dynamite:
                case PickableType.Vitamin:
                case PickableType.Copper:
                case PickableType.Box:
                case PickableType.Trampoline:
                case PickableType.DynamiteBox:
                case PickableType.DynamiteBoxTriggered:
                case PickableType.Salt:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool QueryIsCoin()
        {
            switch (this._pickableType)
            {
                case PickableType.GoldCoin:
                case PickableType.SilverCoin:
                case PickableType.CopperCoin:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            base.OnLastFrame();

            // Dispose object if picked-up animation ended and only if quantity is 1
            // This is because objects with quantities > 0 animate the quantity number.
            // This objects are only disposed when the quantity animation ends
            if (this._PickedUp)
            {
                if (!this._animateQuantity)
                {
                    Stage.CurrentStage.DisposeObject(this);
                }
                // Hide the picked up sprite or else if quantity is animating animation will loop
                this.DynamicFlags &= ~StageObjectDynamicFlags.IsVisible;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this.IsDead || this.IsDisposed)
            {
                return;
            }

            if (this._PickedUp == false)
            {
                if (this._BubbleAnimation != null)
                {
                    this._BubbleAnimation.Update(gameTime);
                }

                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }
            else
            {
                // Animate quantity number if thats the case
                if (this._animateQuantity && this.Quantity > 1)
                {
                    this._fontPosition = new Vector2(this._fontPosition.X, this._fontPosition.Y - (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.05f));
                    this._quantityAnimationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (this._quantityAnimationTime > QUANTITY_ANIMATION_TIME)
                    {
                        this.DisposeFromStage();
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            // Deal with double collisions
            if (this._PickedUp)
            {
                return;
            }

            if (listIdx == Stage.QUADTREE_SNAIL_LIST_IDX)
            {
                Snail snail = (Snail)obj;
                if (!snail.CanPickupObject)
                {
                    return;
                }
                if (this._IsTool)
                {
                    ToolObjectType toolType = (ToolObjectType)Enum.Parse(typeof(ToolObjectType), this._pickableType.ToString(), true);
                    if(Stage.CurrentStage.StageHUD._toolsMenu.AllowAddToolQuantity(toolType, this.Quantity))  // add quantity to tool (if tool doesn't exist create a new one)
                    {
                        if (_toolFoundSample != null && !_toolFoundSample.IsPlaying)
                        {
                            _toolFoundSample.Play();
                            this.FadeOut(1f);
                        }
                        this._PickedUp = true;
                    }
                }
                else
                if (this._IsCoin)
                {
                    if (_coinFoundSample != null && !_coinFoundSample.IsPlaying)
                    {
                        _coinFoundSample.Play();
                    }
                    //    SnailsGame.Tutorial.ShowTopic(TutorialTags.ABOUT_COINS);
                    switch (this._pickableType)
                    {
                        case PickableType.CopperCoin:
                            Stage.CurrentStage.Stats.NumBronzeCoins++;
                            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins++;
                            break;
                        case PickableType.SilverCoin:
                            Stage.CurrentStage.Stats.NumSilverCoins++;
                            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins++;
                            break;
                        case PickableType.GoldCoin:
                            Stage.CurrentStage.Stats.NumGoldCoins++;
                            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins++;
                            break;
                    }
                    this._PickedUp = true;
                }

                if (this._PickedUp)
                {
                    this.Sprite = this._PickedSprite;
                    this.CurrentFrame = 0;
                   // this.KillMe();
                }
            }
            else
            {
                // if colide with a tile, than make it fall
                if (obj is TileObject) 
                {
                    this._IsTool = false; // take bubble off (should be PickedUp flag bug it has other things attached on OnLastFrame event)
                    this.StaticFlags &= ~StageObjectStaticFlags.CanCollide;
                    this.StaticFlags |= StageObjectStaticFlags.CanFall;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);

            if (this._IsTool && !this._PickedUp)
            {
                this._BubbleAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
            }

            if (this.Quantity > 1)
            {
                this._font.DrawString(Stage.CurrentStage.SpriteBatch, this._quantityString, this._fontPosition, new Vector2(1f, 1f), this.BlendColor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitSpriteSet()
        {
            this.ResourceId = "spriteset/pickable-objects";
            this.SpriteId = this._pickableType.ToString();

            this._toolSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, this.SpriteId);
            this.Sprite = this._toolSprite;
            // Some hammering here. No need to complicate. Keep this simple
            this._PickedSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "BubblePickUp");
            if (this._IsCoin)
            {
                this._PickedSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "CoinPickUp");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetPickableType(PickableType pickType)
        {
            this._pickableType = pickType;
            this.InitSpriteSet();
        }


        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            string pickType = record.GetFieldValue<string>("pickableType", null);
            if (!string.IsNullOrEmpty(pickType))
            {
                this._pickableType = (PickableType)Enum.Parse(typeof(PickableType), pickType, true);
            }
            this.Quantity = record.GetFieldValue<int>("quantity", 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("pickableType", (int)this._pickableType);
            record.AddField("quantity", this.Quantity);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }
        #endregion
	}
}
