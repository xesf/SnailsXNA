using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UICreditsPanel : UIPanel
    {
        private static Color HeadingsColor =  new Color(255, 170, 20);
        private static Color NamesColor = new Color(255, 255, 50);

        const float HEADING_LINE_SPACING = 400.0f;
        const float LINE_SPACING = 400.0f;
        const float CATEGORY_SPACING = 400.0f;

        float _posY;


        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                foreach (UIControl ctl in this.Controls)
                {
                    ctl.Visible = value;
                }
            }
        }

        public UICreditsPanel(UIScreen screenOwner) :
            base(screenOwner)
        {
            this._posY = 0.0f;
            this.Size = new Size(0.0f, 0.0f); // Size is set when labels are added
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddCategory(string textResourceId)
        {
            if (this.Controls.Count > 0)
            {
                this._posY += CATEGORY_SPACING;
            }

            UICaption caption = new UICaption(this.ScreenOwner, "", UICreditsPanel.HeadingsColor, UICaption.CaptionStyle.CreditsCategory);
            caption.TextResourceId = textResourceId;
            caption.Position = new Vector2(0.0f, this._posY);
            this.Controls.Add(caption);
            this._posY += HEADING_LINE_SPACING;

            if (caption.Size.Width > this.Size.Width)
            {
                this.Size = new Size(caption.Size.Width, this.Size.Height);
            }

            if (caption.Position.Y + caption.Size.Height > this.Size.Height)
            {
                this.Size = new Size(this.Size.Width, caption.Position.Y + caption.Size.Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddName(string text)
        {
            UICaption caption = new UICaption(this.ScreenOwner, text, UICreditsPanel.NamesColor, UICaption.CaptionStyle.CreditsName);
            caption.Position = new Vector2(0.0f, this._posY);
            this.Controls.Add(caption);
            this._posY += LINE_SPACING;

            if (caption.Size.Width > this.Size.Width)
            {
                this.Size = new Size(caption.Size.Width, this.Size.Height);
            }

            if (caption.Position.Y + caption.Size.Height > this.Size.Height)
            {
                this.Size = new Size(this.Size.Width, caption.Position.Y + caption.Size.Height);
            }
        }
    }
}
