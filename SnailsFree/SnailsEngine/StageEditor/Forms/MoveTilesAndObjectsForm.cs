using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using LevelEditor.Forms;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class MoveTilesAndObjectsForm : BaseForm
    {
        public int OffsetX { get { return (int)this._numX.Value; } }
        public int OffsetY { get { return (int)this._numY.Value; } }

        EditorStage Stage { get; set; }

        public MoveTilesAndObjectsForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, EditorStage stage)
        {
            this.Stage = stage;
            return this.ShowDialog(owner);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                int moveXInPixels = this.OffsetX * Stage.TileSize.Width;
                int moveYInPixels = this.OffsetY * Stage.TileSize.Height;

                if (this.Stage.CheckAnyObjectOrTileOutOfBoard(moveXInPixels, moveYInPixels))
                {
                    if (MessageBox.Show(this.ParentForm, "There are objects out of the board bounds. This objects will be deleted.\nProceed?", StageEditor.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                        DialogResult.Cancel)
                    {
                        return;
                    }
                }

                // Aldrabar...
                MainForm.StageEditingMode currentEditingMode = MainForm._instance.EditingMode;

                // Stage
                MainForm._instance.EditingMode = MainForm.StageEditingMode.Stage;
                List<Object> allObjs = this.Stage.GetAllObjectsAndTiles();
                this.Stage.MoveObjectsAndTiles(allObjs, moveXInPixels, moveYInPixels, this.OffsetX, this.OffsetY);

                // Hints
                if (this.Stage.Stage.HintManager.Hints.Count > 0)
                {
                    MainForm._instance.EditingMode = MainForm.StageEditingMode.Hints;
                    allObjs = this.Stage.GetAllObjectsAndTiles();
                    this.Stage.MoveObjectsAndTiles(allObjs, moveXInPixels, moveYInPixels, this.OffsetX, this.OffsetY);
                }

                MainForm._instance.EditingMode = currentEditingMode;
                this.Close();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
