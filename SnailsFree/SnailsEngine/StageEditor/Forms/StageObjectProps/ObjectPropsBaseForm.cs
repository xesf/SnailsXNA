using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.StageEditor.Controls;
using Microsoft.Xna.Framework.Graphics;
using LevelEditor.Forms;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class ObjectPropsBaseForm : BaseForm
    {
        protected EditorStage EditorStage { get; set; }
        protected StageObject GameObject { get; set; }
        protected BaseComboBox SpriteEffectCombo { get; set; }
        protected BaseComboBox RotationCombo { get; set; }
        private int _heightAfterPropsHide;

        /// <summary>
        /// 
        /// </summary>
        public ObjectPropsBaseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DialogResult ShowDialog(IWin32Window owner, EditorStage stage, StageObject obj)
        {
          /*  if (obj is PickableObject)
            {
                PickablePropsForm form = new PickablePropsForm();
                form.ShowDialog(owner, stage, (PickableObject)obj);
            }
            */
            this.GameObject = obj;
            this.EditorStage = stage;
            return this.ShowDialog(owner);
        }


        /// <summary>
        /// 
        /// </summary>
        private void ObjectPropsBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this._txtUniqueId != null)
                {
                    if (string.IsNullOrEmpty(this._txtUniqueId.Text))
                    {
                        e.Cancel = true;
                        throw new ApplicationException("Objects Id is mandatory.");
                    }

                    if (this.EditorStage.ObjectIdIsAvailable(this._txtUniqueId.Text, this.GameObject) == false)
                    {
                        e.Cancel = true;
                        throw new ApplicationException("Objects Ids must be unique.");
                    }
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
        private void ObjectPropsBaseForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._heightAfterPropsHide = this._gbCommon.Height;

                this.InitControls();
                if (this.GameObject != null)
                {
                    // Hide/show properties

                    this._pnlSpriteEffect.Visible = this.GameObject.EditorBehaviour.PropertiesForm.ShowSpriteEffect;
                    this._pnlLinks.Visible = this.GameObject.EditorBehaviour.PropertiesForm.ShowLinks;
                    this._pnlRotation.Visible = this.GameObject.EditorBehaviour.PropertiesForm.ShowRotation;

                    this._txtUniqueId.Text = this.GameObject.UniqueId;
                    this._cbSpriteEffect.SelectedItem = this.GameObject.SpriteEffect;
                    this._numRotation.Value = ((decimal)this.GameObject.Rotation > 180 ? (decimal)this.GameObject.Rotation - 360 : (decimal)this.GameObject.Rotation);

                    // Adjust group box size to fit visible items
                    this._gbCommon.Height = this._pnlId.Height + 20;
                    this._gbCommon.Height += (this._pnlSpriteEffect.Visible ? this._pnlSpriteEffect.Height : 0);
                    this._gbCommon.Height += (this._pnlLinks.Visible ? this._pnlLinks.Height : 0);
                    this._gbCommon.Height += (this._pnlRotation.Visible ? this._pnlRotation.Height : 0);

                    if (this.GameObject.EditorBehaviour.PropertiesForm.ShowLinks)
                    {
                        if (this.GameObject.EditorBehaviour.PropertiesForm.LinksTypeFilter != null)
                        {
                            this._objLinks.ObjectTypeFilter = this.GameObject.EditorBehaviour.PropertiesForm.LinksTypeFilter;
                        }
                        this._objLinks.SetObjectList(this.GameObject.LinkedObjects);
                    }
                }

                this.OnLoadValues();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ObjectPropsBaseForm_Shown(object sender, EventArgs e)
        {
            try
            {
                this._txtUniqueId.Focus();
               
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height - this._heightAfterPropsHide + this._gbCommon.Height);
                if (this._gbProps.HasChildren == false && !this.DesignMode)
                {
                    this._gbProps.Visible = false;
                    this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height - this._gbProps.Height);
                }
                if (this.GameObject != null)
                {
                    this._gbProps.Text = (this.GameObject.GetType().Name + " Properties");
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
        private void ObjectPropsBaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this.GameObject != null)
                {
                    this.GameObject.UniqueId = this._txtUniqueId.Text;
                    this.GameObject.SpriteEffect = (SpriteEffects)this._cbSpriteEffect.SelectedItem;
                    this.GameObject.SetLinkedObjects(this._objLinks.GetObjectList());
                    this.GameObject.Rotation = ((int)this._numRotation.Value < 0 ? (int)this._numRotation.Value + 360 : (int)this._numRotation.Value);
//                    if (this.RotationCombo != null)
//                        this.GameObject.Rotation = (float)this.RotationCombo.SelectedItem;
                }

                this.OnSaveValues();
                if (this.EditorStage != null)
                    this.EditorStage.Changed = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ObjectPropsBaseForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 27)
                    this.Close();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            } 
        }

        private void InitControls()
        {
            foreach (Control ctl in this.Controls)
            {
                this.InitControl(ctl);
            }
        }

        private void InitControl(Control ctl)
        {
            if (ctl is BaseComboBox)
            {
                BaseComboBox cb = (BaseComboBox)ctl;
                switch (cb.ComboType)
                {
           /*         case BaseComboBox.ComboBoxType.SpriteEffect:
                        cb.SelectedItem = this.GameObject.SpriteEffect;
                        this.SpriteEffectCombo = cb;
                        break;*/
                    case BaseComboBox.ComboBoxType.Rotation:
                        cb.SelectedItem = this.GameObject.Rotation;
                        this.RotationCombo = cb;
                        break;
                }
            }

            foreach (Control ctl1 in ctl.Controls)
            {
                this.InitControl(ctl1);
            }
         }

        protected virtual void OnLoadValues() { }
        protected virtual void OnSaveValues() { }
        protected virtual bool OnValidate() { return true; }

        private void _btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.OnValidate())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            } 
        }

    }
}
