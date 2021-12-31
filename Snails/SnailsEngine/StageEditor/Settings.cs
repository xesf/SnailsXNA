using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.StageObjects;
using System.Drawing;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;
using System.IO;
using System.Reflection;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.StageEditor
{
    class Settings
    {
        public struct ThemeSettings
        {
            public Color GridColor { get; set; }

        }

        public class InformationSignItem
        {
            public string SignId { get; set; }
            public string ThumbnailResId { get; set; }
            public bool Visible { get; set; }
            public Image Thumbnail { get; set; }
            public InformationSign InformationSign { get; set; }

            public override string ToString()
            {
                if (this.InformationSign != null && this.InformationSign.Id != null)
                {
                    return this.InformationSign.SignId;
                }
                return base.ToString();
            }
        }

        public static string AppName { get { return "Snails Stage Editor"; } }
        public static string StageDataFilename = "stagedata.sd";
        public const string Filename = "TwoBrainsGames.Snails.StageEditor.StageEditor.Settings.xml"; // Embedded resource
        static List<ObjectBehaviour> _ObjectBehaviours;
        public static List<ITileToolboxItem> TileToolboxItems;
        public static List<IObjectToolboxItem> ObjectToolboxItems;

        static List<InformationSignItem> _informationSignItems;
        public static List<InformationSignItem> InformationSignItems
        {
            get
            {
                if (_informationSignItems == null)
                {
                    _informationSignItems = Settings.LoadInformationSigns();
                }
                return _informationSignItems;
            }
        }

        public static Pen ObjectSelectionPen { get; private set; }
        public static Pen ObjectSelectionPenBack { get; private set; }

        public static Font ObjectPropsFont { get; private set; }
        public static SolidBrush ObjectPropsColor { get; private set; }
        public static ThemeSettings[] Theme = new ThemeSettings[(int)ThemeType.ThemeD + 1];

        /// <summary>
        /// 
        /// </summary>
        private static DataFile Open()
        {
            XmlDataFileReader reader = new XmlDataFileReader();
            return reader.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream(Settings.Filename));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {
            DataFile dataFile = Settings.Open();

            Settings.LoadTileToolboxItems();
            Settings.LoadObjectToolboxItems();
            Settings.SetupPens();
            Settings.SetupFonts();
            Settings.SetupColors();
            Settings.LoadThemeSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadTileToolboxItems()
        {
            // Tile toolbox items
            if (StageEditor.CurrentStageEdited == null)
            {
                return;
            }
            DataFile dataFile = Settings.Open();
            Settings.TileToolboxItems = new List<ITileToolboxItem>();
            DataFileRecordList tileRecords = dataFile.RootRecord.SelectRecords("TilesToolbox\\Item");
            for (int i = 0; i < tileRecords.Count; i++)
            {
                ITileToolboxItem item = TileToolboxItem.FromDataFileRecord(tileRecords[i]);
                if (item.Visible == false)
                {
                    continue;
                }

                if (item.Tile.ValidThemes != ThemeType.All &&
                    item.Tile.ValidThemes != StageEditor.CurrentStageEdited.LevelStage.ThemeId)
                {
                    continue;
                }

                Settings.TileToolboxItems.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadObjectToolboxItems()
        {
            if (StageEditor.CurrentStageEdited == null)
            {
                return;
            }
            // Object toolbox items
            DataFile dataFile = Settings.Open();
            Settings._ObjectBehaviours = new List<ObjectBehaviour>();
            Settings.ObjectToolboxItems = new List<IObjectToolboxItem>();
            DataFileRecordList objRecords = dataFile.RootRecord.SelectRecords("ObjectToolbox\\Item");
            for (int i = 0; i < objRecords.Count; i++)
            {
                IObjectToolboxItem item = ObjectToolboxItem.FromDataFileRecord(objRecords[i]);
                if (StageEditor.CurrentStageEdited.LevelStage.ThemeId != item.ValidTheme &&
                    item.ValidTheme != ThemeType.All)
                {
                    continue;
                }
                Settings.ObjectToolboxItems.Add(item);
                DataFileRecord objectBehaviour = objRecords[i].SelectRecord("ObjectBehaviour");
                if (objectBehaviour != null)
                {
                    Settings._ObjectBehaviours.Add(ObjectBehaviour.FromDataFileRecord(objectBehaviour, item.StageObject.Id));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private static List<InformationSignItem> LoadInformationSigns()
        {
            List<InformationSignItem> items = new List<InformationSignItem>();
            DataFile dataFile = Settings.Open();
            DataFileRecord record = dataFile.RootRecord.SelectRecordByField("ObjectToolbox\\Item", "stageDataObjId", "INFORMATION_SIGN");
            if (record != null)
            {
                DataFileRecordList objRecords = record.SelectRecords("Signs\\Sign");
                for (int i = 0; i < objRecords.Count; i++)
                {
                    InformationSignItem item = new InformationSignItem();
                    bool visible = objRecords[i].GetFieldValue<bool>("visible", true);
                    if (visible)
                    {
                        item.SignId = objRecords[i].GetFieldValue<string>("signId");
                        item.ThumbnailResId = objRecords[i].GetFieldValue<string>("thumbnailResId");
                        item.Thumbnail = StageEditor.GetImageFromResource(item.ThumbnailResId);
                        item.InformationSign = InformationSign.Create(StageEditor.StageData, item.SignId);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ObjectBehaviour GetObjectBehaviour(string stageDataId)
        {
            foreach (ObjectBehaviour behaviour in Settings._ObjectBehaviours)
            {
                if (stageDataId == behaviour.StageDataId)
                {
                    return behaviour;
                }
            }

            return new ObjectBehaviour(); // Return defaul settings for object
        }

        /// <summary>
        /// 
        /// </summary>
        public static ObjectToolboxItem GetObjectToolboxItem(StageObjectType type)
        {
            if (Settings.ObjectToolboxItems == null)
            {
                return null;
            }

            foreach (IObjectToolboxItem item in Settings.ObjectToolboxItems)
            {
                if (type == item.StageObject.Type)
                    return (ObjectToolboxItem)item;
            }

            return null; // Return defaul settings for object
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<SnailsBackgroundLayer> GetDefaultBackgroundLayers(ThemeType theme)
        {
            List<SnailsBackgroundLayer> layers = new List<SnailsBackgroundLayer>();
            DataFile dataFile = Settings.Open();

            DataFileRecord themeRecord = dataFile.RootRecord.SelectRecordByField("Themes\\Theme", "type", theme.ToString());
            if (themeRecord != null)
            {
                DataFileRecordList layerRecords = themeRecord.SelectRecords("DefaultLayers\\Layer");
                foreach (DataFileRecord record in layerRecords)
                {
                    SnailsBackgroundLayer layer = StageEditor.StageData.GetLayer(record.GetFieldValue<string>("id"));
                    layer.LoadContent();
                    layers.Add(layer);
                }
            }
            return layers;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void SetupPens()
        {
            Settings.ObjectSelectionPen = new Pen(Color.Yellow);
            Settings.ObjectSelectionPen.DashPattern = new float[] { 3.0F, 3.0F };
            Settings.ObjectSelectionPen.Width = 2.0F;
            Settings.ObjectSelectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

            Settings.ObjectSelectionPenBack = new Pen(Color.Black);
            Settings.ObjectSelectionPenBack.Width = 3.0F;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void SetupFonts()
        {
            Settings.ObjectPropsFont = new Font("Arial", 8, FontStyle.Regular);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void SetupColors()
        {
            Settings.ObjectPropsColor = new SolidBrush(Color.Red);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void LoadThemeSettings()
        {
            DataFile dataFile = Settings.Open();
            Settings.Theme = new ThemeSettings[(int)ThemeType.ThemeD + 1];
            DataFileRecordList themeRecords = dataFile.RootRecord.SelectRecords("Themes\\Theme");
            if (themeRecords != null)
            {
                foreach (DataFileRecord themeRecord in themeRecords)
                {
                    ThemeType type = (ThemeType)Enum.Parse(typeof(ThemeType), themeRecord.GetFieldValue<string>("type", ThemeType.None.ToString()), true);
                    if (type != ThemeType.None)
                    {
                        DataFileRecord settingsRec = themeRecord.SelectRecord("Settings");
                        if (settingsRec != null)
                        {
                            Microsoft.Xna.Framework.Color xnaColor = settingsRec.GetFieldValue<Microsoft.Xna.Framework.Color>("gridColor", Microsoft.Xna.Framework.Color.Black);
                            Settings.Theme[(int)type].GridColor = Color.FromArgb(xnaColor.R, xnaColor.G, xnaColor.B);
                        }
                    }
                }

            }
        }
    }
}
