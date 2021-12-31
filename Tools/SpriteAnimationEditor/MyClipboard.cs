using System;
using System.Collections.Generic;
using System.Text;

namespace SpriteAnimationEditor
{
  // Clipboard class was getting on my nerves. Don't know what the fk was happening with GetData
  class MyClipboard
  {
    static object _data;
    public static void SetData(object data)
    {
      MyClipboard._data = data;
    }

    public static object GetData()
    {
      return MyClipboard._data;
    }
  }
}
