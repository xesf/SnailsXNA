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
    public partial class LiquidPumpPropsForm : ObjectPropsBaseForm
    {
        private LiquidPump Pump
        {
            get { return (LiquidPump)this.GameObject; }
        }

        public LiquidPumpPropsForm()
        {
            InitializeComponent();
        }

        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Pump.PumpSpeed = (float)this._numPumpSpeed.Value;
            this.Pump.PumpType = (this._rbPumpingIn.Checked ? LiquidPump.PumpTypes.PumpIn : LiquidPump.PumpTypes.PumpOut);
        }

        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            if ((decimal)this.Pump.PumpSpeed >= this._numPumpSpeed.Minimum &&
                (decimal)this.Pump.PumpSpeed <= this._numPumpSpeed.Maximum)
            {
                this._numPumpSpeed.Value = (decimal)this.Pump.PumpSpeed;
            }
            this._rbPumpingIn.Checked = (this.Pump.PumpType == LiquidPump.PumpTypes.PumpIn);
            this._rbPumpingOut.Checked = (this.Pump.PumpType == LiquidPump.PumpTypes.PumpOut);
        }

        private void LiquidPumpPropsForm_Load(object sender, EventArgs e)
        {
            this._numPumpSpeed.Minimum = (decimal)LiquidSwitch.MIN_PUMP_SPEED;
            this._numPumpSpeed.Maximum = (decimal)LiquidSwitch.MAX_PUMP_SPEED;
        }

    }
}
