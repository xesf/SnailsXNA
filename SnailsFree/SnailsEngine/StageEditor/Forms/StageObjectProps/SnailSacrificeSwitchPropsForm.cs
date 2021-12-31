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

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class SnailSacrificeSwitchPropsForm : ObjectPropsBaseForm
    {

        SnailSacrificeSwitch Switch
        { get { return (SnailSacrificeSwitch)this.GameObject; } }

        public SnailSacrificeSwitchPropsForm()
        {
            InitializeComponent();
        }


        private void _btnOk_Click(object sender, EventArgs e)
        {

        }

        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._numSnailsNeeded.Value = this.Switch.SnailsToSacrifice;
            this._cmbAction.SelectedItem = this.Switch.SwitchOnAction;
        }

        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Switch.SnailsToSacrifice = (int)this._numSnailsNeeded.Value;
            this.Switch.SwitchOnAction = (Switch.SwitchOnActionType)this._cmbAction.SelectedItem;
        }
    }
}
