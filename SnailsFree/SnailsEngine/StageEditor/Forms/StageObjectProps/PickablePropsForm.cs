using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class PickablePropsForm :  ObjectPropsBaseForm
    {
        PickableObject PickableObject { get { return (PickableObject)base.GameObject; } }
        public PickablePropsForm()

        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PickablePropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._numQuantity.Value = (int)this.PickableObject.Quantity;
                this._numQuantity.Enabled = this.PickableObject.QueryIsTool();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PickablePropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.PickableObject.Quantity = (int)this._numQuantity.Value;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _btnOk_Click(object sender, EventArgs e)
        {

        }

    }
}
