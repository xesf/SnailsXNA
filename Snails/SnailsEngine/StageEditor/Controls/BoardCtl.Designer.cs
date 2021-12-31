namespace LevelEditor.Controls
{
  partial class BoardCtl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this._SelectionTimer = new System.Windows.Forms.Timer(this.components);
            this._mnuStageObject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._optStageObjProps = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._optStageObjRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._optSendToBack = new System.Windows.Forms.ToolStripMenuItem();
            this._optBringToFront = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._optDelStageObj = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuMultipleSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._optMultipleRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._optMultipleDelete = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuTileSelected = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._optTileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this._grpBoard = new TwoBrains.Common.Controls.Group();
            this._pnlBoard = new TwoBrainsGames.Snails.StageEditor.Controls.Panelx();
            this._mnuStageObject.SuspendLayout();
            this._mnuMultipleSelected.SuspendLayout();
            this._mnuTileSelected.SuspendLayout();
            this._grpBoard.BodyPanel.SuspendLayout();
            this._grpBoard.SuspendLayout();
            this.SuspendLayout();
            // 
            // _SelectionTimer
            // 
            this._SelectionTimer.Interval = 250;
            this._SelectionTimer.Tick += new System.EventHandler(this._SelectionTimer_Tick);
            // 
            // _mnuStageObject
            // 
            this._mnuStageObject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optStageObjProps,
            this.toolStripSeparator3,
            this._optStageObjRotate,
            this.toolStripSeparator1,
            this._optSendToBack,
            this._optBringToFront,
            this.toolStripSeparator2,
            this._optDelStageObj});
            this._mnuStageObject.Name = "_mnuStageObject";
            this._mnuStageObject.Size = new System.Drawing.Size(150, 132);
            this._mnuStageObject.Opening += new System.ComponentModel.CancelEventHandler(this._mnuStageObject_Opening);
            // 
            // _optStageObjProps
            // 
            this._optStageObjProps.Name = "_optStageObjProps";
            this._optStageObjProps.Size = new System.Drawing.Size(149, 22);
            this._optStageObjProps.Text = "Properties...";
            this._optStageObjProps.Click += new System.EventHandler(this._optStageObjProps_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(146, 6);
            // 
            // _optStageObjRotate
            // 
            this._optStageObjRotate.Name = "_optStageObjRotate";
            this._optStageObjRotate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this._optStageObjRotate.Size = new System.Drawing.Size(149, 22);
            this._optStageObjRotate.Text = "Rotate";
            this._optStageObjRotate.Click += new System.EventHandler(this._optStageObjRotate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // _optSendToBack
            // 
            this._optSendToBack.Name = "_optSendToBack";
            this._optSendToBack.Size = new System.Drawing.Size(149, 22);
            this._optSendToBack.Text = "Send to back";
            this._optSendToBack.Click += new System.EventHandler(this._optSendToBack_Click);
            // 
            // _optBringToFront
            // 
            this._optBringToFront.Name = "_optBringToFront";
            this._optBringToFront.Size = new System.Drawing.Size(149, 22);
            this._optBringToFront.Text = "Bring to front";
            this._optBringToFront.Click += new System.EventHandler(this._optBringToFront_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // _optDelStageObj
            // 
            this._optDelStageObj.Name = "_optDelStageObj";
            this._optDelStageObj.Size = new System.Drawing.Size(149, 22);
            this._optDelStageObj.Text = "Delete";
            this._optDelStageObj.Click += new System.EventHandler(this._optDelStageObj_Click);
            // 
            // _mnuMultipleSelected
            // 
            this._mnuMultipleSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optMultipleRotate,
            this.toolStripSeparator4,
            this._optMultipleDelete});
            this._mnuMultipleSelected.Name = "_mnuMultipleSelected";
            this._mnuMultipleSelected.Size = new System.Drawing.Size(150, 54);
            // 
            // _optMultipleRotate
            // 
            this._optMultipleRotate.Name = "_optMultipleRotate";
            this._optMultipleRotate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this._optMultipleRotate.Size = new System.Drawing.Size(149, 22);
            this._optMultipleRotate.Text = "Rotate";
            this._optMultipleRotate.Click += new System.EventHandler(this._optMultipleRotate_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(146, 6);
            // 
            // _optMultipleDelete
            // 
            this._optMultipleDelete.Name = "_optMultipleDelete";
            this._optMultipleDelete.Size = new System.Drawing.Size(149, 22);
            this._optMultipleDelete.Text = "Delete";
            this._optMultipleDelete.Click += new System.EventHandler(this._optMultipleDelete_Click);
            // 
            // _mnuTileSelected
            // 
            this._mnuTileSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optTileDelete});
            this._mnuTileSelected.Name = "_mnuTileSelected";
            this._mnuTileSelected.Size = new System.Drawing.Size(108, 26);
            // 
            // _optTileDelete
            // 
            this._optTileDelete.Name = "_optTileDelete";
            this._optTileDelete.Size = new System.Drawing.Size(107, 22);
            this._optTileDelete.Text = "Delete";
            this._optTileDelete.Click += new System.EventHandler(this._optTileDelete_Click);
            // 
            // _grpBoard
            // 
            this._grpBoard.AllowCollapse = false;
            this._grpBoard.BackColor = System.Drawing.SystemColors.ActiveCaption;
            // 
            // _grpBoard._PanelBody
            // 
            this._grpBoard.BodyPanel.AutoScroll = true;
            this._grpBoard.BodyPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this._grpBoard.BodyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._grpBoard.BodyPanel.Controls.Add(this._pnlBoard);
            this._grpBoard.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpBoard.BodyPanel.Location = new System.Drawing.Point(5, 23);
            this._grpBoard.BodyPanel.Name = "_PanelBody";
            this._grpBoard.BodyPanel.Size = new System.Drawing.Size(555, 369);
            this._grpBoard.BodyPanel.TabIndex = 2;
            this._grpBoard.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
            this._grpBoard.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._grpBoard.CaptionVisible = true;
            this._grpBoard.Collapsed = false;
            this._grpBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpBoard.Location = new System.Drawing.Point(0, 0);
            this._grpBoard.Name = "_grpBoard";
            this._grpBoard.Padding = new System.Windows.Forms.Padding(5);
            this._grpBoard.Size = new System.Drawing.Size(565, 397);
            this._grpBoard.TabIndex = 1;
            this._grpBoard.Text = "Stage Map";
            this._grpBoard.Load += new System.EventHandler(this._grpBoard_Load);
            // 
            // _pnlBoard
            // 
            this._pnlBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlBoard.Location = new System.Drawing.Point(2, 4);
            this._pnlBoard.Name = "_pnlBoard";
            this._pnlBoard.Size = new System.Drawing.Size(348, 269);
            this._pnlBoard.TabIndex = 1;
            this._pnlBoard.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlBoard_Paint);
            this._pnlBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this._pnlBoard_MouseClick);
            this._pnlBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this._pnlBoard_MouseDown);
            this._pnlBoard.MouseEnter += new System.EventHandler(this._pnlBoard_MouseEnter);
            this._pnlBoard.MouseLeave += new System.EventHandler(this._pnlBoard_MouseLeave);
            this._pnlBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this._pnlBoard_MouseMove);
            this._pnlBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this._pnlBoard_MouseUp);
            // 
            // BoardCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grpBoard);
            this.Name = "BoardCtl";
            this.Size = new System.Drawing.Size(565, 397);
            this._mnuStageObject.ResumeLayout(false);
            this._mnuMultipleSelected.ResumeLayout(false);
            this._mnuTileSelected.ResumeLayout(false);
            this._grpBoard.BodyPanel.ResumeLayout(false);
            this._grpBoard.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private TwoBrains.Common.Controls.Group _grpBoard;
    private TwoBrainsGames.Snails.StageEditor.Controls.Panelx _pnlBoard;
    private System.Windows.Forms.Timer _SelectionTimer;
    private System.Windows.Forms.ContextMenuStrip _mnuStageObject;
    private System.Windows.Forms.ToolStripMenuItem _optStageObjProps;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem _optDelStageObj;
    private System.Windows.Forms.ContextMenuStrip _mnuMultipleSelected;
    private System.Windows.Forms.ContextMenuStrip _mnuTileSelected;
    private System.Windows.Forms.ToolStripMenuItem _optMultipleDelete;
    private System.Windows.Forms.ToolStripMenuItem _optTileDelete;
    private System.Windows.Forms.ToolStripMenuItem _optSendToBack;
    private System.Windows.Forms.ToolStripMenuItem _optBringToFront;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem _optStageObjRotate;
    private System.Windows.Forms.ToolStripMenuItem _optMultipleRotate;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
  }
}
