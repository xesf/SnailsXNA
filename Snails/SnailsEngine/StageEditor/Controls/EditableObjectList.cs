using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.StageEditor.Forms;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    /// <summary>
    /// This is a listbox with add/remove buttons to build a list of objects
    /// This list can be used to set object links on a SnailSwith for instance
    /// When the Add button is pressed, a pop up with a list of objects in the current stage
    /// is displayed to select objects and add them to the list
    /// A filter by type can be used in this dialog
    /// </summary>
    public partial class EditableObjectList : UserControl
    {
        [Browsable(false)]
        public Type ObjectTypeFilter { get; set; }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Bindable(true)]
        public override string Text
        {
            get { return this._gpContainer.Text; }
            set { this._gpContainer.Text = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public EditableObjectList()
        {
            InitializeComponent();
            this.EnableButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnAdd_Click(object sender, EventArgs e)
        {
            StageObjectsSelectorForm form = new StageObjectsSelectorForm();
            form.TypeFilter = this.ObjectTypeFilter;
            form.ObjectsToExclude = this.GetObjectList();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this._objList.SelectedItems.Clear();
                foreach (StageObject obj in form.SelectedObjects)
                {
                    this._objList.Items.Add(obj);
                }
                this.EnableButtons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnRemove_Click(object sender, EventArgs e)
        {
            if (this._objList.SelectedItem == null)
            {
                return;
            }
            this._objList.Items.Remove(this._objList.SelectedItem);
            this.EnableButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnClear_Click(object sender, EventArgs e)
        {
            this._objList.Items.Clear();
            this.EnableButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableButtons()
        {
            this._btnClear.Enabled = (this._objList.Items.Count > 0);
            this._btnRemove.Enabled = (this._objList.SelectedItem != null);
        }

        public List<StageObject> GetObjectList()
        {
            List<StageObject> list = new List<StageObject>();
            foreach (StageObject obj in this._objList.Items)
            {
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetObjectList(List<StageObject> list)
        {
            this._objList.Items.Clear();
            foreach (StageObject obj in list)
            {
                
                this._objList.Items.Add(obj);
            }

        }

        private void _objList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }

    }
}
