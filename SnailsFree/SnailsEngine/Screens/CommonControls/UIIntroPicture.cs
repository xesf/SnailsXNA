using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIIntroPicture : UIControl
    {
        public class IntroPictureSaveState 
        {
            public Vector2 cloud1Pos;
            public Vector2 cloud2Pos;
            public Vector2 cloud3Pos;
            public float sunLightRot;
        }

        const int BKG_ALPHA = 120;

        const float CLOUD1_SPEED = 0.08f;
        const float CLOUD2_SPEED = 0.1f;
        const float CLOUD3_SPEED = 0.05f;
        const float SUNLIGHT_SPEED = 0.008f;

        // BB index for each element in intro1 sprite
        // This BB are used to place the objects
        const int BB_IDX_CLOUDS = 0;
        const int BB_IDX_SKY = 1;
        const int BB_IDX_SUN = 2;
        const int BB_IDX_CROWN = 3;

        const int BB_CROWN_COUNT = 4;

        UIImage _imgFrame;
        UIImage _imgCrownShine;
        UISnailsTitle _gameTitle;

        public Vector2 GameTitlePosition
        {
            set
            {
                this._gameTitle.Position = value;
            }
        }
        
        UIPanel _bkgPanel;

        // In screen units
        BoundingSquare SkyArea
        {
            get
            {
                return this.PixelsToScreenUnits(this._imgFrame.Sprite.BoundingBoxes[BB_IDX_SKY]);
            }
        }

        // In screen units
        BoundingSquare SunArea
        {
            get
            {
                return this.PixelsToScreenUnits(this._imgFrame.Sprite.BoundingBoxes[BB_IDX_SUN]);
            }
        }


        public UIIntroPicture(UIScreen ownerScreen) :
            base(ownerScreen)
        {
           // Img frame
            this._imgFrame = new UIImage(this.ScreenOwner, "spriteset/intro2-1/Frame", ResourceManager.RES_MANAGER_ID_TEMPORARY);
			this._imgFrame.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this.Controls.Add(this._imgFrame);

            // Crown shine
			this._imgCrownShine = new UIImage(this.ScreenOwner, "spriteset/intro2-1/CrownShine", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgCrownShine.OnLastFrame += new UIImage.LastFrameHandler(_imgCrownShine_OnLastFrame);
			this.Controls.Add(this._imgCrownShine);
            
            
            this._bkgPanel = new UIPanel(this.ScreenOwner);
            this._bkgPanel.BackgroundColor = new Color(0, 0, 0, 0);
            this._bkgPanel.Visible = false;
            this._bkgPanel.UseBlendColorOnBackground = true;
            this._bkgPanel.CanBeScaled = false;
            this._bkgPanel.Size = this.ScreenOwner.Size;// new BrainEngine.UI.Size(10000, 10000);
            this._bkgPanel.ShowEffect = new ColorEffect(new Color(0, 0, 0, 0), new Color(0, 0, 0, BKG_ALPHA), 0.05f, false);
            this._bkgPanel.HideEffect = new ColorEffect(new Color(0, 0, 0, BKG_ALPHA), new Color(0, 0, 0, 0), 0.05f, false);
            this.Controls.Add(this._bkgPanel);

            // Game title
            this._gameTitle = new UISnailsTitle(this.ScreenOwner);
            this._gameTitle.Mode = UISnailsTitle.TitleMode.Leaf;
            this._gameTitle.Position = new Vector2(0f, 300f); //new Vector2(0f, 800f);
			if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LDW) 
			{
					this._gameTitle.Scale = new Vector2 (0.85f, 0.85f);
			}
			this.Controls.Add(this._gameTitle);

            this.Size = this.ScreenOwner.Size;
            this.AcceptControllerInput = false;
 

			float scale = SnailsGame.GameSettings.RatioNativeResolutionWidth;
			this.ScaleChilds(new Vector2(scale, scale));


		}


        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this.PlaceCrownShine();
        }


        /// <summary>
        /// 
        /// </summary>
        void PlaceCrownShine()
        {
            int idx = BrainGame.Rand.Next(UIIntroPicture.BB_CROWN_COUNT) + UIIntroPicture.BB_IDX_CROWN;
            BoundingSquare bb = this._imgFrame.Sprite.BoundingBoxes[idx];
            this._imgCrownShine.Position = this._imgFrame.Position + this.PixelsToScreenUnits(new Vector2(bb.Left, bb.Top) * this._imgCrownShine.Scale);
        }

        /// <summary>
        /// 
        /// </summary>
        void _imgCrownShine_OnLastFrame()
        {
            this.PlaceCrownShine();
        }

        public void ResetBackgroundAlpha()
        {
            this._bkgPanel.BackgroundColor = new Color(0, 0, 0, 0);
            this._bkgPanel.BlendColor = new Color(0, 0, 0, 0);
            this._bkgPanel.Visible = false;
        }

        public void SetBackgroundAlpha()
        {
            this._bkgPanel.BackgroundColor = new Color(0, 0, 0, BKG_ALPHA);
            this._bkgPanel.BlendColor = new Color(0, 0, 0, BKG_ALPHA);
            this._bkgPanel.Visible = true;
        }

        public void FadeInBackground()
        {
            this._bkgPanel.Show();
        }

        public void FadeOutBackground()
        {
            this._bkgPanel.Hide();
        }

        public void ShowBackgroundPanel()
        {
            this._bkgPanel.Visible = true;
        }

        public IntroPictureSaveState GetSaveState()
        {
            IntroPictureSaveState save = new IntroPictureSaveState();
            return save;
        }

        public void SetSaveState(IntroPictureSaveState save)
        {
            if (save == null)
            {
                return;
            }

        }
    }
}
