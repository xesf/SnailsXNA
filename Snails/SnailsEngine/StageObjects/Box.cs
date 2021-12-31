using System.Collections.Generic;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class Box : TileObject
    {
        public enum BoxType
        {
            WoodBox,
            MetalBox,
            DynamiteBox,
            DynamiteBoxTriggered
        }

      protected enum BoxDeployStatus
      {
        Deploying,
        Deployed
      }

        #region Constants
        public const string ID = "BOX";
        #endregion

        protected BoxDeployStatus _deployStatus;
        private List<ISwitchable> _objectsToSwitch; // Objects to be switched on when the deploy ends
                                                   // This is used in PopUpBoxes to create a nice chain reaction popping effect
        private bool _autodeployOnLastFrame;
        private Sprite _idleSprite;
        private string _idleSpriteResource;

        protected TileCell _tileCell;

        private int _countSnailsDeadByBox = 0;
        Sample _sampleBoxDrop;

        public Box()
            : this(StageObjectType.Box)
        {
        }

        public Box(StageObjectType type)
            : base(type)
        {
            this._autodeployOnLastFrame = true;
        }

        public Box(Box other)
            : base(other)
        {
            Copy(other);
        }

        /// <summary>
        /// Creates and deploys a box object
        /// </summary>
        public static Box CreateAndDeploy(BoxType type, Vector2 position)
        {

            string objectId = null;
            switch (type)
            {
                case BoxType.WoodBox:
                    objectId = Box.ID;
                    break;
                case BoxType.MetalBox:
                    objectId = Copper.ID;
                    break;
                case BoxType.DynamiteBoxTriggered:
                    objectId = DynamiteBoxTriggered.ID;
                    break;
                case BoxType.DynamiteBox:
                    objectId = DynamiteBox.ID;
                    break;
                default:
                    throw new SnailsException("Invalid box type " + type.ToString());
            }

            StageObject obj = Stage.CurrentStage.StageData.GetObject(objectId);
            obj.Position = position;
            obj.SnapIt();
            obj.UpdateBoundingBox();

            Stage.CurrentStage.AddObjectInRuntime(obj);
            Stage.CurrentStage.Board.SetTileAt(Tile.Empty(), obj.BoardX, obj.BoardY);
            ((Box)obj).BoxDeployed();
            return (Box)obj;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BoxDeployed()
        {
            this._sampleBoxDrop.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            Box box = (Box) other;
            this._autodeployOnLastFrame = box._autodeployOnLastFrame;
            this._idleSpriteResource = box._idleSpriteResource;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            if (!string.IsNullOrEmpty(this._idleSpriteResource))
            {
                this._idleSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._idleSpriteResource);
            }
            this._sampleBoxDrop = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.BOX_DROP);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
          base.Initialize();
          this._deployStatus = BoxDeployStatus.Deploying;
          this._objectsToSwitch = new List<ISwitchable>();
        }

        /// <summary>
        /// Deployment occurs when the tile is actually placed (after the pop animation)
        /// Box deployment may happen in 2 cases:
        /// -When the stage starts with already placed tiles of this type
        /// -When the player places the tile
        /// 
        /// In the first case, we don't want to add paths (because this are computed when the stage loads)
        /// and we don't want to check coliisions with the snails (the quadtree doesn't even exist at this stage).
        /// That's why there are this flags
        /// </summary>
        protected virtual void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
        {
            if (addTile)
            {
                Tile tile = this._tile;
                this._tileCell = Stage.CurrentStage.Board.SetTileAtWithoutPaths(tile, this.BoardX, this.BoardY);
                if (addPaths)
                {
                    List<PathSegment> segments = new List<PathSegment>();
                    segments.AddRange(this._tileCell.Segments);
                    Stage.CurrentStage.Board.AddPathSegments(segments);
                    Stage.CurrentStage.Board.RemoveCoincidentPathSegments();
                }
            }

            // Check collisions with any snail and destroy them method OnCollide() is called when collisions happen
            if (checkCollisions)
            {
                this.Quadtree.DoCollisions(this, Stage.QUADTREE_SNAIL_LIST_IDX);
                this.Quadtree.DoCollisions(this, Stage.QUADTREE_STAGEOBJ_LIST_IDX);
            }
            this._deployStatus = BoxDeployStatus.Deployed;
        }

		/// <summary>
		/// 
		/// </summary>
		public override void OnCollide(IQuadtreeContainable obj, int listIdx)
		{
            if (listIdx == Stage.QUADTREE_SNAIL_LIST_IDX)
            {
                Snail snail = obj as Snail;
                snail.KillByCrate();
                if (!(snail is EvilSnail))
                {
                    _countSnailsDeadByBox++; // used for the achievement
                }
                if (_countSnailsDeadByBox >= Achievements.Kill50WithASingleBox_Quantity)
                {
                    SnailsGame.AchievementsManager.Notify((int)AchievementsType.Kill20SnailsWithASingleBox);
                }
            }
            else
            {
                if (obj is MovingObject)
                {
                    MovingObject movObj = (MovingObject)obj;
                    if (movObj.CanDieWithCrates)
                    {
                        movObj.KillByCrate();
                    }
                }
            }

		}

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
          if (this._deployStatus == BoxDeployStatus.Deploying && this._autodeployOnLastFrame)
          {
            Tile tile = this._tile;
            this._tileCell = Stage.CurrentStage.Board.SetTileAtWithoutPaths(tile, this.BoardX, this.BoardY);

            List<PathSegment> segments = new List<PathSegment>();
            segments.AddRange(this._tileCell.Segments);
            Stage.CurrentStage.Board.AddPathSegments(segments);
            Stage.CurrentStage.Board.RemoveCoincidentPathSegments();

            // Check collisions with any objects and destroy them method OnCollide() is called when collisions happen
            this.Quadtree.DoCollisions(this, Stage.QUADTREE_SNAIL_LIST_IDX);
            this.Quadtree.DoCollisions(this, Stage.QUADTREE_STAGEOBJ_LIST_IDX);

            Stage.CurrentStage.RemoveObject(this);
            this._deployStatus = BoxDeployStatus.Deployed;

            this.SwitchObjects();
          }
	    }


        /// <summary>
        /// 
        /// </summary>
        protected void SwitchObjects()
        {
            foreach (ISwitchable switchable in this._objectsToSwitch)
            {
                switchable.SwitchOn();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddSwitchableObject(ISwitchable switchable)
        {
            this._objectsToSwitch.Add(switchable);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetSpriteWhenIdle()
        {
            this.Sprite = this._idleSprite;
            this.CurrentFrame = 0;
        }


        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record.AddField("autodeployOnLastFrame", this._autodeployOnLastFrame);
                record.AddField("idleSpriteRes", this._idleSpriteResource);
            }
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._autodeployOnLastFrame = record.GetFieldValue<bool>("autodeployOnLastFrame", this._autodeployOnLastFrame);
            this._idleSpriteResource = record.GetFieldValue<string>("idleSpriteRes", this._idleSpriteResource);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        #endregion
    }
}
