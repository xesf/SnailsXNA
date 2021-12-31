using System;
using System.Collections.Generic;
using TwoBrainsGames.Snails.StageObjects; 
using TwoBrainsGames.Snails.ToolObjects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using System.IO;

namespace TwoBrainsGames.Snails.Stages
{
    public class StageData : ISnailsDataFileSerializable
    {
       
        #region Members
        private Dictionary<string, StageObject> _objects;
        private Dictionary<string, ToolObject> _tools;
        private Dictionary<string, Tile> _tiles;
        private Dictionary<string, SnailsBackgroundLayer> _layers;
        private Dictionary<string, LightSource> _lightSources;
        private TileFragments _tileFragments;
        #endregion

        #region Properties

        public Dictionary<string, StageObject> Objects
        {
            get { return _objects; }
        }

        // The TilesByStyle array is an array that stores the tile list in a bidimensional array
        // Dimension 0 of the array is the tile style type (rock, solid rock, metal, etc)
        // Dimension 1 stores the tile using the WalkFlags (WalkFlags combinations must be uinque within the same tile for this to work
        // The main objective of this array is to get a tile using it's WalkFlags and style as fast as possible
        // The array is initialized when list of tiles is loaded
        public Tile[,] TilesByStyleId { get; set; }

        public Dictionary<string, Tile> Tiles
        {
            get { return _tiles; }
        }

        public Dictionary<string, ToolObject> Tools
        {
            get { return _tools; }
        }


        public Dictionary<string, LightSource> LightSources
        {
            get { return this._lightSources; }
        }

        public TileFragments TileFragments { get { return this._tileFragments; } }
        #endregion

        public StageData()
        {
        }

        public bool ContainsTile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            return this._tiles.ContainsKey(id);
        }

        public Tile GetTile(string id)
        {
            if (this.ContainsTile(id) == false)
            {
                throw new SnailsException("Tile cannot be retrieved. Tile with id [" + id + "] does not exist.");
            }
            return _tiles[id];
        }

        public StageObject GetObjectNoInitialize(string id)
        {
            #if DEBUG
            // Added this because I was getting errors and didn't knew which was the object causing the problem
            if (!_objects.ContainsKey(id))
            {
                throw new SnailsException("Object with id [" + id + "] not found on StageData");
            }
#endif
            StageObject obj = _objects[id].Clone();
            return obj;
        }

        public StageObject GetObject(string id)
        {
#if DEBUG
            // Added this because I was getting errors and didn't knew which was the object causing the problem
            if (!_objects.ContainsKey(id))
            {
                throw new SnailsException("Object with id [" + id + "] not found on StageData");
            }
#endif
            StageObject obj = _objects[id].Clone();
            obj.LoadContent(); // this force the creation of new instance of resources
            obj.Initialize();
            return obj;
        }

        public ToolObject GetTool(string id)
        {
            return _tools[id];
        }

        public LightSource GetLightSource(string id)
        {
            LightSource light = this._lightSources[id].Clone();
            light.LoadContent(); 
            return light;
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetFragmentSprite(int styleGroupId)
        {
            if (styleGroupId < 0 ||
                styleGroupId >= this._tileFragments.FragmentsSprites.Length)
            {
                return null;
            }

            return this._tileFragments.FragmentsSprites[styleGroupId];
        }

        public static StageData FromDataFileRecord(DataFileRecord record)
        {
            StageData stageData = new StageData();
            stageData.InitFromDataFileRecord(record);
            return stageData;
        }

        /// <summary>
        /// Initializes the array of tiles by style (see property TilesByStyleId for more details)
        /// The total number of possible diferent styles must be provided
        /// </summary>
        private void InitTileByStyleArray(int styleCount)
        {
          this.TilesByStyleId = new Tile[styleCount, (int)WalkFlags.MaxFlag + 1];

          foreach (Tile tile in this.Tiles.Values)
          {
            this.TilesByStyleId[tile.StyleGroupId, (int)tile._walkFlags] = tile;
          }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tile GetTileByStyle(int styleGroupId, WalkFlags walkFlags)
        {
          return this.TilesByStyleId[styleGroupId, (int)walkFlags];
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent(ThemeType theme)
        {

            // load sprites into stage data tiles
            foreach (Tile tile in this.Tiles.Values)
            {
                // Don't this if the tile is not valid on the current theme
                // We don't want to load unecessary content
                if (tile.ValidThemes == ThemeType.All ||
                    tile.ValidThemes == theme)
                {
                    if (tile.ResourceId.Contains("%THEME%"))
                    {
                        tile.ResourceId = tile.ResourceId.Replace("%THEME%", theme.ToString());
                        tile._contentManagerId = ResourceManagerIds.STAGE_THEME_RESOURCES;
                    }
                    tile.LoadContent();
                }
            }


            // load sprites into stage data objects
            foreach (StageObject obj in this.Objects.Values)
            {
                // Bring the hammer
                if (obj.ResourceId.Contains("%THEME%") ||
                    obj.ResourceId.Contains(ThemeType.ThemeA.ToString()) ||
                    obj.ResourceId.Contains(ThemeType.ThemeB.ToString()) ||
                    obj.ResourceId.Contains(ThemeType.ThemeC.ToString()) ||
                    obj.ResourceId.Contains(ThemeType.ThemeD.ToString()))
                {
                    obj.ResourceId = obj.ResourceId.Replace("%THEME%", theme.ToString());
                    obj._contentManagerId = ResourceManagerIds.STAGE_THEME_RESOURCES;
                }
                if (obj.PreLoad)
                {
                    obj.LoadContent();
                }
            }

            // load sprites into stage data tools
            foreach (ToolObject tool in this.Tools.Values)
            {
                tool.LoadContent();
            }

            // Tile fragments
            this._tileFragments.LoadContent();
        }


        /// <summary>
        /// 
        /// </summary>
        public SnailsBackgroundLayer GetLayer(string id)
        {
            SnailsBackgroundLayer layer = this._layers[id].Clone();
            return layer;
        }

        /// <summary>
        /// Better follow a rule with StageData: always clone any objects taken from here
        /// </summary>
        public List<SnailsBackgroundLayer> GetLayers()
        {
            List<SnailsBackgroundLayer> layersCopy = new List<SnailsBackgroundLayer>();

            foreach (SnailsBackgroundLayer layer in this._layers.Values)
            {
                layersCopy.Add(new SnailsBackgroundLayer(layer));
            }
            return layersCopy;
        }

        /// <summary>
        /// Returns the list of distinct style groups ids in the tile list 
        /// </summary>
        public List<int> GetStyleGroupIdList()
        {
            List<int> groupList = new List<int>();

            foreach (Tile tile in this.Tiles.Values)
            {
                bool inList = false;
                foreach (int id in groupList)
                {
                    if (id == tile.StyleGroupId)
                    {
                        inList = true;
                        break;
                    }
                }

                if (!inList)
                    groupList.Add(tile.StyleGroupId);
            }

            return groupList;
        }

        /// <summary>
        /// 
        /// </summary>
        public Tile GetTileMatchingWalkFlags(int styleGroupId, WalkFlags flags)
        {
      /*     flags &= ~WalkFlags.LLCorner;
            flags &= ~WalkFlags.LRCorner;
            flags &= ~WalkFlags.URCorner;
            flags &= ~WalkFlags.ULCorner;*/
            
            Tile tile = this.TilesByStyleId[styleGroupId, (int)flags];
            if (tile != null)
            {
                return tile;
            }
            return this.TilesByStyleId[styleGroupId, (int)WalkFlags.All];
        }

        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._objects = new Dictionary<string, StageObject>();
            this._tiles = new Dictionary<string, Tile>();
            this._tools = new Dictionary<string, ToolObject>();
            this._layers = new Dictionary<string, SnailsBackgroundLayer>();
            this._lightSources = new Dictionary<string, LightSource>();
            this._tileFragments = new TileFragments();

            // Background layers
            DataFileRecordList bgLayerRecords = record.SelectRecords("Layers\\Layer");
            foreach (DataFileRecord bgRecord in bgLayerRecords)
            {
                SnailsBackgroundLayer bg = new SnailsBackgroundLayer();
                bg.InitFromDataFileRecord(bgRecord);

                this._layers.Add(bg.Id, bg);
            }

            // Tiles
            DataFileRecordList tileRecords = record.SelectRecords("Tiles\\Tile");
            int maxStyleId = 0;
            foreach (DataFileRecord tileRecord in tileRecords)
            {
                Tile tile = new Tile();
                tile.InitFromDataFileRecord(tileRecord);

                this._tiles[tile.Id] = tile;

              if (tile.StyleGroupId > maxStyleId)
              {
                maxStyleId = tile.StyleGroupId;
              }
            }


            // Objects
            DataFileRecordList objRecords = record.SelectRecords("Objects\\Object");
            foreach (DataFileRecord objRecord in objRecords)
            {
                StageObjectType type = (StageObjectType)Enum.Parse(typeof(StageObjectType), objRecord.GetFieldValue<string>("type"), false);

                StageObject obj = StageObjectFactory.Create(type);
                obj.InitFromDataFileRecord(objRecord);

                this._objects[obj.Id] = obj;
            }

            DataFileRecordList toolRecords = record.SelectRecords("Tools\\Tool");
            foreach (DataFileRecord toolRecord in toolRecords)
            {
                ToolObjectType type = (ToolObjectType)Enum.Parse(typeof(ToolObjectType), toolRecord.GetFieldValue<string>("type"),false);

                ToolObject tool = ToolObject.Create(type);
                tool.InitFromDataFileRecord(toolRecord);

                this._tools[tool.Id] = tool;
            }

            // Light sources
            DataFileRecordList lightRecords = record.SelectRecords("LightSources\\LightSource");
            foreach (DataFileRecord lightRecord in lightRecords)
            {
                LightSource.LightSourceType type = (LightSource.LightSourceType)Enum.Parse(typeof(LightSource.LightSourceType), lightRecord.GetFieldValue<string>("type", LightSource.LightSourceType.Baselight.ToString()), false);

                LightSource light = LightSource.Create(type);
                light.InitFromDataFileRecord(lightRecord);
                this._lightSources[light.Id] = light;
            }

            // Tile fragments
            DataFileRecord fragmentsRec = record.SelectRecord("TileFragments");
            this._tileFragments.InitFromDataFileRecord(fragmentsRec);

            // Create tile by style array. +1 is because style 0 is possible
            this.InitTileByStyleArray(maxStyleId + 1);
        }

		/// <summary>
		/// 
		/// </summary>
		public virtual DataFileRecord ToDataFileRecord()
		{
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
		}

		/// <summary>
		/// 
		/// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFile dataFile = new DataFile();
            dataFile.RootRecord.Name = "StageData";

            // Background layers
            DataFileRecord bgRecords = dataFile.RootRecord.AddRecord("Layers");
            foreach (SnailsBackgroundLayer bg in this._layers.Values)
            {
                bgRecords.AddRecord(bg.ToDataFileRecord());
            }

            // Tiles
            DataFileRecord tilesRecord = dataFile.RootRecord.AddRecord("Tiles");
            foreach (Tile tile in this._tiles.Values)
            {
                tilesRecord.AddRecord(tile.ToDataFileRecord());
            }

            // Objects
            DataFileRecord objsRecord = dataFile.RootRecord.AddRecord("Objects");
            foreach (StageObject obj in this._objects.Values)
            {
                objsRecord.AddRecord(obj.ToDataFileRecord());
            }

            DataFileRecord toolsRecord = dataFile.RootRecord.AddRecord("Tools");
            foreach (ToolObject tool in this._tools.Values)
            {
                toolsRecord.AddRecord(tool.ToDataFileRecord());
            }

            // Light sources
            DataFileRecord lightsRecord = dataFile.RootRecord.AddRecord("LightSources");
            foreach (LightSource light in this._lightSources.Values)
            {
                lightsRecord.AddRecord(light.ToDataFileRecord());
            }

            // Tile fragments
            dataFile.RootRecord.AddRecord(this._tileFragments.ToDataFileRecord(context));

            return dataFile.RootRecord;
        }

        #endregion
    }
}

