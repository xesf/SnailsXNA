using System.Windows.Forms;

namespace TwoBrainsGames.BrainEngine.Windows
{
    class BEMessageBox
    {
        /// <summary>
        /// 
        /// </summary>
        internal static void ShowException(IWin32Window owner, System.Exception ex)
        {
            MessageBox.Show(owner, "Application Error.\n" + ex.Message, BrainGame.GameName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
