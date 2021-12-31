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
        UIImage _imgCloud1;
        UIImage _imgCloud2;
        UIImage _imgCloud3;
        UIImage _imgSun;
        UIImage _imgSky;
        UIImage _imgSunLight;
        UIImage _imgCrownShine;
        UISnailsTitle _gameTitle;
        
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
            // Img Sky
            this._imgSky = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Sky", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgSky.SizeMode = BrainEngine.UI.ImageSizeMode.Stretch;
            this._imgSky.Name = "_imgSky";
            this.Controls.Add(this._imgSky);

            // Img Sun light
            this._imgSunLight = new UIImage(this.ScreenOwner, "spriteset/intro2-2/SunLight", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgSunLight.Scale = new Vector2(5f, 5f);
            this._imgSunLight.Name = "SunLight";
            this.Controls.Add(this._imgSunLight);

            // Img Sun
            this._imgSun = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Sun", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this.Controls.Add(this._imgSun);

            // Img cloud1
            this._imgCloud1 = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Cloud1", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this.Controls.Add(this._imgCloud1);

            // Img cloud2
            this._imgCloud2 = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Cloud2", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this.Controls.Add(this._imgCloud2);

            // Img cloud3
            this._imgCloud3 = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Cloud3", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this.Controls.Add(this._imgCloud3);

            // Img frame
            this._imgFrame = new UIImage(this.ScreenOwner, "spriteset/intro2-1/Frame", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgFrame.ParentAlignment = BrainEngine.UI.AlignModes.Bottom;
            this._imgFrame.Margins.Bottom = -1;
            this.Controls.Add(this._imgFrame);

            // Crown shine
            this._imgCrownShine = new UIImage(this.ScreenOwner, "spriteset/intro2-2/CrownShine", ResourceManager.RES_MANAGER_ID_TEMPORARY);
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
            this.Controls.Add(this._gameTitle);

            this.Size = this.ScreenOwner.Size;
            this._imgSky.Size = this.Size;
            this.AcceptControllerInput = false;
            this.OnInitializeFromContent += new UIEvent(UIIntroPicture_OnInitializeFromContent);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void InitializeFromContent()
        {
            base.InitializeFromContent();
            this.InitializeFromContent("UIIntroPicture");
        }

        /// <summary>
        /// 
        /// </summary>
        void UIIntroPicture_OnInitializeFromContent(IUIControl sender)
        {
            this._gameTitle.Scale = this.GetContentPropertyValue<Vector2>("titleScale", this._gameTitle.Scale);
        }

       
        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            // Set positons

            // Set the clouds positions. We don't want to randomize this because we want it to look good
            this._imgCloud1.Position = new Vector2(200f, 1000f);
            this._imgCloud2.Position = new Vector2(4000f, 2250f);
            this._imgCloud3.Position = new Vector2(7500f, 1600f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this.PlaceCrownShine();
            // Initialize positions here
            // This is because wp martela o scale AFTER OnLoad, and this positions depend on the scale
            this._imgSun.Position = this._imgFrame.Position + new Vector2(SunArea.Left * this._imgSun.Scale.X,
                                                                          SunArea.Top * this._imgSun.Scale.Y);
            this._imgSunLight.Position = this._imgSun.Position;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            this.UpdateCloud(this._imgCloud1, CLOUD1_SPEED, gameTime);
            this.UpdateCloud(this._imgCloud2, CLOUD2_SPEED, gameTime);
            this.UpdateCloud(this._imgCloud3, CLOUD3_SPEED, gameTime);

            this._imgSunLight.Rotation += (SUNLIGHT_SPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cloud"></param>
        /// <param name="speed"></param>
        /// <param name="gameTime"></param>
        private void UpdateCloud(UIImage cloud, float speed, BrainEngine.BrainGameTime gameTime)
        {
            cloud.Position += new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0f);
            if (cloud.Position.X > UIScreen.MAX_SCREEN_WITDH_IN_POINTS)
            {
                cloud.Position = new Vector2(-cloud.Width, cloud.Position.Y);
            }
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
            save.cloud1Pos = this._imgCloud1.Position;
            save.cloud2Pos = this._imgCloud2.Position;
            save.cloud3Pos = this._imgCloud3.Position;
            save.sunLightRot = this._imgSunLight.Rotation;
            return save;
        }

        public void SetSaveState(IntroPictureSaveState save)
        {
            if (save == null)
            {
                return;
            }

            this._imgCloud1.Position = save.cloud1Pos;
            this._imgCloud2.Position = save.cloud2Pos;
            this._imgCloud3.Position = save.cloud3Pos;
            this._imgSunLight.Rotation = save.sunLightRot;
        }
    }
}
