using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor.Forms;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    // This is the base form to be used in forms that are displayed when
    // a object is selected from the toolbar
    // Pickable objects and StageProps are examples that use this feature
    public partial class ObjectSelectedBaseForm : BaseForm
    {
        protected StageObject CurrentSelectedObject { get; set; }

        public ObjectSelectedBaseForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, StageObject selectedObject)
        {
            this.CurrentSelectedObject = selectedObject;
            return base.ShowDialog(owner);
        }
    }
}
