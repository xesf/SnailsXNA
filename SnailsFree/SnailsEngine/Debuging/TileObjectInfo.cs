using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using System.Windows.Forms;

namespace TwoBrainsGames.Snails.Debuging
{
    public partial class TileObjectInfo : UserControl
    {
        StageObject _TileObject;
        public StageObject TileObject
        {
            get { return this._TileObject; }
            set
            {
                this._TileObject = value;
                if (this._TileObject != null)
                {
                    this._txtLeft.Text = this._TileObject.Position.X.ToString();
                    this._TxtTop.Text = this._TileObject.Position.Y.ToString();
                }
                else
                {
                    this._txtLeft.Text = "";
                    this._TxtTop.Text = "";
                }
            }
        }

        public TileObjectInfo()
        {
            InitializeComponent();
        }
    }
}
