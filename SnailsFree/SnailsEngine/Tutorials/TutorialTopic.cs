using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Effects;
using System;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.BrainEngine.Collision;

namespace TwoBrainsGames.Snails.Tutorials
{
    public class TutorialTopic : Object2D, ISnailsDataFileSerializable
    {
        protected enum TopicState
        {
            Starting,
            Closed,
            Opening,
            Open,
            Closing,
            BeforeOpen
        }

        private const float LINE_SPACING = 22;
        private const float DOTS_DISTANCE = 30;
        private const float MINIMUM_DISPLAY_TIME = 2000;
        private const int TEXT_PLACEMENT_BB_IDX = 1;

        //private SpriteAnimation _spriteRedDot;
        //private Sprite _spriteWhiteDot;
        //private Sprite _spriteButton;

        public TextFont _font;
        private TopicState _state;
        private double _ellapsedShowDelayTime;
        private double _showDelay; // A timer can be used to delay the show of the tutorial topid
        private List<TutorialLine> _lines;
        private Rectangle _topicRect;
        private Tutorial _tutorialParent;
        private double _ellapsedDisplayTime;
        private double _displayTime;
        private ToolObjectType _pointingToolId;
        private TrembleEffect _trembleEffect;
        private HooverEffect _topicEffect;
        private SquashEffect _showEffect;
        private string _ballonSprite;
        public string _stageId; // The stage where the topic should be displayed
        private int _id;
        public Dictionary<LanguageCode, List<TutorialLine>> _stCodeLines; // st - snails tutorial code
        public bool _inQueue;
        public Vector2 _scale;

        private string _continueString;
        private Vector2 _clickToContinuePosition;
        private Vector2 _xboxButtonPosition;
        
        private string _posStr; // this is only used to read/save tutorial file (the actual used position is the Object2D position)

        public bool IsClosed { get { return this._state == TopicState.Closed; } }
        public bool IsOpen { get { return (this._state == TopicState.Opening || this._state == TopicState.Open); } }
        private string FontName { get; set; }
        private string PictureResource { get; set; } // Ilustração do tópico
        private Sprite PictureSprite { get; set; }
        public bool AlwaysUnlockedInHelp { get; private set; } // If true, the topic is always unlocked in the help option
        public BrainSettings.PlaformType Platforms { get; private set; }

        private bool IsStageCursorInside
        {
            get
            {
                Vector2 pos = Stage.CurrentStage.Input.MotionPosition;
                return (this._topicRect.Contains((int)pos.X, (int)pos.Y));
            }
        }

        public int TopicId
        {
            get
            {
               return this._id;
            }
        }

        public Vector2 Size
        {
            get { return new Vector2(this._topicRect.Width, this._topicRect.Height); }
        }

        // Topic number inside the topic list
        public int Number
        {
            get 
            {
                if (this._tutorialParent == null)
                {
                    return 0;
                }
                for (int i = 0; i < this._tutorialParent.Topics.Count; i++)
                {
                    if (this._tutorialParent.Topics[i] == this)
                    {
                        return i + 1;
                    }
                }
                return 0;
            }
        }

        public TutorialTopic(Tutorial tutorialParent)
        {
            this._tutorialParent = tutorialParent;
            this._state = TopicState.Closed;
            this._scale = new Vector2(1f, 1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public static TutorialTopic FromDataFileRecord(DataFileRecord record, Tutorial tutorialParent)
        {
            TutorialTopic topic = new TutorialTopic(tutorialParent);
            topic.InitFromDataFileRecord(record);
            return topic;
        }


        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {

            this._lines = new List<TutorialLine>();
            foreach (TutorialLine line in this._stCodeLines[BrainGame.CurrentLanguage])
            {
                line.ParseStCode();
                this._lines.Add(line);
            }

            this._displayTime = MINIMUM_DISPLAY_TIME;
            this._font = BrainGame.ResourceManager.Load<TextFont>(this.FontName, ResourceManager.ResourceManagerCacheType.Static);
            this.Sprite = BrainGame.ResourceManager.GetSprite(this._ballonSprite, ResourceManagerIds.TUTORIAL_RESOURCES);
           // this._spriteButton = BrainGame.ResourceManager.GetSprite("spriteset/Tutorial/XBoxButtons", ResourceManagerIds.TUTORIAL_RESOURCES);

            foreach (TutorialLine line in this._lines)
            {
                foreach (TutorialItem item in line.Items)
                {
                    item._parentTopic = this;
                    item.LoadContent();
                }
            }

            foreach (TutorialLine line in this._lines)
            {
                foreach (TutorialItem item in line.Items)
                {
                    item._parentTopic = this;
                }
            }
            this._trembleEffect = new TrembleEffect();
            this._topicEffect = new HooverEffect(0.015f, 0.1f, 0.0f);
            this._continueString = LanguageManager.GetString("MSG_CLOSE_TUTORIAL");

            // Picture
            if (this.PictureResource != null)
            {
                this.PictureSprite = BrainGame.ResourceManager.GetSprite(this.PictureResource, ResourceManagerIds.TUTORIAL_RESOURCES);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void UpdatePositions()
        {
            this._topicRect = new Rectangle((int)(this.Position.X + this.Sprite.BoundingBox.Left),
                                        (int)(this.Position.Y + this.Sprite.BoundingBox.Top),
                                        (int)this.Sprite.BoundingBox.Width,
                                        (int)this.Sprite.BoundingBox.Height);

            // Measure topic height
            float topicHeight = this.ComputeTopicHeight();

            // Precompute item positions
            Vector2 position = new Vector2(0f, (this.Sprite.BoundingBoxes[TEXT_PLACEMENT_BB_IDX].Height / 2) - (topicHeight / 2));

            foreach (TutorialLine line in this._lines)
            {
                float totalWidth = 0;
                foreach (TutorialItem item in line.Items)
                {
                    item.LoadContent();
                    item.Position = new Vector2((int)position.X, (int)position.Y);

                    position += new Vector2(item.GetWidth(), 0f);
                    this._displayTime += item._displayTime;
                }
                totalWidth = position.X;
                float offset = (this._topicRect.Width / 2) - (totalWidth / 2);
                // Center the items 
                foreach (TutorialItem item in line.Items)
                {
                    item.Position += new Vector2(offset, 0f);
                }
                totalWidth = position.X;
                position = new Vector2(0, position.Y + LINE_SPACING);

            }

            this._clickToContinuePosition = new Vector2((this._topicRect.Width / 2) - (this._font.MeasureString(this._continueString) / 2),
                                                 this._topicRect.Height - 5f) -
                                                 this.Sprite.Offset;
            this._xboxButtonPosition = new Vector2(15f, this._topicRect.Height - 20f) - this.Sprite.Offset;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Show()
        {
            this.LoadContent();
            // Center on the stage area if zero
            this.Position = new Vector2(BrainGame.ScreenWidth / 2, BrainGame.ScreenHeight / 2);
            this.UpdatePositions();
            
            this._state = TopicState.Starting;
            this._scale = new Vector2(1f, 1f);
            this._showEffect = new SquashEffect(0.85f, 4.0f, 0.02f, Color.White, new Vector2(1.0f, 1.0f));
            this._trembleEffect.Reset();
            this.EffectsBlender.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            // This is a martelada. We don't want the tutorial to be affected by the time warp
            // Just store the game time and restore it at the end
            TimeSpan storeEllapsed = gameTime.ElapsedGameTime;
            gameTime.ElapsedGameTime = gameTime.ElapsedRealTime;

            base.Update(gameTime);
            switch (this._state)
            {
                case TopicState.Starting:
                    this._ellapsedShowDelayTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                    if (this._ellapsedShowDelayTime > this._showDelay)
                    {
                        this._state = TopicState.BeforeOpen;
                        this.EffectsBlender.Add(this._showEffect);
                   //     this.EffectsBlender.Add(this._trembleEffect);
                    }
                    break;

                case TopicState.BeforeOpen:
                    Stage.CurrentStage.TutorialTopicOpened();
                    this._state = TopicState.Opening;
                    break;

                case TopicState.Opening:
                    this._scale = this._showEffect.Scale;
                    if (this._showEffect.Ended)
                    {
                        this._state = TopicState.Open;
                        this._scale = new Vector2(1f, 1f);
                        this.EffectsBlender.Clear();
                        if (this._topicEffect != null)
                        {
                            this.EffectsBlender.Add(this._topicEffect);
                        }
                    }
                    break;

                case TopicState.Open:
                    if (this._topicEffect != null)
                    {
                        this.Rotation = this._topicEffect.Rotation;
                        this.Position += this._topicEffect.PositionV2;
                    }
                  
                    if (SnailsGame.GameSettings.PauseInTutorial == false)
                    {
                        this._ellapsedDisplayTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                        if (this._ellapsedDisplayTime > this._displayTime)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        // Check for click or button that closes the topic
                        if (Stage.CurrentStage.Input.CloseTutorialSelected || 
                            (Stage.CurrentStage.Input.IsActionClicked &&
                             this.IsStageCursorInside))
                        {

                            Stage.CurrentStage.Input.Reset();
                            this.Close();
                        }
                    }
                    break;
            }

            gameTime.ElapsedGameTime = storeEllapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawTopicBallon(SpriteBatch spriteBatch, Color color)
        {
            this.Sprite.Draw(this.Position, 0, this.Rotation, Vector2.Zero, this._scale.X, this._scale.Y, color, spriteBatch);
        }

        /// <summary>
        /// Draws only the topic content
        /// </summary>
        public void DrawTopic(SpriteBatch spriteBatch, Color color)
        {
            this.DrawTopicBallon(spriteBatch, color);

            // The text
            foreach (TutorialLine line in this._lines)
            {
                foreach (TutorialItem item in line.Items)
                {
                    Vector2 pos = new Vector2((int)this.Position.X, (int)this.Position.Y);
                    item.Draw(pos + this.Sprite.BoundingBox.UpperLeft, color, spriteBatch);
                }
            }

            // The picture
            if (this.PictureSprite != null)
            {
                this.PictureSprite.Draw(this.Position + new Vector2(0f, 20f), 0, 0f, Vector2.Zero, this._scale.X, this._scale.Y, color, spriteBatch);
            }
        }

        /// <summary>
        /// Draws everything (content, continue message, dots, etc)
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this._state == TopicState.Open ||
                this._state == TopicState.Opening)
            {
               
                this.DrawTopic(spriteBatch, Color.White);
                if (SnailsGame.GameSettings.ShowCloseTutorialMessage)
                {
                    this._font.DrawString(spriteBatch, this._continueString, this._clickToContinuePosition + this.Position, this._scale, Color.Black);
                }
           /*     // Button help
                if (SnailsGame.GameSettings.ShowCloseTutorialShortcut && 
                    this._state == TopicState.Open)
                {
                    Vector2 pos = new Vector2((int)this.Position.X, (int)this.Position.Y);
                    this._spriteButton.Draw(pos + this._xboxButtonPosition, 0, spriteBatch);
                }*/
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private float ComputeTopicHeight()
        {
            return this._lines.Count * (LINE_SPACING + 1); // This isn't accurate but it will have to do...
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            this._state = TopicState.Closed;
            Stage.CurrentStage.TutorialTopicClosed();
        }
    
        // string format  x,y
        private Vector2 getPosition(string strPos)
        {
            Vector2 pos = Vector2.Zero;
            string [] platStr = strPos.Split(',');
            pos.X = int.Parse(platStr[0]);
            pos.Y = int.Parse(platStr[1]);
            return pos;
        }


        #region IDataFileSerializable
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            this._id = record.GetFieldValue<int>("id");
            this._showDelay = record.GetFieldValue<double>("showDelay");
            this._pointingToolId = (ToolObjectType)Enum.Parse(typeof(ToolObjectType), record.GetFieldValue<string>("pointingToolId", ToolObjectType.None.ToString()), true);
            this._stageId = record.GetFieldValue<string>("stageId");
            this._ballonSprite = record.GetFieldValue<string>("ballonSprite", "spriteset/Tutorial/BalloonMedium");
            this.FontName = record.GetFieldValue<string>("font", FontResources.TUTORIAL);
            this.AlwaysUnlockedInHelp = record.GetFieldValue<bool>("alwaysUnlockedInHelp", false);
            this.Platforms = (BrainSettings.PlaformType)Enum.Parse(typeof(BrainSettings.PlaformType), record.GetFieldValue<string>("platforms", BrainSettings.PlaformType.All.ToString()), false);
            // Text lines per language
            DataFileRecordList languagesRecords = record.SelectRecords("language");
            this._stCodeLines = new Dictionary<LanguageCode, List<TutorialLine>>();
            foreach (DataFileRecord languageRecord in languagesRecords)
            {
                LanguageCode language = (LanguageCode)Enum.Parse(typeof(LanguageCode), languageRecord.GetFieldValue<string>("id"), true);
                DataFileRecordList lineRecords = languageRecord.SelectRecords("line");
                this._stCodeLines.Add(language, new List<TutorialLine>());
                foreach (DataFileRecord lineRecord in lineRecords)
                {
                    this._stCodeLines[language].Add(TutorialLine.CreateFromDataFileRecord(lineRecord));
                }
            }

            this._posStr = record.GetFieldValue<string>("position", null);
            this.PictureResource = record.GetFieldValue<string>("picture", null);

        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("TutorialTopic");
            record.AddField("id", this._id);
            foreach (KeyValuePair<LanguageCode, List<TutorialLine>> language in this._stCodeLines)
            {
                DataFileRecord langRecord = new DataFileRecord("language");
                langRecord.AddField("id", language.Key.ToString());
                foreach (TutorialLine line in language.Value)
                {
                    langRecord.AddRecord(line.ToDataFileRecord());
                }
                record.AddRecord(langRecord);
            }
            record.AddField("showDelay", this._showDelay);
            record.AddField("pointingToolId", this._pointingToolId.ToString());
            record.AddField("position", this._posStr);
            record.AddField("stageId", this._stageId);
            record.AddField("ballonSprite", this._ballonSprite);
            record.AddField("font", this.FontName);
            record.AddField("alwaysUnlockedInHelp", this.AlwaysUnlockedInHelp);
            record.AddField("picture", this.PictureResource);
            record.AddField("platforms", this.Platforms.ToString());
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override  DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion
    }
}
