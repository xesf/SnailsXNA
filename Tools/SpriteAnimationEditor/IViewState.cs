using System;
using System.Xml;

namespace SpriteAnimationEditor
{
  public interface IViewState
  {
    void InitFromXml(XmlElement elemParent);
    XmlElement ToXml(XmlDocument xmlDoc);
  }
}
