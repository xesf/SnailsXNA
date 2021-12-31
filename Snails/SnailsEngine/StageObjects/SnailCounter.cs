using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageObjects
{
    public partial class SnailCounter : StageObject
    {
        #region Constants
        public const string FONT_SNAIL_COUNTER = "SNAIL_COUNTER_FONT";

        const int FRAME_IDX_STAGE_EXIT = 0;
        const int FRAME_IDX_RELEASE_RIGHT = 1;
        const int FRAME_IDX_RELEASE_LEFT = 2;
        const int FRAME_IDX_RELEASE_BOTH = 3;
        #endregion

        #region Members
        private int _counter;
        private TextFont _font;
        private Rectangle _textRect;
        #endregion

        public SnailCounter()
            : base(StageObjectType.SnailCounter)
        {
            this._counter = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/snail-counter-" + Levels.CurrentTheme.ToString(), ResourceManager.ResourceManagerCacheType.Static);
        }
         
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this._font = ((SnailCounter)other)._font;
        }

        public override void Initialize()
        {
            base.Initialize();
            this._textRect = this.TransformCurrentFrameBB().ToRect();
        }

        public override void Draw(bool shadow)
        {
            base.Draw(shadow);

            this._font.DrawString(Stage.CurrentStage.SpriteBatch, this._counter.ToString(), this._textRect, TextFont.TextHorizontalAlign.Center);
        }

        public void SetCounter(int value)
        {
            this._counter = value;
        }


        /// <summary>
        /// 
        /// </summary>
        public void RemoveLinks()
        {
            if (this.LinkedObjects.Count == 0)
                return;

            foreach (StageObject obj in this.LinkedObjects)
            {
                if (obj is StageEntrance)
                {
                    ((StageEntrance)obj).SnailCounter = null;
                }
                else
                if (obj is StageExit)
                {
                    ((StageExit)obj).SnailCounter = null;
                }
            }

            this.LinkString = "";
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConnectToEntrance(StageEntrance entrance)
        {
            switch (entrance.ReleaseDirection)
            {
                case StageEntrance.EntranceReleaseDirection.Clockwise:
                    this.CurrentFrame = FRAME_IDX_RELEASE_RIGHT;
                    break;
                case StageEntrance.EntranceReleaseDirection.CounterClockwise:
                    this.CurrentFrame = FRAME_IDX_RELEASE_LEFT;
                    break;
                case StageEntrance.EntranceReleaseDirection.Both:
                    this.CurrentFrame = FRAME_IDX_RELEASE_BOTH;
                    break;
            }
        }
    }
}
