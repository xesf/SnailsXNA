using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;
using System.Diagnostics;

namespace Snails.ContentExtension
{
  class GenericContentWriter
  {
    internal static string Ek = "123#odeprot";
    /// <summary>
    /// 
    /// </summary>
    public static void Write(ContentWriter output, IDataFileSerializable value)
    {
      DataFile dataFile = new DataFile();
      dataFile.RootRecord = value.ToDataFileRecord();
      BinaryDataFileWriter writer = new BinaryDataFileWriter();
      writer.Write(output, dataFile, GenericContentWriter.Ek);
    } 
  }
}
