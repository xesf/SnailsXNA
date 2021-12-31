using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace SpriteAnimationEditor
{
  class Goodies
  {

   /*  /// <summary>
    /// 
    /// </summary>
    public Image CreateShadow(Image image)
    {
    Bitmap bmp = (bmp)image;
      BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);

      Bitmap output = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
      BitmapData outputData = output.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, image.PixelFormat);
      byte[] bmpBytes = new byte[bmpData.Width * bmpData.Stride];
      
      for (int i = 0; i < bmpData.Height * bmpData.Stride; i+=4)
      {

      }
    }*/

    /// <summary>
    /// 
    /// </summary>
    public static string ToAbsolutePath(string filename, string currentDir)
    {
      string saveCurDir = Directory.GetCurrentDirectory();
      Directory.SetCurrentDirectory(currentDir);

      if (!string.IsNullOrEmpty(Path.GetDirectoryName(filename)))
      {
        Directory.SetCurrentDirectory(Path.GetDirectoryName(filename));
      }

      string retFilename  = Path.GetFullPath(filename);
      Directory.SetCurrentDirectory(saveCurDir);

      return retFilename;
    }

    /// <summary>
    /// 
    /// </summary>
    public static string RelativePath(string absolutePath, string relativeTo)
    {
      if (relativeTo == null)
        return relativeTo;

      if (relativeTo[0] == '.' || relativeTo[0] == '\\')
        return relativeTo;

      string[] absoluteDirectories = absolutePath.Split('\\');
      string[] relativeDirectories = relativeTo.Split('\\');

      //Get the shortest of the two paths
      int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

      //Use to determine where in the loop we exited
      int lastCommonRoot = -1;
      int index;

      //Find common root
      for (index = 0; index < length; index++)
      {
        if (absoluteDirectories[index] == relativeDirectories[index])
        {
          lastCommonRoot = index;
        }
        else
          break;
      }

      //If we didn't find a common prefix then throw
      if (lastCommonRoot == -1)
      {
        throw new ArgumentException("Paths do not have a common base");
      }

      //Build up the relative path
      StringBuilder relativePath = new StringBuilder();

      //Add on the ..
      for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
      {
        if (absoluteDirectories[index].Length > 0)
          relativePath.Append("..\\");
      }

      //Add on the folders
      for (index = lastCommonRoot + 1; index < relativeDirectories.Length - 1; index++)
      {
        relativePath.Append(relativeDirectories[index] + "\\");
      }
      relativePath.Append(relativeDirectories[relativeDirectories.Length - 1]);

      return relativePath.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    public static string GetNextFilename(string baseName)
    {
      string extension = Path.GetExtension(baseName);
      string filename = Path.GetFileNameWithoutExtension(baseName);
      string retName;
      int idx = 1;
      while (File.Exists((retName = (filename + idx.ToString() + extension))))
      {
        idx++;
      }
      return retName;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void CheckFrameSizes(List<string> files)
    {
      Goodies.CheckFrameSizes(null, files.ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    public static void CheckFrameSizes(string reference, string[] files)
    {
      Image firstFrame;
      if (reference != null)
      {
        firstFrame = Image.FromFile(reference);
      }
      else
      {
        firstFrame = Image.FromFile(files[0]);
      }

      for (int i = 0; i < files.Length; i++)
      {
        Image frame = Image.FromFile(files[i]);
        if (frame.Width != firstFrame.Width ||
            frame.Height != firstFrame.Height)
        {
          throw new ApplicationException("Cannot add frames. All frames must have the same size.");
        }
      }
    }
  }
}
