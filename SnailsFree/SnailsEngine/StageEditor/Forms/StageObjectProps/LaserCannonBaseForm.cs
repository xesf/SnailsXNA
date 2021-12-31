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
    public partial class LaserCannonBasePropsForm : ObjectPropsBaseForm
    {
        LaserCannonBase LaserCannon
        { get { return (LaserCannonBase)this.GameObject; } }

        public LaserCannonBasePropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            try
            {
                this._gbBlink.Enabled = this._chkWithBlink.Checked;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues() 
        {
            if (this.LaserCannon != null)
            {
                this._rbOn.Checked = this.LaserCannon.TurnedOn;
                this._rbOff.Checked = !this.LaserCannon.TurnedOn;
                this._chkWithBlink.Checked = this.LaserCannon.WithBlink;
                this._numBlinkOn.Value = (decimal)this.LaserCannon.BlinkTimeOn;
                this._numBlinkOff.Value = (decimal)this.LaserCannon.BlinkTimeOff;
                this._cmbColor.SelectedItem = this.LaserCannon.LaserColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues() 
        {
            this.LaserCannon.TurnedOn = this._rbOn.Checked;
            this.LaserCannon.BlinkTimeOn = (double)this._numBlinkOn.Value;
            this.LaserCannon.BlinkTimeOff = (double)this._numBlinkOff.Value;
            this.LaserCannon.WithBlink = this._chkWithBlink.Checked;
            this.LaserCannon.LaserColor = (LaserBeam.LaserBeamColor)this._cmbColor.SelectedItem;
        }
    }
}
