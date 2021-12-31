using System;
using System.Windows.Forms;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.Forms;
using System.IO;

namespace TwoBrainsGames.Snails.Winforms
{
    public partial class SaveGameplayForm : Form
    {
        Stage Stage { get; set; }
        public string Filename { get { return Path.Combine(this._txtPath.Text, this._lbFilename.Text); } }
        public string Description { get { return this._cmbDescription.Text; } }

        public SaveGameplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSaveDialog_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(Stage stage)
        {
            this.Stage = stage;
            return this.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveGameplayForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._lblTheme.Text = this.Stage.LevelStage.ThemeId.ToString();
                this._lblStage.Text = this.Stage.LevelStage.StageNr.ToString();
                this._lblMedal.Text = this.Stage.Stats.MedalWon.ToString();
                this._lblPoints.Text = this.Stage.Stats.TotalScore.ToString();
                this._txtPath.Text = SnailsGame.GameSettings.GameplayRecordingPath;
                this._lblBuildNr.Text = this.Stage.BuildNr.ToString();
                this._dlgSave.Filter = string.Format("{0} | *.{1}", GameplayRecorder.FILE_DESCRIPTION, GameplayRecorder.DEFAULT_EXTENSION);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Exit whithout saving?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this._txtSufix.Text))
                {
                    MessageBox.Show(this, "Please select a filename.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(this._cmbDescription.Text))
                {
                    MessageBox.Show(this, "Please enter a description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (File.Exists(this.Filename))
                {
                    if (MessageBox.Show(this, "File already exists. Overwrite?", "File overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                }

                SnailsGame.GameSettings.GameplayRecordingPath = this._txtPath.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSaveDialog_Click_1(object sender, EventArgs e)
        {
            if (STADialog.ShowDialog(this._dlgFolder) == System.Windows.Forms.DialogResult.OK)
            {
                this._txtPath.Text = this._dlgFolder.SelectedPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _txtFilename_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.RefreshFilename();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshFilename()
        {
            this._lbFilename.Text = string.Format("{0}_{1:00}", this.Stage.LevelStage.ThemeId.ToString(), this.Stage.LevelStage.StageNr);
            this._lbFilename.Text += string.Format("_{0}.{1}", this._txtSufix.Text, GameplayRecorder.DEFAULT_EXTENSION);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _txtPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.RefreshFilename();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }
    }
}
