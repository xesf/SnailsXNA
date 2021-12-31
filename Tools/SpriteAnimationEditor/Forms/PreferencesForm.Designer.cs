namespace SpriteAnimationEditor.Forms
{
  partial class PreferencesForm
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this._PrefsGrid = new System.Windows.Forms.PropertyGrid();
      this.fontDialog1 = new System.Windows.Forms.FontDialog();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this._BtnOk = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._PrefsGrid);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(452, 241);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      // 
      // _PrefsGrid
      // 
      this._PrefsGrid.HelpVisible = false;
      this._PrefsGrid.Location = new System.Drawing.Point(6, 12);
      this._PrefsGrid.Name = "_PrefsGrid";
      this._PrefsGrid.Size = new System.Drawing.Size(440, 218);
      this._PrefsGrid.TabIndex = 0;
      this._PrefsGrid.ToolbarVisible = false;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this._BtnOk);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox2.Location = new System.Drawing.Point(0, 241);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(452, 45);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      // 
      // _BtnOk
      // 
      this._BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this._BtnOk.Location = new System.Drawing.Point(189, 14);
      this._BtnOk.Name = "_BtnOk";
      this._BtnOk.Size = new System.Drawing.Size(75, 23);
      this._BtnOk.TabIndex = 0;
      this._BtnOk.Text = "&Ok";
      this._BtnOk.UseVisualStyleBackColor = true;
      this._BtnOk.Click += new System.EventHandler(this._BtnOk_Click);
      // 
      // PreferencesForm
      // 
      this.AcceptButton = this._BtnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(452, 286);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.groupBox2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PreferencesForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Preferences";
      this.Load += new System.EventHandler(this.PreferencesForm_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.FontDialog fontDialog1;
    private System.Windows.Forms.PropertyGrid _PrefsGrid;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button _BtnOk;
  }
}