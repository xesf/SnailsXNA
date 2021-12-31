using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Input;
using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.UI.Screens;
using System.IO;
using TwoBrainsGames.BrainEngine.Resources;
#if DEBUG && FORMS_SUPPORT
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
#endif
namespace TwoBrainsGames.Snails.Screens
{
	class DebugOptionsScreen : TwoBrainsGames.BrainEngine.UI.Screens.Screen
    {
        const int THUMBNAIL_WIDTH = 200;
        const int THUMBNAIL_HEIGHT = 127;

        struct KeyHelpItem
        {
            public int _XboxControlFrameNr;
            public string _HelpText;
            public Keys _Key;

            public KeyHelpItem(Keys key, int xboxControlFrameNr, string helpText)
            {
                this._XboxControlFrameNr = xboxControlFrameNr;
                this._HelpText = helpText;
                this._Key = key;
            }
        };

        public DebugOptionsInput Input;

        Rectangle _Rect;
        Color _BackColor;
//        Sprite _DebugSprite;
        Vector2 _Position;
        SpriteFont _Font;
        Color _Textcolor;

        List<KeyHelpItem> _KeyList;

        public DebugOptionsScreen(ScreenNavigator owner) :
            base(owner)
        { }

        /// <summary>
        /// 
        /// </summary>
    public override void OnLoad()
        {
            this._inputController = new DebugOptionsInput();
			this._inputController.Initialize();
			this.Input = (DebugOptionsInput)this._inputController;
            int marginX = (int)(SnailsGame.ScreenWidth * 0.1f);
            int marginY = (int)(SnailsGame.ScreenHeight * 0.1f);
            this._Position = new Vector2(marginX - (int)SnailsGame.DefaultCamera.Origin.X, 
                                         marginY - (int)SnailsGame.DefaultCamera.Origin.Y);
            this._Rect = new Rectangle((int)this._Position.X,
                                       (int)this._Position.Y,
                                       SnailsGame.ScreenWidth - (marginX * 2),
                                       SnailsGame.ScreenHeight - (marginY * 2));
            this._BackColor = new Color(0, 0, 100, 128);
			this._KeyList = new List<KeyHelpItem>();
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_HIDE_HELP, 5, "Show / hide this help"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_HIDE_SPRITES, 9, "Show / hide sprites"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_HIDE_TILES, 8, "Show / hide tiles"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_HIDE_PATHS, 11, "Show / hide paths"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_QUADTREE, 10, "Show / hide quadtree"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_HIDE_BOUNDINGBOXES, 0, "Show / hide bounding boxes"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_SHOW_HIDE_DEBUG_INFO, 1, "Show / hide debug info"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_ENABLE_AVERAGES, 3, "Enable / disable averages"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_RELOAD_STAGE, 2, "Reset stage"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_DEBUG_INFO_POSITION, 12, "Change Debug Info position"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_NEXT_STAGE, 14, "Go to next stage"));
			this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_PREV_STAGE, 15, "Go to previous stage"));
            this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_GENERATE_ALL_THUMBS, 0, "Generate Thumbnails Textures"));
            this._KeyList.Add(new KeyHelpItem(DebugOptionsInput.KEY_GENERATE_CURRENT_THUMB, 0, "Generate Stage Thumbnail"));

			this._Textcolor = Color.Yellow;

         //   this._DebugSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/debug", "XBoxButtons");
            this._Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/dbgWindow", ResourceManager.ResourceManagerCacheType.Static);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUpdate(BrainGameTime gameTime)
        {
            bool changed = false;
			// Show / hide sprites
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.HideDebugOptionsInput))
			{
				this.Close();
				return;
			}

			// Show / hide sprites
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowHideSprites))
			{
                SnailsGame.GameSettings.ShowSprites = !SnailsGame.GameSettings.ShowSprites;
                changed = true;
            }

			// Show / hide paths
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowHidePaths))
			{
                SnailsGame.GameSettings.ShowPaths = !SnailsGame.GameSettings.ShowPaths;
                changed = true;
            }

			// Show / hide quadtree
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowHideQuatree))
			{
                SnailsGame.GameSettings.ShowQuadtree = !SnailsGame.GameSettings.ShowQuadtree;
                changed = true;
            }

			// Show / hide quadtree
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowTiles))
			{
                SnailsGame.GameSettings.ShowTiles = !SnailsGame.GameSettings.ShowTiles;
                changed = true;
            }

			// Show / hide quadtree
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowBoundingBoxes))
			{
				SnailsGame.Settings.ShowBoundingBoxes = !SnailsGame.Settings.ShowBoundingBoxes;
                changed = true;
            }
#if DEBUG
			// Show / hide debug info
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowDebugInfo))
			{
				SnailsGame.DebugInfo.Visible = !SnailsGame.DebugInfo.Visible;
                changed = true;
            }

			// Enable / disable averages
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.EnableAverage))
			{
				SnailsGame.DebugInfo.ToggleAverage();
                changed = true;
            }

			// DebugInfo position
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.DebugInfoChangePosition))
			{
				SnailsGame.DebugInfo.TogglePosition();
                changed = true;
            }
#endif
			// Move to next stage
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.NextStage))
			{
				Levels.CurrentLevel.StartNextStage();
                changed = true;
            }

			// Move to previous stage
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.PrevStage))
			{
				Levels.CurrentLevel.StartPrevStage();
                changed = true;
            }


			// Move to previous stage
			if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ReloadStage))
			{
				//Levels.CurrentLevel.ReloadStage(); Not implemented
			}

            if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.GenerateAllThumbs))
            {
                this.GenerateAllThumbs();
            }
            if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.GenerateCurrentThumb))
            {
                this.GenerateThumb((int)Levels.CurrentTheme, Levels.CurrentStageNr);
            }

            if (changed)
            {
                SnailsGame.GameSettings.SaveToFile();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDraw()
        {
            this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, null, SnailsGame.DefaultCamera.Transform);

            BrainGame.DrawRectangleFilled(this._spriteBatch, this._Rect, this._BackColor);

            float x = 20.0f;
            float y = 20.0f;
            foreach (KeyHelpItem key in this._KeyList)
            {
                Vector2 position = new Vector2(x, y);
                if (SnailsGame.GameSettings.UseGamepad)
                {
          //          this._DebugSprite.Draw(this._Position + position, key._XboxControlFrameNr, this._spriteBatch);
                }
                else
                {
                    this._spriteBatch.DrawString(this._Font, string.Format("({0})", key._Key.ToString()),
                    this._Position + position + new Vector2(0.0f, 15.0f), this._Textcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
                this._spriteBatch.DrawString(this._Font, key._HelpText,
                    this._Position + position + new Vector2(55.0f, 15.0f), this._Textcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                y += 50.0f;

                if (y + 80.0f > this._Rect.Height)
                {
                    y = 20.0f;
                    x += 300.0f;
                }
            }
            this._spriteBatch.End();
        }

        /// <summary>
        /// Warning: this is a slow function that generates screenshots for all Stages in the game.
        /// It take up to 2min to completly run.
        /// </summary>
        private void GenerateThumb(int theme, int stageId)
        {
#if DEBUG && FORMS_SUPPORT
            string filename = string.Format("thumbs\\{0}_{1:00}.png", ((ThemeType)theme).ToString(), stageId);
            string filenameThumb = string.Format("thumbs\\{0}_{1:00}_thumb.png", ((ThemeType)theme).ToString(), stageId);

            Levels.CurrentLevel.LoadStage((ThemeType)theme, stageId);
            Levels.CurrentLevel.Stage.DrawToRenderTargetThumbs();
            Texture2D tex = Levels.CurrentLevel.Stage.RenderTarget as Texture2D;
            Stream stream = File.OpenWrite(filename);
            tex.SaveAsPng(stream, SnailsGame.ScreenWidth, SnailsGame.ScreenHeight);
            stream.Close();

            // create high quality thumbnail
            int newWidth = THUMBNAIL_WIDTH;
            int newHeight = THUMBNAIL_HEIGHT;

            System.Drawing.Image image = System.Drawing.Image.FromFile(filename);
            System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(newWidth, newHeight);

            System.Drawing.Graphics grfx = System.Drawing.Graphics.FromImage(newImage);
            grfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            grfx.DrawImage(image, new System.Drawing.Rectangle(0, 0, newWidth, newHeight), 0, 0, 1132, 720, System.Drawing.GraphicsUnit.Pixel);

            newImage.Save(filenameThumb, System.Drawing.Imaging.ImageFormat.Png);
#endif
        }

        /// <summary>
        /// Warning: this is a slow function that generates screenshots for all Stages in the game.
        /// It take up to 2min to completly run.
        /// </summary>
        private void GenerateAllThumbs()
        {
#if DEBUG && FORMS_SUPPORT
            int saveStageId = Levels.CurrentStageNr;
            ThemeType saveTheme = Levels.CurrentTheme;

            Directory.CreateDirectory("thumbs");
            /*
            for (int theme = 0; theme < 4; theme++)
            {
                for (int stageId = 1; stageId <= 21; stageId++)
                {
                    GenerateThumb(theme, stageId);
                }

                GenerateThumbsTexture(theme);
            }
            */
            foreach (ThemeSettings settings in Levels.ThemeSettings)
            {
                if (settings._visible == false)
                {
                    continue;
                }

                foreach (LevelStage stage in Levels._instance.StagesByTheme[(int)settings._themeType])
                {
                    GenerateThumb((int)settings._themeType, stage.StageNr);
                }

                GenerateThumbsTexture((int)settings._themeType);
            }
            // restore stage
            Levels.CurrentLevel.LoadStage(saveTheme, saveStageId);
#endif
        }

        private void GenerateThumbsTexture(int theme)
        {
#if DEBUG && FORMS_SUPPORT
            ThemeType themeType = (ThemeType)theme;
            Directory.CreateDirectory("thumbs\\" + ((ThemeType)theme).ToString());
            string filenameTexture = string.Format("..\\..\\..\\..\\SnailsResources\\Images\\{0}\\thumbnails.png", themeType.ToString());

            System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(1024, 1024);
            
            // Update spritesets
            XmlDataFileReader reader = new XmlDataFileReader();
            string ssDataFilename = "..\\..\\..\\..\\SnailsResourcesCustom\\spriteset\\" + themeType + "\\thumbnails.ss";
            DataFile ssDataFile = reader.Read(ssDataFilename);

            int x = 0;
            int y = 0;
            foreach (LevelStage stage in Levels._instance.StagesByTheme[theme])
            {
                string filenameThumb = string.Format("thumbs\\{0}_{1:00}_thumb.png", ((ThemeType)theme).ToString(), stage.StageNr);

                System.Drawing.Image image = System.Drawing.Image.FromFile(filenameThumb);

                System.Drawing.Graphics grfx = System.Drawing.Graphics.FromImage(newImage);
                grfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                grfx.DrawImage(image, new System.Drawing.Rectangle(x, y, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT),
                                    0, 0, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT, System.Drawing.GraphicsUnit.Pixel);

                if (ssDataFile != null)
                {
                    DataFileRecord stageRec = ssDataFile.RootRecord.SelectRecordByField("Sprite", "Id", stage.StageKey);
                    if (stageRec != null)
                    {
                        DataFileRecord framesRec = stageRec.SelectRecord("Frames");
                        if (framesRec == null)
                        {
                            framesRec = new DataFileRecord("Frames");
                            stageRec.AddRecord(framesRec);
                        }
                        DataFileRecord frameRec = framesRec.SelectRecord("Frame");
                        if (frameRec == null)
                        {
                            frameRec = new DataFileRecord("Frame");
                            framesRec.AddRecord(frameRec);
                        }

                        frameRec.AddField("Left", x, true);
                        frameRec.AddField("Top", y, true);
                        frameRec.AddField("Width", THUMBNAIL_WIDTH, true);
                        frameRec.AddField("Height", THUMBNAIL_HEIGHT, true);
                    }
                }
                x += 200;
                if (stage.StageNr % 5 == 0)
                {
                    x = 0;
                    y += 127;
                }
            }

            newImage.Save(filenameTexture, System.Drawing.Imaging.ImageFormat.Png);
            XmlDataFileWriter writer = new XmlDataFileWriter();
            writer.Write(ssDataFilename, ssDataFile);

#endif
        }

	}
}
