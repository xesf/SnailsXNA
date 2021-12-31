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
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class EditLayerForm : BaseForm
    {
        public SnailsBackgroundLayer Layer
        {
            get;
            private set;
        }

        private Vector2 Position
        {
            get
            {
                float x;
                float.TryParse(this._txtX.Text, out x);
                float y;
                float.TryParse(this._txtY.Text, out y);

                return new Vector2(x, y);
            }
            set
            {
                this._txtX.Text = value.X.ToString();
                this._txtY.Text = value.Y.ToString();
            }
        }

        private float Speed
        {
            get
            {
                float f;
                float.TryParse(this._txtSpeed.Text, out f);
               

                return f;
            }
            set
            {
                this._txtSpeed.Text = value.ToString();
            }
        }

        private LayerType LayerType
        {
            get
            {
                return (LayerType) this._cmbType.SelectedItem;
            }
            set
            {
                this._cmbType.SelectedItem = value;
            }
        }

        private string Id
        {
            get
            {
                return ((SnailsBackgroundLayer)this._cmbId.SelectedItem).Id;
            }
            set
            {
                if (value != null)
                {
                    SnailsBackgroundLayer layer = StageEditor.StageData.GetLayer(value);
                    foreach (SnailsBackgroundLayer l in this._cmbId.Items)
                    {
                        if (layer.Id == l.Id)
                        {
                            this._cmbId.SelectedItem = l;
                            break;
                        }
                    }
                }
                else
                {
                    this._cmbId.SelectedItem = null;
                }
            }
        }

        public EditLayerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult OpenNew()
        {
            this.Layer = new SnailsBackgroundLayer();
            return this.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult OpenEdit(SnailsBackgroundLayer layer)
        {
            this.Layer = layer.Clone();
            return this.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditLayerForm_Load(object sender, EventArgs e)
        {
            try
            {
               this.RefreshProperties();
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
                if (this._cmbId.SelectedItem == null)
                {
                    this._cmbId.Focus();
                    Diag.ShowFieldInputValidationError(this, "Please select layer Id.");
                    return;
                }
                if (this._cmbType.SelectedItem == null)
                {
                    this._cmbType.Focus();
                    Diag.ShowFieldInputValidationError(this, "Please select layer type.");
                    return;
                }

                this.Layer._position = this.Position;
                this.Layer._speed = this.Speed;
                this.Layer._layerType = this.LayerType;
                this.Layer.Id = this.Id;
                this.DialogResult = DialogResult.OK;
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
        private void RefreshProperties()
        {
            this.Position = this.Layer._position;
            this.Id = this.Layer.Id;
            this.Speed = this.Layer._speed;
            this.LayerType = this.Layer._layerType;
        }
    }
}
