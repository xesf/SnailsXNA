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
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    public partial class LampPropsForm  : ObjectPropsBaseForm
    {
        Lamp Lamp
        { get { return (Lamp)this.GameObject; } }

        public LampPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LampPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._rbOn.Checked = (this.Lamp.IsOn);
                this._rbOff.Checked = (this.Lamp.IsOff);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LampPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Lamp.State = (this._rbOn.Checked ?
                                   LightSource.LightState.On :
                                   LightSource.LightState.Off);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
