using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.UI;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Screens.Transitions;

namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
    public class UITheme : UIControl
    {
        protected static Vector2 DEFAULT_SCALE = new Vector2(0.85f, 0.85f);

        public UIEvent OnMoveToEnded;
        public UIEvent OnUnselectEnded;

        protected UIImage _imgBoard;
        protected UILocker _lockerImage;
        protected UIImage _imgSmallLocker;
        protected UIImage _imgMedal;
        protected UISnailsThemeIcon _imgTheme;
        protected bool _locked;
        protected Vector2 _originalPosition;
        protected AlignModes _saveAlignment;
        protected UIPanel _pnlContainer;
        protected UICaption _lblToUnlock;
        protected UICaption _lblLockedInTrial;
        protected UICaption _lblGardenNeeded;
        protected UICaption _lblEgyptNeeded;
        protected UICaption _lblFactoryNeeded;
        protected UICaption _lblStagesUnlocked;
        protected UICaption _lblGoldMedalsEarned;
        protected Sample _focusSound;

        public ThemeType ThemeId { get; private set; }
        public bool FocusEffectEnabled { get; set; }

        public override BrainEngine.Collision.BoundingSquare BoundingBox
        {
            get
            {
                Vector2 pos = this._pnlContainer.AbsolutePositionInPixels + new Vector2(this._imgBoard.Sprite.BoundingBoxes[0].Left,
                                                                this._imgBoard.Sprite.BoundingBoxes[0].Top);
                return new BoundingSquare(pos, this._imgBoard.Sprite.BoundingBoxes[0].Width * this._pnlContainer.Scale.X,
                                                 this._imgBoard.Sprite.BoundingBoxes[0].Height * this._pnlContainer.Scale.Y);
            }
        }

        public bool LockedInDemo { get; set; }

        public bool Locked
        {
            get { return this._locked; }
            private set
            {
                this._locked = value;
            }

        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                if (this._pnlContainer != null)
                {
                    this._pnlContainer.Visible = value;
                    this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
                }
            }
        }

        private bool WithThemeSelectionAnimations { get; set; }

        public UITheme(UIScreen screenOwner, ThemeType themeId) :
            base(screenOwner)
        {
            this.ThemeId = themeId;
            this.Name = "theme_" + this.ThemeId.ToString();
            this.OnMoveToEnded = null;
            this.OnEnter += new UIEvent(this.Theme_OnEnter);
            this.OnLeave += new UIEvent(this.Theme_OnLeave);
            this.OnInitializeFromContent += new UIEvent(UITheme_OnInitializeFromContent);

            // Container
            this._pnlContainer = new UIPanel(screenOwner);
            this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
            this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
            this._pnlContainer.ShowEffect = new SquashEffect(0.7f, 3.2f, 0.08f, this.BlendColor, this._pnlContainer.Scale);
            this._pnlContainer.HideEffect = new PopOutEffect(new Vector2(1.2f, 1.2f), 6.0f);
            this._pnlContainer.OnAccept += new UIEvent(_pnlContainer_OnAccept);
            this._pnlContainer.OnShow += new UIEvent(_pnlContainer_OnShow);
            this.Controls.Add(this._pnlContainer);

            this._imgBoard = new UIImage(screenOwner, "spriteset/boards/DarkWoodMedium");
            this._imgBoard.Name = "icon_" + this.ThemeId.ToString();
            this._imgBoard.ParentAlignment = AlignModes.HorizontalyVertically;
            this._pnlContainer.Controls.Add(this._imgBoard);

            this._imgTheme = new UISnailsThemeIcon(screenOwner);
            this._imgTheme.Name = "title_" + this.ThemeId.ToString();
            this._imgTheme.Position = new Vector2(300f, 450f);
            this._pnlContainer.Controls.Add(this._imgTheme);

            // Unlocked stages Locker
            this._imgSmallLocker = new UIImage(screenOwner, "spriteset/common-elements-1/LockerSmallOpen", ResourceManager.RES_MANAGER_ID_STATIC);
            this._imgSmallLocker.Position = new Vector2(2600f, 600f);
            this._pnlContainer.Controls.Add(this._imgSmallLocker);

            // Stages unlocked
            this._lblStagesUnlocked = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeStageStats, UICaption.CaptionStyle.ThemeStats);
            this._lblStagesUnlocked.Position = this._imgSmallLocker.Position + new Vector2(600f, 300f);
            this._pnlContainer.Controls.Add(this._lblStagesUnlocked);

            // Gold medals earned medal
            this._imgMedal = new UIImage(screenOwner, "spriteset/menu-elements-1/GoldMedal", ResourceManager.RES_MANAGER_ID_TEMPORARY);
            this._imgMedal.Position = new Vector2(2650f, 1750f);
            this._pnlContainer.Controls.Add(this._imgMedal);

            // Gold medals earned 
            this._lblGoldMedalsEarned = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeStageStats, UICaption.CaptionStyle.ThemeStats);
            this._lblGoldMedalsEarned.Position = this._imgMedal.Position + new Vector2(550f, 300f);
            this._pnlContainer.Controls.Add(this._lblGoldMedalsEarned);

            // Theme locker
            this._lockerImage = new UILocker(screenOwner);
            this._lockerImage.Position = new Vector2(1950, 780);
            this._pnlContainer.Controls.Add(this._lockerImage);

            // Needed to unlock
            this._lblToUnlock = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeSelectionNeeded, UICaption.CaptionStyle.ThemeUnlockInfoTitle);
            this._lblToUnlock.ParentAlignment = AlignModes.Horizontaly;
            this._lblToUnlock.TextResourceId = "LBL_NEEDED_UNLOCK";
            this._lblToUnlock.Position = new Vector2(0, 1630);
            this._lblToUnlock.BlendColorWithParent = false;
            this._pnlContainer.Controls.Add(this._lblToUnlock);

            // Locked in the trial
            this._lblLockedInTrial = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeSelectionNeeded, UICaption.CaptionStyle.ThemeUnlockInfoTitle);
            this._lblLockedInTrial.ParentAlignment = AlignModes.Horizontaly;
            this._lblLockedInTrial.TextResourceId = "LBL_LOCKED_IN_TRIAL";
            this._lblLockedInTrial.Position = this.NativeResolution(new Vector2(0, 1000));
            this._lblLockedInTrial.BlendColorWithParent = false;
            this._pnlContainer.Controls.Add(this._lblLockedInTrial);

            // Garden stages needed
            this._lblGardenNeeded = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeSelectionNeededFromThemes, UICaption.CaptionStyle.ThemeUnlockInfo);
            this._lblGardenNeeded.ParentAlignment = AlignModes.Horizontaly;
            this._lblGardenNeeded.Position = this._lblToUnlock.Position + new Vector2(0, 420);
            this._lblGardenNeeded.BlendColorWithParent = false;
            this._pnlContainer.Controls.Add(this._lblGardenNeeded);

            // Ancient egypt stages needed
            this._lblEgyptNeeded = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeSelectionNeededFromThemes, UICaption.CaptionStyle.ThemeUnlockInfo);
            this._lblEgyptNeeded.ParentAlignment = AlignModes.Horizontaly;
            this._lblEgyptNeeded.Position = this._lblGardenNeeded.Position + new Vector2(0, 350);
            this._lblEgyptNeeded.BlendColorWithParent = false;
            this._pnlContainer.Controls.Add(this._lblEgyptNeeded);


            // Robot factory stages needed
            this._lblFactoryNeeded = new UICaption(screenOwner, "", TwoBrainsGames.Snails.Colors.ThemeSelectionNeededFromThemes, UICaption.CaptionStyle.ThemeUnlockInfo);
            this._lblFactoryNeeded.ParentAlignment = AlignModes.Horizontaly;
            this._lblFactoryNeeded.Position = this._lblEgyptNeeded.Position + new Vector2(0, 350);
            this._lblFactoryNeeded.BlendColorWithParent = false;
            this._pnlContainer.Controls.Add(this._lblFactoryNeeded);

            this._imgTheme.Theme = this.ThemeId;
            this._pnlContainer.Size = this._imgBoard.Size;

            this.Size = this._imgBoard.Size;
            this._focusSound = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_FOCUS);
            this.AcceptControllerInput = true;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitializeFromContent()
        {
            base.InitializeFromContent();
            this.InitializeFromContent("UITheme");
        }

        /// <summary>
        /// 
        /// </summary>
        void UITheme_OnInitializeFromContent(IUIControl sender)
        {
            this.FocusEffectEnabled = this.GetContentPropertyValue<bool>("withFocusEffect", this.FocusEffectEnabled);
            this.WithThemeSelectionAnimations = this.GetContentPropertyValue<bool>("withThemeSelectionAnimations", this.FocusEffectEnabled);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private string FormatStagesNeededText(ThemeType theme, int stagesNeeded)
        {
            if (SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(theme))
            {
             return string.Format(LanguageManager.GetString("LBL_STAGES_NEEDED_UNLOCK_THEME"),
                          Formater.GetThemeName(theme), stagesNeeded);
            }

             return string.Format(LanguageManager.GetString("LBL_STAGES_NEEDED_UNLOCK_THEME"),
                                        LanguageManager.GetString("LBL_LOCKED_THEME"), stagesNeeded);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateUnlockGoal()
        {
            // Garden
            int stagesNeeded = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.StagesNeededToUnlockTheme(this.ThemeId, ThemeType.ThemeA);
            this._lblGardenNeeded.Text = this.FormatStagesNeededText(ThemeType.ThemeA, stagesNeeded);
            this._lblGardenNeeded.Visible = (stagesNeeded > 0 && !this.LockedInDemo);

            // Ancient egypt
            stagesNeeded = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.StagesNeededToUnlockTheme(this.ThemeId, ThemeType.ThemeB);
            this._lblEgyptNeeded.Text = this.FormatStagesNeededText(ThemeType.ThemeB, stagesNeeded);
            this._lblEgyptNeeded.Visible = (stagesNeeded > 0 && !this.LockedInDemo);

            // Bot Factory
            stagesNeeded = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.StagesNeededToUnlockTheme(this.ThemeId, ThemeType.ThemeC);
            this._lblFactoryNeeded.Text = this.FormatStagesNeededText(ThemeType.ThemeC, stagesNeeded);
            this._lblFactoryNeeded.Visible = (stagesNeeded > 0 && !this.LockedInDemo);

        }

        /// <summary>
        /// 
        /// </summary>
        void _pnlContainer_OnShow(IUIControl sender)
        {
            this.InvokeOnShow();
        }

        /// <summary>
        /// 
        /// </summary>
        void _pnlContainer_OnAccept(IUIControl sender)
        {
            if (!this.Locked)
            {
                this.InvokeOnAccept();
            }
           /* else
            {
                if (this.LockedInDemo && SnailsGame.GameSettings.WithAppStore)
                {
                    ((SnailsScreen)this.ScreenOwner).NavigateToPurchase();
                }
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            this._pnlContainer.Visible = true;
            base.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Select(Vector2 position)
        {
            if (!this.Locked)
            {
                this.MoveTo(position);
            }
        }

        /// <summary>
        /// This method selectes the theme without using anymations
        /// The theme icon is placed in the top left corner
        /// We have to set _originalPosition to the real position of the icon
        /// This will alow the move animation to the icon original position when unselected
        /// </summary>
        public void SelectWithoutAnimations(Vector2 position)
        {
            this._pnlContainer.Scale = new Vector2(1.0f, 1.0f);
            this._originalPosition = this.PositionInPixels;
            this.Position = position;
            this._saveAlignment = this.ParentAlignment;
            this.ParentAlignment = AlignModes.None;
            this.FocusEffectEnabled = false;
        }

        /// <summary>
        /// Called by the Screen when the theme is selected
        /// The theme is moved to the moveTo position
        /// Original position is stored to restore position if needed
        /// </summary>
        private void MoveTo(Vector2 position)
        {
            this.FocusEffectEnabled = false;
            this._saveAlignment = this.ParentAlignment;
            this.ParentAlignment = AlignModes.None;
            this._originalPosition = this.PositionInPixels;

            PathFollowEffect effect = this.CreateMoveEffect(position);
            effect.OnEnd = this.MoveToEffect_OnEnd;
            this.EffectsBlender.Add(effect, 1);
        }

        /// <summary>
        /// Called by the Screen when the theme is selected
        /// The theme is moved to the moveTo position
        /// Original position is stored to restore position if needed
        /// </summary>
        public void Unselect()
        {
            if (this.WithThemeSelectionAnimations)
            {
                PathFollowEffect effect = this.CreateMoveEffect(this._originalPosition);
                effect.OnEnd = this.UnselectMoveToEffect_OnEnd;
                this.EffectsBlender.Add(effect, 1);
            }
            else // if not, just simply call the event
            {
                UnselectMoveToEffect_OnEnd(null);
            }
        }

        /// <summary>
        /// Make the speed depend on distance
        /// The idea here is to make the object take the same time to travel diferent distances
        /// <returns></returns>
        private PathFollowEffect CreateMoveEffect(Vector2 destination)
        {
            float speed = (this.PositionInPixels - destination).Length() / 20.0f;
            return new PathFollowEffect(this.PositionInPixels, destination, speed, false);
        }
        /// <summary>
        /// 
        /// </summary>
        private void Theme_OnEnter(IUIControl sender)
        {
            if (this.FocusEffectEnabled && !this.Locked)
            {
                if (this._pnlContainer.Scale.X != 1.0f ||
                    this._pnlContainer.Scale.Y != 1.0f)
                {
                    this._focusSound.Play();
                    this._pnlContainer.Effect = new ScaleEffect(UITheme.DEFAULT_SCALE, 2.0f, new Vector2(1.0f, 1.0f), false);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void Theme_OnLeave(IUIControl sender)
        {
            if (this.FocusEffectEnabled)
            {
                this._pnlContainer.Effect = null;
                this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
            }
        }

        public void ClearFocusEffect()
        {
            if (this.FocusEffectEnabled)
            {
                this._pnlContainer.Effect = null;
                this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveToEffect_OnEnd(object param)
        {
            this.EffectsBlender.Clear();
            if (this.OnMoveToEnded != null)
            {
                this.OnMoveToEnded(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnselectMoveToEffect_OnEnd(object param)
        {
            this.EffectsBlender.Clear();
            this.Enabled = true;
//            this.AcceptControllerInput = true;
            this.ParentAlignment = this._saveAlignment;
            if (this.OnUnselectEnded != null)
            {
                this.OnUnselectEnded(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            this.Locked = !SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(this.ThemeId);
            this.LockedInDemo = (Levels._instance.IsLockedInDemo(this.ThemeId) && SnailsGame.IsTrial);
          
            int unlockedStages = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetUnlockedStagesForTheme(this.ThemeId);
            this._lblStagesUnlocked.Text = string.Format("{0}/{1}", unlockedStages, Levels.MAX_NUMBER_STAGES_PER_THEME);

            int goldMedals = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.GetTotalMedalsForTheme(MedalType.Gold, this.ThemeId);
            this._lblGoldMedalsEarned.Text = string.Format("{0}/{1}", goldMedals, Levels.MAX_NUMBER_STAGES_PER_THEME);

            this._lblToUnlock.Visible = (this.Locked && !this.LockedInDemo);
            this._lblLockedInTrial.Visible = (this.Locked && this.LockedInDemo);

            // Garden
            this.UpdateUnlockGoal();

            this.BlendColor = (Locked ? Color.Black : Color.White);
            this._lockerImage.Visible = this._locked;
            this.Enabled = !this._locked;

            if (this.Locked)
            {
                this._lockerImage.LockerType = (this.LockedInDemo ? UILocker.LockerImageType.Demo : UILocker.LockerImageType.Normal);
                this._lockerImage.Position = this.NativeResolution(new Vector2(1950, 780));
                if (this.LockedInDemo)
                {
                    this._lblFactoryNeeded.Visible = false;
                    this._lblEgyptNeeded.Visible = false;
                    this._lblGardenNeeded.Visible = false;
                    this._lockerImage.Position = this.NativeResolution(new Vector2(1950, 2000));
                }
            }
        }

    }
}
