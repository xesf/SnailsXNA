using System;
using System.IO;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.Runtime.InteropServices;

namespace TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile
{
    class Consts
    {
        public const string FILE_STAMP = "BRAINEBINARYDATAFILE";
    }

    public enum BlockId
    {
        HeaderStart = 0xA001,
        RecordStart = 0xA002,
        FieldStart = 0xA003,
        HeaderEnd = 0xF001,
        RecordEnd = 0xF002,
        FieldEnd = 0xF003
    }

}
