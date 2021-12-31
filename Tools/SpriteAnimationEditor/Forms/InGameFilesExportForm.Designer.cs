namespace SpriteAnimationEditor.Forms
{
  partial class InGameFilesExportForm
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
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this._ProjectList = new System.Windows.Forms.CheckedListBox();
      this._BtnOk = new System.Windows.Forms.Button();
      this._BtnCancel = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._ChkOverride = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._TxtTextureFiles = new System.Windows.Forms.TextBox();
      this._TxtDataFiles = new System.Windows.Forms.TextBox();
      this._BtnTexturesFolder = new System.Windows.Forms.Button();
      this._BtnDataFolder = new System.Windows.Forms.Button();
      this._PanelOverrides = new System.Windows.Forms.Panel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this._DlgSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this._PanelOverrides.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this._ProjectList);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(453, 209);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(77, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select projects";
      // 
      // _ProjectList
      // 
      this._ProjectList.CheckOnClick = true;
      this._ProjectList.FormattingEnabled = true;
      this._ProjectList.Location = new System.Drawing.Point(12, 34);
      this._ProjectList.Name = "_ProjectList";
      this._ProjectList.Size = new System.Drawing.Size(429, 169);
      this._ProjectList.TabIndex = 1;
      this._ProjectList.SelectedIndexChanged += new System.EventHandler(this._ProjectList_SelectedIndexChanged);
      // 
      // _BtnOk
      // 
      this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._BtnOk.Location = new System.Drawing.Point(161, 17);
      this._BtnOk.Name = "_BtnOk";
      this._BtnOk.Size = new System.Drawing.Size(75, 23);
      this._BtnOk.TabIndex = 0;
      this._BtnOk.Text = "&Ok";
      this._BtnOk.UseVisualStyleBackColor = true;
      this._BtnOk.Click += new System.EventHandler(this._BtnOk_Click);
      // 
      // _BtnCancel
      // 
      this._BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._BtnCancel.Location = new System.Drawing.Point(242, 17);
      this._BtnCancel.Name = "_BtnCancel";
      this._BtnCancel.Size = new System.Drawing.Size(75, 23);
      this._BtnCancel.TabIndex = 1;
      this._BtnCancel.Text = "&Cancel";
      this._BtnCancel.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._BtnCancel);
      this.groupBox1.Controls.Add(this._BtnOk);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox1.Location = new System.Drawing.Point(0, 314);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(453, 52);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      // 
      // _ChkOverride
      // 
      this._ChkOverride.AutoSize = true;
      this._ChkOverride.Location = new System.Drawing.Point(12, 19);
      this._ChkOverride.Name = "_ChkOverride";
      this._ChkOverride.Size = new System.Drawing.Size(168, 17);
      this._ChkOverride.TabIndex = 0;
      this._ChkOverride.Text = "Override output project folders";
      this._ChkOverride.UseVisualStyleBackColor = true;
      this._ChkOverride.CheckedChanged += new System.EventHandler(this._ChkOverride_CheckedChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(64, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Texture files";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 32);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(98, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Animation data files";
      // 
      // _TxtTextureFiles
      // 
      this._TxtTextureFiles.Location = new System.Drawing.Point(120, 5);
      this._TxtTextureFiles.Name = "_TxtTextureFiles";
      this._TxtTextureFiles.Size = new System.Drawing.Size(272, 20);
      this._TxtTextureFiles.TabIndex = 1;
      // 
      // _TxtDataFiles
      // 
      this._TxtDataFiles.Location = new System.Drawing.Point(120, 29);
      this._TxtDataFiles.Name = "_TxtDataFiles";
      this._TxtDataFiles.Size = new System.Drawing.Size(271, 20);
      this._TxtDataFiles.TabIndex = 4;
      // 
      // _BtnTexturesFolder
      // 
      this._BtnTexturesFolder.Location = new System.Drawing.Point(398, 3);
      this._BtnTexturesFolder.Name = "_BtnTexturesFolder";
      this._BtnTexturesFolder.Size = new System.Drawing.Size(24, 23);
      this._BtnTexturesFolder.TabIndex = 2;
      this._BtnTexturesFolder.Text = "...";
      this._BtnTexturesFolder.UseVisualStyleBackColor = true;
      this._BtnTexturesFolder.Click += new System.EventHandler(this._BtnTexturesFolder_Click);
      // 
      // _BtnDataFolder
      // 
      this._BtnDataFolder.Location = new System.Drawing.Point(397, 29);
      this._BtnDataFolder.Name = "_BtnDataFolder";
      this._BtnDataFolder.Size = new System.Drawing.Size(25, 23);
      this._BtnDataFolder.TabIndex = 5;
      this._BtnDataFolder.Text = "...";
      this._BtnDataFolder.UseVisualStyleBackColor = true;
      this._BtnDataFolder.Click += new System.EventHandler(this._BtnDataFolder_Click);
      // 
      // _PanelOverrides
      // 
      this._PanelOverrides.Controls.Add(this._BtnTexturesFolder);
      this._PanelOverrides.Controls.Add(this._BtnDataFolder);
      this._PanelOverrides.Controls.Add(this.label2);
      this._PanelOverrides.Controls.Add(this.label3);
      this._PanelOverrides.Controls.Add(this._TxtDataFiles);
      this._PanelOverrides.Controls.Add(this._TxtTextureFiles);
      this._PanelOverrides.Location = new System.Drawing.Point(6, 43);
      this._PanelOverrides.Name = "_PanelOverrides";
      this._PanelOverrides.Size = new System.Drawing.Size(435, 56);
      this._PanelOverrides.TabIndex = 1;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this._ChkOverride);
      this.groupBox3.Controls.Add(this._PanelOverrides);
      this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox3.Location = new System.Drawing.Point(0, 209);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(453, 105);
      this.groupBox3.TabIndex = 2;
      this.groupBox3.TabStop = false;
      // 
      // InGameFilesExportForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(453, 366);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "InGameFilesExportForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Export in-game textures and animation data files";
      this.Load += new System.EventHandler(this.InGameFilesExportForm_Load);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this._PanelOverrides.ResumeLayout(false);
      this._PanelOverrides.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckedListBox _ProjectList;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _BtnOk;
    private System.Windows.Forms.Button _BtnCancel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox _ChkOverride;
    private System.Windows.Forms.Button _BtnDataFolder;
    private System.Windows.Forms.Button _BtnTexturesFolder;
    private System.Windows.Forms.TextBox _TxtDataFiles;
    private System.Windows.Forms.TextBox _TxtTextureFiles;
    private System.Windows.Forms.Panel _PanelOverrides;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.FolderBrowserDialog _DlgSelectFolder;
  }
}