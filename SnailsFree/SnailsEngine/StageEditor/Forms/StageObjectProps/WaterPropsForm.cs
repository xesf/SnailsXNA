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
    public partial class WaterPropsForm : ObjectPropsBaseForm
    {
        private Liquid Liquid
        {
            get { return (Liquid)this.GameObject; }
        }

        public WaterPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._txtSizeX.Value = (int)this.Liquid.Size.X;
            this._txtSizeY.Value = (int)this.Liquid.Size.Y;

            if ((decimal)this.Liquid.LiquidLevel >= this._numLevel.Minimum &&
                (decimal)this.Liquid.LiquidLevel <= this._numLevel.Maximum)
            {
                this._numLevel.Value = (decimal)this.Liquid.LiquidLevel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Liquid.Size = new Microsoft.Xna.Framework.Vector2((int)this._txtSizeX.Value, (int)this._txtSizeY.Value);
            this.Liquid.LiquidLevel = (float)this._numLevel.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override bool OnValidate()
        {
            if (this._numLevel.Value < (decimal)Liquid.LIQUID_LEVEL_EMPTY ||
                this._numLevel.Value > (decimal)Liquid.LIQUID_LEVEL_FULL)
            {
                MessageBox.Show(this, "Liquid level must be between 0.0 and 1.0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void WaterPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._numLevel.Minimum = (decimal)Liquid.LIQUID_LEVEL_EMPTY;
                this._numLevel.Maximum = (decimal)Liquid.LIQUID_LEVEL_FULL;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            } 
        }
    }
}
