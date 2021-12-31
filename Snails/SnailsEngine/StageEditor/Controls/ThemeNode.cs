using System.Windows.Forms;
using LevelEditor.Controls;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
	class ThemeNode : TreeNode
	{
        public ThemeType Theme { get; private set; }

		public ThemeNode(ThemeType theme):
            base(Formater.GetThemeName(theme))
		{
            this.Theme = theme;
            this.ImageIndex = this.SelectedImageIndex = (SolutionCtl.IMG_IDX_FIRST_THEME + (int)theme);
        }
	}
}
