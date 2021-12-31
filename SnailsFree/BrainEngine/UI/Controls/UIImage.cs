using System;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using System.IO;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIImage : UIControl
    {
        public delegate void LastFrameHandler();
        public event LastFrameHandler OnLastFrame;

        #region Vars
        private SpriteAnimation _animation;
        private ImageSizeMode _sizeMode;
        #endregion

        #region Properties
        public string ResourceName { get; set;}
        public bool Animate { get; set; }
        public Vector2 Offset { get; set; }
        public int FrameCount
        {
            get { return this._animation.FrameCount; }
        }

        public int CurrentFrame
        {
            get
            {
                return this._animation.CurrentFrame;
            }
            set
            {
                this._animation.CurrentFrame = value;
            }
        }

        public override Vector2 AbsolutePositionInPixels
        {
            get
            {
                Vector2 pos = base.AbsolutePositionInPixels;
                if (this._animation.Sprite != null)
                {
                    pos -= this._animation.Sprite.Offset;
                }
                return pos;
            }
        }

        /// <summary>
        /// The bounding box in pixels
        /// </summary>
        public override BoundingSquare BoundingBox
        {
            get
            {
                if (this._animation.Sprite == null)
                {
                    return new BoundingSquare(this.AbsolutePositionInPixels,
                                              this.SizeInPixels.Width, this.SizeInPixels.Height);
                }

                return new BoundingSquare(this.AbsolutePositionInPixels - this.ScreenUnitToPixels(this._animation.Sprite.Offset),
                                          this.SizeInPixels.Width, this.SizeInPixels.Height);
            }
        }

        public ImageSizeMode SizeMode
        {
            get
            {
                return this._sizeMode;
            }
            set
            {
                this._sizeMode = value;
                this.CalculateSize();
            }
        }

        public Sprite Sprite
        {
            set 
            { 
                if (this._animation == null)
                {
                    this._animation = new SpriteAnimation(value);
                }
                else
                {
                    this._animation.Sprite = value;
                }

                this.CalculateSize();
            }
            get { return (this._animation != null ? this._animation.Sprite : null); }
                
        }

        private Vector2 SpriteOffset
        {
            get
            {
                if (this._animation == null || this._animation.Sprite == null)
                {
                    return Vector2.Zero;
                }

                return (this._animation.Sprite.Offset * this.Scale);
            }
        }

        internal bool UseHotSpot
        {
            get;
            set;
        }


       
        #endregion

        public UIImage(UIScreen screenOwner) :
            this(screenOwner, (Sprite)null)
        {
            this.UseHotSpot = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public UIImage(UIScreen screenOwner, string resourceName) :
            this(screenOwner, resourceName, ResourceManager.RES_MANAGER_ID_STATIC)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public UIImage(UIScreen screenOwner, string resourceName, string resourceManagerId) :
            this(screenOwner)
        {
            if (resourceName == null)
            {
                this.Initialize(null);
            }
            else
            {
                this.Initialize(BrainGame.ResourceManager.GetSprite(resourceName, resourceManagerId));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UIImage(UIScreen screenOwner, Sprite sprite):
            base(screenOwner)
        {
            this.Initialize(sprite);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize(Sprite sprite)
        {
            this.SizeMode = ImageSizeMode.Autosize;
            this.Sprite = sprite;
            this.AcceptControllerInput = false;
            this._animation.OnLastFrame += new SpriteAnimation.LastFrameHandler(_animation_OnLastFrame);
            this.Animate = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void _animation_OnLastFrame()
        {
            if (this.OnLastFrame != null)
            {
                this.OnLastFrame();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal override void ParentChanged()
        {
            base.ParentChanged();
            this.CalculateSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnResize()
        {
            this.CalculateSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            if (this.ResourceName != null)
            {
                string assetName = BrainPath.GetDirectoryName(this.ResourceName);
                string spriteName = BrainPath.GetFileName(this.ResourceName);
                this.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(assetName, spriteName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this._animation != null && this._animation.Sprite != null && this.Animate)
            {
                this._animation.Update(gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DrawBackground()
        {
           
            if (this._animation != null && this._animation.Sprite != null)
            {
                // I know subtracting BB left/top is strange, but this is because I want a hotspot for the image
                // The hotspot is the sprite BB, if this is not subtracted, the BB position will not be the same has the 
                // control position
                Vector2 offset = Vector2.Zero;
                if (this.UseHotSpot)
                {
                    offset = new Vector2((this._animation.Sprite.BoundingBox.Left + this._animation.Sprite.OffsetX) * this.Scale.X,
                                             (this._animation.Sprite.BoundingBox.Top + this._animation.Sprite.OffsetY) * this.Scale.Y);
                }

                if (this.DropShadow)
                {
                    this.Draw(base.AbsolutePositionInPixels + this.ShadowDistance - offset - this.Offset, this.SpriteOffset, this.ShadowColor);
                }
                this.Draw(base.AbsolutePositionInPixels - offset - this.Offset, this.SpriteOffset, this.BlendColor);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void Draw(Vector2 position, Vector2 offset, Color color)
        {
            if (this.SizeMode != ImageSizeMode.Stretch)
            {
              
                if (offset == Vector2.Zero)
                {
                    this._animation.Draw(position, this.Rotation, color, this.Scale, Vector2.Zero, this.SpriteBatch);
                }
                else
                {
                    this._animation.Draw(position, this.Rotation, color, this.Scale, offset , this.SpriteBatch);
                }
            }
            else // Rotation not supported, implement if needed
            {
              
                this._animation.Draw(this.ClientRectInPixels, color, this.SpriteBatch);
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        private void CalculateSize()
        {
            switch (this.SizeMode)
            {
                case ImageSizeMode.None:
                    // Nothing to do here
                    break;
                case ImageSizeMode.Autosize:
                    // The control is resized to fit the image size
                    if (this.Sprite != null)
                    {

                        if (this.UseHotSpot)
                        {
                            this.Size = this.PixelsToScreenUnits(new Size(this.Sprite.BoundingBoxes[0].Width,
                                                                          this.Sprite.BoundingBoxes[0].Height));
                        }
                        else
                        {
                            this.Size = this.PixelsToScreenUnits(new Size(this.Sprite.Frames[0].Width,
                                                                          this.Sprite.Frames[0].Height));
                        }
                    }
                    break;
                case ImageSizeMode.Center:
                    // Implement if needed. Image should be centered in the control
                    break;
                case ImageSizeMode.Stretch:
                    if (this.Parent != null)
                    {
                        this.Size = this.Parent.Size;
                    }
                    break;

            }
        }
    }
}
