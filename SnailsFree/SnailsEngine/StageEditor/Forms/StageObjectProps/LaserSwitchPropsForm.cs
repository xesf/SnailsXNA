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
    public partial class LaserSwitchPropsForm : ObjectPropsBaseForm
    {
        private LaserBeamSwitch LaserSwitch
        {
            get { return (LaserBeamSwitch)this.GameObject; }
        }

        public LaserSwitchPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._cmbColor.SelectedItem = this.LaserSwitch.LaserColor;
            this._cmbAction.SelectedItem = this.LaserSwitch.SwitchOnAction;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.LaserSwitch.LaserColor = (LaserBeam.LaserBeamColor)this._cmbColor.SelectedItem;
            this.LaserSwitch.SwitchOnAction = (Switch.SwitchOnActionType)this._cmbAction.SelectedItem;
        }
    }
}
