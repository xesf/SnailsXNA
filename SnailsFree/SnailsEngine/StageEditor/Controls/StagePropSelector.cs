using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.Stages;
using System.Collections;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    public partial class StagePropSelector : UserControl
    {
        public event EventHandler SelectedPropChanged;

        public StageProp SelectedProp
        {
            get
            {
                if (this._propsList.SelectedItem == null)
                    return null;

                if (this._propsList.SelectedItem is ImageListItemSetPiece == false)
                    return null;

                return ((ImageListItemSetPiece)this._propsList.SelectedItem).Prop;
            }

            set
            {
                if (this._propsList.SelectedItem != null)
                {
                    this._propsList.SelectedItem.Selected = false;
                }
                this._propsList.SelectedItem = null;
            }
        }

        public StagePropSelector()
        {
            InitializeComponent();
        }

        private void _imgList_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void OnSelectedPropChanged()
        {
            if (this.SelectedPropChanged != null)
            {
                this.SelectedPropChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _propsList_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                this.OnSelectedPropChanged();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }
    }
}
