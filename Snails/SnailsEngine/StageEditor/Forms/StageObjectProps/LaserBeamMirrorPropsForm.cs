using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    public partial class LaserBeamMirrorPropsForm : ObjectPropsBaseForm
    {
        LaserBeamMirror Mirror
        { get { return (LaserBeamMirror)this.GameObject; } }

        public LaserBeamMirrorPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._numMirrorAngle.Minimum = (decimal) LaserBeamMirror.MIN_ROTATION;
            this._numMirrorAngle.Maximum = (decimal) LaserBeamMirror.MAX_ROTATION;

            this._numMirrorAngle.Value = (decimal)this.Mirror.MirrorRotation;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Mirror.MirrorRotation = (float)this._numMirrorAngle.Value;
        }
    }
}
