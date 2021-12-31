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
    public partial class StagePropertiesForm : BaseForm
    {
        struct StageLayer
        {
            public SnailsBackgroundLayer _layer;

            public StageLayer(SnailsBackgroundLayer layer)
            {
                this._layer = layer;
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", this._layer.Id, this._layer._layerType.ToString());
            }
        }

        EditorStage _stage;
        public StagePropertiesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(EditorStage stage)
        {
            this._stage = stage;
            return this.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this._stage.Stage.Layers.Clear();
                foreach (StageLayer layer in this._lstLayers.Items)
                {
                    this._stage.Stage.Layers.Add(layer._layer);
                }

                this._stage.Stage._withShadows = this._chkWithShadows.Checked;
                this._stage.Changed = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void RefreshProperties()
        {
            this._txtId.Text = this._stage.Stage.Id;
			this._lblBuildNr.Text = this._stage.Stage.BuildNr.ToString();
            foreach (SnailsBackgroundLayer layer in this._stage.Layers)
            {
                this._lstLayers.Items.Add(new StageLayer(layer));
            }

            this._chkWithShadows.Checked = this._stage.Stage._withShadows;
            this._lblBronzeMedals.Text = this._stage.Stage.CountPickableObjects(StageObjects.PickableObject.PickableType.CopperCoin).ToString();
            this._lblSilverMedals.Text = this._stage.Stage.CountPickableObjects(StageObjects.PickableObject.PickableType.SilverCoin).ToString();
            this._lblGoldMedals.Text = this._stage.Stage.CountPickableObjects(StageObjects.PickableObject.PickableType.GoldCoin).ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        private void StagePropertiesForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.RefreshProperties();
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
        private void _btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                EditLayerForm form = new EditLayerForm();
                if (form.OpenNew() == System.Windows.Forms.DialogResult.OK)
                {
                    this._lstLayers.Items.Add(new StageLayer(form.Layer));
                }
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
                if (this._lstLayers.SelectedItem == null)
                {
                    return;
                }
                EditLayerForm form = new EditLayerForm();
                if (form.OpenEdit(((StageLayer)this._lstLayers.SelectedItem)._layer) == System.Windows.Forms.DialogResult.OK)
                {
                    this._lstLayers.Items[this._lstLayers.SelectedIndex] = new StageLayer(form.Layer);
                }
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
                if (this._lstLayers.SelectedItem == null)
                {
                    return;
                }
                if (Diag.Confirm(this, "Delete selected layer?"))
                {
                    this._lstLayers.Items.Remove(this._lstLayers.SelectedItem);
                    this.EnableButtons();
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._lstLayers.SelectedIndex <= 0)
                {
                    return;
                }
                StageLayer layer = (StageLayer)this._lstLayers.SelectedItem;
                this._lstLayers.Items[this._lstLayers.SelectedIndex] = this._lstLayers.Items[this._lstLayers.SelectedIndex - 1];
                this._lstLayers.Items[this._lstLayers.SelectedIndex - 1] = layer;
                this._lstLayers.SelectedItem = layer;

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
        private void _btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._lstLayers.SelectedIndex >= this._lstLayers.Items.Count - 1 ||
                    this._lstLayers.SelectedIndex == -1)
                {
                    return;
                }
                StageLayer layer = (StageLayer)this._lstLayers.SelectedItem;
                this._lstLayers.Items[this._lstLayers.SelectedIndex] = this._lstLayers.Items[this._lstLayers.SelectedIndex + 1];
                this._lstLayers.Items[this._lstLayers.SelectedIndex + 1] = layer;
                this._lstLayers.SelectedItem = layer;

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
            this._btnUp.Enabled = (this._lstLayers.SelectedItem != null);
            this._btnDown.Enabled = (this._lstLayers.SelectedItem != null);
            this._btnEdit.Enabled = (this._lstLayers.SelectedItem != null);
            this._btnDelete.Enabled = (this._lstLayers.SelectedItem != null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lstLayers_SelectedIndexChanged(object sender, EventArgs e)
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

        private void _lblBuildNr_Click(object sender, EventArgs e)
        {

        }
      

    }
}
