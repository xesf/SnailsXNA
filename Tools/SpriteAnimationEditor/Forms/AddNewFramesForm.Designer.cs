namespace SpriteAnimationEditor.Forms
{
  partial class AddNewFramesForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      SpriteAnimationEditor.Grid grid1 = new SpriteAnimationEditor.Grid();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this._btnToggleGrid = new System.Windows.Forms.ToolStripButton();
      this._BtnSetToPrjGrid = new System.Windows.Forms.ToolStripButton();
      this._txtHeight = new System.Windows.Forms.TextBox();
      this._txtY = new System.Windows.Forms.TextBox();
      this._txtWidth = new System.Windows.Forms.TextBox();
      this._txtX = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this._pnlFrame = new System.Windows.Forms.Panel();
      this._BtnAdd = new System.Windows.Forms.Button();
      this._grpFrame = new System.Windows.Forms.GroupBox();
      this._BtnOk = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this._menuView = new System.Windows.Forms.ToolStripMenuItem();
      this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._optViewGrid = new System.Windows.Forms.ToolStripMenuItem();
      this._optGridSize = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this._lblPosition = new System.Windows.Forms.ToolStripStatusLabel();
      this._pnlHelp = new System.Windows.Forms.Panel();
      this.label5 = new System.Windows.Forms.Label();
      this._pnlFrameProps = new System.Windows.Forms.Panel();
      this._Sprite = new SpriteAnimationEditor.Controls.OutputSprite();
      this._cmbZoom = new SpriteAnimationEditor.Controls.ZoomCombo();
      this.toolStrip1.SuspendLayout();
      this._grpFrame.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this._pnlHelp.SuspendLayout();
      this._pnlFrameProps.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnToggleGrid,
            this._BtnSetToPrjGrid});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(536, 25);
      this.toolStrip1.TabIndex = 2;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // _btnToggleGrid
      // 
      this._btnToggleGrid.CheckOnClick = true;
      this._btnToggleGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._btnToggleGrid.Image = global::SpriteAnimationEditor.Properties.Resources.grid;
      this._btnToggleGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._btnToggleGrid.Name = "_btnToggleGrid";
      this._btnToggleGrid.Size = new System.Drawing.Size(23, 22);
      this._btnToggleGrid.Text = "Toggle Grid";
      this._btnToggleGrid.Click += new System.EventHandler(this._btnToggleGrid_Click);
      // 
      // _BtnSetToPrjGrid
      // 
      this._BtnSetToPrjGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._BtnSetToPrjGrid.Image = global::SpriteAnimationEditor.Properties.Resources.project_grid;
      this._BtnSetToPrjGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._BtnSetToPrjGrid.Name = "_BtnSetToPrjGrid";
      this._BtnSetToPrjGrid.Size = new System.Drawing.Size(23, 22);
      this._BtnSetToPrjGrid.Text = "toolStripButton1";
      this._BtnSetToPrjGrid.ToolTipText = "Set to current animation grid";
      this._BtnSetToPrjGrid.Click += new System.EventHandler(this._BtnSetToPrjGrid_Click);
      // 
      // _txtHeight
      // 
      this._txtHeight.Location = new System.Drawing.Point(147, 47);
      this._txtHeight.Name = "_txtHeight";
      this._txtHeight.Size = new System.Drawing.Size(41, 20);
      this._txtHeight.TabIndex = 7;
      this._txtHeight.TextChanged += new System.EventHandler(this._txtHeight_TextChanged);
      // 
      // _txtY
      // 
      this._txtY.Location = new System.Drawing.Point(147, 23);
      this._txtY.Name = "_txtY";
      this._txtY.Size = new System.Drawing.Size(41, 20);
      this._txtY.TabIndex = 3;
      this._txtY.TextChanged += new System.EventHandler(this._txtY_TextChanged);
      // 
      // _txtWidth
      // 
      this._txtWidth.Location = new System.Drawing.Point(45, 47);
      this._txtWidth.Name = "_txtWidth";
      this._txtWidth.Size = new System.Drawing.Size(43, 20);
      this._txtWidth.TabIndex = 5;
      this._txtWidth.TextChanged += new System.EventHandler(this._txtWidth_TextChanged);
      // 
      // _txtX
      // 
      this._txtX.Location = new System.Drawing.Point(45, 23);
      this._txtX.Name = "_txtX";
      this._txtX.Size = new System.Drawing.Size(43, 20);
      this._txtX.TabIndex = 1;
      this._txtX.TextChanged += new System.EventHandler(this._txtX_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(103, 47);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(38, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Height";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(5, 50);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(35, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Width";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(106, 26);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(14, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Y";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(5, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(14, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "X";
      // 
      // _pnlFrame
      // 
      this._pnlFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._pnlFrame.Location = new System.Drawing.Point(6, 110);
      this._pnlFrame.Name = "_pnlFrame";
      this._pnlFrame.Size = new System.Drawing.Size(186, 129);
      this._pnlFrame.TabIndex = 10;
      this._pnlFrame.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlFrame_Paint);
      // 
      // _BtnAdd
      // 
      this._BtnAdd.Location = new System.Drawing.Point(8, 81);
      this._BtnAdd.Name = "_BtnAdd";
      this._BtnAdd.Size = new System.Drawing.Size(82, 23);
      this._BtnAdd.TabIndex = 9;
      this._BtnAdd.Text = "&Add Frame";
      this._BtnAdd.UseVisualStyleBackColor = true;
      this._BtnAdd.Click += new System.EventHandler(this._BtnAdd_Click);
      // 
      // _grpFrame
      // 
      this._grpFrame.Controls.Add(this._BtnOk);
      this._grpFrame.Controls.Add(this._btnCancel);
      this._grpFrame.Controls.Add(this._pnlFrame);
      this._grpFrame.Controls.Add(this._BtnAdd);
      this._grpFrame.Controls.Add(this.label1);
      this._grpFrame.Controls.Add(this._txtHeight);
      this._grpFrame.Controls.Add(this.label2);
      this._grpFrame.Controls.Add(this._txtY);
      this._grpFrame.Controls.Add(this.label3);
      this._grpFrame.Controls.Add(this._txtWidth);
      this._grpFrame.Controls.Add(this.label4);
      this._grpFrame.Controls.Add(this._txtX);
      this._grpFrame.Dock = System.Windows.Forms.DockStyle.Fill;
      this._grpFrame.Location = new System.Drawing.Point(3, 0);
      this._grpFrame.Name = "_grpFrame";
      this._grpFrame.Size = new System.Drawing.Size(204, 256);
      this._grpFrame.TabIndex = 4;
      this._grpFrame.TabStop = false;
      this._grpFrame.Text = "Frame";
      // 
      // _BtnOk
      // 
      this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._BtnOk.Location = new System.Drawing.Point(8, 81);
      this._BtnOk.Name = "_BtnOk";
      this._BtnOk.Size = new System.Drawing.Size(82, 23);
      this._BtnOk.TabIndex = 8;
      this._BtnOk.Text = "&Ok";
      this._BtnOk.UseVisualStyleBackColor = true;
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(109, 81);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(82, 23);
      this._btnCancel.TabIndex = 9;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuView});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(536, 24);
      this.menuStrip1.TabIndex = 0;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // _menuView
      // 
      this._menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem,
            this.toolStripSeparator1});
      this._menuView.Name = "_menuView";
      this._menuView.Size = new System.Drawing.Size(41, 20);
      this._menuView.Text = "View";
      // 
      // gridToolStripMenuItem
      // 
      this.gridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optViewGrid,
            this._optGridSize});
      this.gridToolStripMenuItem.Image = global::SpriteAnimationEditor.Properties.Resources.grid;
      this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
      this.gridToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
      this.gridToolStripMenuItem.Text = "Grid";
      this.gridToolStripMenuItem.ToolTipText = "Toggle grid";
      // 
      // _optViewGrid
      // 
      this._optViewGrid.Name = "_optViewGrid";
      this._optViewGrid.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
      this._optViewGrid.Size = new System.Drawing.Size(153, 22);
      this._optViewGrid.Text = "Visible";
      this._optViewGrid.Click += new System.EventHandler(this._optViewGrid_Click);
      // 
      // _optGridSize
      // 
      this._optGridSize.Name = "_optGridSize";
      this._optGridSize.Size = new System.Drawing.Size(153, 22);
      this._optGridSize.Text = "Attributes...";
      this._optGridSize.Click += new System.EventHandler(this._optGridSize_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(101, 6);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lblPosition});
      this.statusStrip1.Location = new System.Drawing.Point(0, 349);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(536, 22);
      this.statusStrip1.TabIndex = 5;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // _lblPosition
      // 
      this._lblPosition.AutoSize = false;
      this._lblPosition.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                  | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                  | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
      this._lblPosition.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
      this._lblPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this._lblPosition.Name = "_lblPosition";
      this._lblPosition.Size = new System.Drawing.Size(109, 17);
      // 
      // _pnlHelp
      // 
      this._pnlHelp.Controls.Add(this.label5);
      this._pnlHelp.Dock = System.Windows.Forms.DockStyle.Top;
      this._pnlHelp.Location = new System.Drawing.Point(0, 49);
      this._pnlHelp.Name = "_pnlHelp";
      this._pnlHelp.Padding = new System.Windows.Forms.Padding(6);
      this._pnlHelp.Size = new System.Drawing.Size(536, 43);
      this._pnlHelp.TabIndex = 6;
      // 
      // label5
      // 
      this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label5.Location = new System.Drawing.Point(6, 6);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(524, 31);
      this.label5.TabIndex = 0;
      this.label5.Text = "Use Ctrl+G to set the frame size to fit the clicked grid cell. Double click will " +
          "automatically add that frame.";
      // 
      // _pnlFrameProps
      // 
      this._pnlFrameProps.Controls.Add(this._grpFrame);
      this._pnlFrameProps.Dock = System.Windows.Forms.DockStyle.Right;
      this._pnlFrameProps.Location = new System.Drawing.Point(326, 92);
      this._pnlFrameProps.Name = "_pnlFrameProps";
      this._pnlFrameProps.Padding = new System.Windows.Forms.Padding(3, 0, 3, 1);
      this._pnlFrameProps.Size = new System.Drawing.Size(210, 257);
      this._pnlFrameProps.TabIndex = 7;
      // 
      // _Sprite
      // 
      this._Sprite.Animation = null;
      this._Sprite.CaptionVisible = false;
      this._Sprite.Dock = System.Windows.Forms.DockStyle.Fill;
      this._Sprite.Frame = null;
      this._Sprite.FrameColor = System.Drawing.Color.White;
      grid1.ForeColor = System.Drawing.Color.Black;
      grid1.Height = 32;
      grid1.OffsetX = 0;
      grid1.OffsetY = 0;
      grid1.Visible = true;
      grid1.Width = 32;
      this._Sprite.Grid = grid1;
      this._Sprite.GridColor = System.Drawing.Color.Black;
      this._Sprite.GridVisible = true;
      this._Sprite.Location = new System.Drawing.Point(0, 92);
      this._Sprite.Name = "_Sprite";
      this._Sprite.Project = null;
      this._Sprite.ShowImages = false;
      this._Sprite.Size = new System.Drawing.Size(326, 257);
      this._Sprite.SpriteBackColor = System.Drawing.Color.Transparent;
      this._Sprite.TabIndex = 3;
      this._Sprite.Zoom = SpriteAnimationEditor.ZoomFactor.x1;
      this._Sprite.SpriteMouseDown += new System.Windows.Forms.MouseEventHandler(this._Sprite_SpriteMouseDown);
      this._Sprite.ZoomChanged += new System.EventHandler(this._Sprite_ZoomChanged);
      this._Sprite.BeforeGridPaint += new SpriteAnimationEditor.Controls.OutputSprite.GraphicsHandler(this._Sprite_BeforeGridPaint);
      this._Sprite.GridCellDoubleClicked += new SpriteAnimationEditor.Controls.OutputSprite.GridCellClickedHandler(this._Sprite_GridCellDoubleClicked);
      this._Sprite.SpritePaint += new SpriteAnimationEditor.Controls.OutputSprite.GraphicsHandler(this._Sprite_SpritePaint);
      this._Sprite.SpriteMouseMove += new System.Windows.Forms.MouseEventHandler(this._Sprite_SpriteMouseMove);
      this._Sprite.SpriteMouseUp += new System.Windows.Forms.MouseEventHandler(this._Sprite_SpriteMouseUp);
      this._Sprite.GridCellClicked += new SpriteAnimationEditor.Controls.OutputSprite.GridCellClickedHandler(this._Sprite_GridCellClicked);
      this._Sprite.MouseLeave += new System.EventHandler(this._Sprite_MouseLeave);
      // 
      // _cmbZoom
      // 
      this._cmbZoom.FormattingEnabled = true;
      this._cmbZoom.Items.AddRange(new object[] {
            SpriteAnimationEditor.ZoomFactor.x1,
            SpriteAnimationEditor.ZoomFactor.x2,
            SpriteAnimationEditor.ZoomFactor.x4,
            SpriteAnimationEditor.ZoomFactor.x8,
            SpriteAnimationEditor.ZoomFactor.x16});
      this._cmbZoom.Location = new System.Drawing.Point(23, 2);
      this._cmbZoom.Name = "_cmbZoom";
      this._cmbZoom.Size = new System.Drawing.Size(70, 21);
      this._cmbZoom.TabIndex = 1;
      this._cmbZoom.Zoom = SpriteAnimationEditor.ZoomFactor.x1;
      this._cmbZoom.SelectedIndexChanged += new System.EventHandler(this.zoomCombo1_SelectedIndexChanged);
      // 
      // AddNewFramesForm
      // 
      this.AcceptButton = this._BtnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(536, 371);
      this.Controls.Add(this._Sprite);
      this.Controls.Add(this._pnlFrameProps);
      this.Controls.Add(this._pnlHelp);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this._cmbZoom);
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddNewFramesForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Add New Frames";
      this.Load += new System.EventHandler(this.AddNewFramesForm_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this._grpFrame.ResumeLayout(false);
      this._grpFrame.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this._pnlHelp.ResumeLayout(false);
      this._pnlFrameProps.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStrip1;
    private SpriteAnimationEditor.Controls.OutputSprite _Sprite;
    private System.Windows.Forms.ToolStripButton _btnToggleGrid;
    private System.Windows.Forms.TextBox _txtHeight;
    private System.Windows.Forms.TextBox _txtY;
    private System.Windows.Forms.TextBox _txtWidth;
    private System.Windows.Forms.TextBox _txtX;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _BtnAdd;
    private System.Windows.Forms.Panel _pnlFrame;
    private System.Windows.Forms.GroupBox _grpFrame;
    private SpriteAnimationEditor.Controls.ZoomCombo _cmbZoom;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem _menuView;
    private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem _optViewGrid;
    private System.Windows.Forms.ToolStripMenuItem _optGridSize;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel _lblPosition;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.Button _BtnOk;
    private System.Windows.Forms.ToolStripButton _BtnSetToPrjGrid;
    private System.Windows.Forms.Panel _pnlHelp;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Panel _pnlFrameProps;
  }
}