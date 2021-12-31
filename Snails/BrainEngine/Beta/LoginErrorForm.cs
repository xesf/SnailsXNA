using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.BrainEngine.Windows;

namespace TwoBrainsGames.BrainEngine.Beta
{
    internal partial class LoginErrorForm : Form
    {
        public enum LoginErrorFormResult
        {
            PlayNormaly,
            Quit,
            Retry
        }

        public bool DontWantToBeBetaTester { get { return this._chkPlayNormalyNowOn.Checked; } }

        public LoginErrorForm()
        {
            InitializeComponent();
        }

        public new LoginErrorFormResult ShowDialog()
        {
            DialogResult dr = base.ShowDialog();
            switch (dr)
            {
                case System.Windows.Forms.DialogResult.OK:
                    return LoginErrorFormResult.PlayNormaly;

                case System.Windows.Forms.DialogResult.Retry:
                    return LoginErrorFormResult.Retry;

            }

            return LoginErrorFormResult.Quit;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _chkPlayNormalyNowOn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this._bntRetry.Enabled = (this._chkPlayNormalyNowOn.Checked == false);
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
        }
    }
}
