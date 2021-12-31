using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.Stages;
using LevelEditor;
using System.IO;
using TwoBrainsGames.Snails.Configuration;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class NewCustomStageForm : BaseForm
    {
     

        public LevelStage LevelStage
        {
            get
            { 
                LevelStage levelStage = LevelStage.CreateForCustomStage((ThemeType)this._cmbTheme.SelectedItem, this._txtName.Text);
                levelStage.CustomStageFilename = this._txtFilename.Text;
                return levelStage;
            }

        }
        public NewCustomStageForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(this._txtFilename.Text))
                {
                    Diag.ShowFieldInputValidationError(this, "A file with that name already exists!\nPlease choose another name.");
                    return;
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
        private void EnableButtons()
        {
            this._btnOk.Enabled = (!string.IsNullOrEmpty(this._txtName.Text) && this._cmbTheme.SelectedItem != null); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void _txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._txtFilename.Text = Path.Combine(SnailsGame.GameSettings.CustomStagesFolder,
                                        this._txtName.Text.Replace(" ", "_") + "." + GameSettings.CustomStagesExtension);
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewCustomStageForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _cmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
