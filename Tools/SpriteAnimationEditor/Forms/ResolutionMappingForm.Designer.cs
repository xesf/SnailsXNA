namespace SpriteAnimationEditor.Forms
{
  partial class ResolutionMappingForm
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
      this._imgImage = new System.Windows.Forms.PictureBox();
      this._txtName = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this._lblProject = new System.Windows.Forms.Label();
      this._pnlImage = new System.Windows.Forms.Panel();
      this._pnlProperties = new System.Windows.Forms.Panel();
      this._chkActive = new System.Windows.Forms.CheckBox();
      this._btnSelFolder = new System.Windows.Forms.Button();
      this._txtSSOutput = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this._mnuMain = new System.Windows.Forms.MenuStrip();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._optSelImage = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this._optClose = new System.Windows.Forms.ToolStripMenuItem();
      this._pnlAnimations = new System.Windows.Forms.Panel();
      this._lstAnimations = new System.Windows.Forms.ListBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._DlgOpen = new System.Windows.Forms.OpenFileDialog();
      this.panel1 = new System.Windows.Forms.Panel();
      this._btnCancel = new System.Windows.Forms.Button();
      this._btnOk = new System.Windows.Forms.Button();
      this._dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
      this._lblImage = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.ttPngFile = new System.Windows.Forms.ToolTip(this.components);
      this.ttSSFile = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this._imgImage)).BeginInit();
      this._pnlImage.SuspendLayout();
      this._pnlProperties.SuspendLayout();
      this._mnuMain.SuspendLayout();
      this._pnlAnimations.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _imgImage
      // 
      this._imgImage.Location = new System.Drawing.Point(3, 3);
      this._imgImage.Name = "_imgImage";
      this._imgImage.Size = new System.Drawing.Size(196, 113);
      this._imgImage.TabIndex = 0;
      this._imgImage.TabStop = false;
      this._imgImage.Paint += new System.Windows.Forms.PaintEventHandler(this._imgImage_Paint);
      // 
      // _txtName
      // 
      this._txtName.Location = new System.Drawing.Point(89, 34);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new System.Drawing.Size(162, 20);
      this._txtName.TabIndex = 1;
      this._txtName.TextChanged += new System.EventHandler(this._txtName_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 37);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Name";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 13);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(43, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Project:";
      // 
      // _lblProject
      // 
      this._lblProject.AutoSize = true;
      this._lblProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._lblProject.Location = new System.Drawing.Point(86, 13);
      this._lblProject.Name = "_lblProject";
      this._lblProject.Size = new System.Drawing.Size(54, 13);
      this._lblProject.TabIndex = 0;
      this._lblProject.Text = "[project]";
      // 
      // _pnlImage
      // 
      this._pnlImage.AutoScroll = true;
      this._pnlImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._pnlImage.Controls.Add(this._imgImage);
      this._pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pnlImage.Location = new System.Drawing.Point(0, 143);
      this._pnlImage.Name = "_pnlImage";
      this._pnlImage.Size = new System.Drawing.Size(292, 227);
      this._pnlImage.TabIndex = 5;
      // 
      // _pnlProperties
      // 
      this._pnlProperties.Controls.Add(this._lblImage);
      this._pnlProperties.Controls.Add(this.label7);
      this._pnlProperties.Controls.Add(this._chkActive);
      this._pnlProperties.Controls.Add(this._btnSelFolder);
      this._pnlProperties.Controls.Add(this._txtSSOutput);
      this._pnlProperties.Controls.Add(this.label4);
      this._pnlProperties.Controls.Add(this._lblProject);
      this._pnlProperties.Controls.Add(this._txtName);
      this._pnlProperties.Controls.Add(this.label1);
      this._pnlProperties.Controls.Add(this.label2);
      this._pnlProperties.Dock = System.Windows.Forms.DockStyle.Top;
      this._pnlProperties.Location = new System.Drawing.Point(0, 24);
      this._pnlProperties.Name = "_pnlProperties";
      this._pnlProperties.Size = new System.Drawing.Size(492, 119);
      this._pnlProperties.TabIndex = 6;
      // 
      // _chkActive
      // 
      this._chkActive.AutoSize = true;
      this._chkActive.Location = new System.Drawing.Point(16, 80);
      this._chkActive.Name = "_chkActive";
      this._chkActive.Size = new System.Drawing.Size(56, 17);
      this._chkActive.TabIndex = 3;
      this._chkActive.Text = "Active";
      this._chkActive.UseVisualStyleBackColor = true;
      // 
      // _btnSelFolder
      // 
      this._btnSelFolder.Location = new System.Drawing.Point(458, 58);
      this._btnSelFolder.Name = "_btnSelFolder";
      this._btnSelFolder.Size = new System.Drawing.Size(26, 20);
      this._btnSelFolder.TabIndex = 9;
      this._btnSelFolder.Text = "...";
      this._btnSelFolder.UseVisualStyleBackColor = true;
      this._btnSelFolder.Click += new System.EventHandler(this._btnSelFolder_Click);
      // 
      // _txtSSOutput
      // 
      this._txtSSOutput.Location = new System.Drawing.Point(89, 58);
      this._txtSSOutput.Name = "_txtSSOutput";
      this._txtSSOutput.Size = new System.Drawing.Size(363, 20);
      this._txtSSOutput.TabIndex = 2;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 61);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(70, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "SS output file";
      // 
      // _mnuMain
      // 
      this._mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
      this._mnuMain.Location = new System.Drawing.Point(0, 0);
      this._mnuMain.Name = "_mnuMain";
      this._mnuMain.Size = new System.Drawing.Size(492, 24);
      this._mnuMain.TabIndex = 7;
      this._mnuMain.Text = "menuStrip1";
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optSelImage,
            this.toolStripSeparator1,
            this._optClose});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.editToolStripMenuItem.Text = "Edit";
      // 
      // _optSelImage
      // 
      this._optSelImage.Name = "_optSelImage";
      this._optSelImage.Size = new System.Drawing.Size(157, 22);
      this._optSelImage.Text = "Select image...";
      this._optSelImage.Click += new System.EventHandler(this._optSelImage_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
      // 
      // _optClose
      // 
      this._optClose.Name = "_optClose";
      this._optClose.Size = new System.Drawing.Size(157, 22);
      this._optClose.Text = "Close";
      // 
      // _pnlAnimations
      // 
      this._pnlAnimations.Controls.Add(this._lstAnimations);
      this._pnlAnimations.Controls.Add(this.label5);
      this._pnlAnimations.Controls.Add(this.label3);
      this._pnlAnimations.Dock = System.Windows.Forms.DockStyle.Right;
      this._pnlAnimations.Location = new System.Drawing.Point(292, 143);
      this._pnlAnimations.Name = "_pnlAnimations";
      this._pnlAnimations.Padding = new System.Windows.Forms.Padding(5);
      this._pnlAnimations.Size = new System.Drawing.Size(200, 227);
      this._pnlAnimations.TabIndex = 8;
      // 
      // _lstAnimations
      // 
      this._lstAnimations.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._lstAnimations.FormattingEnabled = true;
      this._lstAnimations.Location = new System.Drawing.Point(5, 49);
      this._lstAnimations.Name = "_lstAnimations";
      this._lstAnimations.Size = new System.Drawing.Size(190, 173);
      this._lstAnimations.TabIndex = 0;
      this._lstAnimations.SelectedIndexChanged += new System.EventHandler(this._lstAnimations_SelectedIndexChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 33);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(123, 13);
      this.label5.TabIndex = 1;
      this.label5.Text = "Animations in the Project";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(2, 5);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(124, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Browse animation frames";
      // 
      // _DlgOpen
      // 
      this._DlgOpen.DefaultExt = "png";
      this._DlgOpen.Filter = "PNG Files|*.png";
      this._DlgOpen.InitialDirectory = "c:\\temp\\a\\";
      this._DlgOpen.Multiselect = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this._btnCancel);
      this.panel1.Controls.Add(this._btnOk);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 370);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(492, 40);
      this.panel1.TabIndex = 9;
      // 
      // _btnCancel
      // 
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(248, 7);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // _btnOk
      // 
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._btnOk.Location = new System.Drawing.Point(167, 7);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 0;
      this._btnOk.Text = "&Ok";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      this._btnOk.Validating += new System.ComponentModel.CancelEventHandler(this._btnOk_Validating);
      // 
      // _lblImage
      // 
      this._lblImage.AutoSize = true;
      this._lblImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._lblImage.Location = new System.Drawing.Point(86, 100);
      this._lblImage.Name = "_lblImage";
      this._lblImage.Size = new System.Drawing.Size(48, 13);
      this._lblImage.TabIndex = 10;
      this._lblImage.Text = "[image]";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(13, 100);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(39, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Image:";
      // 
      // ResolutionMappingForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(492, 410);
      this.Controls.Add(this._pnlImage);
      this.Controls.Add(this._pnlAnimations);
      this.Controls.Add(this._pnlProperties);
      this.Controls.Add(this._mnuMain);
      this.Controls.Add(this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ResolutionMappingForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Resolution Mapping";
      this.Load += new System.EventHandler(this.ResolutionMappingForm_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ResolutionMappingForm_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this._imgImage)).EndInit();
      this._pnlImage.ResumeLayout(false);
      this._pnlProperties.ResumeLayout(false);
      this._pnlProperties.PerformLayout();
      this._mnuMain.ResumeLayout(false);
      this._mnuMain.PerformLayout();
      this._pnlAnimations.ResumeLayout(false);
      this._pnlAnimations.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox _imgImage;
    private System.Windows.Forms.TextBox _txtName;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label _lblProject;
    private System.Windows.Forms.Panel _pnlImage;
    private System.Windows.Forms.Panel _pnlProperties;
    private System.Windows.Forms.TextBox _txtSSOutput;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.MenuStrip _mnuMain;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _optSelImage;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem _optClose;
    private System.Windows.Forms.Panel _pnlAnimations;
    private System.Windows.Forms.ListBox _lstAnimations;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button _btnSelFolder;
    private System.Windows.Forms.CheckBox _chkActive;
    private System.Windows.Forms.OpenFileDialog _DlgOpen;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.FolderBrowserDialog _dlgFolder;
    private System.Windows.Forms.Label _lblImage;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.ToolTip ttPngFile;
    private System.Windows.Forms.ToolTip ttSSFile;
  }
}