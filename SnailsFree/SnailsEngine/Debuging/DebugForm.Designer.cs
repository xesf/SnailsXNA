namespace TwoBrainsGames.Snails.Debuging
{
    partial class DebugForm
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
      this._List = new System.Windows.Forms.ListBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this._cbVerbose = new System.Windows.Forms.CheckBox();
      this._btnClear = new System.Windows.Forms.Button();
      this.panel2 = new System.Windows.Forms.Panel();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this._SettingsInfo = new TwoBrainsGames.Snails.Debuging.SettingsInfo();
      this.panel3 = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.btnSetSnailProps = new System.Windows.Forms.Button();
      this._chkDrawQuadtree = new System.Windows.Forms.CheckBox();
      this._btnKillUnselected = new System.Windows.Forms.Button();
      this._btnRefresh = new System.Windows.Forms.Button();
      this._btnKill = new System.Windows.Forms.Button();
      this._lstSnails = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // _List
      // 
      this._List.Dock = System.Windows.Forms.DockStyle.Fill;
      this._List.FormattingEnabled = true;
      this._List.Location = new System.Drawing.Point(5, 5);
      this._List.Name = "_List";
      this._List.Size = new System.Drawing.Size(474, 199);
      this._List.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this._cbVerbose);
      this.panel1.Controls.Add(this._btnClear);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 156);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(802, 42);
      this.panel1.TabIndex = 1;
      // 
      // _cbVerbose
      // 
      this._cbVerbose.AutoSize = true;
      this._cbVerbose.Location = new System.Drawing.Point(88, 11);
      this._cbVerbose.Name = "_cbVerbose";
      this._cbVerbose.Size = new System.Drawing.Size(65, 17);
      this._cbVerbose.TabIndex = 1;
      this._cbVerbose.Text = "Verbose";
      this._cbVerbose.UseVisualStyleBackColor = true;
      // 
      // _btnClear
      // 
      this._btnClear.Location = new System.Drawing.Point(7, 7);
      this._btnClear.Name = "_btnClear";
      this._btnClear.Size = new System.Drawing.Size(75, 23);
      this._btnClear.TabIndex = 0;
      this._btnClear.Text = "Clear";
      this._btnClear.UseVisualStyleBackColor = true;
      this._btnClear.Click += new System.EventHandler(this._btnClear_Click);
      // 
      // panel2
      // 
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.groupBox2);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(802, 156);
      this.panel2.TabIndex = 2;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this._SettingsInfo);
      this.groupBox2.Location = new System.Drawing.Point(4, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(785, 143);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Game Settings";
      // 
      // _SettingsInfo
      // 
      this._SettingsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this._SettingsInfo.Location = new System.Drawing.Point(3, 16);
      this._SettingsInfo.Name = "_SettingsInfo";
      this._SettingsInfo.Padding = new System.Windows.Forms.Padding(3);
      this._SettingsInfo.Settings = null;
      this._SettingsInfo.Size = new System.Drawing.Size(779, 124);
      this._SettingsInfo.TabIndex = 1;
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this._List);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel3.Location = new System.Drawing.Point(0, 198);
      this.panel3.Name = "panel3";
      this.panel3.Padding = new System.Windows.Forms.Padding(5);
      this.panel3.Size = new System.Drawing.Size(484, 210);
      this.panel3.TabIndex = 3;
      // 
      // panel4
      // 
      this.panel4.Controls.Add(this.groupBox3);
      this.panel4.Controls.Add(this._btnKillUnselected);
      this.panel4.Controls.Add(this._btnRefresh);
      this.panel4.Controls.Add(this._btnKill);
      this.panel4.Controls.Add(this._lstSnails);
      this.panel4.Controls.Add(this.label1);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel4.Location = new System.Drawing.Point(484, 198);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(318, 210);
      this.panel4.TabIndex = 4;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.btnSetSnailProps);
      this.groupBox3.Controls.Add(this._chkDrawQuadtree);
      this.groupBox3.Location = new System.Drawing.Point(113, 118);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(156, 86);
      this.groupBox3.TabIndex = 8;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Snail properties";
      // 
      // btnSetSnailProps
      // 
      this.btnSetSnailProps.Location = new System.Drawing.Point(6, 57);
      this.btnSetSnailProps.Name = "btnSetSnailProps";
      this.btnSetSnailProps.Size = new System.Drawing.Size(144, 23);
      this.btnSetSnailProps.TabIndex = 7;
      this.btnSetSnailProps.Text = "Set Props On Selected";
      this.btnSetSnailProps.UseVisualStyleBackColor = true;
      this.btnSetSnailProps.Click += new System.EventHandler(this.btnSetSnailProps_Click);
      // 
      // _chkDrawQuadtree
      // 
      this._chkDrawQuadtree.AutoSize = true;
      this._chkDrawQuadtree.Location = new System.Drawing.Point(16, 19);
      this._chkDrawQuadtree.Name = "_chkDrawQuadtree";
      this._chkDrawQuadtree.Size = new System.Drawing.Size(98, 17);
      this._chkDrawQuadtree.TabIndex = 6;
      this._chkDrawQuadtree.Text = "Draw Quadtree";
      this._chkDrawQuadtree.UseVisualStyleBackColor = true;
      // 
      // _btnKillUnselected
      // 
      this._btnKillUnselected.Location = new System.Drawing.Point(112, 89);
      this._btnKillUnselected.Name = "_btnKillUnselected";
      this._btnKillUnselected.Size = new System.Drawing.Size(89, 23);
      this._btnKillUnselected.TabIndex = 5;
      this._btnKillUnselected.Text = "Kill Unselected";
      this._btnKillUnselected.UseVisualStyleBackColor = true;
      this._btnKillUnselected.Click += new System.EventHandler(this._btnKillUnselected_Click);
      // 
      // _btnRefresh
      // 
      this._btnRefresh.Location = new System.Drawing.Point(112, 31);
      this._btnRefresh.Name = "_btnRefresh";
      this._btnRefresh.Size = new System.Drawing.Size(89, 23);
      this._btnRefresh.TabIndex = 4;
      this._btnRefresh.Text = "Refresh";
      this._btnRefresh.UseVisualStyleBackColor = true;
      this._btnRefresh.Click += new System.EventHandler(this._btnRefresh_Click);
      // 
      // _btnKill
      // 
      this._btnKill.Location = new System.Drawing.Point(113, 60);
      this._btnKill.Name = "_btnKill";
      this._btnKill.Size = new System.Drawing.Size(89, 23);
      this._btnKill.TabIndex = 2;
      this._btnKill.Text = "Kill Selected";
      this._btnKill.UseVisualStyleBackColor = true;
      this._btnKill.Click += new System.EventHandler(this._btnKill_Click);
      // 
      // _lstSnails
      // 
      this._lstSnails.FormattingEnabled = true;
      this._lstSnails.Location = new System.Drawing.Point(9, 31);
      this._lstSnails.Name = "_lstSnails";
      this._lstSnails.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this._lstSnails.Size = new System.Drawing.Size(98, 173);
      this._lstSnails.TabIndex = 1;
      this._lstSnails.SelectedIndexChanged += new System.EventHandler(this._lstSnails_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 5);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Snails";
      // 
      // DebugForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(802, 408);
      this.Controls.Add(this.panel3);
      this.Controls.Add(this.panel4);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.panel2);
      this.Name = "DebugForm";
      this.Text = "Debug Window";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox _List;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button _btnClear;
    private System.Windows.Forms.GroupBox groupBox2;
    private SettingsInfo _SettingsInfo;
    private System.Windows.Forms.CheckBox _cbVerbose;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _btnRefresh;
    private System.Windows.Forms.Button _btnKill;
    private System.Windows.Forms.ListBox _lstSnails;
    private System.Windows.Forms.Button _btnKillUnselected;
    private System.Windows.Forms.Button btnSetSnailProps;
    private System.Windows.Forms.CheckBox _chkDrawQuadtree;
    private System.Windows.Forms.GroupBox groupBox3;
  }
}