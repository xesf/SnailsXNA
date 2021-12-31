using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TwoBrainsGames.Snails;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using System.IO;
using System.Drawing;
using TwoBrainsGames.Snails.Configuration;

namespace LevelEditor
{
    public class UserSettings
    {
        #region Const
        public const string Filename = "StageEditor.UserSettings.xml";

        #endregion

        #region Properties

        static Color _ToolItemSelectedBackColor = Color.FromArgb(40, 150, 255);
        static Color _ToolItemBackColor = Color.FromArgb(235, 235, 235);

        public static bool ShowImages { get; set; }
        public static bool GridVisible { get; set; }
        public static bool GridOnTop { get; set; } 
        public static bool ShowTiles { get; set; }
        public static bool ShowCameraFrame { get; set; }
        
        public static int TilesToobarHeight { get; set; }
        public static int ObjectsToolbarHeight { get; set; }

		public static Color ToolItemSelectedBackColor
        {
            get { return UserSettings._ToolItemSelectedBackColor; }
            set { UserSettings._ToolItemSelectedBackColor = value; }
        }
        public static Color ToolItemBackColor
        {
            get { return UserSettings._ToolItemBackColor; }
            set { UserSettings._ToolItemBackColor = value; }
        }

        public static Size WindowSize { get; set; }
        public static Point WindowLocation { get; set; }
        static string _stagesPath;
        public static string StagesPath 
        { 
            get 
            {
                if (string.IsNullOrEmpty(_stagesPath))
                {
                    return GameSettings.StagesOutputFolder;
                }
                return _stagesPath; 
            }
            set
            {
                _stagesPath = value;
            }
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        public static void Save()
        {
            DataFile dataFile = new DataFile();
            dataFile.RootRecord = new DataFileRecord("StageEditor.UserSetttings");


            dataFile.RootRecord.AddField("tilesToobarHeight", UserSettings.TilesToobarHeight);
            dataFile.RootRecord.AddField("objectsToolbarHeight", UserSettings.ObjectsToolbarHeight);
            dataFile.RootRecord.AddField("showImages", UserSettings.ShowImages);
            dataFile.RootRecord.AddField("stagesPath", UserSettings.StagesPath);

            DataFileRecord formState = new DataFileRecord("FormStateSaves");
            dataFile.RootRecord.AddRecord(formState);
            if (Globals.MainForm != null)
            {
                formState.AddRecord(Globals.MainForm.CreateSaveSate());
            }

            DataFileRecord gridRecord = new DataFileRecord("Grid");
            gridRecord.AddField("Visible", UserSettings.GridVisible);
            gridRecord.AddField("ShowTiles", UserSettings.ShowTiles);
            gridRecord.AddField("OnTop", UserSettings.GridOnTop);
            gridRecord.AddField("ShowCameraFrame", UserSettings.ShowCameraFrame);
            dataFile.RootRecord.AddRecord(gridRecord);

            XmlDataFileWriter writer = new XmlDataFileWriter();
            writer.Write(UserSettings.Filename, dataFile);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadFormsStates()
        {
            if (!File.Exists(UserSettings.Filename))
            {
                return;
            }

            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile dataFile = reader.Read(UserSettings.Filename);

            DataFileRecord formStateSave = dataFile.RootRecord.SelectRecord("FormStateSaves");
            if (formStateSave != null)
            {
                Globals.MainForm.RestoreSaveSate(formStateSave.SelectRecordByField("FormStateSave", "Name", Globals.MainForm.Name));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(UserSettings.Filename))
            {
                UserSettings.TilesToobarHeight = 100;
                UserSettings.ObjectsToolbarHeight = 100; 
                return;
            }
            XmlDataFileReader reader = new XmlDataFileReader();
            DataFile dataFile = reader.Read(UserSettings.Filename);

            UserSettings.TilesToobarHeight = dataFile.RootRecord.GetFieldValue<int>("tilesToobarHeight", 100);
            UserSettings.ObjectsToolbarHeight = dataFile.RootRecord.GetFieldValue<int>("objectsToolbarHeight", 100);
            UserSettings.ShowImages = dataFile.RootRecord.GetFieldValue<bool>("ShowImages", true);
            UserSettings.ShowTiles = dataFile.RootRecord.GetFieldValue<bool>("ShowTiles", true);
            UserSettings.StagesPath = dataFile.RootRecord.GetFieldValue<string>("stagesPath", null);

            DataFileRecord gridRecord = dataFile.RootRecord.SelectRecord("Grid");
            if (gridRecord != null)
            {
                UserSettings.GridVisible = gridRecord.GetFieldValue<bool>("Visible", true);
                UserSettings.GridOnTop = gridRecord.GetFieldValue<bool>("OnTop", true);
                UserSettings.ShowCameraFrame = gridRecord.GetFieldValue<bool>("ShowCameraFrame", true);
            }
        }
    }
}
