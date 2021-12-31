using System;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsMenu : UIControl
    {
        #region Consts
        private const float SELECTED_ITEM_SCALE = 1.0f;// Item scale when gets focus
        private const float MENU_ITEMS_SPACING = -130f;
        #endregion

        #region Events
        public event UIEvent OnMenuShownBegin; // Occurs when the menu is shown (after all effects have ended and the menu is ready to be used)
        public event UIEvent OnMenuShown; // Occurs when the menu is shown (after all effects have ended and the menu is ready to be used)
        public event UIEvent OnItemSelectedBegin; // Occurs when any item is selected prior to any effects take effect
        public event UIEvent OnMenuHideBegin;
        public event UIEvent OnBackPressed;
        #endregion

        public enum MenuItemSize
        {
            Medium,
            Big
        }


        #region Vars
        private UIMenu _menu;
        private UITimer _tmrShow;
        private UITimer _hideTimer;
        private int _idxLastItemShowned;
        private UISnailsMenuTitle _title;
        private MenuItemSize _itemSize;

        private Sample _menuClosedSample;
        private Sample _menuShownSound;

        private UIBackButton _btnBack;

        #endregion

        #region Properties
        public TextFont MenuItemsFont { get; private set; }
        public TextFont MenuItemsFontUnselected { get; private set; }
        public int IdxLastItemSelected { get; set; }
        public UISnailsMenuItem ItemSelected { get; private set; }
        public int DefaultItemIndex { get; set; }
        public int Columns { get { return this._menu.Columns; } set { this._menu.Columns = value; } }
 
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                if (this.Visible == false)
                {
                    if (this._title != null)
                    {
                        this._title.Visible = value; 
                    }
                    if (this._menu != null)
                    {
                        foreach (UIMenuItem item in this._menu.Items)
                        {
                            item.Visible = value;
                        }
                    }
                }
            }
        }

        public UISnailsMenuTitle.TitleSize TitleSize
        {
            get
            {
                return this._title.BoardSize;
            }
            set
            {
                this._title.BoardSize = value;
            }
        }

        public MenuItemSize ItemSize
        {
            get
            {
                return this._itemSize;
            }
            set
            {
                this._itemSize = value;
            }
        }

        public override string TextResourceId
        {
            get
            {
                return this._title.TextResourceId;
            }
            set
            {
                this._title.TextResourceId = value;
            }
        }
     

        private float TitleMenuSpacing { get; set; }
        private bool BackPressed { get; set; }
        public bool WithBackButton
        {
            get { return this._btnBack.Visible; }
            set
            {
                this._btnBack.Visible = value;
            }
        }

        public int ItemCount
        {
            get { return this._menu.Items.Count; }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UISnailsMenu(UIScreen ownerScreen) :
            base(ownerScreen)
        {
            this.IdxLastItemSelected = 0;
            this.DefaultItemIndex = -1;
            this.DropShadow = false;
            this.ItemSize = MenuItemSize.Medium;
            this.OnScreenStart += new UIEvent(UISnailsMenu_OnScreenStart);
            this.OnInitializeFromContent += new UIEvent(UISnailsMenu_OnInitializeFromContent);

            // load samples content
            _menuClosedSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_CLOSED);
            _menuShownSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.MENU_SHOWN);

            // Create a timer for show/hide the options
            this.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this.DropShadow = false;

            this._idxLastItemShowned = -1;
            // Show timer - to show items 1 by 1
            this._tmrShow = new UITimer(ownerScreen, 10, true);
            this._tmrShow.Enabled = false;
            this._tmrShow.OnTimer += this.ShowTimer_OnTimer;
            this.Controls.Add(this._tmrShow);

            // HIde timer - items are all hidden at the same time, but a timer is used
            // to set the menu visible to false. We just give enouh time to hide the items
            this._hideTimer = new UITimer(ownerScreen, 400, false);
            this._hideTimer.Enabled = false;
            this._hideTimer.OnTimer += new UIEvent(_hideTimer_OnTimer);
            this.Controls.Add(this._hideTimer);

            this.MenuItemsFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
            this.MenuItemsFontUnselected = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium-2", ResourceManager.ResourceManagerCacheType.Static);

            // Title
            this._title = new UISnailsMenuTitle(ownerScreen);
            this._title.AcceptControllerInput = true;
            this.Controls.Add(this._title);


            // Back button
            this._btnBack = new UIBackButton(ownerScreen);
            this._btnBack.Effect = new HooverEffect(0.2f, 0.5f, -90.0f);
            this._btnBack.Position = new Vector2(0f, 150f);
            this._btnBack.OnAccept += new UIEvent(_btnBack_OnAccept);
            this._btnBack.AcceptControllerInput = true;
            this._btnBack.OnAcceptEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130);
            this._btnBack.ControllerActionCode = (int)InputBase.InputActions.Back;
            this._title.Controls.Add(this._btnBack);

            // Menu 
            this._menu = new UIMenu(ownerScreen);
            this._menu.ShowDisabledItems = false;
            this._menu.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
            this._menu.DropShadow = false;
            this._menu.ItemSpacing = MENU_ITEMS_SPACING;
            this.Controls.Add(this._menu);

            this.WithBackButton = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnBack_OnAccept(IUIControl sender)
        {
            ((SnailsScreen)this.ScreenOwner).DisableInput();
            this.BackPressed = true;
            this.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitializeFromContent()
        {
            base.InitializeFromContent();
            this.InitializeFromContent("UISnailsMenu");
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsMenu_OnInitializeFromContent(IUIControl sender)
        {
            this.TitleMenuSpacing = this.GetContentPropertyValue<float>("titleMenuSpacing", this.TitleMenuSpacing);
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsMenu_OnScreenStart(IUIControl sender)
        {
            this.UpdateSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowWithoutEffects()
        {
            this.BackPressed = false;
            this.Visible = true;
            this._title.Visible = true;
            this.ItemSelected = null;
            this._idxLastItemShowned = -1;
            for (int i = 0; i < this._menu.Items.Count; i++)
            {
                ((UISnailsMenuItem)this._menu.Items[i]).Initialize();
                if (this._menu.Items[i].Enabled)
                {
                    this._menu.Items[i].Visible = true;
                }
            }
            this._menu.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            this.BackPressed = false;

            if (this.Visible)
            {
                return;
            }
            if (this.OnMenuShownBegin != null)
            {
                this.OnMenuShownBegin(this);
            }
            this.ItemSelected = null;
            this.Visible = true;
            this._tmrShow.Reset();
            this._idxLastItemShowned = -1;
            this._title.Show();
            this._tmrShow.Enabled = true; // Items are showed 1 by 1 
            if (this._menu.Items.Count > 0)
            {
                this._menu.Items[this._menu.Items.Count - 1].OnShow -= new UIEvent(UISnailsMenu_OnShow); // what the heck is this?!
                this._menu.Items[this._menu.Items.Count - 1].OnShow += new UIEvent(UISnailsMenu_OnShow); // what the heck is this?!
            }

            for (int i = 0; i < this._menu.Items.Count; i++)
            {
                if (this._menu.Items[i].Enabled)
                {
                    UISnailsMenuItem snailsItem = (UISnailsMenuItem)this._menu.Items[i];
                    snailsItem.Initialize();
                }
            }

            this._menu.Refresh();
            _menuShownSound.Play();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Hide()
        {
            this._menuClosedSample.Play();
            this.AcceptControllerInput = false;
            foreach (UIMenuItem item in this._menu.Items)
            {
                item.Hide();
            }
            this._title.Hide();
            this._hideTimer.Enabled = true;

            if (this.OnMenuHideBegin != null)
            {
                this.OnMenuHideBegin(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsMenuItem AddMenuItem(string textResourceId, UIEvent acceptEvent, InputBase.InputActions controllerActionCode)
        {
            return this.AddMenuItem(textResourceId, acceptEvent, controllerActionCode, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsMenuItem AddMenuItem(string textResourceId, UIEvent acceptEvent, InputBase.InputActions controllerActionCode, bool autohideMenu, bool shouldCreate)
        {
            if (shouldCreate == false)
            {
                return null;
            }
            UISnailsMenuItem item = new UISnailsMenuItem(this.ScreenOwner, textResourceId, this.MenuItemsFont, this, true);
            item.AutoHideMenu = autohideMenu;
            this.InitMenuItem(item, acceptEvent, controllerActionCode, true);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsMenuItem AddMenuItem(string textResourceId, UIEvent acceptEvent, InputBase.InputActions controllerActionCode, bool autohideMenu)
        {
            return this.AddMenuItem(textResourceId, acceptEvent, controllerActionCode, autohideMenu, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsSliderMenuItem AddSliderItem(string textResource)
        {
            UISnailsSliderMenuItem item = new UISnailsSliderMenuItem(this.ScreenOwner, this);
            this.InitMenuItem((UISnailsMenuItem)item, null, InputBase.InputActions.None, false);
            item.TextResourceId = textResource;
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMenuItem(UISnailsMenuItem item, UIEvent acceptEvent, InputBase.InputActions controllerActionCode, bool withAccept)
        {
            item.Visible = this.Visible;

            item.OnFocus += new UIEvent(this.MenuItem_OnFocus);
            item.OnLostFocus += new UIEvent(this.MenuItem_OnLostFocus);
            if (withAccept)
            {
                item.OnAcceptBegin += new UIEvent(this.MenuItem_OnAcceptBegin);
                item.OnAcceptEffect = new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 100);
                item.OnAccept += this.MenuItem_OnAccept;
            }
            item.OnSelect += acceptEvent;
            item.ControllerActionCode = (int)controllerActionCode;
            if (this.Columns > 1)
            {
                item.ParentAlignment = AlignModes.None;
            }
            this._menu.Items.Add(item);
            this.UpdateSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateSize()
        {
            this._menu.Position = new Vector2(0.0f, this._title.Position.Y + this.TitleMenuSpacing + this._title.Size.Height);
            this._menu.AdjustSizeToFitItems();
            this.Size = new Size(Math.Max(this._title.Size.Width, this._menu.Width), this._title.Size.Height + this.TitleMenuSpacing + this._menu.Size.Height);
        }

        /// <summary>
        /// Sets the focus on the specified menu item
        /// </summary>
        public void SetFocus(int itemIdx)
        {
            // For free cursors do nothing
            if (this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
            {
                this._menu.Items[itemIdx].Focus();
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        public void SetFocus(UISnailsMenuItem item)
        {
            // For free cursors do nothing
            if (this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
            {
                for (int i = 0; i < this._menu.Items.Count; i++ )
                {
                    if (item == this._menu.Items[i])
                    {
                        this.SetFocus(i);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetFocusOnLastSelectedItem()
        {
            // For free cursors do nothing
            if (this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
            {
                if (this.IdxLastItemSelected >= 0 &&
                    this.IdxLastItemSelected < this._menu.Items.Count)
                {
                    this._menu.Items[this.IdxLastItemSelected].Focus();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowTimer_OnTimer(IUIControl sender)
        {
            this._idxLastItemShowned++;
            if (this._idxLastItemShowned < this._menu.Items.Count)
            {
                if (this._menu.Items[this._idxLastItemShowned].Enabled)
                {
                    this._menu.Items[this._idxLastItemShowned].Show();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UISnailsMenu_OnShow(IUIControl sender)
        {
           // ((UISnailsMenuItem)sender).OnShow -= this.UISnailsMenu_OnShow; // ? what the heck is this?
            this._tmrShow.Enabled = false;
            this.AcceptControllerInput = true;
            if (this.DefaultItemIndex >= 0 &&
                this.DefaultItemIndex < this._menu.Items.Count)
            {
                this.Focus();
                this.SetFocus(this.DefaultItemIndex);
            }
            if (this.OnMenuShown != null)
            {
                this.OnMenuShown(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnLostFocus(IUIControl sender)
        {
            UISnailsMenuItem menuItem = (UISnailsMenuItem)sender;
            
            if (this.ItemSelected != menuItem)
            {
                menuItem.Reset();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnFocus(IUIControl sender)
        {
            if (((UISnailsMenuItem)sender).WithFocus)
            {
                return;
            }
            this.ResetAll();
            
            UISnailsMenuItem menuItem = (UISnailsMenuItem)sender;
            menuItem.GotFocus();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetAll()
        {
            foreach (UISnailsMenuItem item in this._menu.Items)
            {
                item.Reset();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnAccept(IUIControl sender)
        {
            UISnailsMenuItem menuItem = (UISnailsMenuItem)sender;
            if (menuItem.AutoHideMenu)
            {
                this.Hide(); // Hide starts the hide timer. Item selected event will occur when the timer ellapses
            }
            else
            {
                menuItem.Reset();
                this.InvokeItemSelected();
                this.ItemSelected = null;
            }

            this.IdxLastItemSelected = this.GetItemIndex((UISnailsMenuItem)sender);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnAcceptBegin(IUIControl sender)
        {
            ((SnailsScreen)this.ScreenOwner).DisableInput();
            UISnailsMenuItem menuItem = (UISnailsMenuItem)sender;
            
            this.ItemSelected = menuItem;
            if (!this.ItemSelected.WithFocus)
            {
                this.ItemSelected.Focus();
            }

            if (this.OnItemSelectedBegin != null)
            {
                this.OnItemSelectedBegin(menuItem);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void _hideTimer_OnTimer(IUIControl sender)
        {
            this.Visible = false;
            if (this.BackPressed)
            {
                if (this.OnBackPressed != null)
                {
                    this.OnBackPressed(this);
                }
            }
            else
            {
                this.InvokeItemSelected();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InvokeItemSelected()
        {
            if (this.ItemSelected != null) // Invoke the onselect event on the selected item
                                           // This may happen because the menu is auto-hidden when a item is selected
            {
//                this.ItemSelected.Focus();
                this.ItemSelected.InvokeOnSelect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int GetItemIndex(UISnailsMenuItem toFind)
        {
            for (int i = 0; i < this._menu.Items.Count; i++)
            {
                if (this._menu.Items[i] == toFind)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
