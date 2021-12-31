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
    public partial class LoadCustomStageForm : BaseForm
    {
        class StageItem
        {
            public LevelStage Stage { get; private set; }
            public StageItem(LevelStage stage)
            {
                this.Stage = stage;
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", this.Stage.StageId, Formater.GetThemeName(this.Stage.ThemeId));
            }
        }
        public LoadCustomStageForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadCustomStageForm_Load(object sender, EventArgs e)
        {
            try
            {
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
        private void RefreshList()
        {
            this._lslCustomStages.Items.Clear();
            foreach (LevelStage stage in StageEditor.GetCustomStages())
            {
                this._lslCustomStages.Items.Add(new StageItem(stage));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._btnOk.Enabled = (this._lslCustomStages.SelectedItem != null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lslCustomStages_SelectedIndexChanged(object sender, EventArgs e)
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
        private void _btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectStage();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }    
        }

        /// <summary>
        /// 
        /// </summary>
        private void SelectStage()
        {
            if (this._lslCustomStages.SelectedItem == null)
            {
                return;
            }
            StageEditor.Instance.CurrentLevelStage = ((StageItem)this._lslCustomStages.SelectedItem).Stage;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lslCustomStages_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.SelectStage();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }    
        }
    }
}
