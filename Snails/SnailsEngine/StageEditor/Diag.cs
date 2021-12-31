using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageEditor;

namespace LevelEditor
{
  class Diag
  {
    public static void ShowException(Form parent, System.Exception ex)
    {
    
        MessageBox.Show(parent, ex.ToString(), Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void ShowFieldInputValidationError(Form parent, string message)
    {
        MessageBox.Show(parent, message, Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    public static bool Confirm(Form parent, string message)
    {
        return (MessageBox.Show(parent, message, Settings.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
    }
  }
}
