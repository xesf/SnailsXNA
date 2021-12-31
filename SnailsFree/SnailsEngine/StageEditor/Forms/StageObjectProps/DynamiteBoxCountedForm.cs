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
    public partial class DynamiteBoxCountedForm : ObjectPropsBaseForm
    {
        DynamiteBoxCounted Box
        { get { return (DynamiteBoxCounted)this.GameObject; } }


        public DynamiteBoxCountedForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            this._numSnailsAllowed.Value = Box.SnailsAllowed;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            Box.SnailsAllowed = (int)this._numSnailsAllowed.Value;
        }
    }
}
