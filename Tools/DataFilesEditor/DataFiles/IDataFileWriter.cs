using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public interface IDataFileWriter
    {
        Stream ToStream(DataFile file);
        void Write(string filename, DataFile file);
    }
}
