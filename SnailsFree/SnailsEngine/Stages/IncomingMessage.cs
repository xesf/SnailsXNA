using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.Snails.Stages
{
    /// <summary>
    /// An IncomingMessage is a message that is displayed in game to the user
    /// This messages come from the left, stop at the middle of the screen and then go away to the right of the screen
    /// </summary>
    public class IncomingMessage
    {
        const float SPEED = 2f;
        const double BLINK_TIME = 300;
        enum MessagePositionType
        {
            Center,
            Bottom
        }
        enum MessageState
        {
            Inactive,
            Entering,
            Active,
            Leaving
        }

        public enum MessageType
        {
            ClickToStart,
            DeliveryStart,
            TimeAttackStart,
            MissionFailed,
            MissionCompleted,
            TimeIsUp,
            SnailKillerStart,
            SnailKingStart,
            InspectHints
        }

        string [] _textLines;
        Vector2 [] _linesOffsets;
        Vector2 _position;

        TextFont _font;
        TextFont _fontMedium;
        TextFont _fontBig;
        MessageState _state;
        Color _color;
        float _textStopPosition; // The position at the center of the screen where the text should stop
        double _pause;
        double _messagePause;
        BoundingSquare _stageArea;
        bool _visible;
        bool _withAnimation;
        double _blinkTime;
        //Sample _blinkSample;
        Sample _incStartSample;
        Sample _incEndSample;

        public Color BackgroundColor { get; set; }
        Rectangle BoundingRect { get; set; }

        public bool IsActive { get { return (this._state != MessageState.Inactive) && (!SnailsGame.Tutorial.TopicVisible); } }

        public IncomingMessage(BoundingSquare stageArea)
        {
            this._state = MessageState.Inactive;
            this._stageArea = stageArea;
            this._withAnimation = true;
            this.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this._fontMedium = BrainGame.ResourceManager.Load<TextFont>(FontResources.MAIN_FONT_MEDIUM, ResourceManager.ResourceManagerCacheType.Static);
            this._fontBig = BrainGame.ResourceManager.Load<TextFont>(FontResources.MAIN_FONT_BIG, ResourceManager.ResourceManagerCacheType.Static);

            _incStartSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.INCOMMING_START);
            _incEndSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.INCOMMING_END);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            switch (this._state)
            {
                case MessageState.Inactive:
                    break;

                case MessageState.Entering:
                    this._position += new Vector2(SPEED * (float)gameTime.ElapsedRealTime.TotalMilliseconds, 0f);
                    if (this._position.X >= this._textStopPosition)
                    {
                        this._position.X = this._textStopPosition;
                        this._state = MessageState.Active;
                        this._pause = 0;
                    }
                    break;
                case MessageState.Active:
                    if (this._withAnimation)
                    {
                        this._pause += gameTime.ElapsedRealTime.TotalMilliseconds;
                        if (this._pause > this._messagePause)
                        {
                            this._state = MessageState.Leaving;
                            this._visible = true;
                            this._incEndSample.Play();
                        }
                    }
                    else
                    {
                        this._blinkTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                        if (this._blinkTime > (BLINK_TIME * (this._visible? 5 : 1)))
                        {
                            this._blinkTime = 0;
                            this._visible = !this._visible;
                            if (this._visible)
                            {
                        //        this._blinkSample.Play(); // Barulho a mais, comentar para já
                            }
                        }
                    }
                    break;
                case MessageState.Leaving:
                    this._position += new Vector2(SPEED * (float)gameTime.ElapsedRealTime.TotalMilliseconds, 0f);
                    if (this._position.X >= BrainGame.ScreenWidth)
                    {
                        this._state = MessageState.Inactive;
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this._visible)
            {
                if (this.BackgroundColor != Color.Transparent)
                {
                    spriteBatch.Draw(UIScreen.ClearTexture, this.BoundingRect, this.BackgroundColor);
                }

                for (int i = 0; i < this._textLines.Length; i++)
                {
                    this._font.DrawString(spriteBatch, this._textLines[i], this._position + this._linesOffsets[i], new Vector2(1f, 1f), this._color);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Show(string text, Color color, bool withAnimation, TextFont font, MessagePositionType positionType)
        {
            this.Show(text, color, withAnimation, font, positionType, Color.Transparent);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Show(string text, Color color, bool withAnimation, TextFont font, MessagePositionType positionType, Color backColor)
        {
            this.BackgroundColor = backColor;
            this._font = font;
            this._textLines = text.Split(UILabel.LINE_SEPARATOR);
            float totalHeight = 0f;
            float maxWidth = 0f;
            float minX = 99999f;
            this._linesOffsets = new Vector2[this._textLines.Length];
            for (int i = 0; i < this._textLines.Length; i++)
            {
                float width = this._font.MeasureString(this._textLines[i], new Vector2(1f, 1f));
                float height = this._font.MeasureStringHeight(this._textLines[i], new Vector2(1f, 1f));
                if (width > maxWidth)
                {
                    maxWidth = width;
                }
                this._linesOffsets[i] = new Vector2(-(width / 2f), totalHeight);
                if (this._linesOffsets[i].X < minX)
                {
                    minX = this._linesOffsets[i].X;
                }
                totalHeight += height;
            }

            switch (positionType)
            {
                case MessagePositionType.Center:
                    this._position = new Vector2(this._stageArea.Left - (maxWidth * 2) + BrainGame.ScreenRectangle.X,
                                                 this._stageArea.Top + (this._stageArea.Height / 2) - totalHeight); // * 2 makes the label *really* off screen
                    break;

                case MessagePositionType.Bottom:
                    this._position = new Vector2(this._stageArea.Left - (maxWidth * 2) + BrainGame.ScreenRectangle.X,
                                                         this._stageArea.Bottom - totalHeight); // * 2 makes the label *really* off screen
                    break;
            }

            this._state = MessageState.Entering;
            this._color = color;
            this._textStopPosition = this._stageArea.Left + (this._stageArea.Width / 2) + BrainGame.ScreenRectangle.X;
            this._messagePause = 1500;
            this._visible = true;
            this._withAnimation = withAnimation;
            this._blinkTime = 0;

            if (!this._withAnimation)
            {
                this._state = MessageState.Active;
                this._position = new Vector2(this._textStopPosition, this._position.Y);
            }
            else
            {
                _incStartSample.Play();
            }
            this.BoundingRect = new Rectangle((int)(this._position.X + minX) - 10, (int)this._position.Y, (int)maxWidth + 20, (int)totalHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Hide()
        {
            this._visible = false;
            this._state = MessageState.Inactive;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Show(MessageType message)
        {
            string msg;
            switch (message)
            {
                case MessageType.ClickToStart:
                    this.Show(LanguageManager.GetString("INCOMING_MSG_CLICK_TO_START"), Color.Orange, false, this._fontMedium, MessagePositionType.Bottom, new Color(0, 0, 0, 150));
                    break;
                case MessageType.DeliveryStart:
                    msg = Formater.FormatGoalDescription(Stage.CurrentStage.LevelStage, true);
                    this.Show(msg, Colors.IncomingMessageInfo, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.TimeAttackStart:
                    msg = Formater.FormatGoalDescription(Stage.CurrentStage.LevelStage, true);
                    this.Show(msg, Colors.IncomingMessageInfo, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.MissionFailed:
                    this.Show(LanguageManager.GetString("INCOMING_MSG_MISSION_FAILED"), Colors.IncomingMessageError, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.MissionCompleted:
                    this.Show(LanguageManager.GetString("INCOMING_MSG_STAGE_COMPL"), Colors.IncomingMessageInfo, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.TimeIsUp:
                    this.Show(LanguageManager.GetString("INCOMING_MSG_TIME_UP"), Colors.IncomingMessageError, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.SnailKillerStart:
                    msg = Formater.FormatGoalDescription(Stage.CurrentStage.LevelStage, true);
                    this.Show(msg, Colors.IncomingMessageInfo, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.SnailKingStart:
                    msg = Formater.FormatGoalDescription(Stage.CurrentStage.LevelStage, true);
                    this.Show(msg, Colors.IncomingMessageInfo, true, this._fontBig, MessagePositionType.Center);
                    break;
                case MessageType.InspectHints:
                    this.Show(LanguageManager.GetString("INCOMING_MSG_INSPECT_HINT"), Colors.IncomingMessageHints, false, this._fontMedium, MessagePositionType.Bottom, new Color(0, 0, 0, 150));
                    break;

            }
        }
    }
}
