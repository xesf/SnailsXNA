namespace SpriteAnimationEditor.Controls
{
  partial class SolutionManager
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionManager));
      this._TreeImages = new System.Windows.Forms.ImageList(this.components);
      this._Splitter = new System.Windows.Forms.Splitter();
      this._MenuSolution = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._OptAddNewProject = new System.Windows.Forms.ToolStripMenuItem();
      this._OptAddExistingProject = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this._OptCloseSolution = new System.Windows.Forms.ToolStripMenuItem();
      this._MenuProject = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._OptRemoveProject = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this._OptAddNewAnim = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this._OptAddFramesToProject = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this._optSelProjImage = new System.Windows.Forms.ToolStripMenuItem();
      this._optRefreshProjImage = new System.Windows.Forms.ToolStripMenuItem();
      this.separ = new System.Windows.Forms.ToolStripSeparator();
      this._optMoveProjUp = new System.Windows.Forms.ToolStripMenuItem();
      this._optMoveProjDown = new System.Windows.Forms.ToolStripMenuItem();
      this._MenuAnimation = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._OptDelAnimation = new System.Windows.Forms.ToolStripMenuItem();
      this._MenuFrame = new System.Windows.Forms.ContextMenuStrip(this.components);
      this._optCopyFrame = new System.Windows.Forms.ToolStripMenuItem();
      this._OptDelFrame = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this._OptEditColZones = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this._OptMoveFrameUp = new System.Windows.Forms.ToolStripMenuItem();
      this._OptMoveFrameDown = new System.Windows.Forms.ToolStripMenuItem();
      this._menuSep1 = new System.Windows.Forms.ToolStripSeparator();
      this._optPasteFrame = new System.Windows.Forms.ToolStripMenuItem();
      this._GrpProps = new SpriteAnimationEditor.Controls.Group();
      this._ObjProps = new System.Windows.Forms.PropertyGrid();
      this._GrpSolution = new SpriteAnimationEditor.Controls.Group();
      this._SolutionTree = new System.Windows.Forms.TreeView();
      this._MenuSolution.SuspendLayout();
      this._MenuProject.SuspendLayout();
      this._MenuAnimation.SuspendLayout();
      this._MenuFrame.SuspendLayout();
      this._GrpProps.BodyPanel.SuspendLayout();
      this._GrpProps.SuspendLayout();
      this._GrpSolution.BodyPanel.SuspendLayout();
      this._GrpSolution.SuspendLayout();
      this.SuspendLayout();
      // 
      // _TreeImages
      // 
      this._TreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_TreeImages.ImageStream")));
      this._TreeImages.TransparentColor = System.Drawing.Color.Transparent;
      this._TreeImages.Images.SetKeyName(0, "solution.png");
      this._TreeImages.Images.SetKeyName(1, "project.png");
      this._TreeImages.Images.SetKeyName(2, "animation.png");
      this._TreeImages.Images.SetKeyName(3, "frame.png");
      // 
      // _Splitter
      // 
      this._Splitter.Dock = System.Windows.Forms.DockStyle.Top;
      this._Splitter.Location = new System.Drawing.Point(0, 131);
      this._Splitter.Name = "_Splitter";
      this._Splitter.Size = new System.Drawing.Size(369, 3);
      this._Splitter.TabIndex = 4;
      this._Splitter.TabStop = false;
      // 
      // _MenuSolution
      // 
      this._MenuSolution.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptAddNewProject,
            this._OptAddExistingProject,
            this.toolStripSeparator3,
            this._OptCloseSolution});
      this._MenuSolution.Name = "_MenuSolution";
      this._MenuSolution.Size = new System.Drawing.Size(189, 76);
      // 
      // _OptAddNewProject
      // 
      this._OptAddNewProject.Image = global::SpriteAnimationEditor.Properties.Resources.new_project;
      this._OptAddNewProject.Name = "_OptAddNewProject";
      this._OptAddNewProject.Size = new System.Drawing.Size(188, 22);
      this._OptAddNewProject.Text = "Add New Project...";
      this._OptAddNewProject.Click += new System.EventHandler(this._OptAddNewProject_Click);
      // 
      // _OptAddExistingProject
      // 
      this._OptAddExistingProject.Name = "_OptAddExistingProject";
      this._OptAddExistingProject.Size = new System.Drawing.Size(188, 22);
      this._OptAddExistingProject.Text = "Add Existing Project...";
      this._OptAddExistingProject.Click += new System.EventHandler(this._OptAddExistingProject_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(185, 6);
      // 
      // _OptCloseSolution
      // 
      this._OptCloseSolution.Image = global::SpriteAnimationEditor.Properties.Resources.close_solution;
      this._OptCloseSolution.Name = "_OptCloseSolution";
      this._OptCloseSolution.Size = new System.Drawing.Size(188, 22);
      this._OptCloseSolution.Text = "Close";
      this._OptCloseSolution.Click += new System.EventHandler(this._OptCloseSolution_Click);
      // 
      // _MenuProject
      // 
      this._MenuProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptRemoveProject,
            this.toolStripSeparator2,
            this._OptAddNewAnim,
            this.toolStripSeparator1,
            this._OptAddFramesToProject,
            this.toolStripSeparator5,
            this._optSelProjImage,
            this._optRefreshProjImage,
            this.separ,
            this._optMoveProjUp,
            this._optMoveProjDown});
      this._MenuProject.Name = "_MenuProject";
      this._MenuProject.Size = new System.Drawing.Size(216, 182);
      this._MenuProject.Opened += new System.EventHandler(this._MenuProject_Opened);
      this._MenuProject.Opening += new System.ComponentModel.CancelEventHandler(this._MenuProject_Opening);
      // 
      // _OptRemoveProject
      // 
      this._OptRemoveProject.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptRemoveProject.Name = "_OptRemoveProject";
      this._OptRemoveProject.Size = new System.Drawing.Size(215, 22);
      this._OptRemoveProject.Text = "Remove From the Solution";
      this._OptRemoveProject.Click += new System.EventHandler(this._OptRemoveProject_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(212, 6);
      // 
      // _OptAddNewAnim
      // 
      this._OptAddNewAnim.Image = global::SpriteAnimationEditor.Properties.Resources.new_animattion;
      this._OptAddNewAnim.Name = "_OptAddNewAnim";
      this._OptAddNewAnim.Size = new System.Drawing.Size(215, 22);
      this._OptAddNewAnim.Text = "Add New Animation";
      this._OptAddNewAnim.Click += new System.EventHandler(this._OptAddNewAnim_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
      // 
      // _OptAddFramesToProject
      // 
      this._OptAddFramesToProject.Name = "_OptAddFramesToProject";
      this._OptAddFramesToProject.Size = new System.Drawing.Size(215, 22);
      this._OptAddFramesToProject.Text = "Add Frames From Files...";
      this._OptAddFramesToProject.Click += new System.EventHandler(this._OptAddFramesToProject_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(212, 6);
      // 
      // _optSelProjImage
      // 
      this._optSelProjImage.Name = "_optSelProjImage";
      this._optSelProjImage.Size = new System.Drawing.Size(215, 22);
      this._optSelProjImage.Text = "Select Image...";
      this._optSelProjImage.Click += new System.EventHandler(this._optSelProjImage_Click);
      // 
      // _optRefreshProjImage
      // 
      this._optRefreshProjImage.Name = "_optRefreshProjImage";
      this._optRefreshProjImage.Size = new System.Drawing.Size(215, 22);
      this._optRefreshProjImage.Text = "Refresh Image";
      this._optRefreshProjImage.Click += new System.EventHandler(this._optRefreshProjImage_Click);
      // 
      // separ
      // 
      this.separ.Name = "separ";
      this.separ.Size = new System.Drawing.Size(212, 6);
      // 
      // _optMoveProjUp
      // 
      this._optMoveProjUp.Name = "_optMoveProjUp";
      this._optMoveProjUp.Size = new System.Drawing.Size(215, 22);
      this._optMoveProjUp.Text = "Move Up";
      this._optMoveProjUp.Click += new System.EventHandler(this._optMoveUp_Click);
      // 
      // _optMoveProjDown
      // 
      this._optMoveProjDown.Name = "_optMoveProjDown";
      this._optMoveProjDown.Size = new System.Drawing.Size(215, 22);
      this._optMoveProjDown.Text = "Move Down";
      this._optMoveProjDown.Click += new System.EventHandler(this._optMoveDown_Click);
      // 
      // _MenuAnimation
      // 
      this._MenuAnimation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._OptDelAnimation,
            this._menuSep1,
            this._optPasteFrame});
      this._MenuAnimation.Name = "_MenuAnimation";
      this._MenuAnimation.Size = new System.Drawing.Size(179, 76);
      this._MenuAnimation.Opening += new System.ComponentModel.CancelEventHandler(this._MenuAnimation_Opening);
      // 
      // _OptDelAnimation
      // 
      this._OptDelAnimation.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptDelAnimation.Name = "_OptDelAnimation";
      this._OptDelAnimation.Size = new System.Drawing.Size(178, 22);
      this._OptDelAnimation.Text = "Delete From Project";
      this._OptDelAnimation.Click += new System.EventHandler(this._OptDelAnimation_Click);
      // 
      // _MenuFrame
      // 
      this._MenuFrame.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._optCopyFrame,
            this._OptDelFrame,
            this.toolStripSeparator4,
            this._OptEditColZones,
            this.toolStripSeparator6,
            this._OptMoveFrameUp,
            this._OptMoveFrameDown});
      this._MenuFrame.Name = "_MenuFrame";
      this._MenuFrame.Size = new System.Drawing.Size(179, 126);
      this._MenuFrame.Opening += new System.ComponentModel.CancelEventHandler(this._MenuFrame_Opening);
      // 
      // _optCopyFrame
      // 
      this._optCopyFrame.Name = "_optCopyFrame";
      this._optCopyFrame.Size = new System.Drawing.Size(178, 22);
      this._optCopyFrame.Text = "Copy";
      this._optCopyFrame.Click += new System.EventHandler(this._optCopyFrame_Click);
      // 
      // _OptDelFrame
      // 
      this._OptDelFrame.Image = global::SpriteAnimationEditor.Properties.Resources.delete;
      this._OptDelFrame.Name = "_OptDelFrame";
      this._OptDelFrame.Size = new System.Drawing.Size(178, 22);
      this._OptDelFrame.Text = "Delete";
      this._OptDelFrame.Click += new System.EventHandler(this._OptDelFrame_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(175, 6);
      // 
      // _OptEditColZones
      // 
      this._OptEditColZones.Name = "_OptEditColZones";
      this._OptEditColZones.Size = new System.Drawing.Size(178, 22);
      this._OptEditColZones.Text = "Edit Collision Zones";
      this._OptEditColZones.Click += new System.EventHandler(this._OptEditColZones_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(175, 6);
      // 
      // _OptMoveFrameUp
      // 
      this._OptMoveFrameUp.Name = "_OptMoveFrameUp";
      this._OptMoveFrameUp.Size = new System.Drawing.Size(178, 22);
      this._OptMoveFrameUp.Text = "Move Up";
      this._OptMoveFrameUp.Click += new System.EventHandler(this._OptMoveFrameUp_Click);
      // 
      // _OptMoveFrameDown
      // 
      this._OptMoveFrameDown.Name = "_OptMoveFrameDown";
      this._OptMoveFrameDown.Size = new System.Drawing.Size(178, 22);
      this._OptMoveFrameDown.Text = "Move Down";
      this._OptMoveFrameDown.Click += new System.EventHandler(this._OptMoveFrameDown_Click);
      // 
      // _menuSep1
      // 
      this._menuSep1.Name = "_menuSep1";
      this._menuSep1.Size = new System.Drawing.Size(175, 6);
      // 
      // _optPasteFrame
      // 
      this._optPasteFrame.Name = "_optPasteFrame";
      this._optPasteFrame.Size = new System.Drawing.Size(178, 22);
      this._optPasteFrame.Text = "Paste Frame";
      this._optPasteFrame.Click += new System.EventHandler(this._optPasteFrame_Click);
      // 
      // _GrpProps
      // 
      // 
      // _GrpProps._PanelBody
      // 
      this._GrpProps.BodyPanel.BackColor = System.Drawing.Color.Transparent;
      this._GrpProps.BodyPanel.Controls.Add(this._ObjProps);
      this._GrpProps.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this._GrpProps.BodyPanel.Location = new System.Drawing.Point(0, 18);
      this._GrpProps.BodyPanel.Name = "_PanelBody";
      this._GrpProps.BodyPanel.Size = new System.Drawing.Size(369, 125);
      this._GrpProps.BodyPanel.TabIndex = 2;
      this._GrpProps.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
      this._GrpProps.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this._GrpProps.CaptionVisible = true;
      this._GrpProps.Dock = System.Windows.Forms.DockStyle.Fill;
      this._GrpProps.Location = new System.Drawing.Point(0, 134);
      this._GrpProps.Name = "_GrpProps";
      this._GrpProps.Size = new System.Drawing.Size(369, 143);
      this._GrpProps.TabIndex = 3;
      this._GrpProps.Text = "Properties";
      // 
      // _ObjProps
      // 
      this._ObjProps.Dock = System.Windows.Forms.DockStyle.Fill;
      this._ObjProps.HelpVisible = false;
      this._ObjProps.Location = new System.Drawing.Point(0, 0);
      this._ObjProps.Name = "_ObjProps";
      this._ObjProps.Size = new System.Drawing.Size(369, 125);
      this._ObjProps.TabIndex = 0;
      this._ObjProps.ToolbarVisible = false;
      // 
      // _GrpSolution
      // 
      // 
      // _GrpSolution._PanelBody
      // 
      this._GrpSolution.BodyPanel.BackColor = System.Drawing.Color.Transparent;
      this._GrpSolution.BodyPanel.Controls.Add(this._SolutionTree);
      this._GrpSolution.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this._GrpSolution.BodyPanel.Location = new System.Drawing.Point(0, 18);
      this._GrpSolution.BodyPanel.Name = "_PanelBody";
      this._GrpSolution.BodyPanel.Size = new System.Drawing.Size(369, 113);
      this._GrpSolution.BodyPanel.TabIndex = 2;
      this._GrpSolution.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption;
      this._GrpSolution.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this._GrpSolution.CaptionVisible = true;
      this._GrpSolution.Dock = System.Windows.Forms.DockStyle.Top;
      this._GrpSolution.Location = new System.Drawing.Point(0, 0);
      this._GrpSolution.Name = "_GrpSolution";
      this._GrpSolution.Size = new System.Drawing.Size(369, 131);
      this._GrpSolution.TabIndex = 2;
      this._GrpSolution.Text = "Solution";
      // 
      // _SolutionTree
      // 
      this._SolutionTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this._SolutionTree.HideSelection = false;
      this._SolutionTree.ImageIndex = 0;
      this._SolutionTree.ImageList = this._TreeImages;
      this._SolutionTree.Location = new System.Drawing.Point(0, 0);
      this._SolutionTree.Name = "_SolutionTree";
      this._SolutionTree.SelectedImageIndex = 0;
      this._SolutionTree.Size = new System.Drawing.Size(369, 113);
      this._SolutionTree.TabIndex = 0;
      this._SolutionTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._SolutionTree_AfterSelect);
      this._SolutionTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this._SolutionTree_MouseDown);
      // 
      // SolutionManager
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._GrpProps);
      this.Controls.Add(this._Splitter);
      this.Controls.Add(this._GrpSolution);
      this.Name = "SolutionManager";
      this.Size = new System.Drawing.Size(369, 277);
      this._MenuSolution.ResumeLayout(false);
      this._MenuProject.ResumeLayout(false);
      this._MenuAnimation.ResumeLayout(false);
      this._MenuFrame.ResumeLayout(false);
      this._GrpProps.BodyPanel.ResumeLayout(false);
      this._GrpProps.ResumeLayout(false);
      this._GrpSolution.BodyPanel.ResumeLayout(false);
      this._GrpSolution.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView _SolutionTree;
    private Group _GrpSolution;
    private Group _GrpProps;
    private System.Windows.Forms.Splitter _Splitter;
    private System.Windows.Forms.PropertyGrid _ObjProps;
    private System.Windows.Forms.ContextMenuStrip _MenuSolution;
    private System.Windows.Forms.ToolStripMenuItem _OptAddNewProject;
    private System.Windows.Forms.ToolStripMenuItem _OptAddExistingProject;
    private System.Windows.Forms.ContextMenuStrip _MenuProject;
    private System.Windows.Forms.ToolStripMenuItem _OptRemoveProject;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem _OptAddNewAnim;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem _OptAddFramesToProject;
    private System.Windows.Forms.ContextMenuStrip _MenuAnimation;
    private System.Windows.Forms.ToolStripMenuItem _OptDelAnimation;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem _OptCloseSolution;
    private System.Windows.Forms.ImageList _TreeImages;
    private System.Windows.Forms.ContextMenuStrip _MenuFrame;
    private System.Windows.Forms.ToolStripMenuItem _OptDelFrame;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem _OptMoveFrameUp;
    private System.Windows.Forms.ToolStripMenuItem _OptMoveFrameDown;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem _optMoveProjUp;
    private System.Windows.Forms.ToolStripMenuItem _optMoveProjDown;
    private System.Windows.Forms.ToolStripMenuItem _optSelProjImage;
    private System.Windows.Forms.ToolStripSeparator separ;
    private System.Windows.Forms.ToolStripMenuItem _optRefreshProjImage;
	private System.Windows.Forms.ToolStripMenuItem _OptEditColZones;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
  private System.Windows.Forms.ToolStripMenuItem _optCopyFrame;
  private System.Windows.Forms.ToolStripSeparator _menuSep1;
  private System.Windows.Forms.ToolStripMenuItem _optPasteFrame;
  }
}
