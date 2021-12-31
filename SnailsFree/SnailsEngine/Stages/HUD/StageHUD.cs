using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Screens;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    public class StageHUD : IBrainComponent
    {
        #region Consts
        // Max speed when timer is speed up
        public const int XBOX_HELP_TOOL_SEL_FRAME_NR = 0;
        public const int XBOX_HELP_TIME_WARP_FRAME_NR = 1;
        public const int XBOX_HELP_CLOSE_TUT_FRAME_NR = 2;
        public const int XBOX_HELP_END_MISSION_FRAME_NR = 3;
        public const int WIN_HELP_TIME_WARP_FRAME_NR = 4;
        #endregion

        #region Members
        List<HUDItem> _infoItems;
        HUDMinimap _minimap;

        //private TextFont _font;
        private IncomingMessage _incomingMessage;
        private Sprite _spriteXBoxHelp;
        private HUDItemTimer _itemTimer;
        private HUDItemControls _itemControls;
        private FastForwardStrips _ffStrips;
        public ToolsMenu _toolsMenu;
        public BoundingSquare _stageArea;
        public bool IsInteractingWithCursor { get; set; }
        public bool ControlButtonsVisible 
        { 
            get { return this._itemControls._visible; }
            set { this._itemControls._visible = value; }
        }
        public Vector2 HudCenter;
       

        #endregion

        #region Properties

        public static Sprite SpriteXBoxHelp
        {
            get { return Stage.CurrentStage.StageHUD._spriteXBoxHelp; }
        }
        /// <summary>
        /// Cursor Selected Tool
        /// </summary>
        public ToolObject Tool
        {
            get { return this._toolsMenu.GetSelectedTool(); }
        }

        public SpriteBatch SpriteBatch { get { throw new SnailsException("Deprecated. Draw now recieves the SpriteBatch."); } } // Not needed. SpriteBatch is passed to the drar method

       
        #endregion

        public StageHUD()
        {
            this._toolsMenu = new ToolsMenu();
            this._infoItems = new List<HUDItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            // Add information items to the HUD
            this._infoItems.Clear();
            // Snails delivered icon
            if (Stage.CurrentStage.LevelStage._goal == GoalType.SnailDelivery ||
                Stage.CurrentStage.LevelStage._goal == GoalType.TimeAttack)
            {
                this._infoItems.Add(new HUDItemSnailsDelivered());
            }
            else
            if (Stage.CurrentStage.LevelStage._goal == GoalType.SnailKiller)
            {
                this._infoItems.Add(new HUDItemSnailsCounter());
            }
            // Timer
            this._itemTimer = new HUDItemTimer();
            this._infoItems.Add(this._itemTimer);

            // Goal icon
            this._infoItems.Add(new HUDItemGoal());
            // Mission status
            this._infoItems.Add(new HUDItemMissionStatus());
      
            // Warp button
          //  this._infoItems.Add(new HUDItemWarpButton());

            // Initialize the items and set their position
			Vector2 position = new Vector2(10f + SnailsGame.ScreenRectangle.X, 10f + SnailsGame.ScreenRectangle.Y);
            foreach (HUDItem item in this._infoItems)
            {
                item.Initialize(position);
                if (item._autoPosition)
                {
                    position += new Vector2(item._width, 0f);
                }
            }

            // Controls (play,pause, restart, etc)
            this._itemControls = new HUDItemControls();
            this._itemControls.Initialize(Vector2.Zero);
            this._itemControls._visible = false;

            if (SnailsGame.GameSettings.MinimapVisible)
            {
                _minimap = new HUDMinimap();
                _minimap.Initialize(new Vector2(5, 614));
            }

            this._ffStrips = new FastForwardStrips();
            this._ffStrips.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this._spriteXBoxHelp = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ControllsHelp");

            foreach (HUDItem item in this._infoItems)
            {
                item.LoadContent();
            }

            this._toolsMenu.LoadContent();

            // Stage area depends on the tools width so this will have to be set here
            _stageArea = new BoundingSquare(new Vector2(0.0f), this._toolsMenu._ToolsArea.Left, SnailsGame.ScreenHeight - SnailsScreen.BottomMarginInPixels);

            if (SnailsGame.GameSettings.MinimapVisible)
            {
                _minimap.SetCameraProjections();
            }

            this._incomingMessage = new IncomingMessage(this._stageArea);
            this._incomingMessage.LoadContent();

            this._itemControls.LoadContent();
            this._ffStrips.LoadContent();

            this._itemControls.StageAreaChanged(this._stageArea);

            this.HudCenter = new Vector2((int)(this._stageArea.Left + (this._stageArea.Width / 2)),
                                         (int)(this._stageArea.Top + (this._stageArea.Height / 2)));


        }   


        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            foreach (HUDItem item in this._infoItems)
            {
                item.HandleInput(gameTime);
                item.Update(gameTime);
            }

            this._toolsMenu.Update(gameTime);

            this.UpdateIncomingMessage(gameTime);

            if (SnailsGame.GameSettings.MinimapVisible)
            {
                _minimap.Update(gameTime);
            }

            if (this._itemControls._visible)
            {
                this._itemControls.Update(gameTime);
            }

            this._ffStrips.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateIncomingMessage(BrainGameTime gameTime)
        {
      
            if (this._incomingMessage.IsActive)
            {
                this._incomingMessage.Update(gameTime);
            } 
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            throw new SnailsException("Deprecated. Use Draw(spriteBatch) instead.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Incoming messages
            if (this._incomingMessage.IsActive)
            {
                this._incomingMessage.Draw(spriteBatch);
            }

            // Tools menu
            this._toolsMenu.Draw();

            foreach (HUDItem item in this._infoItems)
            {
                item.Draw(spriteBatch);
            }

            if (SnailsGame.GameSettings.MinimapVisible)
            {
                _minimap.Draw(spriteBatch);
            }

            if (this._itemControls._visible)
            {
                this._itemControls.Draw(spriteBatch);
            }

            // Draw the tutorial
            SnailsGame.Tutorial.Draw(spriteBatch);

            this._ffStrips.Draw(spriteBatch);

#if DEBUG
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._stageArea.Draw(Color.Red, Vector2.Zero);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopTimer()
        {
            this._itemTimer.StopTimer();
        }

     

        /// <summary>
        /// 
        /// </summary>
        public void SnailsStageStatsChanged()
        {
            foreach (HUDItem item in this._infoItems)
            {
                item.SnailsStageStatsChanged();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void UnloadContent()
        { }


        /// <summary>
        /// This is called by Stage when the mission status changes
        /// </summary>
        public void MissionStateChanged()
        {
            foreach (HUDItem item in this._infoItems)
            {
                item.MissionStateChanged();
            }
            this._itemControls.MissionStateChanged();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void ShowIncomingMessage(IncomingMessage.MessageType message)
        {
            this._incomingMessage.Show(message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideIncomingMessage()
        {
            if (this._incomingMessage.IsActive)
            {
                this._incomingMessage.Hide();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnStageStarted()
        {
            this._incomingMessage.Hide();
            this._itemControls._visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void TimeWarpChanged()
        {
            foreach (HUDItem item in this._infoItems)
            {
                item.TimeWarpChanged();
            }
        }

    }
}
