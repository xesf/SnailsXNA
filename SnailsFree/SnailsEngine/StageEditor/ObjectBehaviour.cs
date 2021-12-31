using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.StageObjects;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageEditor
{
    public class ObjectBehaviour : IDataFileSerializable
    {
        public class ObjectProperty
        {

            public string Name { get; set; }
            public string FormatText { get; set; }
            public bool ShowWhenUnselected { get; set; }
            public bool ShowWhenSelected { get; set; }

            public ObjectProperty(string name, string formatText) :
                this(name, formatText, false, true)
            {
            }

            public ObjectProperty(string name, string formatText, bool showWhenUnselected, bool showWhenSelected)
            {
                if (name == null || formatText == null)
                {
                    throw new ApplicationException("ObjectProperty name and caption cannot be null.");
                }
                this.Name = name;
                this.FormatText = formatText;
                this.ShowWhenUnselected = showWhenUnselected;
                this.ShowWhenSelected = showWhenSelected;
            }
        }

        public class PorpertiesFormSettings
        {
            public bool ShowSpriteEffect { get; set; }
            public bool ShowRotation{ get; set; }
            public bool ShowLinks { get; set; }
            public Type LinksTypeFilter { get; set; }
            public string FormTypeName { get; set; }

            public PorpertiesFormSettings()
            {
            }
        }

        public class ItemSelectFormFormSettings
        {
            public string FormTypeName { get; set; }

            public ItemSelectFormFormSettings()
            {
            }
        }

        public enum TilePlacement
        {
            TileSnap,
            Arbitrary,
            Center,
            Bottom,
            Top,
            Left,
            Right
        }

        public string StageDataId { get; private set; }
        public StageObjectType Type { get; private set; }
        public TilePlacement TilePlacementX { get; private set; }
        public TilePlacement TilePlacementY { get; private set; }
        public Vector2 PlacementOffset { get; private set; }
        public bool AllowRotation { get; private set; }
        public bool OpenPropertiesWhenAdded { get; private set; }
        public bool ShowSelectionRect { get; private set; } // Show the selection rect around the object when moving the cursor
        public List<ObjectProperty> PropertiesToShow { get; private set; }
        public PorpertiesFormSettings PropertiesForm { get; private set; }
        public ItemSelectFormFormSettings ItemSelectForm { get; private set; }
        public Vector2 PropertiesDrawOffset { get; private set; }
        public bool AllowOutOfTheBoard { get; private set; }

        public ObjectBehaviour()
        {
            this.PlacementOffset = Vector2.Zero;
            this.TilePlacementX = TilePlacement.TileSnap;
            this.TilePlacementY = TilePlacement.TileSnap;
            this.OpenPropertiesWhenAdded = true;
            this.PropertiesToShow = new List<ObjectProperty>();
            this.PropertiesForm = new PorpertiesFormSettings();
            this.ShowSelectionRect = true;
            this.AllowRotation = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public static ObjectBehaviour FromDataFileRecord(DataFileRecord record, string id)
        {
            ObjectBehaviour objBehaviour = new ObjectBehaviour();
            objBehaviour.InitFromDataFileRecord(record);
            objBehaviour.StageDataId = id;
            return objBehaviour;
        }

        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            //this.StageDataId = record.GetFieldValue<string>("stageDataId"); // This is all wrong
                                                                            // In the xml, ObjectsBehaviour is inside the TooboxItem, but in code its separeted
                                                                            // This id is initialized when InitFromDataFileRecord is called
            
            this.PlacementOffset = new Vector2(record.GetFieldValue<float>("offsetX", 0.0f),
                                                record.GetFieldValue<float>("offsetY", 0.0f));
            string tilePlacement = record.GetFieldValue<string>("tilePlacementX", this.TilePlacementX.ToString());
            this.TilePlacementX = (TilePlacement)Enum.Parse(typeof(TilePlacement), tilePlacement, true);
            tilePlacement = record.GetFieldValue<string>("tilePlacementY", this.TilePlacementY.ToString());
            this.TilePlacementY = (TilePlacement)Enum.Parse(typeof(TilePlacement), tilePlacement, true);
            this.OpenPropertiesWhenAdded = record.GetFieldValue<bool>("openPropertiesWhenAdded", this.OpenPropertiesWhenAdded);
            this.ShowSelectionRect = record.GetFieldValue<bool>("showSelectionRect", this.ShowSelectionRect);
            this.AllowRotation = record.GetFieldValue<bool>("allowRotation", this.AllowRotation);
            this.AllowOutOfTheBoard = record.GetFieldValue<bool>("allowOutOfTheBoard", this.AllowOutOfTheBoard);
            this.PropertiesToShow = new List<ObjectProperty>();

            DataFileRecord propToShowRecord = record.SelectRecord("PropertiesToShow");
            if (propToShowRecord != null)
            {
                float x = propToShowRecord.GetFieldValue<float>("offsetx", 0f);
                float y = propToShowRecord.GetFieldValue<float>("offsety", 0f);
                this.PropertiesDrawOffset = new Vector2(x, y);
                DataFileRecordList propList = propToShowRecord.SelectRecords("Property");
                // This could go to ObjectProperty.InitFromDataFileRecord
                foreach (DataFileRecord propRecord in propList)
                {
                    ObjectProperty prop = new ObjectProperty(propRecord.GetFieldValue<string>("name"), propRecord.GetFieldValue<string>("formatText"));
                    prop.ShowWhenSelected = propRecord.GetFieldValue<bool>("showWhenSelected", true);
                    prop.ShowWhenUnselected = propRecord.GetFieldValue<bool>("showWhenUnselected", false);
                    this.PropertiesToShow.Add(prop);
                }
            }

            // ItemSelectForm
            this.ItemSelectForm = null;
            DataFileRecord itemSelectFormRecord = record.SelectRecord("ItemSelectForm");
            if (itemSelectFormRecord != null)
            {
                this.ItemSelectForm = new ItemSelectFormFormSettings();
                this.ItemSelectForm.FormTypeName = itemSelectFormRecord.GetFieldValue<string>("formClass", null);
            }

            // Properties form
            this.PropertiesForm = new PorpertiesFormSettings();
            // This could go to PorpertiesFormSettings.InitFromDataFileRecord
            DataFileRecord propFormRecord = record.SelectRecord("PropertiesForm");
            if (propFormRecord != null)
            {
                this.PropertiesForm.ShowSpriteEffect = propFormRecord.GetFieldValue<bool>("showSpriteEffect", this.PropertiesForm.ShowSpriteEffect);
                this.PropertiesForm.ShowRotation = propFormRecord.GetFieldValue<bool>("showRotation", this.PropertiesForm.ShowRotation);
                this.PropertiesForm.ShowLinks = propFormRecord.GetFieldValue<bool>("showLinks", this.PropertiesForm.ShowLinks);
                string linksFilter = propFormRecord.GetFieldValue<string>("linksTypeFilter");
                if (linksFilter != null)
                {
                    this.PropertiesForm.LinksTypeFilter = System.Type.GetType(linksFilter);

                }
                this.PropertiesForm.FormTypeName = propFormRecord.GetFieldValue<string>("formClass", null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
