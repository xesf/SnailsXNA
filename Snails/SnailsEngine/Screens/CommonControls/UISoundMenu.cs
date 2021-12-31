using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISoundMenu : UISnailsMenu
    {
     
        #region Vars
        UISnailsSliderMenuItem _fxSlider;
        UISnailsSliderMenuItem _musicSlider;
        #endregion

        public bool StopPlayMusic { get; set; }
        private bool ControlReady { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISoundMenu(UIScreen ownerScreen) :
            base(ownerScreen)
        {
            this.TitleSize = UISnailsMenuTitle.TitleSize.Big;
            this.ItemSize = MenuItemSize.Big;
            this.TextResourceId = "MNU_SOUND_SETTINGS";
            // Menu items

            // Fx effects
            this._fxSlider = this.AddSliderItem("MNU_ITEM_SOUND_VOLUME");
            this._fxSlider.OnValueChanged += new UIEvent(_fxSlider_OnValueChanged);

            // Sound effects
            this._musicSlider = this.AddSliderItem("MNU_ITEM_MUSIC_VOLUME");
            this._musicSlider.OnValueChanged += new UIEvent(_musicSlider_OnValueChanged);
            this._musicSlider.OnValueChangedEnded += new UIEvent(_musicSlider_OnValueChangedEnded);
            // Back
        //    this.AddMenuItem("MNU_ITEM_BACK", this.MenuItem_OnBack, InputBase.InputActions.Back);

            this.OnMenuShownBegin += new UIEvent(UISoundMenu_OnMenuShownBegin);
            this.OnMenuShown += new UIEvent(UISoundMenu_OnMenuShown);
            this.OnBackPressed += new UIEvent(this.MenuItem_OnBack);
            this.OnResize();
            this.WithBackButton = true;
        }


        /// <summary>
        /// 
        /// </summary>
        void UISoundMenu_OnMenuShown(IUIControl sender)
        {
            ((SnailsScreen)this.ScreenOwner).EnableInput();
            this._fxSlider.Focus();
            // Don't allow left/right to be used for cursor snap because this movements are needed to move the slider
            this.ScreenOwner.CursorSnapDirections = (BrainEngine.UI.SnapDirection.Up | BrainEngine.UI.SnapDirection.Down);
            this.ControlReady = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void UISoundMenu_OnMenuShownBegin(IUIControl sender)
        {
            this.ControlReady = false;
            // Set slider positions
            this._fxSlider.Value = SnailsGame.ProfilesManager.CurrentProfile.SoundVolume;
            this._fxSlider.PlayChangedSound = true;
        
            this._musicSlider.Value = SnailsGame.ProfilesManager.CurrentProfile.MusicVolume;
            this._musicSlider.PlayChangedSound = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void _fxSlider_OnValueChanged(IUIControl sender)
        {
            // Set fx volume
            if (SnailsGame.ProfilesManager.CurrentProfile != null)
            {
                SnailsGame.ProfilesManager.CurrentProfile.SoundVolume = (int)this._fxSlider.Value;
            }
            BrainGame.SampleManager.MasterVolume = (this._fxSlider.Value / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        void _musicSlider_OnValueChanged(IUIControl sender)
        {
            if (this.StopPlayMusic && this.ControlReady)
            {
                if (BrainGame.MusicManager.IsMusicPaused)
                {
                    BrainGame.MusicManager.ResumeMusic();
                }
                else
                {
                    Levels.CurrentLevel.StageSound.PlayMusic();
                }
            }

            // Set music volume
            if (SnailsGame.ProfilesManager.CurrentProfile != null)
            {
                SnailsGame.ProfilesManager.CurrentProfile.MusicVolume = (int)this._musicSlider.Value;
            }
            BrainGame.MusicManager.MasterVolume = (this._musicSlider.Value / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        void _musicSlider_OnValueChangedEnded(IUIControl sender)
        {
            if (this.StopPlayMusic && this.ControlReady)
            {
                    BrainGame.MusicManager.PauseMusic();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void MenuItem_OnBack(IUIControl sender)
        {
            this.InvokeOnHide();
        }
    }
}
