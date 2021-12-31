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

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class SnailCounterPropsForm : ObjectPropsBaseForm
    {
        private SnailCounter SnailCounter
        {
            get { return (SnailCounter)this.GameObject; }
        }
        
        public SnailCounterPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SnailCounterPropsForm_Load(object sender, EventArgs e)
        {
            try
            {

                List<StageObject> list = this.EditorStage.GetObjectsByType(typeof(StageEntrance));
                this.EditorStage.GetObjectsByType(typeof(StageExit), list);
                foreach (StageObject obj in list)
                {
                    this._cmbLinkTo.Items.Add(obj);
                    if (this.SnailCounter.LinkedObjects.Contains(obj))
                    {
                        this._cmbLinkTo.SelectedItem = obj;
                    }
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
        private void SnailCounterPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this._cmbLinkTo.SelectedItem != null)
                {
                    if (this.SnailCounter.LinkedObjects.Count > 0)
                    {
                        this.SnailCounter.RemoveLinks();
                    }
                    this.SnailCounter.LinkedObjects.Clear();
                    if (this._cmbLinkTo.SelectedItem != null)
                    {
                        if (this._cmbLinkTo.SelectedItem is StageEntrance)
                        {
                            ((StageEntrance)this._cmbLinkTo.SelectedItem).SetSnailCounter(this.SnailCounter);
                        }
                        else
                        if (this._cmbLinkTo.SelectedItem is StageExit)
                        {
                            ((StageExit)this._cmbLinkTo.SelectedItem).SetSnailCounter(this.SnailCounter);
                        }
                    }
                }
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
