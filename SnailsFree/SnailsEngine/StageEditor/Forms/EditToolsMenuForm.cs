using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.StageEditor.Controls;
using TwoBrainsGames.Snails.ToolObjects;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class EditToolsMenuForm : Form
    {
        EditorStage EditorStage { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public EditToolsMenuForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult ShowDialog(IWin32Window owner, EditorStage stage)
        {
            this.EditorStage = stage;
            return this.ShowDialog(owner);
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditToolsMenuForm_Load(object sender, EventArgs e)
        {
            try
            {
                int x = this._pnlTools.Margin.Left, y = this._pnlTools.Margin.Top;
                // Create icons for all existing tools
                foreach (ToolObject obj in this.EditorStage.Stage.StageData.Tools.Values)
                {
                    ToolCtl toolCtl = new ToolCtl();
                    toolCtl.Tool = obj.Clone();
                    toolCtl.Left = x;
                    toolCtl.Top = y;
                    toolCtl.Tool.UpdateBoundingBox();
                    toolCtl.ToolQuantityChanged += new EventHandler(toolCtl_ToolQuantityChanged);
                    this._pnlTools.Controls.Add(toolCtl);

                    x += toolCtl.Width + this._pnlTools.Margin.Left;
                    if (x > this._pnlTools.Width - 10)
                    {
                        y += toolCtl.Height + this._pnlTools.Margin.Top;
                        x = 5;
                    }
                }

                // Initialize quantities from the stage
                foreach (ToolObject obj in this.EditorStage.Stage.StageHUD._toolsMenu.Tools)
                {
                    this.SetQuantity(obj);
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
        private void SetQuantity(ToolObject obj)
        {
            foreach (ToolCtl ctl in this._pnlTools.Controls)
            {
                if (ctl.Tool.Id == obj.Id)
                {
                    ctl.Quantity = obj.Quantity;
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void toolCtl_ToolQuantityChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.EditorStage != null)
                    this.EditorStage.Changed = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditToolsMenuForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (this.EditorStage == null)
                    return;

                this.EditorStage.Stage.StageHUD._toolsMenu.Tools.Clear();

                foreach (ToolCtl ctl in this._pnlTools.Controls)
                {
                    if (ctl.Tool.Quantity > 0)
                    {
                        this.EditorStage.Stage.StageHUD._toolsMenu.Tools.Add(ctl.Tool.Clone());
                    }
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
