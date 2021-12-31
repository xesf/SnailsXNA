namespace SpriteAnimationEditor.Controls
{
  partial class SolutionControl
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
      this._SolutionTree = new System.Windows.Forms.TreeView();
      this._GrpSolution = new SpriteAnimationEditor.Controls.Group();
      this._GrpProps = new SpriteAnimationEditor.Controls.Group();
      this._ObjProps = new System.Windows.Forms.PropertyGrid();
      this.splitter1 = new System.Windows.Forms.Splitter();
      this._GrpSolution.BodyPanel.SuspendLayout();
      this._GrpSolution.SuspendLayout();
      this._GrpProps.BodyPanel.SuspendLayout();
      this._GrpProps.SuspendLayout();
      this.SuspendLayout();
      // 
      // _SolutionTree
      // 
      this._SolutionTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this._SolutionTree.HideSelection = false;
      this._SolutionTree.Location = new System.Drawing.Point(0, 0);
      this._SolutionTree.Name = "_SolutionTree";
      this._SolutionTree.Size = new System.Drawing.Size(369, 93);
      this._SolutionTree.TabIndex = 0;
      this._SolutionTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._SolutionTree_AfterSelect);
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
      this._GrpSolution.BodyPanel.Size = new System.Drawing.Size(369, 93);
      this._GrpSolution.BodyPanel.TabIndex = 2;
      this._GrpSolution.Dock = System.Windows.Forms.DockStyle.Fill;
      this._GrpSolution.Location = new System.Drawing.Point(0, 0);
      this._GrpSolution.Name = "_GrpSolution";
      this._GrpSolution.Size = new System.Drawing.Size(369, 111);
      this._GrpSolution.TabIndex = 2;
      this._GrpSolution.Text = "Solution";
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
      this._GrpProps.BodyPanel.Size = new System.Drawing.Size(369, 145);
      this._GrpProps.BodyPanel.TabIndex = 2;
      this._GrpProps.Dock = System.Windows.Forms.DockStyle.Bottom;
      this._GrpProps.Location = new System.Drawing.Point(0, 114);
      this._GrpProps.Name = "_GrpProps";
      this._GrpProps.Size = new System.Drawing.Size(369, 163);
      this._GrpProps.TabIndex = 3;
      this._GrpProps.Text = "Properties";
      // 
      // _ObjProps
      // 
      this._ObjProps.Dock = System.Windows.Forms.DockStyle.Fill;
      this._ObjProps.HelpVisible = false;
      this._ObjProps.Location = new System.Drawing.Point(0, 0);
      this._ObjProps.Name = "_ObjProps";
      this._ObjProps.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
      this._ObjProps.Size = new System.Drawing.Size(369, 145);
      this._ObjProps.TabIndex = 0;
      this._ObjProps.ToolbarVisible = false;
      // 
      // splitter1
      // 
      this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitter1.Location = new System.Drawing.Point(0, 111);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new System.Drawing.Size(369, 3);
      this.splitter1.TabIndex = 4;
      this.splitter1.TabStop = false;
      // 
      // SolutionControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this._GrpSolution);
      this.Controls.Add(this.splitter1);
      this.Controls.Add(this._GrpProps);
      this.Name = "SolutionControl";
      this.Size = new System.Drawing.Size(369, 277);
      this._GrpSolution.BodyPanel.ResumeLayout(false);
      this._GrpSolution.ResumeLayout(false);
      this._GrpProps.BodyPanel.ResumeLayout(false);
      this._GrpProps.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView _SolutionTree;
    private Group _GrpSolution;
    private Group _GrpProps;
    private System.Windows.Forms.Splitter splitter1;
    private System.Windows.Forms.PropertyGrid _ObjProps;
  }
}
