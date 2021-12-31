using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class NewStageForm : BaseForm
    {
        

        public NewStageForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public LevelStage OpenDialog(IWin32Window owner)
        {
            if (this.ShowDialog(owner) == System.Windows.Forms.DialogResult.Cancel)
            {
                return null;
            }

            LevelStage newStage = new LevelStage();
            newStage.ThemeId = (ThemeType)this._cmbTheme.SelectedItem;
            newStage.StageId = this._txtId.Text;

            return newStage;
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewStageForm_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (ThemeType theme in Enum.GetValues(typeof(ThemeType)))
                {
                    if (theme != ThemeType.None)
                    {
                        this._cmbTheme.Items.Add(theme);
                    }
                }

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
        private void EnableButtons()
        {
            this._btnOk.Enabled = (!string.IsNullOrEmpty(this._txtId.Text) && this._cmbTheme.SelectedItem != null);
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

        /// <summary>
        /// 
        /// </summary>
        private void _txtId_TextChanged(object sender, EventArgs e)
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

        private void _btnOk_Click(object sender, EventArgs e)
        {
       
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {

        }


    }
}
