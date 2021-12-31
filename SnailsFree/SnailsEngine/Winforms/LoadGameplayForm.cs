using System.Windows.Forms;
using System.Collections.Generic;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.Forms;

namespace TwoBrainsGames.Snails.Winforms
{
    public partial class LoadGameplayForm : Form
    {
        Stage Stage { get; set; }
        public string Filename { get; private set; }
        private GameplayRecorder.SnailsGameplayRecord SelectedRecord
        {
            get
            {
                if (!(this._lstFiles.SelectedItem is GameplayRecorder.SnailsGameplayRecord))
                {
                    return null;
                }
                return (GameplayRecorder.SnailsGameplayRecord)this._lstFiles.SelectedItem;
            }
        }
        public LoadGameplayForm()
        {
            InitializeComponent();
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
        private void LoadGameplayForm_Load(object sender, System.EventArgs e)
        {
            try
            {
                this._txtPath.Text = SnailsGame.GameSettings.GameplayRecordingPath;
                this._lblTheme.Text = this.Stage.LevelStage.ThemeId.ToString();
                this._lblStage.Text = this.Stage.LevelStage.StageNr.ToString();
                this._lblBuildNr.Text = this.Stage.BuildNr.ToString();
                this.RefreshFileList();
                this.RefreshFileDetails();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshFileList()
        {
            this._lstFiles.Items.Clear();
            List<GameplayRecorder.SnailsGameplayRecord> files = GameplayRecorder.EnumerateFiles(this._txtPath.Text, this.Stage.LevelStage.StageKey);
            foreach (GameplayRecorder.SnailsGameplayRecord file in files)
            {
                this._lstFiles.Items.Add(file);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshFileDetails()
        {
            this._lblTimeTaken.Text = "";
            this._lblScore.Text = "";
            this._lblMedal.Text = "";
            this._lblDateTime.Text = "";

            if (this.SelectedRecord != null)
            {
                this._lblTimeTaken.Text = string.Format("{0:00}:{1:00}:{2:00}", this.SelectedRecord.TimeTaken.Hours, this.SelectedRecord.TimeTaken.Minutes, this.SelectedRecord.TimeTaken.Seconds);
                this._lblScore.Text = this.SelectedRecord.TotalScore.ToString();
                this._lblMedal.Text = this.SelectedRecord.MedalWon.ToString();
                this._lblDateTime.Text = string.Format("{0:0000}-{1:00}-{2:00}", this.SelectedRecord.FileDate.Year, this.SelectedRecord.FileDate.Month, this.SelectedRecord.FileDate.Day);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lstFiles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.RefreshFileDetails();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._btnLoad.Enabled = (this._lstFiles.SelectedItem != null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnLoad_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.SelectedRecord == null)
                {
                    MessageBox.Show(this, "Please select a file from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (this.SelectedRecord.BuildNr != this.Stage.BuildNr && this.SelectedRecord.BuildNr != 0)
                {
                    if (MessageBox.Show(this, "Selected save file is not from the current build, loaded file may not work properly. Load anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                         == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                this.Filename = this.SelectedRecord.Filename;
                SnailsGame.GameSettings.GameplayRecordingPath = this._txtPath.Text;
                SnailsGame.GameSettings.SaveToFile();
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
        private void _btnSaveDialog_Click(object sender, System.EventArgs e)
        {
            try
            {

                if (STADialog.ShowDialog(this._dlgFolder) == System.Windows.Forms.DialogResult.OK)
                {
                    this._txtPath.Text = this._dlgFolder.SelectedPath;
                    this.RefreshFileList();
                    this.EnableButtons();
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
        private void _btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void _txtPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void _txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.RefreshFileList();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void label8_Click(object sender, System.EventArgs e)
        {

        }

        private void label7_Click(object sender, System.EventArgs e)
        {

        }

        private void label6_Click(object sender, System.EventArgs e)
        {

        }

        private void label10_Click(object sender, System.EventArgs e)
        {

        }
    }
}
