using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.StageObjects;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Configuration;
using System.ComponentModel;

namespace TwoBrainsGames.Snails.Stages
{
    public class Board : IBrainComponent, ISnailsDataFileSerializable
    {
        public const int MINIMUM_BOARD_COLS = 19;
        public const int MINIMUM_BOARD_ROWS = 12;

        private List<BoardPathNode> _nodeAddList = new List<BoardPathNode>(32);

        #region Properties
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int TileWidth { get { return Stage.TILE_WIDTH; } }
        public int TileHeight { get { return Stage.TILE_HEIGHT; } }
        List<BoardPathNode> collisionNodes;
		Rectangle _rcCulling;

        public int Width
        {
            get { return this.Columns * this.TileWidth; }
        }

        public int Height
        {
            get { return this.Rows * this.TileHeight; }
        }

        public float WidthInCameraWorld
        {
            get { return (this.Width * Stage.CurrentStage.Camera.Scale.X); }
        }

        public float HeightInCameraWorld
        {
            get { return (this.Height * Stage.CurrentStage.Camera.Scale.Y); }
        }

        public TileCell[,] Tiles { get; set; }
        public BoardPath Paths { get; set; } // all available paths in the board
        public BoundingSquare BoundingBox { get; private set; }
        public Quadtree Quadtree { get; private set; }

        public SpriteBatch SpriteBatch
        {
            get { return Levels.CurrentLevel.SpriteBatch; }
        }

        // This is used to draw the board limits
        private SamplerState _samplerState; // used to draw the board border with texture wrap
        private SpriteAnimation _arrowAnimation; // To be used on directional paths
        
        private List<TileCell> _drawBackgroundTilesList;
        private List<TileCell> _drawForegroundTilesList;
        #endregion

        #region Constructors
        public Board()
            : this(MINIMUM_BOARD_COLS, MINIMUM_BOARD_ROWS)
        {
            this._drawBackgroundTilesList = new List<TileCell>();
            this._drawForegroundTilesList = new List<TileCell>();
        }

        public Board(int columns, int rows)
        {
            this.Columns = columns;
            this.Rows = rows;
            this.Tiles = new TileCell[rows, columns];
            this.Paths = new BoardPath();
            this.UpdateBoundingBox();
            this.collisionNodes = new List<BoardPathNode>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static Board FromDataFileRecord(DataFileRecord record)
        {
            Board board = new Board();
            board.InitFromDataFileRecord(record);
            return board;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void UpdateBoundingBox()
        {
            this.BoundingBox = new BoundingSquare(new Vector2(0, 0), new Vector2(this.Width, this.Height));
            this.Quadtree = new Quadtree(this.BoundingBox, Stage.QUADTREE_MAX_OBJECTS_PER_NODE, 
                                         Stage.QUADTREE_MIN_NODE_SIZE, Stage.QUADTREE_MIN_NODE_SIZE,
                                         Stage.QUADTREE_OBJ_LIST_COUNT);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddObjectToQuadtree(StageObject obj, int listIdx)
        {
            this.Quadtree.AddObject((IQuadtreeContainable)obj, listIdx);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveObjectFromQuadtree(StageObject obj)
        {
            this.Quadtree.RemoveObject((IQuadtreeContainable)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RepositionObjectInQuadtree(StageObject obj)
        {
            if (!obj.IsDisposed)
            {
                this.Quadtree.ObjectMoved((IQuadtreeContainable)obj);
            }
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {

            // Pre calculate all the stuf needed to draw the board limits
            this._samplerState = new SamplerState();
            this._samplerState.AddressU = TextureAddressMode.Wrap;
            this._samplerState.AddressV = TextureAddressMode.Wrap;
            this._arrowAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/common-tiles/DirectionalBoxArrow"));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
			this._rcCulling = Stage.CurrentStage.Camera.CameraRect;
			this._rcCulling.Width += this.TileWidth;
			this._rcCulling.Height += this.TileWidth;

			if (Stage.CurrentStage.State != Stage.StageState.Startup)
            {
                this._arrowAnimation.Update(gameTime);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void DrawBackground(bool shadow)
        {
			#if DEBUG
			if (SnailsGame.GameSettings.ShowTiles)
			{
				#endif
				if (!shadow)
				{
					foreach (TileCell cell in this._drawBackgroundTilesList)
					{
						if (this._rcCulling.Intersects (cell.rc)) 
						{
							cell.Tile.Draw (cell.Position);
						}
					}
				}
				else
				{
					foreach (TileCell cell in this._drawBackgroundTilesList)
					{
						if (this._rcCulling.Intersects (cell.rc)) 
						{
							cell.Tile.DrawShadow(cell.Position);
						}
					}
				}
				#if DEBUG
			}
			#endif
        }
        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            throw new SnailsException("Not implemented."); // Too sleepy to look for the not implemented exception
        }

        /// <summary>
        /// This will draw directional path arrows but could be used in other type of paths with presentation
        /// </summary>
        private void DrawPathWithPresentation()
        {
            foreach (BoardPathNode pathNode in this.Paths.Container)
            {
                if (pathNode.Value == null)
                {
                    continue; // There's a bug somewere. This has null paths sometimes, it shouldn't
                }
                if (pathNode.Value.Behavior == PathSegmentBehavior.WalkableCW)
                {
                    this._arrowAnimation.Draw(pathNode.Value.Center, pathNode.Value.Rotation, Stage.CurrentStage.SpriteBatch);
                }
                else
                if (pathNode.Value.Behavior == PathSegmentBehavior.WalkableCCW)
                {
                    this._arrowAnimation.Draw(pathNode.Value.Center, pathNode.Value.Rotation, SpriteEffects.FlipHorizontally, Stage.CurrentStage.SpriteBatch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(bool shadow)
        {
			#if DEBUG
			if (SnailsGame.GameSettings.ShowTiles)
			{
				#endif
				if (!shadow)
				{
					foreach (TileCell cell in this._drawForegroundTilesList)
					{
						if (this._rcCulling.Intersects (cell.rc)) 
						{
							cell.Tile.Draw (cell.Position);
						}
					}
				}
				else
				{
					foreach (TileCell cell in this._drawForegroundTilesList)
					{
						if (this._rcCulling.Intersects (cell.rc)) 
						{
							cell.Tile.DrawShadow(cell.Position);
						}
					}
				}
				this.DrawPathWithPresentation();
				#if DEBUG
			} 
			#endif
#if DEBUG
            // Draw all computed paths segments
            if (SnailsGame.GameSettings.ShowPaths)
            {
                Vector2 cam = Stage.CurrentStage.Camera.Position;
                foreach (BoardPathNode pathNode in this.Paths.Container)
                {
                    Color color = Color.Black;
                    PathSegment seg = pathNode.Value;
                    if (seg != null)
                    {
                        switch (seg.Behavior)
                        {
                            case PathSegmentBehavior.Walkable: color = Color.White; break;
                            case PathSegmentBehavior.ReverseWalk: color = Color.Red; break;
                            case PathSegmentBehavior.WalkableCW: color = Color.Green; break;
                            case PathSegmentBehavior.WalkableCCW: color = Color.Pink; break;
                            default: color = Color.Black; break;
                        }
                        BrainGame.DrawLine(seg.P0 - cam, seg.P1 - cam, color, 0.66f);
                        // if (pathNode.Next == null)
                        {
                            Vector2 v = (seg.P1 - seg.P0);
                            Vector2 v1 = new Vector2((float)(v.X * 0.1), (float)(v.Y * 0.1));
                            BrainGame.DrawLine(seg.P1 - cam - v1, seg.P1 - cam, (pathNode.Next == null ? Color.Red : Color.LightGreen), 0.66f);
                        }

                        // if (pathNode.Previous == null)
                        {
                            Vector2 v = (seg.P0 - seg.P1);
                            Vector2 v1 = new Vector2((float)(v.X * 0.1), (float)(v.Y * 0.1));
                            BrainGame.DrawLine(seg.P0 - cam, seg.P0 - cam - v1, (pathNode.Previous == null ? Color.Red : Color.Green), 0.66f);
                        }
                    }
                }
            }

            if (SnailsGame.GameSettings.ShowQuadtree)
            {
                this.Quadtree.Draw(Color.Red, SnailsGame.DebugFont, Stage.CurrentStage.SpriteBatch);
            }

            if (SnailsGame.GameSettings.ShowBoardGrid)
            {
                // Draw board grid
                for (int i = 0; i < this.Width; i += this.TileWidth)
                {
                    BrainGame.DrawLine(new Vector2(i, 0) - Stage.CurrentStage.Camera.Position,
                                       new Vector2(i, this.Height) - Stage.CurrentStage.Camera.Position, Color.Yellow, 0.66f);
                }
                for (int i = 0; i < this.Height; i += this.TileHeight)
                {
                    BrainGame.DrawLine(new Vector2(0, i) - Stage.CurrentStage.Camera.Position,
                                       new Vector2(this.Width, i) - Stage.CurrentStage.Camera.Position, Color.Yellow, 0.66f);
                }
            }

         //   this.BoundingBox.Draw(Color.Red, Stage.CurrentStage.Camera.Origin);
#endif
        }

        public void UnloadContent()
        { }

        public List<PathSegment> GetTileSegmentsToAdd(TileCell tileCell)
        {
            List<PathSegment> segments = new List<PathSegment>(4);
            if (tileCell != null && tileCell.Tile != null)
            {
                int c = tileCell.BoardX;
                int r = tileCell.BoardY;
                int xMin = c * this.TileWidth;
                int yMin = r * this.TileHeight;
                int xMax = xMin + this.TileWidth;
                int yMax = yMin + this.TileHeight;

                // Walkable Tiles
                if (tileCell.Tile._topPath != PathBehaviour.None &&
                    (r - 1 >= 0 && r - 1 < this.Rows &&
                    c >= 0 && c < this.Columns &&
                    this.GetTileAt(r - 1, c) == null))
                {
                    if (yMin <= 0)
                    {
                        tileCell.TopSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.TopSegment);
                }
                if (tileCell.Tile._rightPath != PathBehaviour.None &&
                    r >= 0 && r < this.Rows &&
                    c + 1 >= 0 && c + 1 < this.Columns &&
                    this.GetTileAt(r, c + 1) == null)
                {
                    if (xMax >= this.Width)
                    {
                        tileCell.RightSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.RightSegment);
                }
                if (tileCell.Tile._bottomPath != PathBehaviour.None &&
                   r + 1 >= 0 && r + 1 < this.Rows &&
                    c >= 0 && c < this.Columns &&
                    this.GetTileAt(r + 1, c) == null)
                {
                    if (yMax >= this.Height)
                    {
                        tileCell.BottomSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.BottomSegment);
                }

                if (tileCell.Tile._leftPath != PathBehaviour.None &&
                    r >= 0 && r < this.Rows &&
                    c - 1 >= 0 && c - 1 < this.Columns &&
                    this.GetTileAt(r, c - 1) == null)
                {
                    if (xMin <= 0)
                    {
                        tileCell.LeftSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.LeftSegment);
                }
                
                // Reverse Walk
                if (tileCell.Tile._topPath == PathBehaviour.Invert)
                {
                    segments.Add(tileCell.TopSegment);
                }
                if (tileCell.Tile._rightPath == PathBehaviour.Invert)
                {
                    segments.Add(tileCell.RightSegment);
                }
                if (tileCell.Tile._bottomPath == PathBehaviour.Invert)
                {
                    segments.Add(tileCell.BottomSegment);
                }
                if (tileCell.Tile._leftPath == PathBehaviour.Invert)
                {
                    segments.Add(tileCell.LeftSegment);
                }
            }
            return segments;
        }

        /// <summary>
        /// Used to add neighbor path segments
        /// </summary>
        /// <param name="tileCell"></param>
        /// <returns></returns>
        public List<PathSegment> GetNeighborTileSegmentsToAdd(TileCell tileCell)
        {
            List<PathSegment> segments = new List<PathSegment>(4);
            if (tileCell != null && tileCell.Tile != null)
            {
                int c = tileCell.BoardX;
                int r = tileCell.BoardY;
                int xMin = c * this.TileWidth;
                int yMin = r * this.TileHeight;
                int xMax = xMin + this.TileWidth;
                int yMax = yMin + this.TileHeight;

                // Walkable Tiles
                if (tileCell.Tile._topPath != PathBehaviour.None &&
                    (r - 1 >= 0 && r - 1 < this.Rows &&
                    c >= 0 && c < this.Columns &&
                    this.GetTileAt(r - 1, c) == null))
                {
                    if (yMin <= 0)
                    {
                        tileCell.TopSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.TopSegment);
                }
                if (tileCell.Tile._rightPath != PathBehaviour.None &&
                    r >= 0 && r < this.Rows &&
                    c + 1 >= 0 && c + 1 < this.Columns &&
                    this.GetTileAt(r, c + 1) == null)
                {
                    if (xMax >= this.Width)
                    {
                        tileCell.RightSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.RightSegment);
                }
                if (tileCell.Tile._bottomPath != PathBehaviour.None &&
                   r + 1 >= 0 && r + 1 < this.Rows &&
                    c >= 0 && c < this.Columns &&
                    this.GetTileAt(r + 1, c) == null)
                {
                    if (yMax >= this.Height)
                    {
                        tileCell.BottomSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.BottomSegment);
                }

                if (tileCell.Tile._leftPath != PathBehaviour.None &&
                    r >= 0 && r < this.Rows &&
                    c - 1 >= 0 && c - 1 < this.Columns &&
                    this.GetTileAt(r, c - 1) == null)
                {
                    if (xMin <= 0)
                    {
                        tileCell.LeftSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.LeftSegment);
                }
            }
            return segments;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<PathSegment> GetInitialTileSegmentsToAdd(TileCell tileCell)
        {
            List<PathSegment> segments = new List<PathSegment>(4);
            if (tileCell != null && tileCell.Tile != null)
            {
                int c = tileCell.BoardX;
                int r = tileCell.BoardY;
                int xMin = c * this.TileWidth;
                int yMin = r * this.TileHeight;
                int xMax = xMin + this.TileWidth;
                int yMax = yMin + this.TileHeight;

                // Walkable Tiles
                if (tileCell.Tile._topPath != PathBehaviour.None)
                {
                    if (yMin <= 0)
                    {
                        tileCell.TopSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.TopSegment);
                }
                if (tileCell.Tile._rightPath != PathBehaviour.None)
                {
                    if (xMax >= this.Width)
                    {
                        tileCell.RightSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.RightSegment);
                }
                if (tileCell.Tile._bottomPath != PathBehaviour.None)
                {
                    if (yMax >= this.Height)
                    {
                        tileCell.BottomSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.BottomSegment);
                }

                if (tileCell.Tile._leftPath != PathBehaviour.None)
                {
                    if (xMin <= 0)
                    {
                        tileCell.LeftSegment.Behavior = PathSegmentBehavior.ReverseWalk;
                    }
                    segments.Add(tileCell.LeftSegment);
                }
            }
            return segments;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<PathSegment> GetAllSegments()
        {
            // Moved to class member. This will avoid creating a new instance all the time
            List<PathSegment> addSegments = new List<PathSegment>();
            // find and set all segments in board
            if (this.Tiles != null)
            {
                for (int r = 0; r < this.Rows; r++)
                {
                    for (int c = 0; c < this.Columns; c++)
                    {
                        TileCell tileCell = this.Tiles[r, c];
                        if (tileCell != null)
                        {
                            addSegments.AddRange(GetInitialTileSegmentsToAdd(tileCell));
                        }
                    }
                }
            }

            return addSegments;
        }

        /// <summary>
        /// Get the rigth node to link
        /// </summary>
        /// <param name="node"></param>
        /// <param name="linked"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        internal BoardPathNode GetLinkedNode(BoardPathNode node, BoardPathNode linked, BoardPathNode newNode)
        {
            if (linked != null && linked.Value != null)
            {
                PathSegment prevSeg = node.Value;
                PathSegment aSeg = linked.Value;
                PathSegment bSeg = newNode.Value;

                Vector2 prevPoint = prevSeg.P1 - prevSeg.P0;
                Vector2 aPoint = aSeg.P1 - aSeg.P0;
                Vector2 bPoint = bSeg.P1 - bSeg.P0;

                // find the higher angle to proceed with the path segment
                float angle1 = MathHelper.ToDegrees(BrainHelper.FindAngleBetweenTwoVectors(prevPoint, aPoint));
                float angle2 = MathHelper.ToDegrees(BrainHelper.FindAngleBetweenTwoVectors(prevPoint, bPoint));

                if (angle1 >= angle2)
                    return linked;
            }

            return newNode;
        }

        public void AddPathSegments(List<PathSegment> addSegments)
        {
            // add and link new segments
            if (addSegments != null && addSegments.Count > 0)
            {
                foreach (PathSegment seg in addSegments)
                {
                    BoardPathNode newNode = this.Paths.FindSegmentNode(seg);
                    if (newNode == null) // if node doesn't exist
                    {
                        newNode = this.Paths.AddPathNode(seg);
                    }

                    foreach (BoardPathNode node in this.Paths.Container)
                    {
                        if (node.Value != null && newNode.Value != null && node.Value.P1 == newNode.Value.P0)
                        {
                            newNode.Previous = GetLinkedNode(newNode, newNode.Previous, node);
                            node.Next = GetLinkedNode(node, node.Next, newNode);
                        }

                        if (node.Value != null && newNode.Value != null && node.Value.P0 == newNode.Value.P1)
                        {
                            node.Previous = GetLinkedNode(node, node.Previous, newNode);
                            newNode.Next = GetLinkedNode(newNode, newNode.Next, node);
                        }

                        // fix linked nodes
                        if (node.Next != null)
                        {
                            node.Next.Previous = node;
                        }
                    }

                    _nodeAddList.Add(newNode);
                }
            }
        }

        public List<PathSegment> RemovePathSegments(List<PathSegment> removeSegments)
        {
            List<PathSegment> removed = new List<PathSegment>();

            // remove segments
            if (removeSegments != null && removeSegments.Count > 0 && this.Paths.Container.Count > 0)
            {
                // Moved to class member, this is to avoid creating instances all the time
                List<PathSegment> addSegments = new List<PathSegment>();
                // 1st step remove nodes from container
                foreach (PathSegment seg in removeSegments)
                {
                    BoardPathNode nodePath = this.Paths.Container.Find(
                        delegate(BoardPathNode match)
                        {
                            return match.Value == seg;
                        });

                    if (nodePath != null)
                    {
                        // remove paths
                        this.Paths.Remove(nodePath);

                        removed.Add(nodePath.Value);
                        nodePath.Value = null; // remove its reference from memory
                    }
                }

                // 2nd step find links from removed nodes
                foreach (PathSegment seg in removeSegments)
                {
                    BoardPathNode nodePath = this.Paths.Container.Find(
                        delegate(BoardPathNode match)
                        {
                            if (match.Next != null && match.Next.Value == seg)
                                return true;
                            if (match.Previous != null && match.Previous.Value == seg)
                                return true;
                            return false;
                        });

                    if (nodePath != null)
                    {
                        // reset linked nodes
                        nodePath.Next = null;
                        nodePath.Previous = null;

                        addSegments.Add(nodePath.Value); // add node if exist broken links
                    }
                }

                // re-link nodes if exist
                AddPathSegments(addSegments);
            }

            return removed;
        }

        public void ComputePaths()
        {
            this.Paths = new BoardPath(this.Quadtree);
            // get all segments and create their paths
            List<PathSegment> addSegments = GetAllSegments();
            AddPathSegments(addSegments);
            RemoveCoincidentPathSegments();
        }

        /// <summary>
        /// WARNING: critit method - this must be improved
        /// </summary>
        public void RemoveCoincidentPathSegments()
        {
            if (_nodeAddList != null && _nodeAddList.Count > 0)
            {
                // remove unecessary tiles segments which coincide eachother
                for (int s = 0; s < _nodeAddList.Count; s++)
                {
                    BoardPathNode node = _nodeAddList[s];
                    if (node != null && node.Value != null)
                    {
                        BoardPathNode node2 = this.Paths.Container.Find(delegate(BoardPathNode match)
                        {
                            return (match.Value != null && node.Value != null && match.Value.P0 == node.Value.P1 && match.Value.P1 == node.Value.P0);
                        });
                        if (node2 != null)
                        {
                            this.Paths.RemoveCoicident(node);
                            this.Paths.RemoveCoicident(node2);
                        }
                    }
                }

                // fix linked nodes
                foreach (BoardPathNode node in this.Paths.Container)
                {
                    if (node.Next != null)
                    {
                        node.Next.Previous = node;
                    }
                }

                _nodeAddList.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TileCell SetTileAtWithoutPaths(Tile tile, int col, int row)
        {
            bool addToDrawList = false;
            if (this.Tiles[row, col] == null)
            {
                this.Tiles[row, col] = new TileCell(this, tile, col, row);

                if (tile.Sprite != null) // Tiles may not have a sprite - See PopUpBoxes for instance
                {
                    addToDrawList = true;
                }
            }
            else
            {
                if (this.Tiles[row, col].Tile!= null && this.Tiles[row, col].Tile.Sprite == null)
                {
                    addToDrawList = true;
                }
                this.Tiles[row, col].SetTile(tile);
                
            }

            if (addToDrawList)
            {
                if (this.Tiles[row, col].DrawUnderWater)
                {
#if DEBUG
                    if (this._drawBackgroundTilesList.Contains(this.Tiles[row, col]))
                    {
                        throw new BrainException("Tile is already in the _drawBackgroundTilesList list.");
                    }
#endif
					AddTileCellToDrawList (this._drawBackgroundTilesList, this.Tiles[row, col]);
					//this._drawBackgroundTilesList.Add(this.Tiles[row, col]);
                }
                else
                {
#if DEBUG
                    if (this._drawForegroundTilesList.Contains(this.Tiles[row, col]))
                    {
                        throw new BrainException("Tile is already in the _drawForegroundTilesList list.");
                    }
#endif
					AddTileCellToDrawList (this._drawForegroundTilesList, this.Tiles[row, col]);
					// this._drawForegroundTilesList.Add(this.Tiles[row, col]);
                }
            }

            return this.Tiles[row, col];
        }

        /// <summary>
        /// 
        /// </summary>
        public TileCell SetTileAt(Tile tile, int col, int row)
        {
            TileCell tileCell = SetTileAtWithoutPaths(tile, col, row);

            List<PathSegment> segments = new List<PathSegment>();
            segments.AddRange(GetTileSegmentsToAdd(tileCell));
            AddPathSegments(segments);
            RemoveCoincidentPathSegments();
            return tileCell;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetTileAt(Tile tile, int col, int row, ref List<PathSegment> segments)
        {
            TileCell tileCell = SetTileAtWithoutPaths(tile, col, row);
            segments.AddRange(GetTileSegmentsToAdd(tileCell));
        }
        
       
        /// <summary>
        /// SetTileAt
        /// When a tile is set, all 8 tiles around the placed time may change
        /// </summary>
        public TileCell SetTileAt(int styleGroupId, int col, int row)
        {
#if STAGE_EDITOR
            // If there's already a tile in here, remove it
            // This special case only happens in the stage editor
            if (this.Tiles[row, col] != null &&
                this.Tiles[row, col].Tile != null &&
                this.Tiles[row, col].Tile.StyleGroupId != styleGroupId)
            {
                this.RemoveTileAt(col, row);
            }
#endif
            // First step, get all tiles around (may not exist)
            Tile ulTile = null;
            if (col > 0 && 
                row > 0 && 
                this.Tiles[row - 1, col - 1] != null &&
                this.Tiles[row - 1, col - 1].Tile.StyleGroupId == styleGroupId)
            {
                ulTile = this.Tiles[row - 1, col - 1].Tile;
            }

            Tile topTile = null;
            if (row > 0 && 
                this.Tiles[row - 1, col] != null &&
                this.Tiles[row - 1, col].Tile.StyleGroupId == styleGroupId)
            { 
                topTile = this.Tiles[row - 1, col].Tile;
            }

            Tile urTile = null;
            if (col < (this.Columns - 1) && 
                row > 0 && 
                this.Tiles[row - 1, col + 1] != null &&
                this.Tiles[row - 1, col + 1].Tile.StyleGroupId == styleGroupId)
            {
                urTile = this.Tiles[row - 1, col + 1].Tile;
            }

            Tile rightTile = null;
            if (col < (this.Columns - 1) && 
                this.Tiles[row, col + 1] != null &&
                this.Tiles[row, col + 1].Tile.StyleGroupId == styleGroupId)
            {
                rightTile = this.Tiles[row, col + 1].Tile;
            }

            Tile lrTile = null;
            if (col < (this.Columns - 1) &&
                (row < this.Rows - 1) && 
                this.Tiles[row + 1, col + 1] != null &&
                this.Tiles[row + 1, col + 1].Tile.StyleGroupId == styleGroupId)
            {
                lrTile = this.Tiles[row + 1, col + 1].Tile;
            }

            Tile bottomTile = null;
            if (row < (this.Rows - 1) && 
                this.Tiles[row + 1, col] != null &&
                this.Tiles[row + 1, col].Tile.StyleGroupId == styleGroupId)
            { 
                bottomTile = this.Tiles[row + 1, col].Tile;
            }

            Tile llTile = null;
            if (col > 0 && 
                row < (this.Rows - 1) && 
                this.Tiles[row + 1, col - 1] != null &&
                this.Tiles[row + 1, col - 1].Tile.StyleGroupId == styleGroupId)
            {
                 llTile = this.Tiles[row + 1, col - 1].Tile;
            }

            Tile leftTile = null;
            if (col > 0 && 
                this.Tiles[row, col - 1] != null &&
                this.Tiles[row, col - 1].Tile.StyleGroupId == styleGroupId)
            {
                leftTile = this.Tiles[row, col - 1].Tile;
            }

            WalkFlags newTileflags = WalkFlags.All;
            // Top tile
            if (topTile != null)
            {
                newTileflags &= ~WalkFlags.Top;
                WalkFlags flags = topTile._walkFlags & ~WalkFlags.Bottom;
                if (ulTile != null && ulTile.StyleGroupId == styleGroupId && leftTile == null)
                {
                    flags |= WalkFlags.LLCorner;
                }
                if (urTile != null && urTile.StyleGroupId == styleGroupId && rightTile == null)
                {
                    flags |= WalkFlags.LRCorner;
                }
                this.Tiles[row - 1, col].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags);
            }

            // Left tile
            if (leftTile != null)
            {
                newTileflags &= ~WalkFlags.Left;
                WalkFlags flags = leftTile._walkFlags & ~WalkFlags.Right;
                if (ulTile != null && ulTile.StyleGroupId == styleGroupId && topTile == null)
                {
                    flags |= WalkFlags.URCorner;
                }
                if (llTile != null && llTile.StyleGroupId == styleGroupId && bottomTile == null)
                {
                    flags |= WalkFlags.LRCorner;
                }
                this.Tiles[row, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags);
            }

            // Bottom tile
            if (bottomTile != null)
            {
                newTileflags &= ~WalkFlags.Bottom;
                WalkFlags flags = bottomTile._walkFlags & ~WalkFlags.Top;
                if (llTile != null && llTile.StyleGroupId == styleGroupId && leftTile == null)
                {
                    flags |= WalkFlags.ULCorner;
                }
                if (lrTile != null && lrTile.StyleGroupId == styleGroupId && rightTile == null)
                {
                    flags |= WalkFlags.URCorner;
                }
                this.Tiles[row + 1, col].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags);
            }

            // Right tile
            if (rightTile != null)
            {
                newTileflags &= ~WalkFlags.Right;
                WalkFlags flags = rightTile._walkFlags & ~WalkFlags.Left;
                if (urTile != null && urTile.StyleGroupId == styleGroupId && topTile == null)
                {
                    flags |= WalkFlags.ULCorner;
                }
                if (lrTile != null && lrTile.StyleGroupId == styleGroupId && bottomTile == null)
                {
                    flags |= WalkFlags.LLCorner;
                }
                this.Tiles[row, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags);
            }
            
            // Fix corners
            if (ulTile != null)
            {
                this.Tiles[row - 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, ulTile._walkFlags & ~WalkFlags.LRCorner);
            }

            if (urTile != null)
            {
                this.Tiles[row - 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, urTile._walkFlags & ~WalkFlags.LLCorner);
            }

            if (lrTile != null)
            {
                this.Tiles[row + 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, lrTile._walkFlags & ~WalkFlags.ULCorner);
            }

            if (llTile != null)
            {
                this.Tiles[row + 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, llTile._walkFlags & ~WalkFlags.URCorner);
            }

            // Placed tile
            if (topTile != null && leftTile != null && ulTile == null)
            {
                newTileflags |= WalkFlags.ULCorner;
            }
            if (topTile != null && rightTile != null && urTile == null)
            {
                newTileflags |= WalkFlags.URCorner;
            }
            if (bottomTile != null && leftTile != null && llTile == null)
            {
                newTileflags |= WalkFlags.LLCorner;
            }
            if (bottomTile != null && rightTile != null && lrTile == null)
            {
                newTileflags |= WalkFlags.LRCorner;
            }

            Tile newTile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, newTileflags);
            return this.SetTileAt(newTile, col, row);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveCellAt(int col, int row)
        {
            if (this.Tiles[row, col] == null)
                return;

            this.RemoveTileAt(col, row);
            this.Tiles[row, col] = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveTileAt(int col, int row)
        {
            if (this.Tiles[row, col] == null)
                return;

            if (this.Tiles[row, col].Tile != null)
            {
                int styleGroupId = this.Tiles[row, col].Tile.StyleGroupId;
                // First step, get all tiles around (may not exist)
               
                TileCell topTileCell = null;
                if (row > 0 &&
                    this.Tiles[row - 1, col] != null)
                {
                    topTileCell = this.Tiles[row - 1, col];
                }


                TileCell rightTileCell = null;
                if (col < (this.Columns - 1) && this.Tiles[row, col + 1] != null)
                {
                    rightTileCell = this.Tiles[row, col + 1];
                }

                TileCell bottomTileCell = null;
                if (row < (this.Rows - 1) && this.Tiles[row + 1, col] != null)
                {
                    bottomTileCell = this.Tiles[row + 1, col];
                }

                TileCell leftTileCell = null;
                if (col > 0 && this.Tiles[row, col - 1] != null)
                {
                    leftTileCell = this.Tiles[row, col - 1];
                }

                // This are accessory vars used to correct corner tiles
                bool tileAtTopHasBottomPath = false;
                bool tileAtLeftHasRightPath = false;
                bool tileAtBottomHasTopPath = false;
                bool tileAtRightHasLeftPath = false;

                List<PathSegment> addSegments = new List<PathSegment>(16); // max of 32 segments
                List<PathSegment> removeSegments = new List<PathSegment>(4);
                addSegments.Clear();
                removeSegments.Clear();
                // Remove tile paths
                removeSegments.AddRange(Tiles[row, col].Segments);
                this.RemovePathSegments(removeSegments);


                if (this.Tiles[row, col].DrawUnderWater)
                {
                    this._drawBackgroundTilesList.Remove(this.Tiles[row, col]);
                }
                else
                {
                    this._drawForegroundTilesList.Remove(this.Tiles[row, col]);
                }
                this.Tiles[row, col].SetTile(null);
                this.Tiles[row, col] = null;

                // Tile at left
                if (leftTileCell != null && leftTileCell.Tile != null)
                {
                    if (leftTileCell.Tile.StyleGroupId == styleGroupId)
                    {
                        WalkFlags flags = leftTileCell.Tile._walkFlags | WalkFlags.Right;
                        flags &= ~(WalkFlags.URCorner | WalkFlags.LRCorner);
                        Tile tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(leftTileCell.Tile.StyleGroupId, flags);
                        this.SetTileAt(tile, col - 1, row, ref addSegments);
                        tileAtLeftHasRightPath = ((tile._walkFlags & WalkFlags.Right) == WalkFlags.Right);
                    }
                    else // then reset the tile to normilized its paths
                    {
                        // only add the right segment
                        addSegments.Add(leftTileCell.RightSegment);
                    }
                }

                // Tile at top
                if (topTileCell != null && topTileCell.Tile != null)
                {
                    if (topTileCell.Tile.StyleGroupId == styleGroupId)
                    {
                        WalkFlags flags = topTileCell.Tile._walkFlags | WalkFlags.Bottom;
                        flags &= ~(WalkFlags.LLCorner | WalkFlags.LRCorner);
                        Tile tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(topTileCell.Tile.StyleGroupId, flags);
                        this.SetTileAt(tile, col, row - 1, ref addSegments);
                        tileAtTopHasBottomPath = ((tile._walkFlags & WalkFlags.Bottom) == WalkFlags.Bottom);
                    }
                    else // then reset the tile to normilized its paths
                    {
                        // only add the botom segment
                        addSegments.Add(topTileCell.BottomSegment);
                    }
                }

                // Tile at right
                if (rightTileCell != null && rightTileCell.Tile != null)
                {
                    if (rightTileCell.Tile.StyleGroupId == styleGroupId)
                    {
                        WalkFlags flags = rightTileCell.Tile._walkFlags | WalkFlags.Left;
                        flags &= ~(WalkFlags.ULCorner | WalkFlags.LLCorner);
                        Tile tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(rightTileCell.Tile.StyleGroupId, flags);
                        this.SetTileAt(tile, col + 1, row, ref addSegments);
                        tileAtRightHasLeftPath = ((tile._walkFlags & WalkFlags.Left) == WalkFlags.Left);
                    }
                    else // then reset the tile to normilized its paths
                    {
                        // only add the left segment
                        addSegments.Add(rightTileCell.LeftSegment);
                    }
                }

                // Tile at bottom
                if (bottomTileCell != null && bottomTileCell.Tile != null)
                {

                    if (bottomTileCell.Tile.StyleGroupId == styleGroupId)
                    {
                        WalkFlags flags = bottomTileCell.Tile._walkFlags | WalkFlags.Top;
                        flags &= ~(WalkFlags.ULCorner | WalkFlags.URCorner);
                        Tile tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(bottomTileCell.Tile.StyleGroupId, flags);
                        this.SetTileAt(tile, col, row + 1, ref addSegments);
                        tileAtBottomHasTopPath = ((tile._walkFlags & WalkFlags.Top) == WalkFlags.Top);
                    }
                    else // then reset the tile to normilized its paths
                    {
                        // only add the top segment
                        addSegments.Add(bottomTileCell.TopSegment);
                    }
                }

                // Added neighbor tile segments
                if (row - 1 >= 0 && col - 1 >= 0)
                    addSegments.AddRange(GetNeighborTileSegmentsToAdd(Tiles[row - 1, col - 1]));
                if (row - 1 >= 0 && col + 1 < Columns)
                    addSegments.AddRange(GetNeighborTileSegmentsToAdd(Tiles[row - 1, col + 1]));
                if (row + 1 < Rows && col - 1 >= 0)
                    addSegments.AddRange(GetNeighborTileSegmentsToAdd(Tiles[row + 1, col - 1]));
                if (row + 1 < Rows && col + 1 < Columns)
                    addSegments.AddRange(GetNeighborTileSegmentsToAdd(Tiles[row + 1, col + 1]));

                this.AddPathSegments(addSegments);
                this.RemoveCoincidentPathSegments();

                // Adjust corner tiles
                // Upper left
                if (tileAtTopHasBottomPath && tileAtLeftHasRightPath &&
                    this.Tiles[row - 1, col - 1] != null)
                {
                    Tile ulTile = this.Tiles[row - 1, col - 1].Tile;
                    if (ulTile != null && ulTile.StyleGroupId == styleGroupId)
                    {
                        this.Tiles[row - 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, ulTile._walkFlags | WalkFlags.LRCorner);
                    }
                }

                // Upper right
                if (tileAtTopHasBottomPath && tileAtRightHasLeftPath && 
                    this.Tiles[row - 1, col + 1] != null)
                {
                    Tile urTile = this.Tiles[row - 1, col + 1].Tile; ;
                    if (urTile != null && urTile.StyleGroupId == styleGroupId)
                    {
                        this.Tiles[row - 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, urTile._walkFlags | WalkFlags.LLCorner);
                    }
                }

                // Lower right             
                if (tileAtRightHasLeftPath && tileAtBottomHasTopPath &&
                    this.Tiles[row + 1, col + 1] != null)
                {
                    Tile lrTile = this.Tiles[row + 1, col + 1].Tile;
                    if (lrTile != null && lrTile.StyleGroupId == styleGroupId)
                    {
                        this.Tiles[row + 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, lrTile._walkFlags | WalkFlags.ULCorner);
                    }
                }
                // Lower left
                if (tileAtLeftHasRightPath && tileAtBottomHasTopPath &&
                    this.Tiles[row + 1, col - 1] != null)
                {
                    Tile llTile = this.Tiles[row + 1, col - 1].Tile;
                    if (llTile != null && llTile.StyleGroupId == styleGroupId)
                    {
                        this.Tiles[row + 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, llTile._walkFlags | WalkFlags.URCorner);
                    }
                }

            }
 
        }

        /// <summary>
        /// 
        /// </summary>
        public TileCellCoords GetCoordsFromPosition(Vector2 pos)
        {
            int rx = (int)(pos.X / this.TileWidth);
            int ry = (int)(pos.Y / this.TileHeight);
             
            return new TileCellCoords(rx, ry);
       }

        /// <summary>
        /// 
        /// </summary>
        public TileCell GetTileCellAt(Vector2 v)
        {
         
            int rx = (int)(v.X / this.TileWidth);
            int ry = (int)(v.Y / this.TileHeight);

            if (this.Tiles.GetLength(0) <= ry ||
                this.Tiles.GetLength(1) <= rx)
            {
                return null;
            }
            return this.Tiles[ry, rx];
        }


        /// <summary>
        /// 
        /// </summary>
        public Tile GetTileAt(Vector2 v)
        {
            TileCell cell = this.GetTileCellAt(v);
            if (cell == null)
            {
                return null;
            }

            return cell.Tile;
        }

        /// <summary>
        /// 
        /// </summary>
        public Tile GetTileAt(int row, int col)
        {
            if (row >= this.Rows ||
                col >= this.Columns)
            {
                return null;
            }

            if (this.Tiles[row, col] == null)
            {
                return null;
            }

            return (this.Tiles[row, col].Tile);
        }

        /// <summary>
        /// 
        /// </summary>
        public TileCellCoords GetTileCellCoordsAt(Vector2 v)
        {
            return new TileCellCoords((int)(v.X / this.TileWidth), (int)(v.Y / this.TileHeight));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resize(int cols, int rows)
        {
            TileCell[,] newTiles = new TileCell[rows, cols];
            for (int i = 0; (i < cols) && (i < this.Columns); i++)
            {
                for (int j = 0; (j < rows) && (j < this.Rows); j++)
                {
                    newTiles[j, i] = this.Tiles[j, i];
                }
            } 
            
            this.Columns = cols;
            this.Rows = rows;
            this.Tiles = newTiles;
            this.ComputePaths();
            this.UpdateBoundingBox();
            this.RefreshDrawLists();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshDrawLists()
        {
            this._drawBackgroundTilesList.Clear();
            this._drawForegroundTilesList.Clear();

            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0;  c < this.Columns; c++)
                {
                    if (this.Tiles[r, c] != null)
                    {
                        if (!this.Tiles[r, c].DrawUnderWater)
                        {
							//this._drawForegroundTilesList.Add(this.Tiles[r, c]);
							AddTileCellToDrawList (this._drawForegroundTilesList, this.Tiles[r, c]);
						}
                        else
                        {
							//this._drawBackgroundTilesList.Add(this.Tiles[r, c]);
							AddTileCellToDrawList (this._drawBackgroundTilesList, this.Tiles[r, c]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsObjectInBounds(StageObject obj)
        {
            return (obj.Collides(this.BoundingBox));
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshTiles()
        {
            for (int i = 0; (i < this.Columns); i++)
            {
                for (int j = 0; (j < this.Rows); j++)
                {
                    if (this.Tiles[j, i] != null &&
                        this.Tiles[j, i].Tile != null)
                    {
                        this.Tiles[j, i].Tile = Stage.CurrentStage.StageData.GetTile(this.Tiles[j, i].Tile._id);
                    }
                }
            } 
        }

        #region Collision
        /// <summary>
        /// This can be better if it recieves BB instead of rects
        /// We assume that both rects have the same size or else this will not work, this limitation disappears
        /// if the method recieves BB
        /// </summary>
        public int Collides(StageObject obj, ref OOBoundingBox bbOld, ref OOBoundingBox bbNew, out Vector2 retIntersectPt, out BoardPathNode retNode, out MovingObject.CollidingCorner cornerOfCollision, out Vector2 bbCollidingPoint)
        {
            int collisionCount = 0; // Control the number of colisions, will matter when objects colide in corners
            cornerOfCollision = MovingObject.CollidingCorner.Node;
            retNode = null;
            bbCollidingPoint = Vector2.Zero;

            // Step 1 - Compute the 4 segments that go from the oldRect to the newRect
            PathSegment urSeg, lrSeg, ulSeg, llSeg;

            ulSeg = new PathSegment(bbOld.P0, bbNew.P0);
            urSeg = new PathSegment(bbOld.P1, bbNew.P1);
            llSeg = new PathSegment(bbOld.P3, bbNew.P3);
            lrSeg = new PathSegment(bbOld.P2, bbNew.P2);

            // This var controls the distance from the closestSegment
            float closestSegmentDistance = 999999.0f; // Use a big number here


            // To store the intersecting point in the segment 
            Vector2 intersectPt = new Vector2(0.0f, 0.0f);
            retIntersectPt = intersectPt;
            float distanceToSegment;

            // Loop all PathSegments that are elligible for collision
            // Only the nodes that are in the same tree node matter

            foreach (QuadtreeNode node in obj.QuadtreeNodes)
            {
                foreach (IQuadtreeContainable nodeObj in node.ObjectLists[Stage.QUADTREE_PATH_LIST_IDX])
                {
#if DEBUG
                    SnailsGame.DebugInfo.CollisionCounter.Counter++;
#endif
                    // Compute the cross product between one of the PathSegments computed in step 1
                    // and the PathSegment to test (we can take any of the segments from step 1 
                    // because the result will be the same, no need to test the four segments)
                    // We took the upper right segment for the test
                    // We are discarding segments that don't face the object
                    BoardPathNode pathNode = (BoardPathNode)nodeObj;
                    if (urSeg.Classify(pathNode.Value) >= 0)
                    {
                        continue;
                    }

                    // Test collision with the segment and the four object segments

                    // Upper right
                    if (urSeg.Collides(pathNode.Value, ref intersectPt))
                    {
                        // Compute the distance between the ur point from the old BB and the intersecting point
                        distanceToSegment = Vector2.Distance(new Vector2(intersectPt.X, intersectPt.Y), urSeg.P0);
                        // Is this the closest segment found? If it is, make it the closest segment
                        if (distanceToSegment < closestSegmentDistance)
                        {
                            retNode = pathNode;
                            closestSegmentDistance = distanceToSegment;
                            retIntersectPt = intersectPt;
                            cornerOfCollision = MovingObject.CollidingCorner.UpperRight;
                            collisionCount++;
                            bbCollidingPoint = urSeg.P1;
                        }
                    }
                    
                    // Repeat previous computation for the other 3 corners 
                    // Upper left
                    if (ulSeg.Collides(pathNode.Value, ref intersectPt))
                    {
                        distanceToSegment = Vector2.Distance(new Vector2(intersectPt.X, intersectPt.Y), ulSeg.P0);
                        if (distanceToSegment < closestSegmentDistance)
                        {
                            retNode = pathNode;
                            closestSegmentDistance = distanceToSegment;
                            retIntersectPt = intersectPt;
                            cornerOfCollision = MovingObject.CollidingCorner.UpperLeft;
                            collisionCount++;
                            bbCollidingPoint = ulSeg.P1;
                        }
                    }

                    // Lower right
                    if (lrSeg.Collides(pathNode.Value, ref intersectPt))
                    {
                        distanceToSegment = Vector2.Distance(new Vector2(intersectPt.X, intersectPt.Y), lrSeg.P0);
                        if (distanceToSegment < closestSegmentDistance)
                        {
                            retNode = pathNode;
                            closestSegmentDistance = distanceToSegment;
                            retIntersectPt = intersectPt;
                            cornerOfCollision = MovingObject.CollidingCorner.LowerRight;
                            collisionCount++;
                            bbCollidingPoint = lrSeg.P1;
                        }
                    }

                    // Lower left
                    if (llSeg.Collides(pathNode.Value, ref intersectPt))
                    {
                        distanceToSegment = Vector2.Distance(new Vector2(intersectPt.X, intersectPt.Y), llSeg.P0);
                        if (distanceToSegment < closestSegmentDistance)
                        {
                            retNode = pathNode;
                            closestSegmentDistance = distanceToSegment;
                            retIntersectPt = intersectPt;
                            cornerOfCollision = MovingObject.CollidingCorner.LowerLeft;
                            collisionCount++;
                            bbCollidingPoint = llSeg.P1;
                        }
                    }
                }
            }

            // No segment found, the object didn't collide
            if (retNode == null)
            {
                return 0;
            }

            return collisionCount++;
        }
        
        /// <summary>
        /// I don't wanna mess with existing collision systems, so new methods are created with _1 in the name
        /// </summary>
        public bool Collides(StageObject obj, PathSegment S1, ref Point I, ref BoardPathNode retNode)
        {
            // Probably a list isn't needed here...
            this.collisionNodes.Clear();
            foreach (QuadtreeNode node in obj.QuadtreeNodes)
            {
                foreach (IQuadtreeContainable nodeObj in node.ObjectLists[Stage.QUADTREE_PATH_LIST_IDX])
                {
#if DEBUG
                    SnailsGame.DebugInfo.CollisionCounter.Counter++;
#endif
                    // Compute the cross product between the 2 path segments
                    // This will filter segments that don't face our object
                    BoardPathNode pathNode = (BoardPathNode)nodeObj;

                    if (S1.Classify(pathNode.Value) >= 0)
                    {
                        continue;
                    }

                    if (S1.Collides(pathNode.Value, ref I))
                    {
                        collisionNodes.Add((BoardPathNode)nodeObj);
                    }
                }
            }

            if (collisionNodes.Count > 0) // if collides
            {
                if (collisionNodes.Count == 1)
                {
                    retNode = collisionNodes[0];
                }
                else
                {
                    float minY = -1;
                    // get the max Y coordinate in all this nodes
                    for (int n = 1; n < collisionNodes.Count; n++)
                    {
                        float y = collisionNodes[n].Value.P0.Y;
                        float y2 = collisionNodes[n].Value.P1.Y;

                        if (y2 < y) // get the minim Y from Point 0 and 1
                            y = y2;

                        if (minY == -1 || y < minY)
                        {
                            minY = y;
                            retNode = collisionNodes[n];
                        }
                    }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public BoardPathNode PathCollidesWithObject(StageObject obj)
        {
            foreach (BoardPathNode node in this.Paths.Container)
            {
                if (node.Collides((IQuadtreeContainable)obj))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public BoardPathNode PathCollidesWithBoundingSquare(BoundingSquare bs)
        {
            foreach (BoardPathNode node in this.Paths.Container)
            {
                if (node.Collides(bs))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public BoardPathNode PathCollidesWithVector(Vector2 p0, Vector2 p1)
        {
            foreach (BoardPathNode node in this.Paths.Container)
            {
                if (node.Collides(p0, p1))
                {
                    return node;
                }
            }

            return null;
        }
        #endregion

        #region IDataFileSerializable Members

        public void InitFromDataFileRecord(DataFileRecord boardRecord)
        {
            this.Columns = boardRecord.GetFieldValue<int>("columns");
            this.Rows = boardRecord.GetFieldValue<int>("rows");

            DataFileRecord tilesRecord = boardRecord.SelectRecord("Tiles");

            this.Tiles = new TileCell[this.Rows, this.Columns];
            this.UpdateBoundingBox();

            DataFileRecordList tileRecords = tilesRecord.SelectRecords("Tile");
            foreach (DataFileRecord tileRec in tileRecords)
            {
                string id = tileRec.GetFieldValue<string>("id");
                int boardX = tileRec.GetFieldValue<int>("boardX");
                int boardY = tileRec.GetFieldValue<int>("boardY");
                Tile tile = Stage.CurrentStage.StageData.GetTile(id);

                this.Tiles[boardY, boardX] = new TileCell(this, tile, boardX, boardY);
            }
            this.RefreshDrawLists();
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
            DataFileRecord record = new DataFileRecord("Board");
            record.AddField("columns", this.Columns);
            record.AddField("rows", this.Rows);

            DataFileRecord tilesRecord = new DataFileRecord("Tiles");
            tilesRecord.AddField("width", this.TileWidth);
            tilesRecord.AddField("height", this.TileHeight);
            record.AddRecord(tilesRecord);

            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Columns; c++)
                {
                    if (this.Tiles[r, c] != null)
                    {
                        // we don't need to save unknown/broken tiles (this avoids the code below)
                        if (this.Tiles[r, c].Tile != null)
                        {
                            DataFileRecord tileRecord = this.Tiles[r, c].Tile.ToDataFileRecord(context);
                            tileRecord.AddField("boardX", c.ToString());
                            tileRecord.AddField("boardY", r.ToString());
                            //tileRecord.AddField("drawUnderWater", this.Tiles[r, c].DrawUnderWater);
                            tilesRecord.AddRecord(tileRecord);
                        }
                    }
                }
            }

            return record;
        }

        #endregion

		private void AddTileCellToDrawList(List<TileCell> list, TileCell cell)
		{
			cell.rc = new Rectangle ((int)cell.Position.X, 
				(int)cell.Position.Y, 60, 60); 
			list.Add(cell);

		}
    }
}
