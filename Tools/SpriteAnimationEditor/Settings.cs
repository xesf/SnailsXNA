using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;

namespace SpriteAnimationEditor
{
  class Settings
  {
	
    #region Consts
    const string DEF_CATEGORY = "General";
    const string FRAME_NR_CATEGORY = "Frame Numbering";
    public const string AppName = "Sprite Animation Editor";
    const bool DEF_SHOW_IMAGES = true;
    const ZoomFactor DEF_OUTPUT_SPRITE_ZOOM = ZoomFactor.Normal;
    public const string INGAME_ANIMATION_DATAFILE_EXT = "sa";
    public const string INGAME_TEXTURE_FILE_EXT = "png";
    public const string IMAGE_FILER = "All supported image files|*.png;*.jpg|Png files|*.png|Bitmap files|*.bmp|JPG files|*.jpg";
    public const int GRID_DEF_WIDTH = 32;
    public const int GRID_DEF_HEIGHT = 32;
    public const string SOLUTION_FILE_FILTER = "Sprite Animation Studio Solution|*.saSol";
    public const string PROJECT_FILE_FILTER = "Sprite Animation Studio Sprite Set|*.ss";
    public const string FRAMES_FILE_FILTER = "Image Files|*.png";
    public const string DEFAULT_SOLUTION_FILENAME = "saSolution.saSol";
    public const string DEFAULT_PROJECT_FILENAME = "spriteSet.ss";

    //public static CultureInfo ci = new CultureInfo("en-US");

    #endregion

    #region Events
    public event EventHandler ShowImagesChanged;
    #endregion

    #region Variables
    List<string> _RecentFiles;
    bool _ShowImages;
    string _LastImportFramesFolder;
    string _LastProjectFolder;
    string _LastSolutionFolder;
    static Settings _Instance;
    bool _ExportOverrideFolders;
    string _TextureFolderOverride;
    string _AnimationDataFolderOverride;
    ZoomFactor _OuputSpriteZoom = DEF_OUTPUT_SPRITE_ZOOM;
    Forms.AddNewFramesForm _FormAddNewFramesForm;
    Forms.MainForm _FormMainForm;
    Forms.ColisionZonesForm _FormColisionZones;
    Forms.TileTesterForm _TileTesterForm;

    #endregion

    #region Non browsable attributes

    [BrowsableAttribute(false)]
    public static Settings Instance
    {
      get
      {
        if (_Instance == null)
          _Instance = new Settings();

        return _Instance;
      }

    }

    [BrowsableAttribute(false)]
    public List<string> RecentFiles
    {
      get { return this._RecentFiles; }
      set { this._RecentFiles = value; }
    }

    
    [BrowsableAttribute(false)]
    public string LastFileEdited
    {
      get
      {
        if (this._RecentFiles.Count == 0)
          return null;
        return this._RecentFiles[0];
      }
    }

    [BrowsableAttribute(false)]
    public string LastImportFramesFolder
    {
      get
      {
        return this._LastImportFramesFolder;
      }
      set
      {
        this._LastImportFramesFolder = value;
      }
    }

    [BrowsableAttribute(false)]
    public string LastProjectFolder
    {
      get { return this._LastProjectFolder; }
      set { this._LastProjectFolder = value;}
    }

    [BrowsableAttribute(false)]
    public string LastSolutionFolder
    {
      get { return this._LastSolutionFolder; }
      set { this._LastSolutionFolder = value; }
    }

    [BrowsableAttribute(false)]
    public bool ExportOverrideFolders
    {
      get { return this._ExportOverrideFolders; }
      set { this._ExportOverrideFolders = value; }
    }

    [BrowsableAttribute(false)]
    public string TextureFolderOverride
    {
      get { return this._TextureFolderOverride; }
      set { this._TextureFolderOverride = value; }
    }

    [BrowsableAttribute(false)]
    public string AnimationDataFolderOverride
    {
      get { return this._AnimationDataFolderOverride; }
      set { this._AnimationDataFolderOverride = value; }
    }

    [BrowsableAttribute(false)]
    public ZoomFactor OutputSpriteZoom
    {
      get { return this._OuputSpriteZoom; }
      set { this._OuputSpriteZoom = value; }
    }

    [BrowsableAttribute(false)]
    public static Forms.AddNewFramesForm FormAddNewFramesForm
    {
      get 
      {
        if (Settings.Instance._FormAddNewFramesForm == null)
          Settings.Instance._FormAddNewFramesForm = new SpriteAnimationEditor.Forms.AddNewFramesForm();

        return Settings.Instance._FormAddNewFramesForm; 
      }
      set { Settings.Instance._FormAddNewFramesForm = value; }
    }

    [BrowsableAttribute(false)]
    public static Forms.ColisionZonesForm FormColisionZones
    {
      get
      {
        if (Settings.Instance._FormColisionZones == null)
          Settings.Instance._FormColisionZones = new SpriteAnimationEditor.Forms.ColisionZonesForm();

        return Settings.Instance._FormColisionZones;
      }
      set { Settings.Instance._FormColisionZones = value; }
    }

    [BrowsableAttribute(false)]
    public static Forms.TileTesterForm TileTesterForm
    {
      get
      {
        if (Settings.Instance._TileTesterForm == null)
          Settings.Instance._TileTesterForm = new SpriteAnimationEditor.Forms.TileTesterForm();

        return Settings.Instance._TileTesterForm;
      }
      set { Settings.Instance._TileTesterForm = value; }
    }

    [BrowsableAttribute(false)]
    public static Forms.MainForm FormMainForm
    {
      get 
      {
        if (Settings.Instance._FormMainForm == null)
          Settings.Instance._FormMainForm = new Forms.MainForm();

        return Settings.Instance._FormMainForm; 
      }
      set { Settings.Instance._FormMainForm = value; }
    }

    [BrowsableAttribute(false)]
    public string LastResMapImageFolder {get; set;}

    [BrowsableAttribute(false)]
    public string LastResMapSSFolder { get; set; }
    #endregion

    #region Browsable attributes
    [BrowsableAttribute(true)]
    [DefaultValueAttribute(DEF_SHOW_IMAGES)]
    [CategoryAttribute(DEF_CATEGORY)]
    public bool ShowImages
    {
      get
      {
        return this._ShowImages;
      }
      set
      {
        if (this._ShowImages != value)
        {
          this._ShowImages = value;
          this.OnShowImagesChanged();
        }
      }
    }


    #endregion

    #region Class constructors and overrides

    /// <summary>
    /// 
    /// </summary>
    private Settings()
    {
      this._RecentFiles = new List<string>();
      this._ShowImages = DEF_SHOW_IMAGES;
    }

    #endregion

    #region Save/load
    /// <summary>
    /// 
    /// </summary>
    public void Save()
    {
      string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      string filename = Path.Combine(folder, "SpriteAnimationEditor.Settings.xml");

      XmlDocument xmlDoc = new XmlDocument();
      XmlElement elemRoot = xmlDoc.CreateElement("SpriteAnimationEditor.Settings");
      xmlDoc.AppendChild(elemRoot);
      XmlElement elemSettings = xmlDoc.CreateElement("Settings");
      elemRoot.AppendChild(elemSettings);
      elemSettings.SetAttribute("LastImportFramesFolder", Settings.Instance.LastImportFramesFolder);
      elemSettings.SetAttribute("LastSolutionFolder", Settings.Instance.LastSolutionFolder);
      elemSettings.SetAttribute("LastProjectFolder", Settings.Instance.LastProjectFolder);
      elemSettings.SetAttribute("LastResMapImageFolder", Settings.Instance.LastResMapImageFolder);
      elemSettings.SetAttribute("LastResMapSSFolder", Settings.Instance.LastResMapSSFolder);
      
      elemSettings.SetAttribute("ShowImages", Settings.Instance.ShowImages.ToString());
      elemSettings.SetAttribute("OutputSpriteZoom", Settings.Instance.OutputSpriteZoom.ToString());
      XmlElement elemExportFiles = xmlDoc.CreateElement("ExportFiles");
      elemRoot.AppendChild(elemExportFiles);
      elemExportFiles.SetAttribute("ExportOverrideFolders", this.ExportOverrideFolders.ToString());
      elemExportFiles.SetAttribute("TextureFolderOverride", this.TextureFolderOverride);
      elemExportFiles.SetAttribute("AnimationDataFolderOverride", this.AnimationDataFolderOverride);

      XmlElement elemRecentFiles = xmlDoc.CreateElement("RecentFiles");
      elemRoot.AppendChild(elemRecentFiles);
      foreach(string file in this._RecentFiles)
      {
        XmlElement elemRecentFile = xmlDoc.CreateElement("RecentFile");
        elemRecentFile.SetAttribute("Filename", file);
        elemRecentFiles.AppendChild(elemRecentFile);
      }

     
      XmlElement elemViewState = xmlDoc.CreateElement("ViewState");
      elemViewState.AppendChild(Settings.FormAddNewFramesForm.ToXml(xmlDoc));
      elemViewState.AppendChild(Settings.FormMainForm.ToXml(xmlDoc));
      elemViewState.AppendChild(Settings.TileTesterForm.ToXml(xmlDoc));

      elemRoot.AppendChild(elemViewState);
      xmlDoc.Save(filename);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void Load()
    {
      string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      string filename = Path.Combine(folder, "SpriteAnimationEditor.Settings.xml");

      if (!File.Exists(filename))
      {
        return;
      }

      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.Load(filename);

      XmlElement elemRoot = (XmlElement)xmlDoc.SelectSingleNode("SpriteAnimationEditor.Settings");
      XmlElement elemSettings = (XmlElement)elemRoot.SelectSingleNode("Settings");
      Settings.Instance.ShowImages = XmlHelper.GetAttribute(elemSettings, "ShowImages", true);
      Settings.Instance.OutputSpriteZoom = XmlHelper.GetAttribute(elemSettings, "OutputSpriteZoom", ZoomFactor.Normal);
      Settings.Instance.LastImportFramesFolder = XmlHelper.GetAttribute(elemSettings, "LastImportFramesFolder", "");
      Settings.Instance.LastSolutionFolder = XmlHelper.GetAttribute(elemSettings, "LastSolutionFolder", "");
      Settings.Instance.LastProjectFolder = XmlHelper.GetAttribute(elemSettings, "LastProjectFolder", "");
      Settings.Instance.LastResMapImageFolder = XmlHelper.GetAttribute(elemSettings, "LastResMapImageFolder", "");
      Settings.Instance.LastResMapSSFolder = XmlHelper.GetAttribute(elemSettings, "LastResMapSSFolder", "");


      XmlElement elemExportFiles = (XmlElement)elemRoot.SelectSingleNode("ExportFiles");
      if (elemExportFiles != null)
      {
        this.ExportOverrideFolders = XmlHelper.GetAttribute(elemExportFiles, "ExportOverrideFolders", false);
        this.TextureFolderOverride = XmlHelper.GetAttribute(elemExportFiles, "TextureFolderOverride", "");
        this.AnimationDataFolderOverride = XmlHelper.GetAttribute(elemExportFiles, "AnimationDataFolderOverride", "");
      }

      XmlNodeList recentFilesList = elemRoot.SelectNodes("RecentFiles/RecentFile");
      Settings.Instance.RecentFiles.Clear();
      foreach(XmlElement elemRecent in recentFilesList)
      {
        string recentFile = XmlHelper.GetAttribute(elemRecent, "Filename", "");
        if (string.IsNullOrEmpty(recentFile) == false)
          Settings.Instance.RecentFiles.Add(recentFile);
      }

      XmlElement elemViewState = (XmlElement)elemRoot.SelectSingleNode("ViewState");
      if (elemViewState != null)
      {
        Settings.FormAddNewFramesForm.InitFromXml(elemViewState);
        Settings.FormMainForm.InitFromXml(elemViewState);
        Settings.TileTesterForm.InitFromXml(elemViewState);
      }

      this.LaunchEvents();
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void AddRecentFile(string filename)
    {
      this._RecentFiles.Insert(0, filename);
      for(int i = 1; i < this._RecentFiles.Count; i++)
      {
        if (string.Compare(this._RecentFiles[i], filename, true) == 0)
        {
          this._RecentFiles.RemoveAt(i);
          i--;
        }
      }
    }

    #region Event lauching
    /// <summary>
    /// 
    /// </summary>
    private void LaunchEvents()
    {
      this.OnShowImagesChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnShowImagesChanged()
    {
      if (Settings.Instance.ShowImagesChanged != null)
      {
        Settings.Instance.ShowImagesChanged(this, new EventArgs());
      }
    }

    #endregion
  }
}
