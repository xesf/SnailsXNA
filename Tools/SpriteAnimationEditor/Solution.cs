using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace SpriteAnimationEditor
{
  [DefaultPropertyAttribute("Name")]
  class Solution
  {
    #region Consts
    const string DEF_NAME = "unnamed solution";
    const string DEF_CATEGORY = "Solution";
    const string DEF_FILENAME = null;
    #endregion

    #region Events
    public delegate void ProjectAddedHandler(object sender, Project project);
    public event ProjectAddedHandler ProjectAdded;
    public event EventHandler NameChanged;
    public event EventHandler FilenameChanged;
    #endregion

    #region Variables
    List<Project> _ProjectList = new List<Project>();
    private string _Name;
    private string _Filename;
    private bool _NewSolution;
    private bool _Changed;
    List<TileTest> _TileTestList;
    #endregion

    #region Non browsable properties
    [BrowsableAttribute(false)]
    public List<Project> ProjectList
    {
      get { return this._ProjectList; }
      private set { this._ProjectList = value; }
    }

    [BrowsableAttribute(false)]
    public List<TileTest> TileTestList
    {
      get { return this._TileTestList; }
      private set { this._TileTestList = value; }
    }

    [BrowsableAttribute(false)]
    public bool NewSolution
    {
      get { return this._NewSolution; }
      private set { this._NewSolution = value; }
    }

    [BrowsableAttribute(false)]
    public bool Changed
    {
      get 
      {
        if (this._Changed)
          return true;
        foreach (Project project in this.ProjectList)
        {
          if (project.Changed)
            return true;
        }

        return false; 
      }
      private set 
      { 
        this._Changed = value;
        
      }
    }

    [BrowsableAttribute(false)]
    public List<Project> UnloadedProjects
    {
      get 
      {
        List<Project> projectList = new List<Project>();
        foreach (Project project in this.ProjectList)
        {
          if (project.Loaded == false)
            projectList.Add(project);
        }
        return projectList; 
      }
    }
    #endregion

    #region Browsable properties
    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    [DefaultValueAttribute(DEF_NAME)]
    public string Name
    {
      get { return this._Name; }
      set 
      {
        if (this._Name != value)
        {
          this._Name = value;
          this.Changed = true;
          this.OnNameChanged();
        }
      }
    }

    [BrowsableAttribute(true)]
    [CategoryAttribute(DEF_CATEGORY)]
    [DefaultValueAttribute(DEF_FILENAME)]
    public string Filename
    {
      get { return this._Filename; }
      private set
      {
        if (this._Filename != value)
        {
          this._Filename = value;
          this.Changed = true;
          this.OnFilenameChanged();
        }
      }
    }
    #endregion

    #region Class constructors and overloads
    /// <summary>
    /// 
    /// </summary>
    public Solution()
    {
      this._ProjectList = new List<Project>();
      this.Name = DEF_NAME;
      this.Filename = DEF_FILENAME;
      this.NewSolution = true;
      this.Changed = false;
      this.TileTestList = new List<TileTest>();
    }
    #endregion

    #region Open/save methods
     /// <summary>
    /// 
    /// </summary>
    public static Solution CreateFromXml(string filename)
    {
      Directory.SetCurrentDirectory(Path.GetDirectoryName(filename));
      Solution solution = new Solution();

      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.Load(filename);

      XmlElement rootElem = (XmlElement)xmlDoc.SelectSingleNode("SpriteAnimationStudio.Solution");
      XmlElement propsElem = (XmlElement)rootElem.SelectSingleNode("Properties");
      solution.Name = propsElem.Attributes["Name"].Value;
      solution.Filename = filename;

      XmlNodeList nodesProject = rootElem.SelectNodes("Projects/Project");
      foreach (XmlElement elemProject in nodesProject)
      {
        Directory.SetCurrentDirectory(Path.GetDirectoryName(solution.Filename));
        string projName = elemProject.Attributes["Name"].Value;
        string projFilename = elemProject.Attributes["Filename"].Value;
        if (!string.IsNullOrEmpty(Path.GetDirectoryName(projFilename)))
          Directory.SetCurrentDirectory(Path.GetDirectoryName(projFilename));
        projFilename = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(projFilename));
        Project project = new Project(solution, projName, projFilename);
        solution.ProjectList.Add(project);
      }
    
      solution.Changed = false;
      return solution;
    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadTileTesters()
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.Load(this.Filename);
      XmlElement rootElem = (XmlElement)xmlDoc.SelectSingleNode("SpriteAnimationStudio.Solution");

      // Tile testers
      XmlNodeList nodesTileTesters = rootElem.SelectNodes("TileTesters/TileTest");
      foreach (XmlElement elemTileTest in nodesTileTesters)
      {
        this.TileTestList.Add(TileTest.FromXml(elemTileTest, this));
      }

      this.Changed = false;

    }
    /// <summary>
    /// 
    /// </summary>
    public void Save(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        throw new ApplicationException("Solution filename cannot be null.");
      this.Filename = filename;
      XmlDocument xmlDoc = new XmlDocument();
      XmlElement elemRoot = xmlDoc.CreateElement("SpriteAnimationStudio.Solution");
      xmlDoc.AppendChild(elemRoot);
      XmlElement elemProperties = xmlDoc.CreateElement("Properties");
      elemProperties.SetAttribute("Name", this.Name);
      elemRoot.AppendChild(elemProperties);

      XmlElement elemProjects = xmlDoc.CreateElement("Projects");
      elemRoot.AppendChild(elemProjects);

      // Projects
      foreach (Project project in this.ProjectList)
      {
        XmlElement elemProj = xmlDoc.CreateElement("Project");
        elemProj.SetAttribute("Name", project.Name);
        string fileProj = Goodies.RelativePath(Path.GetDirectoryName(this.Filename), project.Filename);
        elemProj.SetAttribute("Filename", fileProj);
        elemProjects.AppendChild(elemProj);
      }

      // Tile Testers
      XmlElement elemTesters = xmlDoc.CreateElement("TileTesters");
      elemRoot.AppendChild(elemTesters);
      foreach (TileTest test in this.TileTestList)
      {
        elemTesters.AppendChild(test.ToXml(xmlDoc));
      }
      xmlDoc.Save(this.Filename);
      this.Changed = false;
    }
    #endregion

    #region Animation methods
    /// <summary>
    /// 
    /// </summary>
    public Animation FindAnimation(string id)
    {
        string[] path = id.Split('\\');
        if (path.Length >= 2)
        {
            Project proj = this.GetProjectByName(path[0]);
            if (proj != null)
            {
                return proj.GetAnimationByName(path[1]);
            }
        }

        return null;
    }
    #endregion

    #region Project methods
    /// <summary>
    /// 
    /// </summary>
    public void MoveProjectUp(Project project)
    {
      int selIdx = this.GetProjectIndex(project);
      if (selIdx <= 0)
      {
        return;
      }
      Project selProj = this.ProjectList[selIdx];
      this.ProjectList[selIdx] = this.ProjectList[selIdx - 1];
      this.ProjectList[selIdx - 1] = selProj;
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveProjectDown(Project project)
    {
      int selIdx = this.GetProjectIndex(project);
      if (selIdx >= this.ProjectList.Count - 1)
      {
        return;
      }
      Project selProj = this.ProjectList[selIdx];
      this.ProjectList[selIdx] = this.ProjectList[selIdx + 1];
      this.ProjectList[selIdx + 1] = selProj;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddProject(Project project)
    {
      project.ParentSolution = this;
      this.ProjectList.Add(project);
      this.Changed = true;
      this.OnProjectAdded(project);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemoveProject(Project project)
    {
      project.ParentSolution = null;
      this.ProjectList.Remove(project);
      this.Changed = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public Project GetProjectByName(string name)
    {
      foreach (Project project in this.ProjectList)
      {
        if (string.Compare(name, project.Name, true) == 0)
          return project;
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public Animation GetAnimationByName(Project project, string name)
    {
      if (project == null)
        return null;

      foreach (Animation animation in project.AnimationList)
      {
        if (string.Compare(name, animation.Name, true) == 0)
          return animation;
      }

      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public int GetProjectIndex(Project proj)
    {
      for (int i = 0; i < this.ProjectList.Count; i++)
      {
        if (this.ProjectList[i] == proj)
        {
          return i;
        }
      }

      return -1;
    }
    #endregion

    #region Tile Test methods
    /// <summary>
    /// 
    /// </summary>
    public TileTest AddNewTileTest(string name)
    {
      TileTest tileTest = new TileTest(this, name);
      this.TileTestList.Add(tileTest);
      this.Changed = true;
      return tileTest;
    }
    #endregion

    #region Event lauching
    /// <summary>
    /// 
    /// </summary>
    public void LaunchEvents()
    {
      this.OnFilenameChanged();
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
    void OnNameChanged()
    {
      if (this.NameChanged != null)
      {
        this.NameChanged(this, new EventArgs());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnFilenameChanged()
    {
      if (this.FilenameChanged != null)
      {
        this.FilenameChanged(this, new EventArgs());
      }
    }
    #endregion
  }
}
