using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SpriteAnimationEditor
{
  class ColisionAreaBase : ICloneable, IViewState
  {
    public string Description { get; set; }
    public ColisionAreaBase()
    {

    }

    public virtual ColisionAreaBase Scale(double factorX, double factorY)
    {
      throw new NotImplementedException();
    }
    #region ICloneable Members

    public virtual object Clone()
    {
      throw new NotImplementedException();
    }

    public virtual bool IsZero()
    {
      return true;
    }

    public virtual void Draw(Point pt, Graphics e, Color color)
    {

    }
    #endregion

    #region IViewState Members

    public virtual void InitFromXml(System.Xml.XmlElement elemParent)
    {
      throw new NotImplementedException();
    }

    public virtual System.Xml.XmlElement ToXml(System.Xml.XmlDocument xmlDoc)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
