using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.Snails.Input;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Screens
{
    class BrainsLogoScreen : UIScreen
    {
        enum ScreenState
        {
            Startup,
            BetaInfo,
            Logo
        }

        #region Vars
        UIImage _imgLogo;
        UITimer _tmrSkip;
        UIImage _imgFacebok;
        UIImage _imgTwitter;
        UIPanel _panelSocial;

        #endregion

        #region Properties
        ScreenState State { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public BrainsLogoScreen(ScreenNavigator owner) :
            base(owner)
        {
            this.BackgroundColor = Color.White;

            this._imgLogo = new UIImage(this, "spriteset/brains-logo/BrainsLogo", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgLogo.ParentAlignment = AlignModes.HorizontalyVertically;
            this.Controls.Add(this._imgLogo); 

            this._tmrSkip = new UITimer(this, 2500, false);
            this._tmrSkip.OnTimer += new UIControl.UIEvent(_tmrSkip_OnTimer);
            this.Controls.Add(this._tmrSkip);


            this._panelSocial = new UIPanel(this);
            this._panelSocial.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
            this._panelSocial.Margins.Bottom = 500f;
            this._panelSocial.Size = this.NativeResolution(new Size(1000f, 1000f));
            this.Controls.Add(this._panelSocial);

            this._imgFacebok = new UIImage(this, "spriteset/menu-elements-1/Facebook", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgFacebok.ParentAlignment = AlignModes.Left | AlignModes.Vertically;
            this._panelSocial.Controls.Add(this._imgFacebok);

            this._imgTwitter = new UIImage(this, "spriteset/menu-elements-1/Twitter", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgTwitter.ParentAlignment = AlignModes.Right | AlignModes.Vertically;
            this._panelSocial.Controls.Add(this._imgTwitter);

            this.OnAccept += new UIControl.UIEvent(BrainsLogoScreen_OnAccept);
        }


        void BrainsLogoScreen_OnAccept(IUIControl sender)
        {
            this.NavigateToMain();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this._tmrSkip.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void _tmrSkip_OnTimer(IUIControl sender)
        {
            this.NavigateToMain();
        }
       

        /// <summary>
        /// 
        /// </summary>
        void NavigateToMain()
        {
            BrainGame.DisplayHDDAccessIcon = true;
            BrainGame.ClearColor = Color.White;
            if (SnailsGame.GameSettings.ShowAutoSaveScreen)
            {
                this.NavigateTo(ScreenType.AutoSave.ToString(), ScreenTransitions.FadeOut, ScreenTransitions.FadeIn);
            }
            else
            {
                this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), ScreenTransitions.FadeOutWhite, ScreenTransitions.FadeInWhite);
            }
        }
	}
}
