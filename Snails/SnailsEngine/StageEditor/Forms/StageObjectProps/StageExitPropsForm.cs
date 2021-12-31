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
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class StageExitPropsForm : ObjectPropsBaseForm
    {
        StageExit Exit
        { get { return (StageExit)this.GameObject; } }

        /// <summary>
        /// 
        /// </summary>
        public StageExitPropsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        private void StageExitPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._cmbSnailCounters.LoadValues(StageObjectType.SnailCounter);
                if (this.Exit.SnailCounter != null)
                {
                    this._cmbSnailCounters.SelectedItem = this.Exit.SnailCounter;
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StageExitPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Exit.SetSnailCounter(this._cmbSnailCounters.SelectedItem as SnailCounter);
               
                this.EditorStage.Changed = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _btnOk_Click(object sender, EventArgs e)
        {

        }

    }
}
