using System;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsMenuTitle : UIControl
    {
        #region Consts
        private static Color TITLE_COLOR = new Color(255, 240, 26);
        private static Vector2 TITLE_FONT_SIZE = new Vector2(1.0f, 1.0f);
        #endregion

        public enum TitleSize
        {
            Medium,
            Big
        }

        #region Vars
        UITextFontLabel _lblTitle;
        UITextFontLabel _lblSubTitle;
        UIImage _imgTitleBack;
        TextFont _menuTitleFont;
        TextFont _menuSubTitleFont;
        TitleSize _boardSize;
        bool _showSubtitle;
        #endregion

        public bool ShowSubtitle
        {
            get
            {
                return this._showSubtitle;
            }
            set
            {
                this._showSubtitle = value;
                if (this._showSubtitle)
                {
                    this._lblSubTitle.Visible = true;
                    this._lblTitle.ParentAlignment = AlignModes.Top | AlignModes.Horizontaly;
                    this._lblTitle.Margins.Top = 200f;
                }
                else
                {
                    this._lblSubTitle.Visible = false;
                    this._lblTitle.ParentAlignment = AlignModes.HorizontalyVertically;
                    this._lblTitle.Margins.Bottom = 0f;
                }
            }
        }
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            // The login in set value is like this
            // -If we are setting the object to visible, make self visible and all childs visible
            // -If we are hidding the object, don't call the base visible. This is needed because
            //   the label and image have hide effects. If we toggle self to false, then the update would not run and the
            //   effects would not be visible.
            //   Visibuilty to false is set when both hide effects end
            set
            {
                base.Visible = value;
                if (this._lblTitle != null)
                {
                    this._lblTitle.Visible = value;
                }
                if (this._imgTitleBack != null)
                {
                    this._imgTitleBack.Visible = value;
                }
            }
        }

        public override string TextResourceId
        {
            get { return this._lblTitle.TextResourceId; }
            set { this._lblTitle.TextResourceId = value; }
        }

        public string SubTitleTextResourceId
        {
            get { return this._lblSubTitle.TextResourceId; }
            set { this._lblSubTitle.TextResourceId = value; }
        }

        public TitleSize BoardSize
        {
            get
            {
                return this._boardSize;
            }
            set
            {
                this._boardSize = value;
                switch (this._boardSize)
                {
                    case TitleSize.Big:
                        this._imgTitleBack.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuTitleBig");
                        break;
                    case TitleSize.Medium:
                        this._imgTitleBack.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuTitleMedium");
                        break;
                }
                this.Size = this._imgTitleBack.Size;
            }
        }

        public new Vector2 Scale
        {
            get { return base.Scale; }
            set
            {
                this._imgTitleBack.Scale = value;
                this._lblTitle.Scale = TITLE_FONT_SIZE * value;
                base.Scale = value;
            }
        }

        public string SubTitleText
        {
            set
            {
                this._lblSubTitle.Text = value;
            }
        }

        // <summary>
        /// 
        /// </summary>
        public UISnailsMenuTitle(UIScreen ownerScreen) :
            base(ownerScreen)
        {
            this._menuTitleFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", ResourceManager.ResourceManagerCacheType.Static);
            this._menuSubTitleFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-small", ResourceManager.ResourceManagerCacheType.Static);

            // Title background
            this._imgTitleBack = new UIImage(ownerScreen);
            this._imgTitleBack.Name = "_imgTitleBack";
            this._imgTitleBack.ParentAlignment = AlignModes.Horizontaly;
            this._imgTitleBack.Position = new Vector2(0.0f, -150.0f);
            this._imgTitleBack.ParentAlignment = AlignModes.HorizontalyVertically;
            this._imgTitleBack.OnHide += new UIEvent(_imgTitleBack_OnHide);
            this.Controls.Add(this._imgTitleBack);

            // Title 
            this._lblTitle = new UITextFontLabel(ownerScreen, this._menuTitleFont, "");
            this._lblTitle.Name = "_lblTitle";
            this._lblTitle.ParentAlignment = AlignModes.Horizontaly;
            this._lblTitle.BlendColor = UISnailsMenuTitle.TITLE_COLOR;
            this._lblTitle.Scale = TITLE_FONT_SIZE;
            this._lblTitle.ParentAlignment = AlignModes.HorizontalyVertically;
            this._lblTitle.OnHide += new UIEvent(_lblTitle_OnHide);
            this.Controls.Add(this._lblTitle);

            // Sub title 
            this._lblSubTitle = new UITextFontLabel(ownerScreen, this._menuSubTitleFont, "");
            this._lblSubTitle.Name = "_lblSubTitle";
            this._lblSubTitle.ParentAlignment = AlignModes.Horizontaly;
            this._lblSubTitle.BlendColor = UISnailsMenuTitle.TITLE_COLOR;
            this._lblSubTitle.Scale = TITLE_FONT_SIZE;
            this._lblSubTitle.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
            this._lblSubTitle.OnHide += new UIEvent(_lblTitle_OnHide);
            this._lblSubTitle.Margins.Bottom = 300f;
            this.Controls.Add(this._lblSubTitle);

            this.ParentAlignment = AlignModes.Horizontaly;
            this.Size = this._imgTitleBack.Size;
            this.AcceptControllerInput = false;
            this.BoardSize = TitleSize.Medium;
            this.Name = "_menuTitle";
            this.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.03f, this.BlendColor, new Vector2(1.0f, 1.0f));
            this.HideEffect = new PopOutEffect(new Vector2(1.2f, 1.2f), 6.0f);
            this.OnScreenStart += new UIEvent(UISnailsMenuTitle_OnScreenStart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        void UISnailsMenuTitle_OnScreenStart(IUIControl sender)
        {
            this.Scale = Vector2.One;
        }


        /// <summary>
        /// 
        /// </summary>
        void _imgTitleBack_OnHide(IUIControl sender)
        {
            if (this._imgTitleBack.Visible == false &&
                this._lblTitle.Visible == false)
            {
                base.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _lblTitle_OnHide(IUIControl sender)
        {
            if (this._imgTitleBack.Visible == false &&
                this._lblTitle.Visible == false)
            {
                base.Visible = false;
            }
        }
    }
}
