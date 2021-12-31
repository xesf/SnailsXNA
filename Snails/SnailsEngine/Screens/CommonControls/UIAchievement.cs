using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIAchievement : UIPanel
    {
        const int MAX_CHARS_PER_LINE = 38; 

        BrainAchievement _achievement;
        UIImage _image;
        UICaption _capDescriptionLine1;
        UICaption _capDescriptionLine2;
        UICaption _capCompletion;
#if DEBUG
        UISnailsButton _btnToggleWon;
#endif

        Color CaptionColorWon { get; set; }
        Color CaptionColorNotWon { get; set; }

        public float CompletionPercentage { get; set; }

#if DEBUG
        public bool AllowToggle
        {
            get { return this._btnToggleWon.Visible; }
            set 
            { 
                this._btnToggleWon.Visible = value;
                this.UpdateSize();
            }
        }
#else
        public bool AllowToggle
        {
           get { return false; }
        }
#endif

        public BrainAchievement Achievement
        {
            get { return this._achievement; }
            set
            {
                this._achievement = value;
                this.Refresh();
            }
        }

        public UIAchievement(UIScreen screenOwner, BrainAchievement achievement) :
            base(screenOwner)
        {
            this.CaptionColorWon = Color.LightBlue;
            this.CaptionColorNotWon = Color.Gray;

            // Image
            this._image = new UIImage(screenOwner);
            this._image.Position = new Vector2(50f, 0f);
            if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD)
            {
                this._image.Scale = new Vector2(0.85f, 0.85f);
            }
            this.Controls.Add(this._image);

            // Description - line 1
            this._capDescriptionLine1 = new UICaption(screenOwner, "", Color.White, UICaption.CaptionStyle.AwardCaption);
            this._capDescriptionLine1.Position = new Vector2(650f, 200f);
            this.Controls.Add(this._capDescriptionLine1);

            // Description - line 2
            this._capDescriptionLine2 = new UICaption(screenOwner, "", Color.White, UICaption.CaptionStyle.AwardCaption);
            this._capDescriptionLine2.Position = new Vector2(650f, 500f);
            this.Controls.Add(this._capDescriptionLine2);

            // CompletionPercentage
            this._capCompletion = new UICaption(screenOwner, "", Color.White, UICaption.CaptionStyle.AwardCaption);
            this._capCompletion.ParentAlignment = AlignModes.Right | AlignModes.Vertically;
            this.Controls.Add(this._capCompletion);

#if DEBUG
            // Toggle won
            this._btnToggleWon = new UISnailsButton(screenOwner, "BTN_TOGGLE_WON", UISnailsButton.ButtonSizeType.Small, BrainEngine.Input.InputBase.InputActions.None, this._btnToggleWon_OnPress, false);
            this._btnToggleWon.ParentAlignment = AlignModes.Right | AlignModes.Vertically;
            this._btnToggleWon.Scale = this.FromNativeResolution(this._btnToggleWon.Scale);
            this.Controls.Add(this._btnToggleWon);
#endif
            // Control
            this.Achievement = achievement;
            this.UpdateSize();
#if DEBUG
            this.AllowToggle = false;
#endif
          // this.BackgroundColor = Color.Red;
            this.BackgroundColor = new Color(0, 0, 0, 50);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateSize()
        {
            if (!this.AllowToggle)
            {
                this.Size = new Size(6400f, 900f);
                this._capCompletion.Margins.Right = 0;
            }
            else
            {
                this.Size = new Size(6400f, 1100f);
#if DEBUG
                this._capCompletion.Margins.Right = this._btnToggleWon.Size.Width;
#endif
            }
        }

        public float GetCompletionPercentage(BrainAchievement achievement)
        {
            if (SnailsGame.ProfilesManager.CurrentProfile.IsAchievementEarned(achievement.EventType))
            {
                return 100;
            }

            int quantity = achievement.Quantity;

            if (quantity <= 0)
                return 0;

            int value = SnailsGame.AchievementsManager.Verify(achievement.EventType);

            if (value == -1)
                return 0;

            return (float)(((float)value / (float)quantity) * 100);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            if (this.Achievement != null)
            {
                this._image.Sprite = this.Achievement.Trophy;
                string line1 = this.Achievement.Description;
                string line2 = null;

                if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD)
                {
                    if (this.Achievement.Description.Length > MAX_CHARS_PER_LINE)
                    {
                        line1 = line1.Substring(0, MAX_CHARS_PER_LINE);
                        int j = 0;
                        for (j = line1.Length - 1; (j >= 0) && (line1[j] != ' '); j--)
                        { }
                        line1 = line1.Substring(0, j).Trim();
                        line2 = this.Achievement.Description.Substring(line1.Length).Trim();
                    }
                }

                this._capDescriptionLine1.Text = line1;
                this._capDescriptionLine2.Text = line2;
                if (line2 == null)
                {
                    this._capDescriptionLine1.ParentAlignment = AlignModes.Vertically;
                    this._capDescriptionLine2.Visible = false;
                }
                else
                {
                    this._capDescriptionLine1.ParentAlignment = AlignModes.Top;
                    this._capDescriptionLine2.Visible = true;
                    this._capDescriptionLine2.ParentAlignment = AlignModes.Bottom;
                }
                this._capCompletion.Text = string.Format("{0:##0}%", GetCompletionPercentage(this.Achievement));

                if (SnailsGame.ProfilesManager.CurrentProfile.IsAchievementEarned(this.Achievement.EventType))
                {
                    this._image.BlendColor = Color.White;
                    this._capDescriptionLine1.BlendColor = this.CaptionColorWon;
                    this._capDescriptionLine2.BlendColor = this.CaptionColorWon;
                }
                else
                {
                    this._image.BlendColor = new Color(0, 0, 0, 255);
                    this._capDescriptionLine1.BlendColor = this.CaptionColorNotWon;
                    this._capDescriptionLine2.BlendColor = this.CaptionColorNotWon;
                }
            }
        }
#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        void _btnToggleWon_OnPress(IUIControl sender)
        {
            if (!SnailsGame.ProfilesManager.CurrentProfile.IsAchievementEarned(this.Achievement.EventType))
            {
                SnailsGame.ProfilesManager.CurrentProfile.MarkAchievementEarned(this.Achievement.EventType);
            }
            else // remove achievement
            {
                SnailsGame.ProfilesManager.CurrentProfile.MarkAchievementNotEarned(this.Achievement.EventType);
            }
            SnailsGame.ProfilesManager.Save();
            this.Refresh();
        }

#endif
    }
}
