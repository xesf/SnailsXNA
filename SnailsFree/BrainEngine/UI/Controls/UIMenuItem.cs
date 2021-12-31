using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIMenuItem : UIControl
    {
        #region Events
        public UIEvent OnPress; // Occurs when the button is pressed
                                // The button is pressed when 1 of this events happen:
                                // -The OnAction event happens (the user presses the Action button when the cursor is over the button
                                // -The button OnControllerAction is raised
        #endregion

        #region Vars
        private Sprite _sprite;
        private int _hotSpotBBIndex;
        #endregion

        #region Properties
        public int Identifier { get; set; }
        protected UIImage BackgroundImage { get; set; }
        public UIMenu MenuOwner
        {
            get { return (UIMenu)this.Parent; }
        }
        public UILabel Label { get; private set; }
        public MenuItemStyle Style { get; set; }
        public Sprite Image
        {
            get { return this._sprite; }
            set
            {
                this._sprite = value;
                if (this.BackgroundImage == null && this._sprite != null)
                {
                    this.BackgroundImage = new UIImage(this.ScreenOwner, this._sprite);
                    this.BackgroundImage.ParentAlignment = AlignModes.Horizontaly | AlignModes.HorizontalyVertically;
                    this.Controls.InsertAt(0, this.BackgroundImage);
                    this.Resize();
                }
                else
                {
                    this.BackgroundImage.Sprite = null;
                }
            }
        }

        public new Color BlendColor
        {
            get
            {
                return base.BlendColor;
            }
            set
            {
                base.BlendColor = value;
                if (this.Label != null)
                {
                    this.Label.BlendColor = value;
                }
            }
        }

        public Vector2 TextScale
        {
            get
            {
                if (this.Label == null)
                {
                    return new Vector2(1.0f, 1.0f);
                }
                return this.Label.Scale;
            }
            set
            {
                if (this.Label != null)
                {
                    this.Label.Scale = value;
                    this.Resize();
                }
            }
        }

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                this.MenuOwner.RepositionItems();
            }
        }


        public override string TextResourceId
        {
            get { return this.Label.TextResourceId; }
            set { this.Label.TextResourceId = value; }
        }

        
        public override string Text
        {
            get
            {
                return this.Label.Text;
            }
            set
            {
                this.Label.Text = value;
            }
        }

        public int HotSpotBBIndex
        {
            get
            {
                return this._hotSpotBBIndex;
            }
            set
            {
                this._hotSpotBBIndex = value;
                this.Resize();
            }
        }


        /// <summary>
        /// The bounding box in pixels
        /// </summary>
        public override BoundingSquare BoundingBox
        {
            get
            {
                if (this.HotSpotBBIndex == -1 || this.BackgroundImage == null)
                {
                    return new BoundingSquare(this.AbsolutePositionInPixels,
                                          this.SizeInPixelsScaled.Width, this.SizeInPixelsScaled.Height);
                }

                Vector2 pos = this.AbsolutePositionInPixels + new Vector2(this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Left, this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Top);
                return new BoundingSquare(pos, this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Width * this.Scale.X,
                                                 this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Height * this.Scale.Y);

            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UIMenuItem(UIScreen screenOwner, string textResourceId, SpriteFont spriteFont) :
            base(screenOwner)
        {
            this.Initialize(textResourceId, (UILabel)new UISpriteFontLabel(screenOwner, spriteFont));
        }

        /// <summary>
        /// 
        /// </summary>
        public UIMenuItem(UIScreen screenOwner, string textResourceId, TextFont textFont) :
            base(screenOwner)
        {
            this.Initialize(textResourceId, (UILabel)new UITextFontLabel(screenOwner, textFont));
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize(string textResourceId, UILabel label)
        {
            this.HotSpotBBIndex = -1;
            this.Size = new Size(0.0f, 0.0f); // This should be reviewed. Size will be set in this.Resize()
                                              // but a text null or "" will make the control 0, 0 in size
                                              // There should be a flag "Autosize"
            this.Label = label;
            this.Label.ParentAlignment = AlignModes.HorizontalyVertically;
            this.Label.TextResourceId = textResourceId;
            this.Controls.Add(this.Label);
            this.Style = MenuItemStyle.Text;
            this.Resize();
        }

        /// <summary>
        /// Adjusts the control size
        /// Width and height will be set to the biggest values between the label and the image
        /// </summary>
        protected virtual void Resize()
        {
            float width = 0;
            float height = 0;
            if (this.Label != null)
            {
                if (this.Label.Size.Width > width)
                {
                    width = this.Label.Size.Width;
                }

                if (this.Label.Size.Height > height)
                {
                    height = this.Label.Size.Height;
                }
            }

            if (this.BackgroundImage != null)
            {
                if (this.BackgroundImage.Size.Width > width)
                {
                    width = this.BackgroundImage.Size.Width;
                }

                if (this.BackgroundImage.Size.Height > height)
                {
                    height = this.BackgroundImage.Size.Height;
                }
            }

            this.Size = new Size(width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this.DropShadow)
            {
                if (this.BackgroundImage != null)
                {
                    this.BackgroundImage.DropShadow = true;
                    this.Label.DropShadow = false;
                }
                else
                {
                    this.Label.DropShadow = true;
                }
            }
            else
            {
                if (this.BackgroundImage != null)
                {
                    this.BackgroundImage.DropShadow = false;
                }
                this.Label.DropShadow = false;
            }
        }
    }
}
