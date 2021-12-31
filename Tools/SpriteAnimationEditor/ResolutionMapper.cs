using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace SpriteAnimationEditor
{
  class ResolutionMapper
  {
    public Project ParentProject { get; private set; }
    public string Name { get; set; }
    public string OutputFile { get; set; }
    public string ImageFilename { get; set; }
    public bool Active { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ResolutionMapper(Project parent)
    {
      this.ParentProject = parent;
      this.Active = true;
      this.Name = "undefined";
    }

    /// <summary>
    /// 
    /// </summary>
    public static ResolutionMapper CreateFromXml(XmlElement elemAnim, Project parentProject)
    {
      ResolutionMapper mapper = new ResolutionMapper(parentProject);
      mapper.Name = elemAnim.Attributes["Name"].Value;
      mapper.OutputFile = Goodies.ToAbsolutePath(elemAnim.Attributes["OutputFile"].Value, parentProject.ProjectFolder);
      if (!string.IsNullOrEmpty(elemAnim.Attributes["ImageFilename"].Value))
      {
        mapper.ImageFilename = Goodies.ToAbsolutePath(elemAnim.Attributes["ImageFilename"].Value, parentProject.ProjectFolder);
      }
      mapper.Active = Convert.ToBoolean(elemAnim.Attributes["Active"].Value);

      return mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Name))
        return "";

      return this.Name;
    }

    /// <summary>
    /// 
    /// </summary>
    public ResolutionMapper Clone()
    {
      ResolutionMapper clone = new ResolutionMapper(this.ParentProject);

      clone.Name = this.Name;
      clone.OutputFile = this.OutputFile;
      clone.ImageFilename = this.ImageFilename;
      clone.Active = this.Active;

      return clone;
    }

    /// <summary>
    /// 
    /// </summary>
    public XmlElement CreateXmlElement(XmlDocument xmlDoc)
    {
      XmlElement elem = xmlDoc.CreateElement("Mapper");

      elem.SetAttribute("Name", this.Name);
      string file = Goodies.RelativePath(Path.GetDirectoryName(this.ParentProject.Filename), this.ImageFilename);
      elem.SetAttribute("ImageFilename", file);
      if (this.OutputFile == Path.GetFileName(this.OutputFile))
        this.OutputFile = Path.Combine(Path.GetDirectoryName(this.ParentProject.Filename), this.OutputFile);
      file = Goodies.RelativePath(Path.GetDirectoryName(this.ParentProject.Filename), this.OutputFile);
      elem.SetAttribute("OutputFile", file);
      elem.SetAttribute("Active", this.Active.ToString());
      return elem;
    }
  }
}
