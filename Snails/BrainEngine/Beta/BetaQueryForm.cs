using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TwoBrainsGames.BrainEngine.Windows;

namespace TwoBrainsGames.BrainEngine.Beta
{
    public partial class BetaQueryForm : Form
    {
        private const string BETA_NAME_VAR = "%BETA_NAME%";

        public bool BetaTesterChecked
        {
            get { return this._chkBePartOfTheBeta.Checked; }
        }
        public bool DontShowAgainChecked
        {
            get { return this._chkDontShow.Checked; }
        }
        public string EMail
        {
            get { return this._txtMail.Text; }
        }

        private bool IsEmailAddressValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.EMail))
                {
                    return false;
                }
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                      @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                      @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                return (re.IsMatch(this.EMail));
            }
        }
        public BetaQueryForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BetaQueryForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = this.Text.Replace(BETA_NAME_VAR, BrainGame.BetaName);
                this._lblTitle.Text = this._lblTitle.Text.Replace(BETA_NAME_VAR, BrainGame.BetaName);
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
            finally
            {
                this.EnableButtons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _chkBePartOfTheBeta_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this._chkDontShow.Checked = false;
                this.EnableButtons();
                if (this.BetaTesterChecked)
                {
                    this._txtMail.Focus();
                }
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this.ClientSize = new Size(this.ClientRectangle.Width, this._pnlInfo.Height + this._pnlButtons.Height + (this.BetaTesterChecked ? this._pnlMail.Height : 0));
            this._pnlMail.Visible = this.BetaTesterChecked;
            this._chkDontShow.Enabled = !this.BetaTesterChecked;
            this._btnProceed.Enabled = true;
            if (this._chkBePartOfTheBeta.Checked)
            {
                this._chkDontShow.Checked = true;
                this._btnProceed.Enabled = this.IsEmailAddressValid;
            }
            this._btnProceed.Text = (this.BetaTesterChecked ? "I want to become a beta-tester!" : "I just want to try the game");
        }

        /// <summary>
        /// 
        /// </summary>
        private void _txtMail_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.BetaTesterChecked)
                {
                    ClosedBeta.RegisterUser(this.EMail);
                    MessageBox.Show(this, "A message with a beta-key has been sent to the e-mail address provided.\nPlease check your e-mail.", 
                                    "Information",  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                BEMessageBox.ShowException(this, ex);
            }
            finally
            {
                this.Cursor = this.DefaultCursor;
            }

        }
    }
}
