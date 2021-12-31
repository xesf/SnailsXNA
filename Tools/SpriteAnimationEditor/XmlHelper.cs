using System;
using System.Xml;
using System.Drawing;

namespace SpriteAnimationEditor
{
  class XmlHelper
  {
   

    /// <summary>
    /// 
    /// </summary>
    public static string GetAttribute(XmlElement elem, string attribName, string defaultVal)
    {
      if (elem == null)
        return defaultVal;
      if (elem.Attributes[attribName] == null)
        return defaultVal;

      return elem.Attributes[attribName].Value;
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool GetAttribute(XmlElement elem, string attribName, bool defaultVal)
    {
      if (elem == null)
        return defaultVal;
      if (elem.Attributes[attribName] == null)
        return defaultVal;

      return Convert.ToBoolean(elem.Attributes[attribName].Value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static ZoomFactor GetAttribute(XmlElement elem, string attribName, ZoomFactor defaultVal)
    {
      if (elem == null)
        return defaultVal;

      if (elem.Attributes[attribName] == null)
        return defaultVal;

      return (ZoomFactor)Enum.Parse(typeof(ZoomFactor), elem.Attributes[attribName].Value, true);
    }

    /// <summary>
    /// 
    /// </summary>
    public static double GetAttribute(XmlElement elem, string attribName, double defaultVal)
    {
      if (elem == null)
        return defaultVal;

      if (elem.Attributes[attribName] == null)
        return defaultVal;

      return Convert.ToDouble(elem.Attributes[attribName].Value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static float GetAttribute(XmlElement elem, string attribName, float defaultVal)
    {
      if (elem == null)
        return defaultVal;

      if (elem.Attributes[attribName] == null)
        return defaultVal;

      return (float)  Convert.ToDouble(elem.Attributes[attribName].Value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static int GetAttribute(XmlElement elem, string attribName, int defaultVal)
    {
      if (elem == null)
        return defaultVal;

      if (elem.Attributes[attribName] == null)
        return defaultVal;

      return Convert.ToInt32(elem.Attributes[attribName].Value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Color GetAttribute(XmlElement elem, string attribName, Color defaultVal)
    {
      if (elem == null)
        return defaultVal;
      if (elem.Attributes[attribName] == null)
        return defaultVal;
      
      return Color.FromArgb(Convert.ToInt32(elem.Attributes[attribName].Value));
    }
  }
}
