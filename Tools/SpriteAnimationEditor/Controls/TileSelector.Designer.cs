namespace SpriteAnimationEditor.Controls
{
  partial class TileSelector
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
      this._cmbProject = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this._cmbAnimation = new System.Windows.Forms.ComboBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this._pnlTiles = new System.Windows.Forms.Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _cmbProject
      // 
      this._cmbProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbProject.FormattingEnabled = true;
      this._cmbProject.Location = new System.Drawing.Point(65, 3);
      this._cmbProject.Name = "_cmbProject";
      this._cmbProject.Size = new System.Drawing.Size(153, 21);
      this._cmbProject.TabIndex = 0;
      this._cmbProject.SelectedIndexChanged += new System.EventHandler(this._cmbProject_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(40, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Project";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 33);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(53, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Animation";
      // 
      // _cmbAnimation
      // 
      this._cmbAnimation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this._cmbAnimation.FormattingEnabled = true;
      this._cmbAnimation.Location = new System.Drawing.Point(65, 30);
      this._cmbAnimation.Name = "_cmbAnimation";
      this._cmbAnimation.Size = new System.Drawing.Size(153, 21);
      this._cmbAnimation.TabIndex = 2;
      this._cmbAnimation.SelectedIndexChanged += new System.EventHandler(this._cmbAnimation_SelectedIndexChanged);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this._cmbProject);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this._cmbAnimation);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(223, 59);
      this.panel1.TabIndex = 4;
      // 
      // _pnlTiles
      // 
      this._pnlTiles.BackColor = System.Drawing.Color.White;
      this._pnlTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._pnlTiles.Dock = System.Windows.Forms.DockStyle.Fill;
      this._pnlTiles.Location = new System.Drawing.Point(0, 59);
      this._pnlTiles.Name = "_pnlTiles";
      this._pnlTiles.Size = new System.Drawing.Size(223, 341);
      this._pnlTiles.TabIndex = 4;
      this._pnlTiles.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlTiles_Paint);
      // 
      // TileSelector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._pnlTiles);
      this.Controls.Add(this.panel1);
      this.Name = "TileSelector";
      this.Size = new System.Drawing.Size(223, 400);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox _cmbProject;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox _cmbAnimation;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel _pnlTiles;
  }
}
