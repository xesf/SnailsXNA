using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public class STADialog
    {
        public static DialogResult ShowDialog(FolderBrowserDialog dialog)
        {
            DialogState state = new DialogState();
            state.dialog = dialog;
            System.Threading.Thread t = new System.Threading.Thread(state.ThreadProcShowDialog);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            return state.result;
        }
    }

    public class DialogState
    {
        public DialogResult result;
        public FolderBrowserDialog dialog;

        public void ThreadProcShowDialog()
        {
            result = dialog.ShowDialog();
        }

    }
}
