using System.Windows.Forms;
using TwoBrainsGames.Snails.Stages;
using LevelEditor.Controls;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    class CustomStageNode : TreeNode
    {
        public LevelStage LevelStage
        {
            get;  set;
        }

        public CustomStageNode(LevelStage levelStage) :
            base(levelStage.StageId)
		{
            this.LevelStage = levelStage;
            this.ImageIndex = this.SelectedImageIndex = (SolutionCtl.IMG_IDX_FIRST_THEME + (int)levelStage.ThemeId);
        }
    }
}
