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
    public partial class ControllableLaserCannonForm : LaserCannonBasePropsForm
    {
        ControllableLaserCannon ControllableLaser
        { get { return (ControllableLaserCannon)this.GameObject; } }
        public ControllableLaserCannonForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._numCannonAngle.Minimum = (decimal)ControllableLaserCannon.MIN_ROTATION;
            this._numCannonAngle.Maximum = (decimal)ControllableLaserCannon.MAX_ROTATION;

            this._numCannonAngle.Value = (decimal)this.ControllableLaser.CannonRotation;
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.ControllableLaser.CannonRotation = (float)this._numCannonAngle.Value;
        }
    }
}
