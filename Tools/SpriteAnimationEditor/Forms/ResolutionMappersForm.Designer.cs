namespace SpriteAnimationEditor.Forms
{
  partial class ResolutionMappersForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResolutionMappersForm));
      this._lstMappers = new System.Windows.Forms.ListBox();
      this.label1 = new System.Windows.Forms.Label();
      this._btnAdd = new System.Windows.Forms.Button();
      this._lblProject = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this._imgThumb = new System.Windows.Forms.PictureBox();
      this._btnEdit = new System.Windows.Forms.Button();
      this._btnDelete = new System.Windows.Forms.Button();
      this._pnlHelp = new System.Windows.Forms.Panel();
      this.label3 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this._imgThumb)).BeginInit();
      this._pnlHelp.SuspendLayout();
      this.SuspendLayout();
      // 
      // _lstMappers
      // 
      this._lstMappers.FormattingEnabled = true;
      this._lstMappers.Location = new System.Drawing.Point(12, 52);
      this._lstMappers.Name = "_lstMappers";
      this._lstMappers.Size = new System.Drawing.Size(160, 186);
      this._lstMappers.TabIndex = 0;
      this._lstMappers.SelectedIndexChanged += new System.EventHandler(this._lstMappers_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 36);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Mappings";
      // 
      // _btnAdd
      // 
      this._btnAdd.Location = new System.Drawing.Point(178, 52);
      this._btnAdd.Name = "_btnAdd";
      this._btnAdd.Size = new System.Drawing.Size(108, 23);
      this._btnAdd.TabIndex = 2;
      this._btnAdd.Text = "Add...";
      this._btnAdd.UseVisualStyleBackColor = true;
      this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
      // 
      // _lblProject
      // 
      this._lblProject.AutoSize = true;
      this._lblProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._lblProject.Location = new System.Drawing.Point(85, 9);
      this._lblProject.Name = "_lblProject";
      this._lblProject.Size = new System.Drawing.Size(54, 13);
      this._lblProject.TabIndex = 6;
      this._lblProject.Text = "[project]";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(43, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Project:";
      // 
      // _imgThumb
      // 
      this._imgThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this._imgThumb.Location = new System.Drawing.Point(178, 150);
      this._imgThumb.Name = "_imgThumb";
      this._imgThumb.Size = new System.Drawing.Size(110, 88);
      this._imgThumb.TabIndex = 7;
      this._imgThumb.TabStop = false;
      // 
      // _btnEdit
      // 
      this._btnEdit.Location = new System.Drawing.Point(178, 81);
      this._btnEdit.Name = "_btnEdit";
      this._btnEdit.Size = new System.Drawing.Size(108, 23);
      this._btnEdit.TabIndex = 9;
      this._btnEdit.Text = "Edit...";
      this._btnEdit.UseVisualStyleBackColor = true;
      this._btnEdit.Click += new System.EventHandler(this._btnEdit_Click);
      // 
      // _btnDelete
      // 
      this._btnDelete.Location = new System.Drawing.Point(178, 113);
      this._btnDelete.Name = "_btnDelete";
      this._btnDelete.Size = new System.Drawing.Size(108, 23);
      this._btnDelete.TabIndex = 10;
      this._btnDelete.Text = "Delete";
      this._btnDelete.UseVisualStyleBackColor = true;
      this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
      // 
      // _pnlHelp
      // 
      this._pnlHelp.Controls.Add(this.label3);
      this._pnlHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._pnlHelp.Location = new System.Drawing.Point(0, 249);
      this._pnlHelp.Name = "_pnlHelp";
      this._pnlHelp.Padding = new System.Windows.Forms.Padding(3);
      this._pnlHelp.Size = new System.Drawing.Size(298, 78);
      this._pnlHelp.TabIndex = 11;
      // 
      // label3
      // 
      this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label3.Location = new System.Drawing.Point(3, 3);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(292, 72);
      this.label3.TabIndex = 0;
      this.label3.Text = resources.GetString("label3.Text");
      // 
      // ResolutionMappersForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(298, 327);
      this.Controls.Add(this._pnlHelp);
      this.Controls.Add(this._btnDelete);
      this.Controls.Add(this._btnEdit);
      this.Controls.Add(this._imgThumb);
      this.Controls.Add(this._lblProject);
      this.Controls.Add(this.label2);
      this.Controls.Add(this._btnAdd);
      this.Controls.Add(this.label1);
      this.Controls.Add(this._lstMappers);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ResolutionMappersForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Resolution Mapping";
      this.Load += new System.EventHandler(this.ResolutionMappersForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this._imgThumb)).EndInit();
      this._pnlHelp.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox _lstMappers;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _btnAdd;
    private System.Windows.Forms.Label _lblProject;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.PictureBox _imgThumb;
    private System.Windows.Forms.Button _btnEdit;
    private System.Windows.Forms.Button _btnDelete;
    private System.Windows.Forms.Panel _pnlHelp;
    private System.Windows.Forms.Label label3;
  }
}