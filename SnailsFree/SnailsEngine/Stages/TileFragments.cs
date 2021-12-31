using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using System.IO;

namespace TwoBrainsGames.Snails.Stages
{
    public class TileFragments : ISnailsDataFileSerializable
    {
        struct FragmentData
        {
            public string _res;
            public string _sprite;
            public int _styleGroupId;

            public FragmentData(int styleGroupId, string res, string sprite)
            {
                this._styleGroupId = styleGroupId;
                this._res = res;
                this._sprite = sprite;
            }
        }

        private List<FragmentData> _fragmentsData;
        // Sprites per style group id
        private Sprite [] _fragmentsSprites;

        public Sprite [] FragmentsSprites { get { return this._fragmentsSprites; } }
        
        public TileFragments()
        {
            this._fragmentsData = new List<FragmentData>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            int lastStyleGroup = 0;
            foreach (FragmentData data in this._fragmentsData)
            {
                if (data._styleGroupId > lastStyleGroup)
                {
                    lastStyleGroup = data._styleGroupId;
                }
            }

            this._fragmentsSprites = new Sprite[lastStyleGroup + 1];
            foreach (FragmentData data in this._fragmentsData)
            {
                if (data._res.Contains("%THEME%"))
                {
                    string resId = data._res.Replace("%THEME%", Levels.CurrentTheme.ToString());
                    this._fragmentsSprites[data._styleGroupId] = BrainGame.ResourceManager.GetSprite(resId + "/" +  data._sprite, ResourceManagerIds.STAGE_THEME_RESOURCES);
                }
                else
                {
                    this._fragmentsSprites[data._styleGroupId] = BrainGame.ResourceManager.GetSpriteTemporary(data._res, data._sprite);
                }
             }
        }

        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            return this.ToDataFileRecord();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._fragmentsData.Clear();
            DataFileRecordList fragments = record.SelectRecords("Fragment");
            foreach (DataFileRecord fragRec in fragments)
            {
                FragmentData data = new FragmentData();
                data._res = fragRec.GetFieldValue<string>("res");
                data._sprite = fragRec.GetFieldValue<string>("sprite");
                data._styleGroupId = fragRec.GetFieldValue<int>("styleGroupId");
                this._fragmentsData.Add(data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("TileFragments");

            foreach (FragmentData data in this._fragmentsData)
            {
                DataFileRecord fragRec = new DataFileRecord("Fragment");
                fragRec.AddField("styleGroupId", data._styleGroupId);
                fragRec.AddField("res", data._res);
                fragRec.AddField("sprite", data._sprite);
                record.AddRecord(fragRec);
            }
            return record;
        }

        #endregion
    }
}
