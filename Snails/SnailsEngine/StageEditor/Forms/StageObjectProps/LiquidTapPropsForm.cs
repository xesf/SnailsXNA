using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    public partial class LiquidTapPropsForm : ObjectPropsBaseForm
    {
        private LiquidTap Tap
        {
            get { return (LiquidTap)this.GameObject; }
        }

        public LiquidTapPropsForm()
        {
            InitializeComponent();
        }

        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Tap.PumpSpeed = (float)this._numPumpSpeed.Value;
            this.Tap.OpenDirection = (LiquidTap.TapOpenDirection)this._cmbOpendDirection.SelectedIndex;
            this.Tap.SignToShow = (LiquidTap.TapSignToShow)this._cmbSignToShow.SelectedIndex;
        }

        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            if ((decimal)this.Tap.PumpSpeed >= this._numPumpSpeed.Minimum &&
                (decimal)this.Tap.PumpSpeed <= this._numPumpSpeed.Maximum)
            {
                this._numPumpSpeed.Value = (decimal)this.Tap.PumpSpeed;
            }

            this._cmbOpendDirection.SelectedIndex = (int)this.Tap.OpenDirection;
            this._cmbSignToShow.SelectedIndex = (int)this.Tap.SignToShow;
        }

        private void LiquidTapPropsForm_Load(object sender, EventArgs e)
        {
            this._numPumpSpeed.Minimum = (decimal)LiquidSwitch.MIN_PUMP_SPEED;
            this._numPumpSpeed.Maximum = (decimal)LiquidSwitch.MAX_PUMP_SPEED;

        }

    }
}
