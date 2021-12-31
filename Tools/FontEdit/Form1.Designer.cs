namespace FontEdit
{
    partial class Form1
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
          this._MainMenu = new System.Windows.Forms.MenuStrip();
          this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this._OptLoad = new System.Windows.Forms.ToolStripMenuItem();
          this._OptSave = new System.Windows.Forms.ToolStripMenuItem();
          this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
          this._optOpenLast = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
          this.exportGameFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
          this._OptPreferences = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
          this._OptExit = new System.Windows.Forms.ToolStripMenuItem();
          this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this._OptSelectImage = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
          this.generateNewFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this._OptSetDefCharSize = new System.Windows.Forms.ToolStripMenuItem();
          this._optSetSelChar = new System.Windows.Forms.ToolStripMenuItem();
          this._optRemoveChar = new System.Windows.Forms.ToolStripMenuItem();
          this._optAutoSet = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
          this._OptMoveBoxLeft = new System.Windows.Forms.ToolStripMenuItem();
          this._OptMoveBoxRight = new System.Windows.Forms.ToolStripMenuItem();
          this._OptMoveBoxUp = new System.Windows.Forms.ToolStripMenuItem();
          this._OptMoveBoxDown = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
          this._OptMoveBoxLeftSnap = new System.Windows.Forms.ToolStripMenuItem();
          this._OptMoveBoxRightSnap = new System.Windows.Forms.ToolStripMenuItem();
          this._OptMoveBoxUpSnap = new System.Windows.Forms.ToolStripMenuItem();
          this._OptMoveBoxDowntSnap = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
          this._OptDecBoxWidth = new System.Windows.Forms.ToolStripMenuItem();
          this._OptIncBoxWidth = new System.Windows.Forms.ToolStripMenuItem();
          this._OptDecBoxHeight = new System.Windows.Forms.ToolStripMenuItem();
          this._OptIncBoxHeight = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
          this.setDefaultSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this._OptProps = new System.Windows.Forms.ToolStripMenuItem();
          this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this._optNormalZoom = new System.Windows.Forms.ToolStripMenuItem();
          this._optZoom2x = new System.Windows.Forms.ToolStripMenuItem();
          this._optZoom4x = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
          this._optZoomIn = new System.Windows.Forms.ToolStripMenuItem();
          this._optZoomOut = new System.Windows.Forms.ToolStripMenuItem();
          this._OpenFileDlg = new System.Windows.Forms.OpenFileDialog();
          this._SaveFileDlg = new System.Windows.Forms.SaveFileDialog();
          this.splitContainer1 = new System.Windows.Forms.SplitContainer();
          this.splitContainer3 = new System.Windows.Forms.SplitContainer();
          this.txtExemplo = new System.Windows.Forms.TextBox();
          this.btGenerateFrames = new System.Windows.Forms.Button();
          this._pgFontProps = new System.Windows.Forms.PropertyGrid();
          this._ListboxChars = new System.Windows.Forms.ListBox();
          this.splitContainer2 = new System.Windows.Forms.SplitContainer();
          this.picExemplo = new System.Windows.Forms.PictureBox();
          this._pnlFontImage = new System.Windows.Forms.Panel();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
          this._MainMenu.SuspendLayout();
          this.splitContainer1.Panel1.SuspendLayout();
          this.splitContainer1.Panel2.SuspendLayout();
          this.splitContainer1.SuspendLayout();
          this.splitContainer3.Panel1.SuspendLayout();
          this.splitContainer3.Panel2.SuspendLayout();
          this.splitContainer3.SuspendLayout();
          this.splitContainer2.Panel1.SuspendLayout();
          this.splitContainer2.Panel2.SuspendLayout();
          this.splitContainer2.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.picExemplo)).BeginInit();
          this.statusStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // _MainMenu
          // 
          this._MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.fontToolStripMenuItem,
            this.viewToolStripMenuItem});
          this._MainMenu.Location = new System.Drawing.Point(0, 0);
          this._MainMenu.Name = "_MainMenu";
          this._MainMenu.Size = new System.Drawing.Size(1033, 24);
          this._MainMenu.TabIndex = 1;
          this._MainMenu.Text = "menuStrip1";
          // 
          // fileToolStripMenuItem
          // 
          this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this._OptLoad,
            this._OptSave,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator5,
            this._optOpenLast,
            this.toolStripSeparator6,
            this.exportGameFontToolStripMenuItem,
            this.toolStripMenuItem1,
            this._OptPreferences,
            this.toolStripSeparator2,
            this._OptExit});
          this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
          this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
          this.fileToolStripMenuItem.Text = "File";
          this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
          // 
          // saveToolStripMenuItem
          // 
          this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
          this.saveToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
          this.saveToolStripMenuItem.Text = "New";
          // 
          // _OptLoad
          // 
          this._OptLoad.Name = "_OptLoad";
          this._OptLoad.Size = new System.Drawing.Size(175, 22);
          this._OptLoad.Text = "Load...";
          this._OptLoad.Click += new System.EventHandler(this._OptLoad_Click);
          // 
          // _OptSave
          // 
          this._OptSave.Name = "_OptSave";
          this._OptSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
          this._OptSave.Size = new System.Drawing.Size(175, 22);
          this._OptSave.Text = "Save";
          this._OptSave.Click += new System.EventHandler(this._OptSave_Click);
          // 
          // saveAsToolStripMenuItem
          // 
          this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
          this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
          this.saveAsToolStripMenuItem.Text = "Save as...";
          this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
          // 
          // toolStripSeparator5
          // 
          this.toolStripSeparator5.Name = "toolStripSeparator5";
          this.toolStripSeparator5.Size = new System.Drawing.Size(172, 6);
          // 
          // _optOpenLast
          // 
          this._optOpenLast.Name = "_optOpenLast";
          this._optOpenLast.Size = new System.Drawing.Size(175, 22);
          this._optOpenLast.Text = "Open Last Edited File";
          this._optOpenLast.Click += new System.EventHandler(this._optOpenLast_Click);
          // 
          // toolStripSeparator6
          // 
          this.toolStripSeparator6.Name = "toolStripSeparator6";
          this.toolStripSeparator6.Size = new System.Drawing.Size(172, 6);
          // 
          // exportGameFontToolStripMenuItem
          // 
          this.exportGameFontToolStripMenuItem.Name = "exportGameFontToolStripMenuItem";
          this.exportGameFontToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
          this.exportGameFontToolStripMenuItem.Text = "Export Game Font...";
          this.exportGameFontToolStripMenuItem.Click += new System.EventHandler(this.exportGameFontToolStripMenuItem_Click);
          // 
          // toolStripMenuItem1
          // 
          this.toolStripMenuItem1.Name = "toolStripMenuItem1";
          this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 6);
          // 
          // _OptPreferences
          // 
          this._OptPreferences.Name = "_OptPreferences";
          this._OptPreferences.Size = new System.Drawing.Size(175, 22);
          this._OptPreferences.Text = "Preferences...";
          this._OptPreferences.Click += new System.EventHandler(this._OptPreferences_Click);
          // 
          // toolStripSeparator2
          // 
          this.toolStripSeparator2.Name = "toolStripSeparator2";
          this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
          // 
          // _OptExit
          // 
          this._OptExit.Name = "_OptExit";
          this._OptExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
          this._OptExit.Size = new System.Drawing.Size(175, 22);
          this._OptExit.Text = "Exit";
          this._OptExit.Click += new System.EventHandler(this._OptExit_Click);
          // 
          // fontToolStripMenuItem
          // 
          this.fontToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptSelectImage,
            this.toolStripMenuItem2,
            this.generateNewFramesToolStripMenuItem,
            this._OptSetDefCharSize,
            this.toolStripSeparator1,
            this._OptProps});
          this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
          this.fontToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
          this.fontToolStripMenuItem.Text = "Font";
          // 
          // _OptSelectImage
          // 
          this._OptSelectImage.Name = "_OptSelectImage";
          this._OptSelectImage.Size = new System.Drawing.Size(181, 22);
          this._OptSelectImage.Text = "Select image...";
          this._OptSelectImage.Click += new System.EventHandler(this._OptSelectImage_Click);
          // 
          // toolStripMenuItem2
          // 
          this.toolStripMenuItem2.Name = "toolStripMenuItem2";
          this.toolStripMenuItem2.Size = new System.Drawing.Size(178, 6);
          // 
          // generateNewFramesToolStripMenuItem
          // 
          this.generateNewFramesToolStripMenuItem.Name = "generateNewFramesToolStripMenuItem";
          this.generateNewFramesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
          this.generateNewFramesToolStripMenuItem.Text = "Generate New Frames";
          this.generateNewFramesToolStripMenuItem.Click += new System.EventHandler(this.generateNewFramesToolStripMenuItem_Click);
          // 
          // _OptSetDefCharSize
          // 
          this._OptSetDefCharSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optSetSelChar,
            this._optRemoveChar,
            this._optAutoSet,
            this.toolStripSeparator7,
            this._OptMoveBoxLeft,
            this._OptMoveBoxRight,
            this._OptMoveBoxUp,
            this._OptMoveBoxDown,
            this.toolStripSeparator4,
            this._OptMoveBoxLeftSnap,
            this._OptMoveBoxRightSnap,
            this._OptMoveBoxUpSnap,
            this._OptMoveBoxDowntSnap,
            this.toolStripMenuItem3,
            this._OptDecBoxWidth,
            this._OptIncBoxWidth,
            this._OptDecBoxHeight,
            this._OptIncBoxHeight,
            this.toolStripSeparator3,
            this.setDefaultSizeToolStripMenuItem});
          this._OptSetDefCharSize.Name = "_OptSetDefCharSize";
          this._OptSetDefCharSize.Size = new System.Drawing.Size(181, 22);
          this._OptSetDefCharSize.Text = "Current Character";
          // 
          // _optSetSelChar
          // 
          this._optSetSelChar.Name = "_optSetSelChar";
          this._optSetSelChar.ShortcutKeys = System.Windows.Forms.Keys.F10;
          this._optSetSelChar.Size = new System.Drawing.Size(275, 22);
          this._optSetSelChar.Text = "Set Character To Selected Rect";
          this._optSetSelChar.Click += new System.EventHandler(this._optSetSelChar_Click);
          // 
          // _optRemoveChar
          // 
          this._optRemoveChar.Name = "_optRemoveChar";
          this._optRemoveChar.ShortcutKeys = System.Windows.Forms.Keys.Delete;
          this._optRemoveChar.Size = new System.Drawing.Size(275, 22);
          this._optRemoveChar.Text = "Remove Character Rect";
          this._optRemoveChar.Click += new System.EventHandler(this._optRemoveChar_Click);
          // 
          // _optAutoSet
          // 
          this._optAutoSet.Name = "_optAutoSet";
          this._optAutoSet.ShortcutKeys = System.Windows.Forms.Keys.F9;
          this._optAutoSet.Size = new System.Drawing.Size(275, 22);
          this._optAutoSet.Text = "Autoset next char Rect";
          this._optAutoSet.Click += new System.EventHandler(this._optAutoSet_Click);
          // 
          // toolStripSeparator7
          // 
          this.toolStripSeparator7.Name = "toolStripSeparator7";
          this.toolStripSeparator7.Size = new System.Drawing.Size(272, 6);
          // 
          // _OptMoveBoxLeft
          // 
          this._OptMoveBoxLeft.Name = "_OptMoveBoxLeft";
          this._OptMoveBoxLeft.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
          this._OptMoveBoxLeft.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxLeft.Text = "Move Box Left";
          this._OptMoveBoxLeft.Click += new System.EventHandler(this._OptMoveBoxLeft_Click);
          // 
          // _OptMoveBoxRight
          // 
          this._OptMoveBoxRight.Name = "_OptMoveBoxRight";
          this._OptMoveBoxRight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
          this._OptMoveBoxRight.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxRight.Text = "Move Box Right";
          this._OptMoveBoxRight.Click += new System.EventHandler(this._OptMoveBoxRight_Click);
          // 
          // _OptMoveBoxUp
          // 
          this._OptMoveBoxUp.Name = "_OptMoveBoxUp";
          this._OptMoveBoxUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
          this._OptMoveBoxUp.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxUp.Text = "Move Box Up";
          this._OptMoveBoxUp.Click += new System.EventHandler(this._OptMoveBoxUp_Click);
          // 
          // _OptMoveBoxDown
          // 
          this._OptMoveBoxDown.Name = "_OptMoveBoxDown";
          this._OptMoveBoxDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
          this._OptMoveBoxDown.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxDown.Text = "Move Box Down";
          this._OptMoveBoxDown.Click += new System.EventHandler(this._OptMoveBoxDown_Click);
          // 
          // toolStripSeparator4
          // 
          this.toolStripSeparator4.Name = "toolStripSeparator4";
          this.toolStripSeparator4.Size = new System.Drawing.Size(272, 6);
          // 
          // _OptMoveBoxLeftSnap
          // 
          this._OptMoveBoxLeftSnap.Name = "_OptMoveBoxLeftSnap";
          this._OptMoveBoxLeftSnap.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                      | System.Windows.Forms.Keys.Left)));
          this._OptMoveBoxLeftSnap.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxLeftSnap.Text = "Move Box Left (Snap)";
          this._OptMoveBoxLeftSnap.Click += new System.EventHandler(this._OptMoveBoxLeftSnap_Click);
          // 
          // _OptMoveBoxRightSnap
          // 
          this._OptMoveBoxRightSnap.Name = "_OptMoveBoxRightSnap";
          this._OptMoveBoxRightSnap.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                      | System.Windows.Forms.Keys.Right)));
          this._OptMoveBoxRightSnap.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxRightSnap.Text = "Move Box Right (Snap)";
          this._OptMoveBoxRightSnap.Click += new System.EventHandler(this._OptMoveBoxRightSnap_Click);
          // 
          // _OptMoveBoxUpSnap
          // 
          this._OptMoveBoxUpSnap.Name = "_OptMoveBoxUpSnap";
          this._OptMoveBoxUpSnap.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                      | System.Windows.Forms.Keys.Up)));
          this._OptMoveBoxUpSnap.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxUpSnap.Tag = "";
          this._OptMoveBoxUpSnap.Text = "Move Box Up (Snap)";
          this._OptMoveBoxUpSnap.Click += new System.EventHandler(this._OptMoveBoxUpSnap_Click);
          // 
          // _OptMoveBoxDowntSnap
          // 
          this._OptMoveBoxDowntSnap.Name = "_OptMoveBoxDowntSnap";
          this._OptMoveBoxDowntSnap.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                      | System.Windows.Forms.Keys.Down)));
          this._OptMoveBoxDowntSnap.Size = new System.Drawing.Size(275, 22);
          this._OptMoveBoxDowntSnap.Text = "Move Box Down (Snap)";
          this._OptMoveBoxDowntSnap.Click += new System.EventHandler(this._OptMoveBoxDowntSnap_Click);
          // 
          // toolStripMenuItem3
          // 
          this.toolStripMenuItem3.Name = "toolStripMenuItem3";
          this.toolStripMenuItem3.Size = new System.Drawing.Size(272, 6);
          // 
          // _OptDecBoxWidth
          // 
          this._OptDecBoxWidth.Name = "_OptDecBoxWidth";
          this._OptDecBoxWidth.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
          this._OptDecBoxWidth.Size = new System.Drawing.Size(275, 22);
          this._OptDecBoxWidth.Text = "Decrease Width";
          this._OptDecBoxWidth.Click += new System.EventHandler(this._OptDecBoxWidth_Click);
          // 
          // _OptIncBoxWidth
          // 
          this._OptIncBoxWidth.Name = "_OptIncBoxWidth";
          this._OptIncBoxWidth.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
          this._OptIncBoxWidth.Size = new System.Drawing.Size(275, 22);
          this._OptIncBoxWidth.Text = "Increase Width";
          this._OptIncBoxWidth.Click += new System.EventHandler(this._OptIncBoxWidth_Click);
          // 
          // _OptDecBoxHeight
          // 
          this._OptDecBoxHeight.Name = "_OptDecBoxHeight";
          this._OptDecBoxHeight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Up)));
          this._OptDecBoxHeight.Size = new System.Drawing.Size(275, 22);
          this._OptDecBoxHeight.Text = "Decrease Height";
          this._OptDecBoxHeight.Click += new System.EventHandler(this._OptDecBoxHeight_Click);
          // 
          // _OptIncBoxHeight
          // 
          this._OptIncBoxHeight.Name = "_OptIncBoxHeight";
          this._OptIncBoxHeight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Down)));
          this._OptIncBoxHeight.Size = new System.Drawing.Size(275, 22);
          this._OptIncBoxHeight.Text = "Increase Height";
          this._OptIncBoxHeight.Click += new System.EventHandler(this._OptIncBoxHeight_Click);
          // 
          // toolStripSeparator3
          // 
          this.toolStripSeparator3.Name = "toolStripSeparator3";
          this.toolStripSeparator3.Size = new System.Drawing.Size(272, 6);
          // 
          // setDefaultSizeToolStripMenuItem
          // 
          this.setDefaultSizeToolStripMenuItem.Name = "setDefaultSizeToolStripMenuItem";
          this.setDefaultSizeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
          this.setDefaultSizeToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
          this.setDefaultSizeToolStripMenuItem.Text = "Set Default Size";
          this.setDefaultSizeToolStripMenuItem.Click += new System.EventHandler(this.setDefaultSizeToolStripMenuItem_Click);
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
          // 
          // _OptProps
          // 
          this._OptProps.Name = "_OptProps";
          this._OptProps.Size = new System.Drawing.Size(181, 22);
          this._OptProps.Text = "Properties...";
          this._OptProps.Click += new System.EventHandler(this._OptProps_Click);
          // 
          // viewToolStripMenuItem
          // 
          this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem});
          this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
          this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
          this.viewToolStripMenuItem.Text = "View";
          // 
          // zoomToolStripMenuItem
          // 
          this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optNormalZoom,
            this._optZoom2x,
            this._optZoom4x,
            this.toolStripSeparator8,
            this._optZoomIn,
            this._optZoomOut});
          this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
          this.zoomToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
          this.zoomToolStripMenuItem.Text = "Zoom ";
          // 
          // _optNormalZoom
          // 
          this._optNormalZoom.Name = "_optNormalZoom";
          this._optNormalZoom.Size = new System.Drawing.Size(153, 22);
          this._optNormalZoom.Text = "Normal";
          this._optNormalZoom.Click += new System.EventHandler(this._optNormalZoom_Click);
          // 
          // _optZoom2x
          // 
          this._optZoom2x.Name = "_optZoom2x";
          this._optZoom2x.Size = new System.Drawing.Size(153, 22);
          this._optZoom2x.Text = "2x";
          this._optZoom2x.Click += new System.EventHandler(this._optZoom2x_Click);
          // 
          // _optZoom4x
          // 
          this._optZoom4x.Name = "_optZoom4x";
          this._optZoom4x.Size = new System.Drawing.Size(153, 22);
          this._optZoom4x.Text = "4x";
          this._optZoom4x.Click += new System.EventHandler(this._optZoom4x_Click);
          // 
          // toolStripSeparator8
          // 
          this.toolStripSeparator8.Name = "toolStripSeparator8";
          this.toolStripSeparator8.Size = new System.Drawing.Size(150, 6);
          // 
          // _optZoomIn
          // 
          this._optZoomIn.Name = "_optZoomIn";
          this._optZoomIn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
          this._optZoomIn.Size = new System.Drawing.Size(153, 22);
          this._optZoomIn.Text = "Zoom in";
          this._optZoomIn.Click += new System.EventHandler(this._optZoomIn_Click);
          // 
          // _optZoomOut
          // 
          this._optZoomOut.Name = "_optZoomOut";
          this._optZoomOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Z)));
          this._optZoomOut.Size = new System.Drawing.Size(153, 22);
          this._optZoomOut.Text = "Zoom out";
          this._optZoomOut.Click += new System.EventHandler(this._optZoomOut_Click);
          // 
          // _OpenFileDlg
          // 
          this._OpenFileDlg.FileName = "openFileDialog1";
          // 
          // _SaveFileDlg
          // 
          this._SaveFileDlg.FileName = "noname";
          // 
          // splitContainer1
          // 
          this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
          this.splitContainer1.IsSplitterFixed = true;
          this.splitContainer1.Location = new System.Drawing.Point(0, 24);
          this.splitContainer1.Name = "splitContainer1";
          // 
          // splitContainer1.Panel1
          // 
          this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
          // 
          // splitContainer1.Panel2
          // 
          this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
          this.splitContainer1.Size = new System.Drawing.Size(1033, 558);
          this.splitContainer1.SplitterDistance = 222;
          this.splitContainer1.TabIndex = 7;
          // 
          // splitContainer3
          // 
          this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
          this.splitContainer3.IsSplitterFixed = true;
          this.splitContainer3.Location = new System.Drawing.Point(0, 0);
          this.splitContainer3.Name = "splitContainer3";
          this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
          // 
          // splitContainer3.Panel1
          // 
          this.splitContainer3.Panel1.Controls.Add(this.txtExemplo);
          this.splitContainer3.Panel1.Controls.Add(this.btGenerateFrames);
          this.splitContainer3.Panel1.Controls.Add(this._pgFontProps);
          // 
          // splitContainer3.Panel2
          // 
          this.splitContainer3.Panel2.Controls.Add(this._ListboxChars);
          this.splitContainer3.Size = new System.Drawing.Size(222, 558);
          this.splitContainer3.SplitterDistance = 277;
          this.splitContainer3.TabIndex = 13;
          // 
          // txtExemplo
          // 
          this.txtExemplo.Location = new System.Drawing.Point(1, 227);
          this.txtExemplo.Multiline = true;
          this.txtExemplo.Name = "txtExemplo";
          this.txtExemplo.Size = new System.Drawing.Size(219, 47);
          this.txtExemplo.TabIndex = 15;
          this.txtExemplo.Text = resources.GetString("txtExemplo.Text");
          this.txtExemplo.TextChanged += new System.EventHandler(this.txtExemplo_TextChanged);
          // 
          // btGenerateFrames
          // 
          this.btGenerateFrames.Enabled = false;
          this.btGenerateFrames.Location = new System.Drawing.Point(50, 198);
          this.btGenerateFrames.Name = "btGenerateFrames";
          this.btGenerateFrames.Size = new System.Drawing.Size(123, 23);
          this.btGenerateFrames.TabIndex = 14;
          this.btGenerateFrames.Text = "Generate New Frames";
          this.btGenerateFrames.UseVisualStyleBackColor = true;
          this.btGenerateFrames.Click += new System.EventHandler(this.btGenerateFrames_Click);
          // 
          // _pgFontProps
          // 
          this._pgFontProps.HelpVisible = false;
          this._pgFontProps.Location = new System.Drawing.Point(0, 0);
          this._pgFontProps.Name = "_pgFontProps";
          this._pgFontProps.Size = new System.Drawing.Size(223, 192);
          this._pgFontProps.TabIndex = 13;
          this._pgFontProps.ToolbarVisible = false;
          this._pgFontProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
          // 
          // _ListboxChars
          // 
          this._ListboxChars.Dock = System.Windows.Forms.DockStyle.Fill;
          this._ListboxChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this._ListboxChars.FormattingEnabled = true;
          this._ListboxChars.ItemHeight = 20;
          this._ListboxChars.Location = new System.Drawing.Point(0, 0);
          this._ListboxChars.Name = "_ListboxChars";
          this._ListboxChars.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
          this._ListboxChars.Size = new System.Drawing.Size(222, 264);
          this._ListboxChars.TabIndex = 8;
          this._ListboxChars.SelectedIndexChanged += new System.EventHandler(this._ListboxChars_SelectedIndexChanged);
          this._ListboxChars.DoubleClick += new System.EventHandler(this._ListboxChars_DoubleClick);
          // 
          // splitContainer2
          // 
          this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
          this.splitContainer2.Location = new System.Drawing.Point(0, 0);
          this.splitContainer2.Name = "splitContainer2";
          this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
          // 
          // splitContainer2.Panel1
          // 
          this.splitContainer2.Panel1.Controls.Add(this.picExemplo);
          // 
          // splitContainer2.Panel2
          // 
          this.splitContainer2.Panel2.AutoScroll = true;
          this.splitContainer2.Panel2.Controls.Add(this._pnlFontImage);
          this.splitContainer2.Size = new System.Drawing.Size(807, 558);
          this.splitContainer2.SplitterDistance = 122;
          this.splitContainer2.TabIndex = 0;
          // 
          // picExemplo
          // 
          this.picExemplo.BackColor = System.Drawing.Color.Gray;
          this.picExemplo.Dock = System.Windows.Forms.DockStyle.Fill;
          this.picExemplo.Location = new System.Drawing.Point(0, 0);
          this.picExemplo.Name = "picExemplo";
          this.picExemplo.Size = new System.Drawing.Size(807, 122);
          this.picExemplo.TabIndex = 13;
          this.picExemplo.TabStop = false;
          this.picExemplo.Paint += new System.Windows.Forms.PaintEventHandler(this.picExemplo_Paint);
          // 
          // _pnlFontImage
          // 
          this._pnlFontImage.BackColor = System.Drawing.Color.SteelBlue;
          this._pnlFontImage.Location = new System.Drawing.Point(3, 3);
          this._pnlFontImage.Name = "_pnlFontImage";
          this._pnlFontImage.Size = new System.Drawing.Size(200, 100);
          this._pnlFontImage.TabIndex = 0;
          this._pnlFontImage.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlFontImage_Paint);
          this._pnlFontImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this._pnlFontImage_MouseClick);
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
          this.statusStrip1.Location = new System.Drawing.Point(0, 582);
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.Size = new System.Drawing.Size(1033, 22);
          this.statusStrip1.TabIndex = 8;
          this.statusStrip1.Text = "statusStrip1";
          // 
          // toolStripStatusLabel1
          // 
          this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
          this.toolStripStatusLabel1.Padding = new System.Windows.Forms.Padding(450, 0, 0, 0);
          this.toolStripStatusLabel1.Size = new System.Drawing.Size(450, 17);
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(1033, 604);
          this.Controls.Add(this.splitContainer1);
          this.Controls.Add(this.statusStrip1);
          this.Controls.Add(this._MainMenu);
          this.MainMenuStrip = this._MainMenu;
          this.Name = "Form1";
          this.Text = "Font Editor";
          this.Load += new System.EventHandler(this.Form1_Load);
          this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
          this._MainMenu.ResumeLayout(false);
          this._MainMenu.PerformLayout();
          this.splitContainer1.Panel1.ResumeLayout(false);
          this.splitContainer1.Panel2.ResumeLayout(false);
          this.splitContainer1.ResumeLayout(false);
          this.splitContainer3.Panel1.ResumeLayout(false);
          this.splitContainer3.Panel1.PerformLayout();
          this.splitContainer3.Panel2.ResumeLayout(false);
          this.splitContainer3.ResumeLayout(false);
          this.splitContainer2.Panel1.ResumeLayout(false);
          this.splitContainer2.Panel2.ResumeLayout(false);
          this.splitContainer2.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.picExemplo)).EndInit();
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _OptLoad;
        private System.Windows.Forms.ToolStripMenuItem _OptSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _OptExit;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _OptSelectImage;
        private System.Windows.Forms.OpenFileDialog _OpenFileDlg;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem _OptSetDefCharSize;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxLeft;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxRight;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxUp;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxDown;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem _OptDecBoxWidth;
        private System.Windows.Forms.ToolStripMenuItem _OptIncBoxWidth;
        private System.Windows.Forms.ToolStripMenuItem _OptDecBoxHeight;
        private System.Windows.Forms.ToolStripMenuItem _OptIncBoxHeight;
        private System.Windows.Forms.SaveFileDialog _SaveFileDlg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _OptProps;
        private System.Windows.Forms.ToolStripMenuItem _OptPreferences;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem setDefaultSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxLeftSnap;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxRightSnap;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxUpSnap;
        private System.Windows.Forms.ToolStripMenuItem _OptMoveBoxDowntSnap;
        private System.Windows.Forms.ToolStripMenuItem generateNewFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportGameFontToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem _optOpenLast;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem _optSetSelChar;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem _optRemoveChar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox picExemplo;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox txtExemplo;
        private System.Windows.Forms.Button btGenerateFrames;
        private System.Windows.Forms.PropertyGrid _pgFontProps;
        private System.Windows.Forms.ListBox _ListboxChars;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem _optAutoSet;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _optZoom2x;
        private System.Windows.Forms.ToolStripMenuItem _optZoom4x;
        private System.Windows.Forms.Panel _pnlFontImage;
        private System.Windows.Forms.ToolStripMenuItem _optNormalZoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem _optZoomIn;
        private System.Windows.Forms.ToolStripMenuItem _optZoomOut;
    }
}

