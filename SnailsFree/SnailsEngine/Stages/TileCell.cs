using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;

namespace TwoBrainsGames.Snails.Stages
{
    public partial class TileCell
    {
       
        #region Properties
        Board _board;

        public int BoardX { get; set; }
        public int BoardY { get; set; }
        public Vector2 Position { get; set; }
        public Tile Tile { get; set; }
		public Rectangle rc;

        public int Width { get { return Stage.TILE_WIDTH; } }
        public int Height { get { return Stage.TILE_HEIGHT; } }
        public int BoardWidth { get { return this._board.Width; } }
        public int BoardHeight { get { return this._board.Height; } }

        public PathSegment[] Segments { get; private set; }

        public PathSegment TopSegment { get { return Segments[0]; } }
        public PathSegment RightSegment { get { return Segments[1]; } }
        public PathSegment BottomSegment { get { return Segments[2]; } }
        public PathSegment LeftSegment { get { return Segments[3]; } }

        public bool DrawUnderWater 
        { 
            get 
            { 
                if (this.Tile != null)
                    return this.Tile.DrawUnderWater;
                return false;
            }
            set
            {
                if (this.Tile != null)
                {
                    this.Tile.DrawUnderWater = value;
                }
            }
        }

        #endregion
        public TileCell(Tile tile, int col, int row)
        {
            Tile = tile;
            BoardX = col;
            BoardY = row;
            Position = new Vector2(BoardX * Width, BoardY * Height);
        }

        public TileCell(Board board, Tile tile, int col, int row)
        {
            this._board = board;
            Tile = tile;

            BoardX = col;
            BoardY = row;
            Position = new Vector2(BoardX * Width, BoardY * Height);

            if (board != null)
            {
                ComputeSegments();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetTile(Tile tile)
        {
            this.Tile = tile;
            if (tile != null)
            {
                this.ComputeSegments();
            }
            else
            {
                this.Segments = null; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty()
        {
            return (this.Tile == null);
        }

        internal void ComputeSegments()
        {
            Segments = new PathSegment[4];

            if (Tile != null)
            {
                int xMin = BoardX * this.Width;
                int yMin = BoardY * this.Height;
                int xMax = xMin + this.Width;
                int yMax = yMin + this.Height;

                // Walkable Tiles
                if (Tile._topPath != PathBehaviour.None)
                {
                    PathSegmentBehavior behaviour = (PathSegmentBehavior)(yMin != 0 ? Tile._topPath : PathBehaviour.Invert);
                    Segments[0] = new PathSegment(new Vector2(xMin, yMin), new Vector2(xMax, yMin), behaviour, Tile.IsBreakable);
                }
                if (Tile._rightPath != PathBehaviour.None)
                {
                    PathSegmentBehavior behaviour = (PathSegmentBehavior)(xMax != BoardWidth ? Tile._rightPath : PathBehaviour.Invert);
                    Segments[1] = new PathSegment(new Vector2(xMax, yMin), new Vector2(xMax, yMax), behaviour, Tile.IsBreakable);
                }
                if (Tile._bottomPath != PathBehaviour.None)
                {
                    PathSegmentBehavior behaviour = (PathSegmentBehavior)(yMax != BoardHeight ? Tile._bottomPath : PathBehaviour.Invert);
                    Segments[2] = new PathSegment(new Vector2(xMax, yMax), new Vector2(xMin, yMax), behaviour, Tile.IsBreakable);
                }
                if (Tile._leftPath != PathBehaviour.None)
                {
                    PathSegmentBehavior behaviour = (PathSegmentBehavior)(xMin != 0 ? Tile._leftPath : PathBehaviour.Invert);
                    Segments[3] = new PathSegment(new Vector2(xMin, yMax), new Vector2(xMin, yMin), behaviour, Tile.IsBreakable);
                }

                /*
                // Walkable Tiles
                if (Tile._topPath != PathBehaviour.none && yMin != 0)
                {
                    Segments[0] = new PathSegment(new Vector2(xMin, yMin), new Vector2(xMax, yMin), PathSegmentBehavior.Walkable, Tile.IsBreakable);
                }
                if (Tile._rightPath == PathBehaviour.Walk && xMax != BoardWidth)
                {
                    Segments[1] = new PathSegment(new Vector2(xMax, yMin), new Vector2(xMax, yMax), PathSegmentBehavior.Walkable, Tile.IsBreakable);
                }
                if (Tile._bottomPath == PathBehaviour.Walk && yMax != BoardHeight)
                {
                    Segments[2] = new PathSegment(new Vector2(xMax, yMax), new Vector2(xMin, yMax), PathSegmentBehavior.Walkable, Tile.IsBreakable);
                }
                if (Tile._leftPath == PathBehaviour.Walk && xMin != 0)
                {
                    Segments[3] = new PathSegment(new Vector2(xMin, yMax), new Vector2(xMin, yMin), PathSegmentBehavior.Walkable, Tile.IsBreakable);
                }*/
                /*
                // Reverse Walk
                if (Tile._topPath == PathBehaviour.Invert || yMin == 0)
                {
                    Segments[0] = new PathSegment(new Vector2(xMin, yMin), new Vector2(xMax, yMin), PathSegmentBehavior.ReverseWalk, Tile.IsBreakable);
                }
                if (Tile._rightPath == PathBehaviour.Invert || xMax == BoardWidth)
                {
                    Segments[1] = new PathSegment(new Vector2(xMax, yMin), new Vector2(xMax, yMax), PathSegmentBehavior.ReverseWalk, Tile.IsBreakable);
                }
                if (Tile._bottomPath == PathBehaviour.Invert || yMax == BoardHeight)
                {
                    Segments[2] = new PathSegment(new Vector2(xMax, yMax), new Vector2(xMin, yMax), PathSegmentBehavior.ReverseWalk, Tile.IsBreakable);
                }
                if (Tile._leftPath == PathBehaviour.Invert || xMin == 0)
                {
                    Segments[3] = new PathSegment(new Vector2(xMin, yMax), new Vector2(xMin, yMin), PathSegmentBehavior.ReverseWalk, Tile.IsBreakable);
                }*/
            }
        }
       

        public TileCell Clone()
        {
            TileCell cell = new TileCell(this._board, this.Tile, this.BoardX, this.BoardY);
            cell.Segments = this.Segments;
            return cell;
        }


        public void Break()
        {
            if (this.Tile == null)
            {
                return;
            }

            Sprite fragmentsSprite = Stage.CurrentStage.StageData.GetFragmentSprite(this.Tile.StyleGroupId);
            if (fragmentsSprite != null)
            {
                Vector2 particlePos = new Vector2((this.BoardX * Stage.CurrentStage.Board.TileWidth) + (Stage.CurrentStage.Board.TileWidth / 2),
                                                    (this.BoardY * Stage.CurrentStage.Board.TileHeight) + (Stage.CurrentStage.Board.TileWidth / 2));
                ExplosionEffect effect = new ExplosionEffect(particlePos.X, particlePos.Y, fragmentsSprite, 1f, SnailsGame.GameSettings.Gravity);
                effect.Color = this.Tile.BlendColor;
                Stage.CurrentStage.Particles.Add(effect);
            }

        }
    }
}
