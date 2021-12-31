using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
 
namespace SpriteAnimationEditor.Controls
{
  internal partial class SolutionManager : UserControl
  {
    #region Types
    public enum NodeType
    {
      None,
      Solution,
      Project,
      Animation,
      Frame
    }
    #endregion

    #region Events
    public delegate void FrameHandler(object sender, AnimationFrame frame);
    public delegate void AnimationFrameHandler(object sender, AnimationFrame frame);
    public delegate void FrameAttributeChangedHandler(object sender, AnimationFrame frame);
    public delegate void AnimationHandler(object sender, Animation animation);
    public delegate void FrameListHandlerHandler(object sender, List<AnimationFrame> frames);
    public delegate void ProjectHandler(object sender, Project project);
    public delegate void SolutionHandler(object sender, Solution solution);

    public event EventHandler SolutionNameChanged;
    public event EventHandler SolutionFilenameChanged;
    public event EventHandler SolutionClosed;
    public event EventHandler SelectedProjectChanged;
    public event EventHandler SelectedAnimationChanged;
    public event AnimationFrameHandler FrameAddedToAnimation;
    public event FrameListHandlerHandler FramesRemoved;
    public event EventHandler ProjectOutputSpriteChanged;
    public event AnimationHandler AnimationFramesPerSecondChanged;
    public event EventHandler ProjectFrameNrsColorChanged;
    public event EventHandler ProjectFrameRectangleColorChanged;
    public event EventHandler ProjectSpriteBackColorChanged;
    public event EventHandler ProjectAttributeChanged;
    public event FrameAttributeChangedHandler FrameAttributeChanged;
    public event FrameHandler FrameMovedUp;
    public event FrameHandler FrameMovedDown;
	  public event FrameHandler EditFrameCollisionZonesClicked;
    public event AnimationHandler AnimationParentAnimationChanged;
    public event ProjectHandler ProjectLoaded;
    public event ProjectHandler ProjectLoadStarted;
    public event SolutionHandler SolutionDataLoaded;

    // Frame events
    public event EventHandler SelectedFrameChanged;

    // Context menu events
    public event EventHandler CtxMenuAddNewProjectClicked;
    public event EventHandler CtxMenuAddExistingProjectClicked;
    public event EventHandler CtxMenuCloseSolutionClicked;

    public event EventHandler CtxMenuRemoveProjecClicked;
    public event EventHandler CtxMenuAddNewAnimClicked;
    public event EventHandler CtxMenuAddFramesToProjectClicked;

    public event EventHandler CtxMenuDelAnimationClicked;
    #endregion

    #region Variables
    Solution _Solution;
    TreeNode _SolutionNode;
    Project _SelectedProject;
    Animation _SelectedAnimation;
    AnimationFrame _SelectedFrame;
    bool _SelectedNodeChangedSupressed;
    #endregion

    #region Properties
    TreeNode SolutionNode
    {
      get { return this._SolutionNode; }
      set { this._SolutionNode = value; }
    }

    public int SolutionExplorerHeight
    { 
      get { return this._GrpSolution.Height; }
      set { this._GrpSolution.Height = value; }
    }

    public NodeType SelectedNodeType
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
        if (this._SolutionTree.SelectedNode.Tag as AnimationFrame != null)
          return NodeType.Frame;

        return NodeType.None;
      }
    }

    [BrowsableAttribute(false)]
    public Solution Solution
    {
      get { return this._Solution; }
      private set 
      { 
        if (this._Solution != value)
        {
          this._Solution = value;
          if (this._Solution == null)
          {
            this.OnSolutionClosed();
          }
        }
      }
    }
    
    [BrowsableAttribute(false)]
    public bool IsSolutionChanged
    {
      get
      {
        if (this.Solution == null)
          return false;

        return this.Solution.Changed;
      }
    }

    [BrowsableAttribute(false)]
    public Project SelectedProject
    {
      get 
      { 
        return this._SelectedProject; 
      }
      private set 
      {
        if (this._SelectedProject != value)
        {
          if (this.SelectedProject != null)
          {
            TreeNode node = this.GetProjectNode(this._SelectedProject);
            if (node != null)
              node.NodeFont = new Font(this._SolutionTree.Font, FontStyle.Regular);
          }
          this._SelectedProject = value;
          this.OnSelectedProjectChanged();
          if (this._SelectedProject != null)
          {
            TreeNode node = this.SelectedProjectNode;
            node.NodeFont = new Font(this._SolutionTree.Font, FontStyle.Bold);
            node.Text = node.Text; // Workaround for bold labels bug
            if (node.Nodes.Count > 0)
              this.SelectedAnimation = (Animation)node.Nodes[0].Tag;
            else
              this.SelectedAnimation = null;
          }
          else
            this.SelectedAnimation = null;
        }
      }
    }

    [BrowsableAttribute(false)]
    public TreeNode SelectedProjectNode
    {
      get { return this.GetProjectNode(this.SelectedProject); }
    }

    [BrowsableAttribute(false)]
    public Animation SelectedAnimation
    {
      get { return this._SelectedAnimation; }
      private set 
      {
        if (this._SelectedAnimation != value)
        {
          if (this._SelectedAnimation != null)
          {
            TreeNode animNode = this.GetAnimationNode(this._SelectedAnimation);
            if (animNode != null)
              animNode.ForeColor = this._SolutionTree.ForeColor;
          }
          this._SelectedAnimation = value;
          if (this._SelectedAnimation != null)
          {
            TreeNode node = this.GetAnimationNode(this._SelectedAnimation);
            if (node != null)
              node.ForeColor = Color.Green;
          }
          this.OnSelectedAnimationChanged();
        }
      }
    }

    [BrowsableAttribute(false)]
    public AnimationFrame SelectedFrame
    {
      get { return this._SelectedFrame; }
      set
      {
        if (this._SelectedFrame != value)
        {
          this._SelectedFrame = value;
          this._SolutionTree.SelectedNode = this.GetFrameNode(this._SelectedFrame);
          this.OnSelectedFrameChanged();
        }
      }
    }

    public TreeNode SelectedFrameNode
    {
      get 
      { 
        return this.GetFrameNode(this.SelectedFrame); 
      }
 
    }
    [BrowsableAttribute(false)]
    private Project FirstProject
    {
      get
      {
        if (this.Solution == null)
          return null;
        if (this.Solution.ProjectList.Count == 0)
          return null;
        return this.Solution.ProjectList[0];
      }
    }

    [BrowsableAttribute(false)]
    private Animation FirstAnimation
    {
      get
      {
        if (this.SelectedProject == null)
          return null;
        if (this.SelectedProject.AnimationList.Count == 0)
          return null;

        return this.SelectedProject.AnimationList[0];
      }
    }

    [BrowsableAttribute(false)]
    public List<Project> ChangedProjects
    {
      get
      {
        List<Project> projectList = new List<Project>();
        if (this.Solution != null)
        {
          foreach (Project project in this.Solution.ProjectList)
          {
            if (project.Changed)
              projectList.Add(project);
          }
        }
        return projectList;
      }
    }

    [BrowsableAttribute(false)]
    public List<Project> UnloadedProjects
    {
      get
      {
        List<Project> projectList = new List<Project>();
        if (this.Solution != null)
        {
          foreach (Project project in this.Solution.ProjectList)
          {
            if (!project.Loaded)
              projectList.Add(project);
          }
        }
        return projectList;
      }
    }

    [BrowsableAttribute(false)]
    public bool SolutionLoaded
    {
      get { return this.Solution != null; }
    }

    [BrowsableAttribute(false)]
    public int SolutionHeight
    {
      get { return this._GrpSolution.Height; }
      set { this._GrpSolution.Height = value; }
    }
    #endregion

    #region Class constructors and overrides
    /// <summary>
    /// 
    /// </summary>
    public SolutionManager()
    {
      InitializeComponent();
        //this._SolutionTree.TreeViewNodeSorter = new SpritesetNodeSorter();
    }
    #endregion

    #region Frame methods

    /// <summary>
    /// 
    /// </summary>
    public void RemoveFrames(List<AnimationFrame> frames)
    {
      if (this.SelectedAnimation == null)
        return;

      this.SuspendSelectedNodeChanged();
      foreach (AnimationFrame animFrame in frames)
      {
        this.RemoveFrame(animFrame);
      }

      this.ResumeSelectedNodeChanged();
      this.OnFramesRemoved(frames);
      this._SolutionTree_AfterSelect(this._SolutionTree, new TreeViewEventArgs(this._SolutionTree.SelectedNode));
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveFrame(AnimationFrame frame)
    {
      if (this.SelectedAnimation == null)
        return;
      if (frame == null)
        return;

      TreeNode animationNode = this.GetAnimationNode(this.SelectedAnimation);
      TreeNode frameNode = this.GetFrameNode(frame);
      TreeNode sibling = frameNode.NextNode;

      this.SelectedAnimation.RemoveFrame(frame);
      animationNode.Nodes.Remove(frameNode);
      this.RefreshProps();
      if (sibling != null)
      {
        this._SolutionTree.SelectedNode = sibling;
      }
      this.RefreshNode(animationNode);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptDelFrame_Click(object sender, EventArgs e)
    {
      List<AnimationFrame> framesToDel = new List<AnimationFrame>();
      framesToDel.Add(this.SelectedFrame);
      this.RemoveFrames(framesToDel);
    }


    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveFrameUp_Click(object sender, EventArgs e)
    {
      this.MoveFrameUp(this.SelectedFrame);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveFrameDown_Click(object sender, EventArgs e)
    {
      this.MoveFrameDown(this.SelectedFrame);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _MenuFrame_Opening(object sender, CancelEventArgs e)
    {
      this._OptMoveFrameUp.Enabled = false;
      this._OptMoveFrameDown.Enabled = false;

      if (this.SelectedFrameNode == null)
        return;

      this._OptMoveFrameUp.Enabled = (this.SelectedFrameNode.Index > 0);
      this._OptMoveFrameDown.Enabled = (this.SelectedFrameNode.NextNode != null);
    }

    #endregion

    #region Animation methods
    /// <summary>
    /// 
    /// </summary>
    public void DeleteAnimation()
    {
      if (this.SelectedAnimation == null)
        return;

      if (this.SelectedAnimation.ParentProject == null)
        return;

      TreeNode animNode = this.GetAnimationNode(this.SelectedAnimation);
      TreeNode projNode = this.GetProjectNode(this.SelectedAnimation.ParentProject);
      this.SelectedAnimation.ParentProject.RemoveAnimation(this.SelectedAnimation);
      projNode.Nodes.Remove(animNode);
      this.SelectedAnimation = this.FirstAnimation;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddFrameToAnimation(Animation animation, Rectangle rect)
    {
      if (animation == null)
        return;
      AnimationFrame animFrame = new AnimationFrame(rect, animation);
      this.AddFrameToAnimation(animation, animFrame);
      /*
      AnimationFrame animFrame = animation.ParentProject.AddFrameToAnimation(animation, rect);
      this.AddFrameNode(this.GetAnimationNode(animation), animFrame);
      this.OnFrameAddedToAnimation(animFrame);
      this.RefreshProps();*/
    }
    /// <summary>
    /// 
    /// </summary>
    public void AddFrameToAnimation(Animation animation, AnimationFrame animFrame)
    {
      if (animation == null || animFrame == null)
        return;
      animation.AddFrame(animFrame);
      animFrame.FrameAttributeChanged += new EventHandler(frame_FrameAttributeChanged);

      this.AddFrameNode(this.GetAnimationNode(animation), animFrame);
      this.OnFrameAddedToAnimation(animFrame);

      this.RefreshProps();
    }
    /// <summary>
    /// 
    /// </summary>
    private void RegisterAnimationEvents(Animation animation)
    {
      animation.NameChanged += new EventHandler(Animation_NameChanged);
      animation.FramesPerSecondChanged += new EventHandler(Animation_FramesPerSecondChanged);
      animation.AttributeChanged += new EventHandler(Animation_AttributeChanged);
      animation.ParentAnimationChanged += new EventHandler(animation_ParentAnimationChanged);
      foreach (AnimationFrame frame in animation.Frames)
      {
        frame.FrameAttributeChanged += new EventHandler(frame_FrameAttributeChanged);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void frame_FrameAttributeChanged(object sender, EventArgs e)
    {
      this.OnFrameAttributeChangedHandler((AnimationFrame)sender);  
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddAnimation(Project project, Animation animation)
    {
      if (project == null)
        return;

      animation.ParentProject = project;
      this.RegisterAnimationEvents(animation);
      this.SelectedProject.AddAnimation(animation);
      this._SolutionTree.SelectedNode = this.AddAnimationNode(this.SelectedProjectNode, animation);
      this.SelectedAnimation = animation;
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveFrameUp(AnimationFrame frame)
    {
      if (this.SelectedAnimation == null)
        return;

      if (this.SelectedAnimation.ContainsFrame(frame) == false)
        return;

      
      this.SelectedAnimation.MoveFrameUp(frame);
      TreeNode frameNode = this.GetFrameNode(frame);
      int idx = frameNode.Index;
      TreeNode animationNode = this.GetAnimationNode(frame.ParentAnimation);
      frameNode.Remove();
      animationNode.Nodes.Insert(idx - 1, frameNode);
      this._SolutionTree.SelectedNode = frameNode;
      this.RefreshNode(animationNode);
      this.OnFrameMovedUp(frame);
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveFrameDown(AnimationFrame frame)
    {
      if (this.SelectedAnimation == null)
        return;

      if (this.SelectedAnimation.ContainsFrame(frame) == false)
        return;

      this.SelectedAnimation.MoveFrameDown(frame);

      TreeNode frameNode = this.GetFrameNode(frame);
      int idx = frameNode.Index;
      TreeNode animationNode = this.GetAnimationNode(frame.ParentAnimation);
      frameNode.Remove();
      animationNode.Nodes.Insert(idx + 1, frameNode);
      this._SolutionTree.SelectedNode = frameNode;
      this.RefreshNode(animationNode);

      this.OnFrameMovedDown(frame);

    }

    /// <summary>
    /// 
    /// </summary>
    void Animation_NameChanged(object sender, EventArgs e)
    {
      try
      {
        Animation animation = (Animation)sender;
        this.GetAnimationNode(animation).Text = animation.Name;
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Animation_AttributeChanged(object sender, EventArgs e)
    {
      try
      {
        this.RefreshProps();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void animation_ParentAnimationChanged(object sender, EventArgs e)
    {
      this.OnAnimationParentAnimationChanged((Animation)sender);
    }

    /// <summary>
    /// 
    /// </summary>
    void Animation_FramesPerSecondChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnAnimationFramesPerSecondChanged((Animation)sender);
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void SelectFirstAnimation()
    {
      if (this._SolutionNode == null)
        return;

      if (this._SolutionNode.Nodes.Count == 0)
      {
        this.SelectedProject = null;
        this.SelectedAnimation = null;
        this._SolutionTree.SelectedNode = this._SolutionNode;
        return;
      }
      this.SelectedProject = (Project)this._SolutionNode.Nodes[0].Tag;
      if (this._SolutionNode.Nodes[0].Nodes.Count == 0)
      {
        this._SolutionTree.SelectedNode = this._SolutionNode.Nodes[0];
        return;
      }
      this._SolutionTree.SelectedNode = this._SolutionNode.Nodes[0].Nodes[0];

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptDelAnimation_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuDelAnimationClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    #endregion
 
    #region Project methods

    /// <summary>
    /// 
    /// </summary>
    public void SelectProjectImage(Project project)
    {
      if (project == null)
        return;

      OpenFileDialog dlg = new OpenFileDialog();
      dlg.Filter = Settings.IMAGE_FILER;
      dlg.InitialDirectory = Settings.Instance.LastImportFramesFolder;
      if (dlg.ShowDialog(this) == DialogResult.Cancel)
      {
        return;
      }
      Settings.Instance.LastImportFramesFolder = Path.GetDirectoryName(dlg.FileName);
      project.SetSprite(dlg.FileName);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshProjectImage(Project project)
    {
      if (project == null)
        return;
      if (project.WithSprite == false)
        return;
      project.RefreshOutputSprite();
    }


    /// <summary>
    /// 
    /// </summary>
    private void _optRefreshProjImage_Click(object sender, EventArgs e)
    {
      this.RefreshProjectImage(this.SelectedProject);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optSelProjImage_Click(object sender, EventArgs e)
    {
     
       this.SelectProjectImage(this.SelectedProject);
    
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptRemoveProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuRemoveProjecClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptAddNewAnim_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuAddNewAnimClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptAddFramesToProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuAddFramesToProjectClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    private void RegisterProjectEvents(Project project)
    {
      project.NameChanged += new EventHandler(Project_NameChanged);
      project.FilenameChanged += new EventHandler(Project_FilenameChanged);
      project.OutputSpriteChanged += new EventHandler(Project_OutputSpriteChanged);
      project.FramesPerColumnChanged += new EventHandler(Project_FramesPerColumnChanged);
      project.FrameNrsColorChanged += new EventHandler(Project_FrameNrsColorChanged);
      project.FrameRectangleColorChanged += new EventHandler(Project_FrameRectangleColorChanged);
      project.SpriteBackColorChanged += new EventHandler(Project_SpriteBackColorChanged);
      project.AttributeChanged  += new EventHandler(Project_AttributeChanged);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddProject(Project project)
    {
      if (this.Solution == null)
        return;
      project.ParentSolution = this.Solution;
      this.Solution.AddProject(project);
      this.RegisterProjectEvents(project);
      this._SolutionTree.SelectedNode = this.AddProjectNode(project);
    }

    /// <summary>
    /// 
    /// </summary>
    public void DeleteProject()
    {
      if (this.SelectedProject == null)
        return;
      if (this.SelectedProject.ParentSolution == null)
        return;

      TreeNode projNode = this.GetProjectNode(this.SelectedProject);
      this.Solution.RemoveProject(this.SelectedProject);
      this.SolutionNode.Nodes.Remove(projNode);
      this.SelectedProject = this.FirstProject;
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_FramesPerColumnChanged(object sender, EventArgs e)
    {
      try
      {
        Project project = (Project)sender;
        project.RefreshOutputSprite();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_FrameRectangleColorChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnProjectFrameRectangleColorChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_SpriteBackColorChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnProjectSpriteBackColorChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_AttributeChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnProjectAttributeChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_FrameNrsColorChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnProjectFrameNrsColorChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }  
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_NameChanged(object sender, EventArgs e)
    {
      try
      {
        Project project = (Project)sender;
        this.GetProjectNode(project).Text = project.Name;
        this.RefreshProps();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Project_OutputSpriteChanged(object sender, EventArgs e)
    {
      try
      {
        this.OnProjectOutputSpriteChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void Project_FilenameChanged(object sender, EventArgs e)
    {
      try
      {
        this.RefreshProps();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _MenuProject_Opened(object sender, EventArgs e)
    {
      try
      {
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateSpriteFromFiles(List<string> files, int framesPerColumn)
    {
      if (this.SelectedProject == null)
        return;
      this.SelectedProject.CreateSpriteFromFiles(files, framesPerColumn);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveProjectUp()
    {
      if (this.SelectedProject == null)
      {
        return;
      }

      this.Solution.MoveProjectUp(this.SelectedProject);
      this.MoveNodeUp(this.SelectedProjectNode);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveProjectDown()
    {
      if (this.SelectedProject == null)
      {
        return;
      }
      this.Solution.MoveProjectDown(this.SelectedProject);
      this.MoveNodeDown(this.SelectedProjectNode);
    }
    #endregion

    #region Solution methods
    
    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
      this.Solution = null;
      this._SolutionTree.Nodes.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddSolution(Solution solution)
    {
      this.Solution = solution;
      this.Solution.NameChanged += new EventHandler(Solution_NameChanged);
      this.Solution.FilenameChanged += new EventHandler(Solution_FilenameChanged);
      this._SolutionTree.SelectedNode = this.AddSolutionNode(solution);
    }

    /// <summary>
    /// 
    /// </summary>
    TreeNode GetSolutionNode(Solution solution)
    {
      foreach (TreeNode node in this._SolutionTree.Nodes)
      {
        if (node.Tag as Solution != null && (Solution)(node.Tag) == solution)
          return node;
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public void CloseSolution()
    {
      this.SelectedProject = null;
      this.SelectedAnimation = null;
      this.SolutionNode = null;
      this._SolutionTree.Nodes.Clear();
      this.Solution = null;
    }
    /// <summary>
    /// 
    /// </summary>
    void Solution_NameChanged(object sender, EventArgs e)
    {
      try
      {
        this.SolutionNode.Text = this.Solution.Name;
        this.RefreshProps();
        this.OnSolutionNameChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void Solution_FilenameChanged(object sender, EventArgs e)
    {
      try
      {
        this.SolutionNode.Text = this.Solution.Name;
        this.RefreshProps();
        this.OnSolutionFilenameChanged();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OpenSolution(string filename)
    {
      this.CloseSolution();
      Solution solution = Solution.CreateFromXml(filename);
      this.AddSolution(solution);
      this.OnSolutionLoaded(solution);
      
      foreach (Project project in this.UnloadedProjects)
      {
        try
        {
          this.OnProjectLoadStarted(project);
          project.Reload();
          this.AddProjectNode(project);
          this.OnProjectLoaded(project);
          this._SolutionTree.Sort();
          this.SolutionNode.Expand();
        }
        catch (System.Exception ex)
        {
          MessageBox.Show(this.ParentForm, "Unable to load project " + project.Name + ". " + ex.Message, Settings.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      // Load tile testers  - can only be loaded after all projects are loaded
      this.Solution.LoadTileTesters();

      // Register events
      foreach (Project project in this.Solution.ProjectList)
      {
        this.RegisterProjectEvents(project);
        foreach (Animation animation in project.AnimationList)
        {
          this.RegisterAnimationEvents(animation);
        }
      }

      this.Solution.LaunchEvents();
    //  this.RefreshTreeView();
      this.SelectFirstAnimation();
      this.DefaultExpandTree();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptAddNewProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuAddNewProjectClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptAddExistingProject_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuAddExistingProjectClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptCloseSolution_Click(object sender, EventArgs e)
    {
      try
      {
        this.OnCtxMenuCloseSolutionClicked();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    #endregion

    #region Tile test methods
    public TileTest CreateNewTileTest(string name)
    {
      return this.Solution.AddNewTileTest(name);
    }
    #endregion

    #region TreeView events
    /// <summary>
    /// 
    /// </summary>
    private void _SolutionTree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      try
      {
        if (this._SelectedNodeChangedSupressed == true)
          return;

        switch (this.SelectedNodeType)
        {
          case NodeType.Solution:
            this._ObjProps.SelectedObject = this.Solution;
            break;

          case NodeType.Project:
            this.SelectedProject = (Project)this._SolutionTree.SelectedNode.Tag;
            this._ObjProps.SelectedObject = this.SelectedProject;
            break;

          case NodeType.Animation:
            Animation animation = (Animation)this._SolutionTree.SelectedNode.Tag;
            this.SelectedProject = animation.ParentProject;
            this.SelectedAnimation = animation;
            this._ObjProps.SelectedObject = this.SelectedAnimation;
            break;

          case NodeType.Frame:
            AnimationFrame frame = (AnimationFrame)this._SolutionTree.SelectedNode.Tag;
            this.SelectedProject = frame.ParentAnimation.ParentProject;
            this.SelectedAnimation = frame.ParentAnimation;
            this.SelectedFrame = frame;
            this._ObjProps.SelectedObject = this.SelectedFrame;
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

    /// <summary>
    /// 
    /// </summary>
    TreeNode AddSolutionNode(Solution solution)
    {
      TreeNode node = this._SolutionTree.Nodes.Add(solution.Name);
      node.Tag = solution;
      node.ContextMenuStrip = this._MenuSolution;
      node.ImageIndex = node.SelectedImageIndex = 0;
      this.SolutionNode = node;

      return node;
    }

    /// <summary>
    /// 
    /// </summary>
    TreeNode AddProjectNode(Project project)
    {
      if (this.SolutionNode == null)
        return null;

      TreeNode node = this.SolutionNode.Nodes.Add(project.Name);
      node.Tag = project;
      node.ContextMenuStrip = this._MenuProject;
      node.ImageIndex = node.SelectedImageIndex = 1;

      foreach (Animation animation in project.AnimationList)
      {
        this.AddAnimationNode(node, animation);
      }
      return node;
    }

    /// <summary>
    /// 
    /// </summary>
    TreeNode AddAnimationNode(TreeNode projectNode, Animation animation)
    {
      TreeNode node = projectNode.Nodes.Add(animation.Name);
      node.Tag = animation;
      this.RefreshNode(node);
      node.ContextMenuStrip = this._MenuAnimation;
      node.ImageIndex = node.SelectedImageIndex = 2;

      foreach (AnimationFrame frame in animation.Frames)
      {
        this.AddFrameNode(node, frame);
      }
      return node;
    }

    /// <summary>
    /// 
    /// </summary>
    TreeNode AddFrameNode(TreeNode animationNode, AnimationFrame frame)
    {
      TreeNode node = animationNode.Nodes.Add("Frame");
      node.Tag = frame;
      this.RefreshNode(node);
      node.ContextMenuStrip = this._MenuFrame;
      node.ImageIndex = node.SelectedImageIndex = 3;

      return node;
    }
    /// <summary>
    /// 
    /// </summary>
    void RefreshTreeView()
    {
      this._SolutionTree.Nodes.Clear();

      if (this.Solution == null)
        return;

      this.AddSolutionNode(this.Solution);
      foreach (Project project in this.Solution.ProjectList)
      {
        this.AddProjectNode(project);
      }
      this._SolutionNode.ExpandAll();
      this.Sort();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _SolutionTree_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        if (e.Button == MouseButtons.Right)
        {
          TreeNode node = this._SolutionTree.GetNodeAt(e.Location);
          if (node != null)
            this._SolutionTree.SelectedNode = node;
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }

    }
    #endregion

    #region Event lauching
    /// <summary>
    /// 
    /// </summary>
    void OnProjectLoadStarted(Project project)
    {
      if (this.ProjectLoadStarted != null)
      {
        this.ProjectLoadStarted(this, project);
      }
    }
  
    /// <summary>
    /// 
    /// </summary>
    void OnProjectLoaded(Project project)
    {
      if (this.ProjectLoaded != null)
      {
        this.ProjectLoaded(this, project);
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnProjectOutputSpriteChanged()
    {
      if (this.ProjectOutputSpriteChanged != null)
      {
        this.ProjectOutputSpriteChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnProjectFrameRectangleColorChanged()
    {
      if (this.ProjectFrameRectangleColorChanged != null)
      {
        this.ProjectFrameRectangleColorChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnProjectSpriteBackColorChanged()
    {
      if (this.ProjectSpriteBackColorChanged != null)
      {
        this.ProjectSpriteBackColorChanged(this, new EventArgs());
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnProjectAttributeChanged()
    {
      if (this.ProjectAttributeChanged != null)
      {
        this.ProjectAttributeChanged(this, new EventArgs());
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnProjectFrameNrsColorChanged()
    {
      if (this.ProjectFrameNrsColorChanged != null)
      {
        this.ProjectFrameNrsColorChanged(this, new EventArgs());
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnSelectedAnimationChanged()
    {
      if (this.SelectedAnimationChanged != null)
      {
        this.SelectedAnimationChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSelectedFrameChanged()
    {
      if (this.SelectedFrameChanged != null)
      {
        this.SelectedFrameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSelectedProjectChanged()
    {
      if (this.SelectedProjectChanged != null)
      {
        this.SelectedProjectChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnFrameAddedToAnimation(AnimationFrame animFrame)
    {
      if (this.FrameAddedToAnimation != null)
      {
        this.FrameAddedToAnimation(this, animFrame);
      }
    }

    
    /// <summary>
    /// 
    /// </summary>
    void OnFramesRemoved(List<AnimationFrame> animFrames)
    {
      if (this.FramesRemoved != null)
      {
        this.FramesRemoved(this, animFrames);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    void OnSolutionLoaded(Solution solution)
    {
      if (this.SolutionDataLoaded != null)
      {
        this.SolutionDataLoaded(this, solution);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSolutionClosed()
    {
      if (this.SolutionClosed != null)
      {
        this.SolutionClosed(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnAnimationFramesPerSecondChanged(Animation animation)
    {
      if (this.AnimationFramesPerSecondChanged != null)
      {
        this.AnimationFramesPerSecondChanged(this, animation);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnAnimationParentAnimationChanged(Animation animation)
    {
      if (this.AnimationParentAnimationChanged != null)
      {
        this.AnimationParentAnimationChanged(this, animation);
      }
    }

    
    /// <summary>
    /// 
    /// </summary>
    void OnFrameAttributeChangedHandler(AnimationFrame frame)
    {
      if (this.FrameAttributeChanged != null)
      {
        this.FrameAttributeChanged(this, frame);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnFrameMovedUp(AnimationFrame frame)
    {
      if (this.FrameMovedUp != null)
      {
        this.FrameMovedUp(this, frame);
      }
    }

	/// <summary>
    /// 
    /// </summary>
    void OnFrameMovedDown(AnimationFrame frame)
    {
      if (this.FrameMovedDown != null)
      {
        this.FrameMovedDown(this, frame);
      }
    }

	/// <summary>
	/// 
	/// </summary>
	void OnEditFrameCollisionZonesClicked(AnimationFrame frame)
	{
		if (this.EditFrameCollisionZonesClicked != null)
		{
			this.EditFrameCollisionZonesClicked(this, frame);
		}
	}
    /// <summary>
    /// 
    /// </summary>
    void OnSolutionFilenameChanged()
    {
      if (this.SolutionFilenameChanged != null)
      {
        this.SolutionFilenameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSolutionNameChanged()
    {
      if (this.SolutionNameChanged != null)
      {
        this.SolutionNameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuAddNewProjectClicked()
    {
      if (this.CtxMenuAddNewProjectClicked != null)
      {
        this.CtxMenuAddNewProjectClicked(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuAddExistingProjectClicked()
    {
      if (this.CtxMenuAddExistingProjectClicked != null)
      {
        this.CtxMenuAddExistingProjectClicked(this, new EventArgs());
      }
    }

     /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuCloseSolutionClicked()
    {
      if (this.CtxMenuCloseSolutionClicked != null)
      {
        this.CtxMenuCloseSolutionClicked(this, new EventArgs());
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuAddFramesToProjectClicked()
    {
      if (this.CtxMenuAddFramesToProjectClicked != null)
      {
        this.CtxMenuAddFramesToProjectClicked(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuAddNewAnimClicked()
    {
      if (this.CtxMenuAddNewAnimClicked != null)
      {
        this.CtxMenuAddNewAnimClicked(this, new EventArgs());
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuRemoveProjecClicked()
    {
      if (this.CtxMenuRemoveProjecClicked != null)
      {
        this.CtxMenuRemoveProjecClicked(this, new EventArgs());
      }
    }

     /// <summary>
    /// 
    /// </summary>
    void OnCtxMenuDelAnimationClicked()
    {
      if (this.CtxMenuDelAnimationClicked != null)
      {
        this.CtxMenuDelAnimationClicked(this, new EventArgs());
      }
    }
    #endregion

    #region Miscelaneous methods

    /// <summary>
    /// 
    /// </summary>
    private void Sort()
    {
        this._SolutionTree.Sort();
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveNodeUp(TreeNode node)
    {
      TreeNode sibling = node.PrevNode;
      if (sibling == null)
      {
        return;
      }
      int nodeIdx = node.Index;
      sibling.Remove();
      this._SolutionNode.Nodes.Insert(nodeIdx, sibling);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveNodeDown(TreeNode node)
    {
      TreeNode sibling = node.NextNode;
      if (sibling == null)
      {
        return;
      }
      int nodeIdx = node.Index;
      sibling.Remove();
      this._SolutionNode.Nodes.Insert(nodeIdx, sibling);
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
      this.Sort();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Refresh(Solution solution)
    {
      TreeNode node = this.GetSolutionNode(solution);
      if (node == null)
        return;
      node.Text = solution.Name;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshNode(TreeNode node)
    {
      if (node.Tag as AnimationFrame != null)
      {
        node.Text = string.Format("#" + string.Format("{0:0000}", ((AnimationFrame)(node.Tag)).FrameNr));
      }
      else
      if (node.Tag as Animation != null)
      {
        node.Text = ((Animation)node.Tag).Name;
        foreach (TreeNode frameNode in node.Nodes)
        {
          if (frameNode.Tag as AnimationFrame != null)
          {
            this.RefreshNode(frameNode);
          }
        }
      }
    }

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
      TreeNode projNode = this.GetProjectNode(animation.ParentProject);
      if (projNode == null)
        return null;

      foreach (TreeNode animNode in projNode.Nodes)
      {
        if ((Animation)animNode.Tag == animation)
        {
          return animNode;
        }
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    TreeNode GetFrameNode(AnimationFrame frame)
    {
      if (frame == null)
      {
        return null;
      }
      TreeNode animNode = this.GetAnimationNode(frame.ParentAnimation);
      if (animNode == null)
        return null;

      foreach (TreeNode frameNode in animNode.Nodes)
      {
        if ((AnimationFrame)frameNode.Tag == frame)
        {
          return frameNode;
        }
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public void RefreshProps()
    {
      this._ObjProps.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    public void DefaultExpandTree()
    {
      this.SolutionNode.Collapse();
      this.SolutionNode.Expand();
    }

    public void SuspendSelectedNodeChanged()
    {
      base.SuspendLayout();
      this._SelectedNodeChangedSupressed = true;
    }
    private void ResumeSelectedNodeChanged()
    {
      base.ResumeLayout();
      this._SelectedNodeChangedSupressed = false;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void _optMoveUp_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveProjectUp();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optMoveDown_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveProjectDown();
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _MenuProject_Opening(object sender, CancelEventArgs e)
    {
      try
      {
        this._optMoveProjUp.Enabled = false;
        this._optMoveProjDown.Enabled = false;
        if (this.SelectedProject != null)
        {
          int projIdx = this.Solution.GetProjectIndex(this.SelectedProject);
          this._optMoveProjUp.Enabled = (projIdx > 0);
          this._optMoveProjDown.Enabled = (projIdx < this.Solution.ProjectList.Count - 1);
        }
      }
      catch (System.Exception ex)
      {
        Diag.ShowException(this.ParentForm, ex);
      }
    }

	private void _OptEditColZones_Click(object sender, EventArgs e)
	{
		this.OnEditFrameCollisionZonesClicked(this.SelectedFrame);
	}

  /// <summary>
  /// 
  /// </summary>
  private void _optCopyFrame_Click(object sender, EventArgs e)
  {
    if (this.SelectedFrame != null)
    {
      MyClipboard.SetData(this.SelectedFrame);
    }
  }

    /// <summary>
    /// 
    /// </summary>
    private void _MenuAnimation_Opening(object sender, CancelEventArgs e)
    {
      AnimationFrame frame = (MyClipboard.GetData() as AnimationFrame);
      this._menuSep1.Visible = (frame != null);
      this._optPasteFrame.Visible = (frame != null);
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optPasteFrame_Click(object sender, EventArgs e)
    {
      this.PasteFrameFromClipboard();
    }

    /// <summary>
    /// 
    /// </summary>
    public void PasteFrameFromClipboard()
    {
      if (MyClipboard.GetData() is AnimationFrame == false)
      {
        return;
      }
      AnimationFrame frame = MyClipboard.GetData() as AnimationFrame;
      AnimationFrame newFrame = frame.Clone();
      this.AddFrameToAnimation(this.SelectedAnimation, newFrame);
    }
  }

  // Create a node sorter that implements the IComparer interface.
  public class SpritesetNodeSorter : IComparer
  {
      // Compare the length of the strings, or the strings
      // themselves, if they are the same length.
      public int Compare(object x, object y)
      {
          TreeNode tx = x as TreeNode;
          TreeNode ty = y as TreeNode;

          // Compare the length of the strings, returning the difference.
          if (tx.Text.Length != ty.Text.Length)
              return tx.Text.Length - ty.Text.Length;

          // If they are the same length, call Compare.
          return string.Compare(ty.Text, tx.Text);
      }
  }

}
