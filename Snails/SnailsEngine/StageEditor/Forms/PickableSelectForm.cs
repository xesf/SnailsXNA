using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class PickableSelectForm : ObjectSelectedBaseForm
    {
        public PickableObject PickObject { get; set; }

        public PickableObject.PickableType SelectedType
        {
            get
            {
                if (this._lstPickTypes.SelectedItem == null)
                    return PickableObject.PickableType.Proxy;

                return (PickableObject.PickableType)this._lstPickTypes.SelectedItem;
            }
        }

        public PickableSelectForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PickableSelectForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.PickObject = (PickableObject)(this.CurrentSelectedObject.Clone());
                foreach (PickableObject.PickableType pickType in Enum.GetValues(typeof(PickableObject.PickableType)))
                {
                    this._lstPickTypes.Items.Add(pickType);
                }

                if (this._lstPickTypes.Items.Count > 0)
                    this._lstPickTypes.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlImage_Paint(object sender, PaintEventArgs e)
        {
#if STAGE_EDITOR
            try
            {
                if (this.PickObject != null)
                {
                    Microsoft.Xna.Framework.Rectangle frameRect = this.PickObject.Sprite.Frames[0].Rect;
                    Rectangle rc = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
                    e.Graphics.DrawImage(this.PickObject.Sprite.Image,
                                         0,
                                         0, rc, GraphicsUnit.Pixel);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lstPickTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.PickObject != null)
                {
                    this.PickObject.SetPickableType((PickableObject.PickableType)this._lstPickTypes.SelectedItem);
                    this._pnlImage.Refresh();
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
        private void _btnOk_Click(object sender, EventArgs e)
        {
            ((PickableObject)(this.CurrentSelectedObject)).SetPickableType(this.SelectedType);
        }
    }
}
