using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
	class StageNode : TreeNode
	{
        internal const int IMG_IDX_GOAL_KING = 8;
        internal const int IMG_IDX_GOAL_TIME = 9;
        internal const int IMG_IDX_GOAL_ESCORT = 6;
        internal const int IMG_IDX_GOAL_KILL = 7;

        public LevelStage LevelStage
        {
            get;
            set;
        }


        public StageNode(LevelStage levelStage) :
            base(levelStage.StageId)
		{
            this.LevelStage = levelStage;
            this.ImageIndex = this.SelectedImageIndex = (1 + (int)levelStage.ThemeId);
            switch (levelStage._goal)
            {
                case GoalType.SnailDelivery:
                   // this.ImageIndex = IMG_IDX_GOAL_ESCORT;
                    this.ImageIndex = this.SelectedImageIndex = (1 + (int)levelStage.ThemeId);
                    break;
                case GoalType.SnailKiller:
                    this.ImageIndex = this.SelectedImageIndex = IMG_IDX_GOAL_KILL;
                    break;
                case GoalType.SnailKing:
                    this.ImageIndex = this.SelectedImageIndex = IMG_IDX_GOAL_KING;
                    break;
                case GoalType.TimeAttack:
                    this.ImageIndex = this.SelectedImageIndex = IMG_IDX_GOAL_TIME;
                    break;
            }
        }
	}
}
