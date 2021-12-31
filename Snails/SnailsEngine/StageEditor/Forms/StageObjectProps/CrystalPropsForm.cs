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

namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    public partial class CrystalPropsForm : ObjectPropsBaseForm
    {
        private Crystal Crystal
        {
            get { return (Crystal)this.GameObject; }
        }
        public CrystalPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CrystalPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._cmbColor.SelectedItem = this.Crystal.CristalColor;
                this._cmbSize.SelectedItem = this.Crystal.CristalSize;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CrystalPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Crystal.CristalColor = (TwoBrainsGames.Snails.StageObjects.Crystal.CrystalColorType)this._cmbColor.SelectedItem;
                this.Crystal.CristalSize = (TwoBrainsGames.Snails.StageObjects.Crystal.CrystalSizeType)this._cmbSize.SelectedItem;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
