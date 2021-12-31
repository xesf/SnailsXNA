using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public interface IDataFileReader
    {
      DataFile Read(Stream stream);
      DataFile Read(string filename);
    }
}
