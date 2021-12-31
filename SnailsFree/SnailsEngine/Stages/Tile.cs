using System;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.Stages

{

    // Indicates which segments have path
    [Flags]
    public enum WalkFlags
    {
        None,
        Left = 0x1,
        Top = 0x2,
        Right = 0x4,
        Bottom = 0x8,
        All = Left | Top | Right | Bottom,
        ULCorner = 0x10,
        URCorner = 0x20,
        LRCorner = 0x40,
        LLCorner = 0x80,
        MaxFlag = Left | Top | Right | Bottom | ULCorner | URCorner | LRCorner | LLCorner
    }

    public enum PathBehaviour
    {
        None = 0 ,
        Walk = 1,
        Invert = 2,
        WalkCW = 3,
        WalkCCW = 4
    }

    public partial class Tile : Object2D, ISnailsDataFileSerializable
    {
        #region Constants
        public const float TILE_LAYER_DEPTH = 0.2f;
        #endregion

        #region Members
        public PathBehaviour _leftPath;
        public PathBehaviour _topPath;
        public PathBehaviour _rightPath;
        public PathBehaviour _bottomPath;
        public WalkFlags _walkFlags;
        public string _id;
        public string _contentManagerId;

        // Hint support, marteladas...
        private Sprite _arrowsSprite;
        private bool IsDiretionalBox;
        private SpriteEffects _arrowSpriteEffect;

        #endregion

        #region Properties
        // Group id for this tile. A group could be all tiles of type 'Grass' or all tiles of type 'Rock'
        // This id will be used in Set/GetTileAt() to auto-build the set when a tile is removed or added
        public int StyleGroupId {get; set;}
        private bool Breakable { get; set; }
        public bool DrawUnderWater { get; set; }
        public ThemeType ValidThemes { get; set; } // This should be flags, change later if needed
        #endregion

        public Tile()
        {
            ResourceId = "TILES";
            SpriteId = "Tiles";
            BlendColor = Color.White;
            Breakable = true;
            this._contentManagerId = TwoBrainsGames.BrainEngine.Resources.ResourceManager.RES_MANAGER_ID_TEMPORARY;
            base.LayerDepth = TILE_LAYER_DEPTH;
        }

        public Tile(Tile other)
        {
            Copy(other);
        }

        public virtual void Copy(Tile other)
        {
            base.Copy(other);
            Tile tile = (Tile)other;
            this._walkFlags = tile._walkFlags;
            this._leftPath = tile._leftPath;
            this._topPath = tile._topPath;
            this._rightPath = tile._rightPath;
            this._bottomPath = tile._bottomPath;
            this.Breakable = tile.Breakable;
            this.BlendColor = tile.BlendColor;
        }

        public static Tile Empty()
        {
            Tile tile = new Tile();
            tile.Breakable = false;
            return tile;
        }

        public virtual void LoadContent()
        {
            this.Sprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + this.SpriteId, this._contentManagerId);
        }

        public virtual void Update(GameTime gameTime)
        { }

        public virtual void Draw(Vector2 pos)
        {
            if (this.Sprite == null)
            {
                return;
            }
            this.Sprite.Draw(pos, this.CurrentFrame, 0, this.SpriteEffect, TILE_LAYER_DEPTH, this.BlendColor, 1f, Stage.CurrentStage.SpriteBatch);
        }

        public void DrawShadow(Vector2 pos)
        {
            this.Sprite.Draw(pos + GenericConsts.TilesShadowDepth, this.CurrentFrame, 0, this.SpriteEffect, TILE_LAYER_DEPTH, Levels.CurrentThemeSettings._shadowColor, 1f, Stage.CurrentStage.SpriteBatch);
        }

        public void HintInitialize()
        {
            this._arrowsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/common-tiles/DirectionalBoxArrow");
            this.IsDiretionalBox = (this.Id == "TILE_DIRECTIONAL_BOX_CW" ||
                                    this.Id == "TILE_DIRECTIONAL_BOX_CCW");
            this._arrowSpriteEffect = (this.Id == "TILE_DIRECTIONAL_BOX_CW" ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }

        public void HintDraw(Vector2 pos)
        {
            if (this.IsDiretionalBox)
            {
                this._arrowsSprite.Draw(pos + new Vector2(30f, 0f), 0, 0f, this._arrowSpriteEffect, Stage.CurrentStage.SpriteBatch);
                this._arrowsSprite.Draw(pos + new Vector2(60f, 30f), 0, 90f, this._arrowSpriteEffect, Stage.CurrentStage.SpriteBatch);
                this._arrowsSprite.Draw(pos + new Vector2(30f, 60f), 0, 180f, this._arrowSpriteEffect, Stage.CurrentStage.SpriteBatch);
                this._arrowsSprite.Draw(pos + new Vector2(0f, 30f), 0, 270f, this._arrowSpriteEffect, Stage.CurrentStage.SpriteBatch);
            }
        }

        public bool IsBreakable
        {
            get
            {
                return this.Breakable;
            }
        }

       
        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
           
            this.StyleGroupId = record.GetFieldValue<int>("styleGroupId", 0);
            
            this.Breakable = record.GetFieldValue<bool>("breakable", this.Breakable);
            this.DrawUnderWater = record.GetFieldValue<bool>("drawUnderWater", this.DrawUnderWater);
            this._id = record.GetFieldValue<string>("id");
            this.BlendColor = record.GetFieldValue<Color>("color", this.BlendColor);
            this._leftPath = (PathBehaviour)Enum.Parse(typeof(PathBehaviour), record.GetFieldValue<string>("leftPath", PathBehaviour.None.ToString()), false);
            this._topPath = (PathBehaviour)Enum.Parse(typeof(PathBehaviour), record.GetFieldValue<string>("topPath", PathBehaviour.None.ToString()), false);
            this._rightPath = (PathBehaviour)Enum.Parse(typeof(PathBehaviour), record.GetFieldValue<string>("rightPath", PathBehaviour.None.ToString()), false);
            this._bottomPath = (PathBehaviour)Enum.Parse(typeof(PathBehaviour), record.GetFieldValue<string>("bottomPath", PathBehaviour.None.ToString()), false);

            if (this._leftPath != PathBehaviour.None)
                this._walkFlags |= WalkFlags.Left;
            if (this._topPath != PathBehaviour.None)
                this._walkFlags |= WalkFlags.Top;
            if (this._rightPath != PathBehaviour.None)
                this._walkFlags |= WalkFlags.Right;
            if (this._bottomPath != PathBehaviour.None)
                this._walkFlags |= WalkFlags.Bottom;

            WalkFlags walkFlags = (WalkFlags)Enum.Parse(typeof(WalkFlags), record.GetFieldValue<string>("walkFlags", WalkFlags.None.ToString()), false);
            this._walkFlags |= walkFlags;

            this.ValidThemes = (ThemeType)Enum.Parse(typeof(ThemeType), record.GetFieldValue<string>("theme", ThemeType.All.ToString()), false);
        }

        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord();
            record.Name = "Tile";

            switch (context)
            {
                case ToDataFileRecordContext.StageDataSave:
                    record.AddField("leftPath", ((int)this._leftPath).ToString());
                    record.AddField("topPath", ((int)this._topPath).ToString());
                    record.AddField("rightPath", ((int)this._rightPath).ToString());
                    record.AddField("bottomPath", ((int)this._bottomPath).ToString());
                    record.AddField("walkFlags", this._walkFlags.ToString());
                    record.AddField("breakable", this.Breakable);
                    record.AddField("drawUnderWater", this.DrawUnderWater);
                    record.AddField("styleGroupId", this.StyleGroupId);
                    record.AddField("theme", this.ValidThemes.ToString());
                    if (this.BlendColor != Color.White)
                    {
                        record.AddField("color", this.BlendColor);
                    }
                break;

                case ToDataFileRecordContext.StageSave:
                    record.RemoveField("res");
                    record.RemoveField("sprite");
                    record.RemoveField("frame");
                   break;
            }


            return record;
        }
        #endregion
    }
}
