using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    partial class ImageListItemSetPiece : ImageListxItem
    {
        StageProp _prop;
        public StageProp Prop
        {
            get { return this._prop; }
            set
            {
                if (this._prop != value)
                {
                    this._prop = value;
                    this.Width = this.FrameRect.Width + (ImageListxItem.LEFT_MARGIN * 2);
                    this.Height = this.FrameRect.Height + (ImageListxItem.TOP_MARGIN * 2);
                }
            }
        }

        public ImageListItemSetPiece()
        {
            InitializeComponent();
        }

       
        /// <summary>
        /// 
        /// </summary>
        public ImageListItemSetPiece(StagePropToolboxItem objToolboxItem)
        {
            this.ToolboxItem = objToolboxItem;
            this.Prop = objToolboxItem.Prop;
        }
    }
}
