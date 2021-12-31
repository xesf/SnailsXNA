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
    public partial class SimpleSwitchPropsForm : ObjectPropsBaseForm
    {
        SnailTriggerSwitch Switch
        { get { return (SnailTriggerSwitch)this.GameObject; } }

        public SimpleSwitchPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SimpleSwitchPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._rbOn.Checked = (this.Switch.IsOn);
                this._rbOff.Checked = (this.Switch.IsOff);

            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SimpleSwitchPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Switch.State = (this._rbOn.Checked? 
                                        TwoBrainsGames.Snails.StageObjects.Switch.SwitchState.On : 
                                        TwoBrainsGames.Snails.StageObjects.Switch.SwitchState.Off);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
