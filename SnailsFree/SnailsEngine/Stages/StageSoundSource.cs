using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.Stages
{
    public class StageSoundSource : ISnailsDataFileSerializable
    {
        #region Constants
        private const float DEFAULT_VOLUME = 0.2f;
        #endregion

        #region Members/Properties
        public string Res;
        public float Volume;
        public bool Loop;

        /*public bool UseVolume;
        public bool UsePitch;
        public float Pitch;
        public bool UsePan;
        public float Pan;
        public int Repeats;*/
        #endregion

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            if (record != null)
            {
                Res = record.GetFieldValue<string>("res");
                Volume = record.GetFieldValue<float>("volume", DEFAULT_VOLUME);
                Loop = record.GetFieldValue<bool>("loop", false);

                /*UseVolume = record.GetFieldValue<bool>("useVolume", false);
                UsePitch = record.GetFieldValue<bool>("usePitch", false);
                Pitch = record.GetFieldValue<float>("pitch", 1);
                UsePan = record.GetFieldValue<bool>("usePan", false);
                Pan = record.GetFieldValue<float>("pan", 1);
                Repeats = record.GetFieldValue<int>("repeats", 0);*/
            }
        }

        public DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            return ToDataFileRecord("StageSoundSource"); // default node name
        }

        // used to create nodes with different names
        public DataFileRecord ToDataFileRecord(string recordName)
        {
            DataFileRecord record = new DataFileRecord(recordName);

            record.AddField("res", Res);
            record.AddField("volume", Volume);
            record.AddField("loop", Loop);

            /*record.AddField("useVolume", UseVolume);
            record.AddField("usePitch", UsePitch);
            record.AddField("pitch", Pitch);
            record.AddField("usePan", UsePan);
            record.AddField("pan", Pan);
            record.AddField("repeats", Repeats);*/

            return record;
        }
    }

}
