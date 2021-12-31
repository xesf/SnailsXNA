using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UITutorialTopic : UIControl
    {
        const float FADE_SPEED = 0.1f;

        TutorialTopic _topic;
        UILocker _locker;
        UICaption _capTopicCounter;

        bool Locked { get; set; }
       
        public TutorialTopic Topic 
        {
            get { return this._topic; }
            set
            {
                this._topic = value;
                if (this._topic != null)
                {
                    this._topic.LoadContent();
                    this._topic.Position = this.CenterInPixels;
                    this._topic.UpdatePositions();

                    // Check if the player has already viewed the topic
                    this.Locked = !SnailsGame.ProfilesManager.CurrentProfile.IsTutorialTopicRead(this._topic.TopicId);
                    // Some topics are always unlocked, unlock always if that's the case
                    if (this._topic.AlwaysUnlockedInHelp)
                    {
                        this.Locked = false;
                    }
                    Color blendColor = (this.Locked ? Color.Black : Color.White);

                    this.ShowEffect = new ColorEffect(new Color(0, 0, 0, 0), blendColor, FADE_SPEED, false);
                    this.HideEffect = new ColorEffect(blendColor, new Color(0, 0, 0, 0), FADE_SPEED, false);

                    this.Size = new BrainEngine.UI.Size(this.PixelsToScreenUnits(this._topic.Size));

                }
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public UITutorialTopic(UIScreen screenOwner) :
            base(screenOwner)
        {
            // Locker
            this._locker = new UILocker(screenOwner);
            this._locker.Position = this.NativeResolution(new Vector2(1600f, 1600f));
            this._locker.Visible = false;
            this.Controls.Add(this._locker);

            // Topic counter
            this._capTopicCounter = new UICaption(screenOwner, "", Color.Black, UICaption.CaptionStyle.Notebook);
            this._capTopicCounter.ParentAlignment = BrainEngine.UI.AlignModes.Bottom | BrainEngine.UI.AlignModes.Right;
            this._capTopicCounter.Margins.Right = this.NativeResolutionX(100f);
            this._capTopicCounter.Margins.Bottom = this.NativeResolutionY(150f);
            this._capTopicCounter.BlendColorWithParent = false;
            this.Controls.Add(this._capTopicCounter);


            this.OnShow += new UIEvent(UITutorialTopic_OnShow);
            this.OnHideBegin += new UIEvent(UITutorialTopic_OnHideBegin);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void BeginDraw()
        {
            base.BeginDraw();
            if (this.Topic != null)
            {
                if (!this.Locked)
                {
                    this.Topic.DrawTopic(this.SpriteBatch, this.BlendColor);
                }
                else
                {
                    this.Topic.DrawTopicBallon(this.SpriteBatch, this.BlendColor);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UITutorialTopic_OnShow(IUIControl sender)
        {
            this._locker.Visible = this.Locked;
            this._capTopicCounter.Visible = true;
            this._capTopicCounter.BlendColor = (this.Locked ? Color.White : Color.Black);
        }

        /// <summary>
        /// 
        /// </summary>
        void UITutorialTopic_OnHideBegin(IUIControl sender)
        {
            this._locker.Visible = false;
            this._capTopicCounter.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCounter(int topicNum, int totalTopics)
        {
            this._capTopicCounter.Text = string.Format("{0}/{1}", topicNum, totalTopics);
        }
    }
}
