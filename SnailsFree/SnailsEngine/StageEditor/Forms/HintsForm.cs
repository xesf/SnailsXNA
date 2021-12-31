using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.Stages.Hints;
using LevelEditor.Forms;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class HintsForm : BaseForm
    {
        public enum HintsDialogResult
        {
            Ok,
            Add,
            Edit
        }

        public Hint SelectedHint { get; set; }
        public HintsDialogResult DialogAction { get; set; }
        EditorStage Stage { get; set; }

        public HintsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(EditorStage stage)
        {
            this.Stage = stage;
            return this.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedHint = new Hint(this.Stage.Stage.HintManager);
                this.Stage.Stage.HintManager.Hints.Add(this.SelectedHint);

                this.DialogAction = HintsDialogResult.Add;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogAction = HintsDialogResult.Edit;
                this.SelectedHint = this.Stage.Stage.HintManager.Hints[this._lstHints.SelectedIndex];
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Stage.Stage.HintManager.Hints.RemoveAt(this._lstHints.SelectedIndex);
                this.RefreshList();
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
        private void HintsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.RefreshList();
                this.Left = MainForm._instance.Location.X + MainForm._instance.Width - this.Width;
                this.Top = MainForm._instance.Location.Y + MainForm._instance.Height - this.Height;

            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
            finally
            {
                this.EnableButtons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshList()
        {
            this._lstHints.Items.Clear();
            for (int i = 0; i < this.Stage.Stage.HintManager.Hints.Count; i++)
            {
                this._lstHints.Items.Add("Hint #" + (i + 1).ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._btnEdit.Enabled = (this._lstHints.SelectedIndex != -1);
            this._btnDelete.Enabled = (this._lstHints.SelectedIndex != -1);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lstHints_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Stage.Stage.HintManager.CurrentHint = this.Stage.Stage.HintManager.Hints[this._lstHints.SelectedIndex];
                MainForm._instance.RefreshBoard();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
