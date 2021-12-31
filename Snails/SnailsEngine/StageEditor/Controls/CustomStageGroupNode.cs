using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor.Controls;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    class CustomStageGroupNode : TreeNode
    {

        public CustomStageGroupNode() :
			base("Custom Stages")
		{
            this.ImageIndex = this.SelectedImageIndex = SolutionCtl.IMG_IDX_CUSTOM_STAGES_GROUP;
		}
    }
}
