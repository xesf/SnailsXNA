using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.BrainEngine.UI.Screens
{
	public class ScreensData : IDataFileSerializable
	{
     
		#region Structs
		// This struct has only 1 field. The list of screens in the ScreenGroupData could have that field instead of
		// the structure. But a struct is used because in the future a few more fields may be added
	    public struct ScreenData
		{
			public string ScreenId;
            public float SkipTime;
		}

		public struct ScreenGroupData
		{
			public string GroupId;
			public List<ScreenData> ScreensData;

			public override string ToString()
			{
				return this.GetType().ToString();
			}
		}
		#endregion

		#region Member vars
		public String _startupGroup;
		public string _startupScreenId;
		public Dictionary<string, ScreenGroupData> _groupsData;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public ScreensData()
		{
			this._groupsData = new Dictionary<string, ScreenGroupData>();
		}

		/// <summary>
		/// 
		/// </summary>
		public static ScreensData FromDataFileRecord(DataFileRecord record)
		{
			ScreensData screensData = new ScreensData();
			screensData.InitFromDataFileRecord(record);
			return screensData;
		}

		#region IDataFileSerializable Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="record"></param>
		public void InitFromDataFileRecord(DataFileRecord record)
		{
			this._startupGroup = record.GetFieldValue<string>("StartupGroup");
			this._startupScreenId = record.GetFieldValue<string>("StartupScreen");
			this._groupsData = new Dictionary<string, ScreenGroupData>();
			
			DataFileRecordList groupRecs = record.SelectRecords("ScreenGroup");
			foreach (DataFileRecord groupRec in groupRecs)
			{
				ScreenGroupData groupData = new ScreenGroupData();
				groupData.GroupId = groupRec.GetFieldValue<string>("Id");
				groupData.ScreensData = new List<ScreenData>();
				DataFileRecordList screensRecs = groupRec.SelectRecords("Screen");
				foreach (DataFileRecord screenRec in screensRecs)
				{
					ScreenData screenData = new ScreenData();
					screenData.ScreenId = screenRec.GetFieldValue<string>("Type");
                    screenData.SkipTime = screenRec.GetFieldValue<float>("SkipTime");
                   
                    groupData.ScreensData.Add(screenData);
				}
				this._groupsData.Add(groupData.GroupId, groupData);
				
			}
		}
		

		/// <summary>
		/// 
		/// </summary>
        public DataFileRecord ToDataFileRecord()
		{
			DataFile dataFile = new DataFile();
			dataFile.RootRecord.Name = "ScreensData";
			dataFile.RootRecord.AddField("StartupGroup", this._startupGroup);
			dataFile.RootRecord.AddField("StartupScreen", this._startupScreenId);
			foreach (ScreenGroupData group in this._groupsData.Values)
			{
				DataFileRecord groupRec = new DataFileRecord("ScreenGroup");
				groupRec.AddField("Id", group.GroupId);
				foreach (ScreenData screen in group.ScreensData)
				{
					DataFileRecord screenRec = new DataFileRecord("Screen");
					screenRec.AddField("Type", screen.ScreenId);
                    screenRec.AddField("SkipTime", screen.SkipTime);
                   
					groupRec.AddRecord(screenRec);
				}

				dataFile.RootRecord.AddRecord(groupRec);
			}

			return dataFile.RootRecord;
		}
		#endregion
	}
}
