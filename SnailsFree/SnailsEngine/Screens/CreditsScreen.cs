using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens
{
    class CreditsScreen : SnailsScreen
    {
        enum ScreenState
        {
            ShowingPanels,
            Active
        }

        #region Properties
        UICreditsPanel _pnlGameDesign;
        UICreditsPanel _pnlArt;
        UICreditsPanel _pnlMusic;
        UICreditsPanel _pnlAddLevelDesign;
        UICreditsPanel _pnlSpecialThanks;
        UICreditsPanel _pnlTeam;
        UIMiniSnailsTitle _title;
        UIImage _imgBrainsLogo;
        UITimer _tmrShowPanels;
        UICreditsPanel _pnlAdditionaSoundEffects;

        ScreenState State { get; set; }

        public float PanelSpacing { get; set; }
        public float Column1Position { get; set; }
        public float Column2Position { get; set; }
        public float YPosition { get; set; }
        Sample _showSound;
        Sample _applauseSound;         
        #endregion

        public CreditsScreen(ScreenNavigator owner) :
            base(owner, ScreenType.Credits)
        { }


        /// <summary>
        /// 
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            this.Name = "";
            this.BackgroundImageBlendColor = Colors.CreditsScrBkColor;
            this.BackgroundType = ScreenBackgroundType.Image;

            // Game design, Programming and Sound Effects
            this._pnlGameDesign = new UICreditsPanel(this);
            this._pnlGameDesign.AddCategory("CREDITS_DESIGN");
            this._pnlGameDesign.AddName("Alexandre Fontoura");
            this._pnlGameDesign.AddName("Jorge Lima");
            this.Controls.Add(this._pnlGameDesign);

            // Art
            this._pnlArt = new UICreditsPanel(this);
            this._pnlArt.AddCategory("CREDITS_ART");
            this._pnlArt.AddName("Jorge Lima");
            this.Controls.Add(this._pnlArt);

            // Music
            this._pnlMusic = new UICreditsPanel(this);
            this._pnlMusic.AddCategory("CREDITS_MUSIC");
            this._pnlMusic.AddName("Rui Querido");
            this.Controls.Add(this._pnlMusic);

            // Additional Level Design
            this._pnlAddLevelDesign = new UICreditsPanel(this);
            this._pnlAddLevelDesign.AddCategory("CREDITS_ADDLEVEL");
            this._pnlAddLevelDesign.AddName("Francisco Ferreira");
            this._pnlAddLevelDesign.AddName("Jorge Lima");
            this.Controls.Add(this._pnlAddLevelDesign);

            // Special thanks
            this._pnlSpecialThanks = new UICreditsPanel(this);
            this._pnlSpecialThanks.Name = "_pnlSpecialThanks";
            this._pnlSpecialThanks.AddCategory("CREDITS_THANKS");
            this._pnlSpecialThanks.AddName("Leonor Cachapuz");
            this._pnlSpecialThanks.AddName("Michelle Silva");
            this._pnlSpecialThanks.AddName("Miguel Lima");
            this.Controls.Add(this._pnlSpecialThanks);

            // Team
            this._pnlTeam = new UICreditsPanel(this);
            this._pnlTeam.AddCategory("CREDITS_TEAM");
            this._pnlTeam.AddName("Alexandre Fontoura");
            this._pnlTeam.AddName("Jorge Lima");
          //  this.Controls.Add(this._pnlTeam);

            // Additional sound effects
            this._pnlAdditionaSoundEffects = new UICreditsPanel(this);
            this._pnlAdditionaSoundEffects.AddCategory("CREDITS_ADDITIONAL_SOUND_EFFECTS");
            this._pnlAdditionaSoundEffects.AddName("http://www.freesfx.co.uk/");
            this.Controls.Add(this._pnlAdditionaSoundEffects);

            // Snails logo
            this._title = new UIMiniSnailsTitle(this);
            this._title.Name = "_title";
            this._title.ShowEffect = new SquashEffect(this._title.Scale.X * 0.9f, 2.0f, 0.01f, this._title.BlendColor, this._title.Scale);
			this._title.Position = new Vector2 (2550, 1350);
			this.Controls.Add(this._title);

            // Brains logo
            this._imgBrainsLogo = new UIImage(this, "spriteset/common-elements-1/BrainsLogo", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgBrainsLogo.Name = "_imgBrainsLogo";
            this._imgBrainsLogo.ShowEffect = new ColorEffect(new Color(0.0f, 0.0f, 0.0f, 0.0f), Color.White, 0.005f, false);
            this._imgBrainsLogo.ShowEffect = new SquashEffect(0.9f, 2.0f, 0.01f, this._imgBrainsLogo.BlendColor, this._imgBrainsLogo.Scale);
			this._imgBrainsLogo.Position = new Vector2 (6000, 2300);
            this.Controls.Add(this._imgBrainsLogo);

            this._tmrShowPanels = new UITimer(this, 500, true);
            this._tmrShowPanels.OnTimer += new UIControl.UIEvent(_tmrShowPanels_OnTimer);
            this._tmrShowPanels.Parameter = 0;
            //this.Controls.Add(this._tmrShowPanels);

            this._showSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);
            this._applauseSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.CREDIT_YEAH);

        }

  
        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            this._pnlGameDesign.Visible = true;
            this._pnlArt.Visible = true;
            this._pnlMusic.Visible = true;
            this._pnlAddLevelDesign.Visible = true;
            this._pnlSpecialThanks.Visible = true;
            this._pnlTeam.Visible = true;
            this._tmrShowPanels.Enabled = true;
            this._imgBrainsLogo.Visible = true;
            this._title.Visible = true;

            this._tmrShowPanels.Parameter = 0;
            this._tmrShowPanels.Time = 500;
            this._tmrShowPanels.Reset();

            this.State = CreditsScreen.ScreenState.Active;
 

			this.PanelSpacing = 120;
			this.Column1Position = 1000;
			this.Column2Position = 5900;
			this.YPosition = 3400;

            this._pnlGameDesign.Position = new Vector2(this.Column1Position, this.YPosition);
            this._pnlArt.Position = new Vector2(this._pnlGameDesign.Left, _pnlGameDesign.Bottom + this.PanelSpacing);
            this._pnlMusic.Position = new Vector2(this._pnlGameDesign.Left, _pnlArt.Bottom + this.PanelSpacing);
            this._pnlAddLevelDesign.Position = new Vector2(this._pnlGameDesign.Left, _pnlMusic.Bottom + this.PanelSpacing);
			this._pnlSpecialThanks.Position = new Vector2 (this.Column2Position, 4200);
            this._pnlAdditionaSoundEffects.Position = new Vector2(this._pnlSpecialThanks.Left, _pnlSpecialThanks.Bottom + this.PanelSpacing);
            this._pnlTeam.Position = new Vector2(this._pnlSpecialThanks.Left, this._pnlAddLevelDesign.Bottom + this.PanelSpacing);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
         switch (this.State)
            {

                case CreditsScreen.ScreenState.Active:
                    if (this._inputController.ActionBack || this._inputController.ActionAccept)
                    {
                        ScreenType caller = this.Navigator.GlobalCache.Get<ScreenType>("CREDITS_SCREEN_CALLER");
                        if (caller == ScreenType.MainMenu)
                        {
                            this.Navigator.GlobalCache.Set(GlobalCacheKeys.MAIN_SCREEN_STARTUP_MODE, MainMenuScreen.StartupType.TitleAndMenuVisible);
                            this.NavigateTo(ScreenType.MainMenu.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
                        }
                        else
                        if (caller == ScreenType.Options)
                        {
                            this.Navigator.GlobalCache.Set(GlobalCacheKeys.OPTIONS_STARTUP_MODE, OptionsScreen.StartupType.MenuVisible);
                            this.NavigateTo(ScreenType.Options.ToString(), ScreenTransitions.LeafsClosing, ScreenTransitions.LeafsOpening);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _tmrShowPanels_OnTimer(IUIControl sender)
        {
            int index = (int)this._tmrShowPanels.Parameter;
            if (index <= 7)
            {
                this._showSound.Play();
            } 
            
            switch (index)
            {
                case 0:
                    this._title.Show();
                    this._tmrShowPanels.Time = 500;
                    break;
                case 1:
                    this._imgBrainsLogo.Show();
                    this._tmrShowPanels.Time = 500;
                    break;
                case 2:
                    this._pnlGameDesign.Show();
                    break;
                case 3:
                    this._pnlArt.Show();
                    break;
                case 4:
                    this._pnlMusic.Show();
                    break;
                case 5:
                    this._pnlAddLevelDesign.Show();
                    break;
                case 6:
                    this._pnlSpecialThanks.Show();
                    break;
                case 7:
                    this._pnlTeam.Show();
                    this._applauseSound.Play();
                    break;
                
                default:
                    this._tmrShowPanels.Enabled = false;
                    this.State = CreditsScreen.ScreenState.Active;
                    break;
            }
            this._tmrShowPanels.Parameter = (++index);
        }

    }
}
