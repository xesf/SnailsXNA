namespace SpriteAnimationEditor.Forms
{
  partial class TileTesterForm
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
      this.components = new System.ComponentModel.Container();
      this._menuMain = new System.Windows.Forms.MenuStrip();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._OptRefreshProjImages = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this._mnuGrid = new System.Windows.Forms.ToolStripMenuItem();
      this._optGridVisible = new System.Windows.Forms.ToolStripMenuItem();
      this._OptGridOnTop = new System.Windows.Forms.ToolStripMenuItem();
      this._optSnapToGrid = new System.Windows.Forms.ToolStripMenuItem();
      this._optGridSize = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this._optClearCanvas = new System.Windows.Forms.ToolStripMenuItem();
      this._optBakgroundColor = new System.Windows.Forms.ToolStripMenuItem();
      this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._optDeleteTile = new System.Windows.Forms.ToolStripMenuItem();
      this.splitter1 = new System.Windows.Forms.Splitter();
      this._dlgColor = new System.Windows.Forms.ColorDialog();
      this._ctxmnuTile = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._optDeleteTilex = new System.Windows.Forms.ToolStripMenuItem();
      this._pnlSet = new SpriteAnimationEditor.Controls.DoubleBufferedPanel();
      this._TileSelector = new SpriteAnimationEditor.Controls.TileSelector();
      this._menuMain.SuspendLayout();
      this._ctxmnuTile.SuspendLayout();
      this.SuspendLayout();
      // 
      // _menuMain
      // 
      this._menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tileToolStripMenuItem});
      this._menuMain.Location = new System.Drawing.Point(0, 0);
      this._menuMain.Name = "_menuMain";
      this._menuMain.Size = new System.Drawing.Size(656, 24);
      this._menuMain.TabIndex = 0;
      this._menuMain.Text = "menuStrip1";
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptRefreshProjImages,
            this.toolStripSeparator2,
            this._mnuGrid});
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
      this.optionsToolStripMenuItem.Text = "Options";
      // 
      // _OptRefreshProjImages
      // 
      this._OptRefreshProjImages.Name = "_OptRefreshProjImages";
      this._OptRefreshProjImages.ShortcutKeys = System.Windows.Forms.Keys.F5;
      this._OptRefreshProjImages.Size = new System.Drawing.Size(213, 22);
      this._OptRefreshProjImages.Text = "Refresh Project images";
      this._OptRefreshProjImages.Click += new System.EventHandler(this._OptRefreshProjImages_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
      // 
      // _mnuGrid
      // 
      this._mnuGrid.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optGridVisible,
            this._OptGridOnTop,
            this._optSnapToGrid,
            this._optGridSize});
      this._mnuGrid.Name = "_mnuGrid";
      this._mnuGrid.Size = new System.Drawing.Size(213, 22);
      this._mnuGrid.Text = "Grid";
      // 
      // _optGridVisible
      // 
      this._optGridVisible.Name = "_optGridVisible";
      this._optGridVisible.Size = new System.Drawing.Size(139, 22);
      this._optGridVisible.Text = "Visible";
      this._optGridVisible.Click += new System.EventHandler(this._optVisible_Click);
      // 
      // _OptGridOnTop
      // 
      this._OptGridOnTop.Name = "_OptGridOnTop";
      this._OptGridOnTop.Size = new System.Drawing.Size(139, 22);
      this._OptGridOnTop.Text = "Grid On Top";
      this._OptGridOnTop.Click += new System.EventHandler(this._OptGridOnTop_Click);
      // 
      // _optSnapToGrid
      // 
      this._optSnapToGrid.Name = "_optSnapToGrid";
      this._optSnapToGrid.Size = new System.Drawing.Size(139, 22);
      this._optSnapToGrid.Text = "Snap to Grid";
      this._optSnapToGrid.Click += new System.EventHandler(this._optSnapToGrid_Click);
      // 
      // _optGridSize
      // 
      this._optGridSize.Name = "_optGridSize";
      this._optGridSize.Size = new System.Drawing.Size(139, 22);
      this._optGridSize.Text = "Grid Size...";
      this._optGridSize.Click += new System.EventHandler(this._optGridSize_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optClearCanvas,
            this._optBakgroundColor});
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(57, 20);
      this.toolStripMenuItem1.Text = "Canvas";
      // 
      // _optClearCanvas
      // 
      this._optClearCanvas.Name = "_optClearCanvas";
      this._optClearCanvas.Size = new System.Drawing.Size(179, 22);
      this._optClearCanvas.Text = "Clear ";
      this._optClearCanvas.Click += new System.EventHandler(this._optClearCanvas_Click);
      // 
      // _optBakgroundColor
      // 
      this._optBakgroundColor.Name = "_optBakgroundColor";
      this._optBakgroundColor.Size = new System.Drawing.Size(179, 22);
      this._optBakgroundColor.Text = "Background Color...";
      this._optBakgroundColor.Click += new System.EventHandler(this._optBakgroundColor_Click);
      // 
      // tileToolStripMenuItem
      // 
      this.tileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optDeleteTile});
      this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
      this.tileToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
      this.tileToolStripMenuItem.Text = "Tile";
      // 
      // _optDeleteTile
      // 
      this._optDeleteTile.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._optDeleteTile.Name = "_optDeleteTile";
      this._optDeleteTile.ShortcutKeys = System.Windows.Forms.Keys.Delete;
      this._optDeleteTile.Size = new System.Drawing.Size(131, 22);
      this._optDeleteTile.Text = "Delete";
      this._optDeleteTile.Click += new System.EventHandler(this._optDeleteTile_Click);
      // 
      // splitter1
      // 
      this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
      this.splitter1.Location = new System.Drawing.Point(427, 24);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new System.Drawing.Size(3, 381);
      this.splitter1.TabIndex = 2;
      this.splitter1.TabStop = false;
      // 
      // _ctxmnuTile
      // 
      this._ctxmnuTile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optDeleteTilex});
      this._ctxmnuTile.Name = "_ctxmnuTile";
      this._ctxmnuTile.Size = new System.Drawing.Size(108, 26);
      // 
      // _optDeleteTilex
      // 
      this._optDeleteTilex.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._optDeleteTilex.Name = "_optDeleteTilex";
      this._optDeleteTilex.Size = new System.Drawing.Size(107, 22);
      this._optDeleteTilex.Text = "Delete";
      this._optDeleteTilex.Click += new System.EventHandler(this._optDeleteTile_Click);
      // 
      // _pnlSet
      // 
      this._pnlSet.BackColor = System.Drawing.Color.White;
      this._pnlSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._pnlSet.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pnlSet.Location = new System.Drawing.Point(0, 24);
      this._pnlSet.Name = "_pnlSet";
      this._pnlSet.Size = new System.Drawing.Size(427, 381);
      this._pnlSet.TabIndex = 3;
      this._pnlSet.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlSet_Paint);
      this._pnlSet.MouseClick += new System.Windows.Forms.MouseEventHandler(this._pnlSet_MouseClick);
      // 
      // _TileSelector
      // 
      this._TileSelector.Dock = System.Windows.Forms.DockStyle.Right;
      this._TileSelector.Location = new System.Drawing.Point(430, 24);
      this._TileSelector.Name = "_TileSelector";
      this._TileSelector.Size = new System.Drawing.Size(226, 381);
      this._TileSelector.Solution = null;
      this._TileSelector.TabIndex = 0;
      this._TileSelector.Load += new System.EventHandler(this._TileSelector_Load);
      // 
      // TileTesterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(656, 405);
      this.Controls.Add(this._pnlSet);
      this.Controls.Add(this.splitter1);
      this.Controls.Add(this._TileSelector);
      this.Controls.Add(this._menuMain);
      this.KeyPreview = true;
      this.MainMenuStrip = this._menuMain;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TileTesterForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Tile Tester";
      this.Load += new System.EventHandler(this.TileTesterForm_Load);
      this._menuMain.ResumeLayout(false);
      this._menuMain.PerformLayout();
      this._ctxmnuTile.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private SpriteAnimationEditor.Controls.TileSelector _TileSelector;
    private System.Windows.Forms.MenuStrip _menuMain;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _OptRefreshProjImages;
    private System.Windows.Forms.Splitter splitter1;
    private SpriteAnimationEditor.Controls.DoubleBufferedPanel _pnlSet;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem _mnuGrid;
    private System.Windows.Forms.ToolStripMenuItem _optGridVisible;
    private System.Windows.Forms.ToolStripMenuItem _OptGridOnTop;
    private System.Windows.Forms.ToolStripMenuItem _optSnapToGrid;
    private System.Windows.Forms.ToolStripMenuItem _optGridSize;
    private System.Windows.Forms.ColorDialog _dlgColor;
    private System.Windows.Forms.ToolStripMenuItem tileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _optDeleteTile;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem _optClearCanvas;
    private System.Windows.Forms.ToolStripMenuItem _optBakgroundColor;
    private System.Windows.Forms.ContextMenuStrip _ctxmnuTile;
    private System.Windows.Forms.ToolStripMenuItem _optDeleteTilex;
  }
}