using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using System.IO;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class PreferencesForm : BaseForm
    {
        public bool CustomStagesFolderChanged { get; set; }
        public PreferencesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._txtCustomStagesFolder.Text = SnailsGame.GameSettings.CustomStagesFolder;
                this._txtRetailStagesFolder.Text = UserSettings.StagesPath;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(this._txtCustomStagesFolder.Text))
                {
                    MessageBox.Show(this, "Folder does not exists.", Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this._txtCustomStagesFolder.Focus();
                    return;
                }

                this.CustomStagesFolderChanged = (SnailsGame.GameSettings.CustomStagesFolder != this._txtCustomStagesFolder.Text);
                SnailsGame.GameSettings.CustomStagesFolder = this._txtCustomStagesFolder.Text;
                UserSettings.StagesPath = this._txtRetailStagesFolder.Text;
                this.Close();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSelFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (STADialog.ShowDialog(this._dlgSelFolder) == System.Windows.Forms.DialogResult.OK)
                {
                    this._txtCustomStagesFolder.Text = this._dlgSelFolder.SelectedPath;
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this._dlgSelFolder.SelectedPath = this._txtRetailStagesFolder.Text;
                if (STADialog.ShowDialog(this._dlgSelFolder) == System.Windows.Forms.DialogResult.OK)
                {
                    this._txtRetailStagesFolder.Text = this._dlgSelFolder.SelectedPath;
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
