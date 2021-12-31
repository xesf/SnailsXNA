using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Debugging;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.BrainEngine.Debugging
{
    public class DebugInfo : DrawableGameComponent
    {
        public enum DockType
        {
            UpperLeft,
            UpperRight,
            LowerRight,
            LowerLeft,
        }

        #region Constants
        const int MARGIN = 10;
        const int WINDOW_WIDTH = 240;
        const int WINDOW_HEIGHT = 60;

        public const float FPSAlarmThreshold = 60;
        public const float DrawTimeAlarmThreshold = 10;
        public const float UpdateTimeAlarmThreshold = 12;
        public const float CollisionTestsAlarmThreshold = 500;
        #endregion

        #region Members/Properties
        public Vector2 Position;
        public BrainPerformanceCounter SnailCounter;
        public BrainPerformanceCounter ObjectsCounter;
        public BrainPerformanceCounter CollisionCounter;

        public DockType DockStyle
        {
            get { return this._DockStyle; }
            set
            {
                this._DockStyle = value;
                this.DockStyleChanged = true;
            }
        }

        private DockType _DockStyle;
        private SpriteFont Font;
        private SpriteBatch SpriteBatch;
        // Counters
        private FPSCounter FPSCounter;
        private MemoryPeakCounter MemoryPeakCounter;
        private MemoryUsageCounter MemoryUsageCounter;
        private TimerCounter DrawPerformanceTimer;
        private TimerCounter UpdatePerformanceTimer;
        private Color Textcolor;
        //private Color BackColor;
		//private Color Opacity;
        private bool DockStyleChanged;
        private bool Averaging;
        private Rectangle Rect;
        private List<string> _debugMessages;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public DebugInfo(Game game, Vector2 position) :
            base(game)
        {
            this.Position = position;
            this.Textcolor = Color.Yellow;
            this.DockStyle = DockType.UpperLeft;
            this.DockStyleChanged = true;
            this.Visible = true;
            this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, BrainGame.ScreenWidth, DebugInfo.WINDOW_HEIGHT);
            this._debugMessages = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            this.Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/debug", ResourceManager.ResourceManagerCacheType.Static);

            // Fps counter
            this.FPSCounter = new FPSCounter(this.Game);
            //this.FPSCounter.SetAlarmThreshold(DebugInfo.FPSAlarmThreshold,
              //                                BrainPerformanceCounter.AlertConditions.Smaller);

            this.MemoryPeakCounter = new MemoryPeakCounter(this.Game);
            this.MemoryPeakCounter.SetAlarmThreshold(90 * 1024 * 1024, BrainPerformanceCounter.AlertConditions.Greater);
            
            this.MemoryUsageCounter = new MemoryUsageCounter(this.Game);
            this.MemoryUsageCounter.SetAlarmThreshold(90 * 1024 * 1024, BrainPerformanceCounter.AlertConditions.Greater);

            // Draw performance counter
            this.DrawPerformanceTimer = new TimerCounter(this.Game);
            this.DrawPerformanceTimer.SetAlarmThreshold(DebugInfo.DrawTimeAlarmThreshold,
                                                        BrainPerformanceCounter.AlertConditions.Greater);

            // Update performance counter
            this.UpdatePerformanceTimer = new TimerCounter(this.Game);
            this.UpdatePerformanceTimer.SetAlarmThreshold(DebugInfo.UpdateTimeAlarmThreshold,
                                                          BrainPerformanceCounter.AlertConditions.Greater);
            // Collision counter
            this.CollisionCounter = new BrainPerformanceCounter(this.Game);
            this.CollisionCounter.SetAlarmThreshold(DebugInfo.CollisionTestsAlarmThreshold,
                                                    BrainPerformanceCounter.AlertConditions.Greater);
            this.SnailCounter = new BrainPerformanceCounter(this.Game);
            this.ObjectsCounter = new BrainPerformanceCounter(this.Game);

            this.SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //this.Opacity = new Color(255, 255, 255, 170);

            this.Game.Components.Add(new DebugItem(this.Game, this, "FPSCounter", new Vector2(10, 5), this.Font, this.Textcolor, this.FPSCounter, "{0}"));
            this.Game.Components.Add(new DebugItem(this.Game, this, "MemoryPeak", new Vector2(10, 20), this.Font, this.Textcolor, this.MemoryPeakCounter, "{0:###,##0} Kb"));
            this.Game.Components.Add(new DebugItem(this.Game, this, "MemoryUsage", new Vector2(10, 35), this.Font, this.Textcolor, this.MemoryUsageCounter, "{0:###,##0} Kb"));
            // this.Game.Components.Add(new DebugItem(this.Game, this, "Update", new Vector2(10, 20), this.Font, this.Textcolor, this.UpdatePerformanceTimer));
           // this.Game.Components.Add(new DebugItem(this.Game, this, "Draw", new Vector2(10, 35), this.Font, this.Textcolor, this.DrawPerformanceTimer));
        }

        /// <summary>
        /// 
        /// </summary>
        public void TogglePosition()
        {

            int i = (int)this.DockStyle;
            i++;
            if (i > 3)
                i = 0;
            this.DockStyle = (DockType)i;
        }
        /// <summary>
        /// 
        /// </summary>
        public void ToggleAverage()
        {
            if (this.Averaging == false)
            {
                this.FPSCounter.BeginAverage();
                this.UpdatePerformanceTimer.BeginAverage();
                this.DrawPerformanceTimer.BeginAverage();
                this.CollisionCounter.BeginAverage();
                this.SnailCounter.BeginAverage();
                this.ObjectsCounter.BeginAverage();
                this.Averaging = true;
            }
            else
            {
                this.FPSCounter.EndAverage();
                this.UpdatePerformanceTimer.EndAverage();
                this.DrawPerformanceTimer.EndAverage();
                this.CollisionCounter.EndAverage();
                this.SnailCounter.EndAverage();
                this.ObjectsCounter.EndAverage();
                this.Averaging = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (this.DockStyleChanged)
            {
                this.Reposition();
                this.DockStyleChanged = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (this.Visible == false)
                return;
            this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, 100, DebugInfo.WINDOW_HEIGHT + (this._debugMessages.Count * 15));

            this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, BrainGame.RenderEffect);
            BrainGame.DrawRectangleFilled(this.SpriteBatch, this.Rect, new Color(0, 0, 0, 150));

            float y = this.Rect.Top + 45;
            foreach (string msg in this._debugMessages)
            {
                this.SpriteBatch.DrawString(this.Font, msg, new Vector2(10f, y), Color.White);
                y += 15f;
            }

            this.SpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateStarted()
        {
            this.UpdatePerformanceTimer.Reset();
            this.CollisionCounter.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateEnded()
        {
            this.UpdatePerformanceTimer.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawStarted()
        {
            this.DrawPerformanceTimer.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawEnded()
        {
            this.DrawPerformanceTimer.End();
        }


        /// <summary>
        /// 
        /// </summary>
        private void Reposition()
        {
            int frameWidth = this.Rect.Width;
            int frameHeight = this.Rect.Height;

            switch (this.DockStyle)
            {
                case DockType.UpperLeft:
                    this.Position = new Vector2(DebugInfo.MARGIN, DebugInfo.MARGIN);
                    break;

                case DockType.UpperRight:
                    this.Position = new Vector2(BrainGame.ScreenWidth - frameWidth - DebugInfo.MARGIN, DebugInfo.MARGIN);
                    break;

                case DockType.LowerRight:
                    this.Position = new Vector2(BrainGame.ScreenWidth - frameWidth - DebugInfo.MARGIN,
                                                BrainGame.ScreenHeight - frameHeight - DebugInfo.MARGIN);
                    break;

                case DockType.LowerLeft:
                    this.Position = new Vector2(DebugInfo.MARGIN,
                                                BrainGame.ScreenHeight - frameHeight - DebugInfo.MARGIN);
                    break;
            }
            this.Rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Rect.Width, this.Rect.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearMessages()
        {
            this._debugMessages.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddMessage(string message)
        {
            this._debugMessages.Add(message);
        }
    }
}
