using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsStageGoalIcon : UIImage
    {
        public enum GoalIconSize
        {
            Small,
            Big
        }
        GoalType _goal;
        GoalIconSize _iconSize;
        
        public GoalType Goal
        {
            get { return this._goal;}
            set 
            {
                this._goal = value;
                this.CurrentFrame = (int)this._goal;
            }
        }

        public GoalIconSize IconSize
        {
            get { return this._iconSize; }
            set
            {
                this._iconSize = value;
                switch (this._iconSize)
                {
                    case GoalIconSize.Big:
                        this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/Goal");
                        break;
                    case GoalIconSize.Small:
                        this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/GoalSmall");
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UISnailsStageGoalIcon (UIScreen screenOwner) :
            base(screenOwner)
        {
            this.Animate = false;
            this.IconSize = GoalIconSize.Big;
        }
    }
}
