namespace LevelEditor.Controls
{
  partial class SolutionCtl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionCtl));
            this._images = new System.Windows.Forms.ImageList(this.components);
            this._grpSolution = new TwoBrains.Common.Controls.Group();
            this._tviewSolution = new System.Windows.Forms.TreeView();
            this._grpSolution.BodyPanel.SuspendLayout();
            this._grpSolution.SuspendLayout();
            this.SuspendLayout();
            // 
            // _images
            // 
            this._images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_images.ImageStream")));
            this._images.TransparentColor = System.Drawing.Color.Transparent;
            this._images.Images.SetKeyName(0, "solution_icon.png");
            this._images.Images.SetKeyName(1, "theme_a_icon.png");
            this._images.Images.SetKeyName(2, "theme_b_icon.png");
            this._images.Images.SetKeyName(3, "theme_c_icon.png");
            this._images.Images.SetKeyName(4, "theme_d_icon.png");
            this._images.Images.SetKeyName(5, "theme_custom_icon.png");
            this._images.Images.SetKeyName(6, "goal_escort_icon.png");
            this._images.Images.SetKeyName(7, "goal_kill_icon.png");
            this._images.Images.SetKeyName(8, "goal_king_icon.png");
            this._images.Images.SetKeyName(9, "goal_time_icon.png");
            // 
            // _grpSolution
            // 
            this._grpSolution.AllowCollapse = false;
            // 
            // _grpSolution._PanelBody
            // 
            this._grpSolution.BodyPanel.BackColor = System.Drawing.SystemColors.Control;
            this._grpSolution.BodyPanel.Controls.Add(this._tviewSolution);
            this._grpSolution.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpSolution.BodyPanel.Location = new System.Drawing.Point(5, 23);
            this._grpSolution.BodyPanel.Name = "_PanelBody";
            this._grpSolution.BodyPanel.Size = new System.Drawing.Size(189, 363);
            this._grpSolution.BodyPanel.TabIndex = 2;
            this._grpSolution.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
            this._grpSolution.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._grpSolution.CaptionVisible = true;
            this._grpSolution.Collapsed = false;
            this._grpSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpSolution.Location = new System.Drawing.Point(0, 0);
            this._grpSolution.Name = "_grpSolution";
            this._grpSolution.Padding = new System.Windows.Forms.Padding(5);
            this._grpSolution.Size = new System.Drawing.Size(199, 391);
            this._grpSolution.TabIndex = 0;
            this._grpSolution.Text = "Solution";
            // 
            // _tviewSolution
            // 
            this._tviewSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tviewSolution.HideSelection = false;
            this._tviewSolution.ImageIndex = 0;
            this._tviewSolution.ImageList = this._images;
            this._tviewSolution.Location = new System.Drawing.Point(0, 0);
            this._tviewSolution.Name = "_tviewSolution";
            this._tviewSolution.SelectedImageIndex = 0;
            this._tviewSolution.Size = new System.Drawing.Size(189, 363);
            this._tviewSolution.TabIndex = 0;
            this._tviewSolution.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this._tviewSolution_BeforeSelect);
            this._tviewSolution.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tviewSolution_AfterSelect);
            // 
            // SolutionCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grpSolution);
            this.Name = "SolutionCtl";
            this.Size = new System.Drawing.Size(199, 391);
            this._grpSolution.BodyPanel.ResumeLayout(false);
            this._grpSolution.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private TwoBrains.Common.Controls.Group _grpSolution;
    private System.Windows.Forms.TreeView _tviewSolution;
    private System.Windows.Forms.ImageList _images;
  }
}
