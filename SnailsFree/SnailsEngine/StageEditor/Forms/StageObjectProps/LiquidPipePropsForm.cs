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
    public partial class LiquidPipePropsForm : ObjectPropsBaseForm
    {
        private LiquidPipe Pipe
        {
            get { return (LiquidPipe)this.GameObject; }
        }

        public LiquidPipePropsForm()
        {
            InitializeComponent();
        }

        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Pipe.PipeString = this._pipeString.Text;
            this.Pipe.PumpAttachment = (LiquidPipe.PipeLinkType)this._cmbPumpAttachment.SelectedItem;
            this.Pipe.Terminator = (LiquidPipe.PipeLinkType)this._cmbTerminator.SelectedItem;
        }

        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._pipeString.Text = this.Pipe.PipeString;
            this._cmbPumpAttachment.SelectedItem = this.Pipe.PumpAttachment;
            this._cmbTerminator.SelectedItem = this.Pipe.Terminator;
        }

        private void LiquidPipePropsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
