using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.Tutorials
{
    public class TutorialLine
    {
        public List<TutorialItem> Items { get; private set; }
        public Vector2 Position { get; set; }
        public string StCode { get; set; } // Snails Tutorial code
        public bool WithCustomPosition { get { return (this.Position != Vector2.Zero); } }

        public TutorialLine()
        {
            this.Items = new List<TutorialItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(TutorialItem item)
        {
            this.Items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public static TutorialLine CreateFromDataFileRecord(DataFileRecord record)
        {
            TutorialLine line = new TutorialLine();
            line.InitFromDataFileRecord(record);
            return line;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ParseStCode()
        {
            this.Items.Clear();
            TutorialTopicParser.ParseCodeLine(this.StCode, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this.StCode = record.GetFieldValue<string>("stCode");
            this.Position = record.GetFieldValue<Vector2>("position", Vector2.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            DataFileRecord record = new DataFileRecord("line");
            if (this.WithCustomPosition)
            {
                record.AddField("position", this.Position);
            }
            record.AddField("stCode", this.StCode);

            return record;
        }
    }
}
