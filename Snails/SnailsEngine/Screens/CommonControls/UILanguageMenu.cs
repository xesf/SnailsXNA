using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UILanguageMenu : UIControl
    {
        public event UIEvent OnLanguageSelected; 

        UISnailsMenu _menu;
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public UILanguageMenu(UIScreen screenOwner) :
            base(screenOwner)
        {
            // Menu 
            this._menu = new UISnailsMenu(screenOwner);
            this._menu.Size = new BrainEngine.UI.Size(3000.0f, 3800.0f);
            this._menu.Visible = false;
            this._menu.TextResourceId = "MNU_LANGUAGE";
            this._menu.DefaultItemIndex = 1;
            this._menu.OnMenuShown += new UIEvent(_menu_OnMenuShown);
            this._menu.WithBackButton = true;
            this._menu.OnBackPressed += new UIEvent(this.MenuItem_OnBack);
            this._menu.OnSizeChanged += new UIEvent(_menu_OnSizeChanged);
            this.Controls.Add(this._menu);

            // Menu items
            if ((SnailsGame.GameSettings.Languages & LanguageType.English) == LanguageType.English)
            {
                this._menu.AddMenuItem("MNU_ITEM_ENGLISH", this.MenuItem_OnEnglish, 0);
            }

            if ((SnailsGame.GameSettings.Languages & LanguageType.Portuguese) == LanguageType.Portuguese)
            {
                this._menu.AddMenuItem("MNU_ITEM_PORTUGUESE", this.MenuItem_OnPortuguese, 0);
            }

            if ((SnailsGame.GameSettings.Languages & LanguageType.French) == LanguageType.French)
            {
                this._menu.AddMenuItem("MNU_ITEM_FRENCH", this.MenuItem_OnFrench, 0);
            }

            if ((SnailsGame.GameSettings.Languages & LanguageType.Spanish) == LanguageType.Spanish)
            {
                this._menu.AddMenuItem("MNU_ITEM_SPANISH", this.MenuItem_OnSpanish, 0);
            }

            if ((SnailsGame.GameSettings.Languages & LanguageType.Italian) == LanguageType.Italian)
            {
                this._menu.AddMenuItem("MNU_ITEM_ITALIAN", this.MenuItem_OnItalian, 0);
            }

            if ((SnailsGame.GameSettings.Languages & LanguageType.German) == LanguageType.German)
            {
                this._menu.AddMenuItem("MNU_ITEM_GERMAN", this.MenuItem_OnGerman, 0);
            }
            //this._menu.AddMenuItem("MNU_ITEM_BACK", this.MenuItem_OnBack, InputBase.InputActions.Back);

            this.Size = this._menu.Size;
            this.Position = new Vector2(0.0f, 4500.0f);
            this.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._menu.Columns = 1;
            if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD && 
                this._menu.ItemCount > 3)
            {
                this._menu.Columns = 2;
            }

            this._menu.Columns = (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD? 2 : 1);
        }

        void _menu_OnSizeChanged(IUIControl sender)
        {
            this.Size = this._menu.Size;
        }

        /// <summary>
        /// 
        /// </summary>
        void _menu_OnMenuShown(IUIControl sender)
        {
            this.InvokeOnShow();            
        }


        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnEnglish(IUIControl sender)
        {
            this.SetLanguage(LanguageCode.en);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnPortuguese(IUIControl sender)
        {
            this.SetLanguage(LanguageCode.pt);
        }

        private void MenuItem_OnFrench(IUIControl sender)
        {
            this.SetLanguage(LanguageCode.fr);
        }

        private void MenuItem_OnSpanish(IUIControl sender)
        {
            this.SetLanguage(LanguageCode.es);
        }

        private void MenuItem_OnItalian(IUIControl sender)
        {
            this.SetLanguage(LanguageCode.it);
        }

        private void MenuItem_OnGerman(IUIControl sender)
        {
            this.SetLanguage(LanguageCode.de);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage(LanguageCode languageCode)
        {
            BrainGame.CurrentLanguage = languageCode;
            if (this.OnLanguageSelected != null)
            {
                this.OnLanguageSelected(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnBack(IUIControl sender)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            this.Visible = true;
            this._menu.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            this.Visible = false;
            this.InvokeOnHide();
        }

        /// <summary>
        /// Sets the focus on the specified menu item
        /// </summary>
        public void SetFocus(int itemIdx)
        {
            this._menu.SetFocus(itemIdx);
        }
    }
}
