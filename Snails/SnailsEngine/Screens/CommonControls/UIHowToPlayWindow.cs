using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Tutorials;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Input;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIHowToPlayWindow : UISnailsWindow
    {
        UITutorialTopic _topicControl;
        UISnailsButton _btnNext;
        UISnailsButton _btnPrev;
        UIPanel _topicContainer;
        UIImage _infoSignImg;
        public List<TutorialTopic> _topics;

        public bool UseCloseHotKey
        {
            set { this.CloseButton.UseHotKey = value; }
        }

        int CurrentTutorialTopicIdx { get; set; }
        int NextTopicToShowIdx { get; set; }
        public List<TutorialTopic> Topics  
        {
            get
            {
                return this._topics;
            }
            set
            {
                // Sort the topics (locked topics at the end)
                this._topics = new List<TutorialTopic>();
                int lastUnlocked = 0;
                foreach (TutorialTopic topic in value)
                {
                    if (SnailsGame.ProfilesManager.CurrentProfile.IsTutorialTopicRead(topic.TopicId) ||
                        topic.AlwaysUnlockedInHelp)
                    {
                        this._topics.Insert(lastUnlocked, topic);
                        lastUnlocked++;
                    }
                    else
                    {
                        this._topics.Insert(this._topics.Count, topic);
                    }
                }
            }

        }

        bool AllButtonsHidden
        {
            get
            {
                return (!this._btnNext.Visible && !this._btnPrev.Visible && !this.CloseButton.Visible);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public UIHowToPlayWindow(UIScreen screenOwner) :
            base(screenOwner)
        {

            // Container
            this._topicContainer = new UIPanel(screenOwner);
            this._topicContainer.ParentAlignment = AlignModes.Horizontaly;
            this._topicContainer.Position = this.NativeResolution(new Vector2(0f, 200f));
            this._topicContainer.Size = this.NativeResolution(new Size(3500f, 4000f));
            this.Board.Controls.Add(this._topicContainer);

            // Topic
            this._topicControl = new UITutorialTopic(screenOwner);
            this._topicControl.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this._topicControl.Visible = false;
            this._topicControl.OnHide += new UIEvent(_topicControl_OnHide);
            this._topicControl.OnShow += new UIEvent(_topicControl_OnShow);
            this._topicContainer.Controls.Add(this._topicControl);

            // Next button
            this._btnNext = new UISnailsButton(screenOwner, "BTN_NEXT", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Next, null, false);
            this._btnNext.ParentAlignment = AlignModes.Bottom;
            this._btnNext.Position = this.NativeResolution(new Vector2(2600f, 0f));
            this._btnNext.OnClickBegin += new UIEvent(_btnNext_OnClickBegin);
            this._btnNext.ButtonAction = UISnailsButton.ButtonActionType.Next;
            this.Board.Controls.Add(this._btnNext);

            // Previous button
            this._btnPrev = new UISnailsButton(screenOwner, "BTN_PREV", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Prev, null, false);
            this._btnPrev.ParentAlignment = AlignModes.Bottom;
            this._btnPrev.Position = this.NativeResolution(new Vector2(0f, 0f));
            this._btnPrev.OnClickBegin += new UIEvent(_btnPrev_OnClickBegin);
            this._btnPrev.ButtonAction = UISnailsButton.ButtonActionType.Previous;
            this.Board.Controls.Add(this._btnPrev);

            // Info sign
            this._infoSignImg = new UIImage(screenOwner);//, "spriteset/tutorial_with_images/InfoSign", ResourceManagerIds.TUTORIAL_RESOURCES);
            //this._infoSignImg.Visible = false;
            this._infoSignImg.Position = new Vector2(400f, 300f);
            this.Board.Controls.Add(this._infoSignImg);

            this.OnDismissBegin += new UIEvent(Window_OnDesmissBegin);
            this.TitleResourceId = "TITLE_HOW_TO_PLAY";
            this.BoardType = UISnailsBoard.BoardType.LightWoodMedium;
            this.OnShow += new UIEvent(UIHowToPlayWindow_OnShow);
            this.OnShowBegin += new UIEvent(UIHowToPlayWindow_OnShowBegin);
            this.ContinueButton.Visible = !SnailsGame.GameSettings.UseButtonIcons;
            this.OnScreenStart += new UIEvent(UIHowToPlayWindow_OnScreenStart);
        }

        /// <summary>
        /// 
        /// </summary>
        void UIHowToPlayWindow_OnScreenStart(IUIControl sender)
        {
            // Only load the sprite here. No need to take up memory if we are not wathcing the tutorial, and we probably aren't going to 
            this._infoSignImg.Sprite = BrainGame.ResourceManager.GetSprite("spriteset/tutorial_with_images/InfoSign", ResourceManagerIds.TUTORIAL_RESOURCES);     
        }
            

        /// <summary>
        /// 
        /// </summary>
        void UIHowToPlayWindow_OnShowBegin(IUIControl sender)
        {
            this.CurrentTutorialTopicIdx = 0;
            this.EnableButtons();
            this.CloseButton.Visible = false;
            if (!this.DismissButtonVisible && this.Topics.Count <= 1)
            {
                if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.HD)
                {
                    this.Position = new Vector2(0f, 3000f);
                }
                else
                {
                    this.Position = new Vector2(0f, 1800f);
                }
            }
            else
            {
                if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.HD)
                {
                    this.Position = new Vector2(0f, 2600f);
                }
                else
                {
                    this.Position = new Vector2(0f, 1200f);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnNext_OnClickBegin(IUIControl sender)
        {
            ((SnailsScreen)this.ScreenOwner).DisableInput();
            this.HideTopic();
            this.NextTopicToShowIdx = this.CurrentTutorialTopicIdx + 1;

            if (this.NextTopicToShowIdx + 1 > this.Topics.Count)
            {
                this.NextTopicToShowIdx = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Window_OnDesmissBegin(IUIControl sender)
        {
            this.CloseButton.Visible = false;
            ((SnailsScreen)this.ScreenOwner).DisableInput();
            this.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        void UIHowToPlayWindow_OnShow(IUIControl sender)
        {
      //      this._infoSignImg.Visible = true;
            this.ShowTopic(0);
            this.CloseButton.Visible = SnailsGame.GameSettings.UseButtonIcons;
        }

        /// <summary>
        /// 
        /// </summary>
        void _btnPrev_OnClickBegin(IUIControl sender)
        {
            ((SnailsScreen)this.ScreenOwner).DisableInput();
            this.HideTopic();
            this.NextTopicToShowIdx = this.CurrentTutorialTopicIdx - 1;
            if (this.NextTopicToShowIdx < 0)
            {
                this.NextTopicToShowIdx = this.Topics.Count - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideTopic()
        {
            this._topicControl.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowTopic(int topicIdx)
        {
            this._topicControl.Topic = this.Topics[topicIdx];
            this._topicControl.Show();
            this._topicControl.SetCounter(topicIdx + 1, this.Topics.Count);
            this.CurrentTutorialTopicIdx = topicIdx;
            this.EnableButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        void _topicControl_OnShow(IUIControl sender)
        {
            ((SnailsScreen)this.ScreenOwner).EnableInput();
        }

        /// <summary>
        /// 
        /// </summary>
        void _topicControl_OnHide(IUIControl sender)
        {
            if (this.NextTopicToShowIdx != -1)
            {
                this.ShowTopic(this.NextTopicToShowIdx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            this.NextTopicToShowIdx = -1; // If this is not set, the topic will be visible next time the window is opened
            this.HideTopic();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._btnNext.Visible = (this.CurrentTutorialTopicIdx < this.Topics.Count - 1);
            this._btnPrev.Visible = (this.CurrentTutorialTopicIdx > 0);
        }
    }
}
