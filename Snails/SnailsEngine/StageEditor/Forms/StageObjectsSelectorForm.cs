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
using LevelEditor.Forms;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    /// <summary>
    /// A form that displays a list of the objects that are in the current stage and
    /// allows to select one or more objects from it
    /// It can have a filter by Type and by a list of objects to exclude from the list
    /// </summary>
    public partial class StageObjectsSelectorForm : BaseForm
    {

        public List<StageObject> ObjectsToExclude { get; set; }
        public Type TypeFilter { get; set; }
        public List<StageObject> SelectedObjects { get; private set; }

        public StageObjectsSelectorForm()
        {
            InitializeComponent();
            this.SelectedObjects = new List<StageObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StageObjectsSelectorForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.RefreshList();
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshList()
        {
            foreach (StageObject obj in MainForm.CurrentStageEdited.Stage.Objects)
            {
                if (this.ShouldAddObject(obj))
                {
                    this._objList.Items.Add(obj);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool ShouldAddObject(StageObject obj)
        {
            if (this.ObjectsToExclude != null)
            {
                if (this.ObjectsToExclude.Contains(obj))
                {
                    return false;
                }
            }

            if (this.TypeFilter != null)
            {
                if (obj.GetType() == this.TypeFilter)
                {
                    return true;
                }

                if (this.TypeFilter.IsAssignableFrom(obj.GetType()))
                {
                    return true;
                }

                if (this.TypeFilter.IsInterface)
                {
                    if (obj.GetType().GetInterface(this.TypeFilter.FullName) != null)
                    {
                        return true;
                    }
                }
            }
   

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._btnSelect.Enabled = (this._objList.SelectedItems.Count > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnSelect_Click(object sender, EventArgs e)
        {
            this.SelectedObjects.Clear();
            foreach (StageObject obj in this._objList.SelectedItems)
            {
                this.SelectedObjects.Add(obj);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void _objList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }
    }
}
