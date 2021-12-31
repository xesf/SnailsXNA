using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
 
namespace SpriteAnimationEditor.Controls
{
  internal partial class SolutionControl : UserControl
  {
   enum NodeType
    {
      None,
      Solution,
      Project,
      Animation
    }
  
    #region Events
    public delegate void ActiveAnimationChangedHandler(object sender, Animation animation);
    public delegate void ProjectAddedHandler(object sender, Project frame);
    public delegate void AnimationAddedHandler(object sender, Animation animation);

    public event ActiveAnimationChangedHandler ActiveAnimationChanged;
    public event ProjectAddedHandler ProjectAdded;
    public event AnimationAddedHandler AnimationAdded;
    #endregion

    #region Variables
    Solution _Solution;
    TreeNode _SolutionNode;
    Project _ActiveProject;
    Animation _ActiveAnimation;
    #endregion

    #region Properties
    TreeNode SolutionNode
    {
      get { return this._SolutionNode; }
      set { this._SolutionNode = value; }
    }

    public Solution Solution
    {
      get { return this._Solution; }
      set 
      {
        if (this.Solution != value)
        {
          if (value != null)
          {
            this._Solution = value;
            this.Solution.ProjectAdded += new Solution.ProjectAddedHandler(Solution_ProjectAdded);
          }
          else
          {
            this.Solution.ProjectAdded -= new Solution.ProjectAddedHandler(Solution_ProjectAdded);
          }
         this.Refresh();
        }
      }
    }

    NodeType SelectedNodeType
    {
      get
      {
        if (this._SolutionTree.SelectedNode == null)
          return NodeType.None;
        if (this._SolutionTree.SelectedNode.Tag as Solution != null)
          return NodeType.Solution;
        if (this._SolutionTree.SelectedNode.Tag as Project != null)
          return NodeType.Project;
        if (this._SolutionTree.SelectedNode.Tag as Animation != null)
          return NodeType.Animation;

        return NodeType.None;
      }
    }

    [BrowsableAttribute(false)]
    public Project ActiveProject
    {
      get { return this._ActiveProject; }
      private set { this._ActiveProject = value; }
    }

    [BrowsableAttribute(false)]
    public TreeNode ActiveProjectNode
    {
      get { return this.GetProjectNode(this.ActiveProject); }
    }

    [BrowsableAttribute(false)]
    public Animation ActiveAnimation
    {
      get { return this._ActiveAnimation; }
      private set 
      {
        if (this.ActiveAnimation != value)
        {
          this._ActiveAnimation = value;
          this.OnSelectedAnimationChanged(this.ActiveAnimation);
        }
      }
    }
    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    public SolutionControl()
    {
      InitializeComponent();
    }
    
    /// <summary>
    /// 
    /// </summary>
    public override void Refresh()
    {
      base.Refresh();
      this._SolutionTree.Nodes.Clear();
      if (this.Solution == null)
        return;
      this.SolutionNode = this._SolutionTree.Nodes.Add(this.Solution.Name);
      this.SolutionNode.Tag = this.Solution;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddAnimation(Animation anim)
    {
      if (this.ActiveProject == null)
        return;

      TreeNode node = this.ActiveProjectNode.Nodes.Add(anim.Name);
      node.Tag = anim;
      this._SolutionTree.SelectedNode = node;
      this.ActiveAnimation = anim;
      this.OnAnimationAdded(anim);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddProject(Project project)
    {
      TreeNode node = this.SolutionNode.Nodes.Add(project.Name);
      node.Tag = project;
      project.AnimationAdded += new Project.AnimationAddedHandler(Project_AnimationAdded);
      this._SolutionTree.SelectedNode = node;
      this.OnProjectAdded(project);
    }

    #region TreeView events
    /// <summary>
    /// 
    /// </summary>
    private void _SolutionTree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      try
      {
        switch (this.SelectedNodeType)
        {
          case NodeType.Solution:
            this._ObjProps.SelectedObject = this.Solution;
            break;

          case NodeType.Project:
            if (this.ActiveProject != null)
            {
              this.GetProjectNode(this.ActiveProject).NodeFont = new Font(this._SolutionTree.Font, FontStyle.Regular);
            }
            this.ActiveProject = (Project)this._SolutionTree.SelectedNode.Tag;
            this._SolutionTree.SelectedNode.NodeFont = new Font(this._SolutionTree.Font, FontStyle.Bold);
            this._ObjProps.SelectedObject = this.ActiveProject;
            break;

          case NodeType.Animation:
            if (this.ActiveAnimation != null)
            {
              this.GetAnimationNode(this.ActiveAnimation).ForeColor = this._SolutionTree.ForeColor;
            }
            this.ActiveAnimation = (Animation)this._SolutionTree.SelectedNode.Tag;
            this._SolutionTree.SelectedNode.ForeColor = Color.Green;
            this._ObjProps.SelectedObject = this.ActiveAnimation;
            break;

          default:
            this._ObjProps.SelectedObject = null;
            break;
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion

    #region Event Lauching
    /// <summary>
    /// 
    /// </summary>
    void OnSelectedAnimationChanged(Animation animation)
    {
      if (this.ActiveAnimationChanged != null)
      {
        this.ActiveAnimationChanged(this, animation);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnProjectAdded(Project project)
    {
      if (this.ProjectAdded != null)
      {
        this.ProjectAdded(this, project);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnAnimationAdded(Animation animation)
    {
      if (this.AnimationAdded != null)
      {
        this.AnimationAdded(this, animation);
      }
    }
    #endregion

    #region Project Events
    /// <summary>
    /// 
    /// </summary>
    void Project_AnimationAdded(object sender, Animation animation)
    {
      try
      {
        this.AddAnimation(animation);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion

    #region Solution Events
    /// <summary>
    /// 
    /// </summary>
    void Solution_ProjectAdded(object sender, Project project)
    {
      try
      {
        this.AddProject(project);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion

    #region Miscelaneous methods
    /// <summary>
    /// 
    /// </summary>
    TreeNode GetProjectNode(Project project)
    {
      foreach (TreeNode projNode in this.SolutionNode.Nodes)
      {
        if ((Project)projNode.Tag == project)
        {
          return projNode;
        }
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    TreeNode GetAnimationNode(Animation animation)
    {

      foreach (TreeNode animNode in this.GetProjectNode(animation.ParentProject).Nodes)
      {
        if ((Animation)animNode.Tag == animation)
        {
          return animNode;
        }
      }

      return null;
    }
    #endregion
  }
}
