using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
    public interface IDataFileSerializable
    {
        void InitFromDataFileRecord(DataFileRecord record);
        DataFileRecord ToDataFileRecord();
	}
}
