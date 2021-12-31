using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDMinimap : HUDItem
    {
        struct MinimapItem
        {
            public int type; // 0-Tile 1-Breakable 2-Object 3-Snail 4-Entrance 5-Exit (we probably wont use it)
            public Vector2 position; // minimpa position
        }

        private const int MAP_WIDTH = 180;
        private const int MAP_HEIGHT = 101;

        private int tileSizeX;
        private int tileSizeY;
        private List<MinimapItem> items = new List<MinimapItem>();

        private int camX;
        private int camY;
        private int camW;
        private int camH;

        public HUDMinimap()
        {
        }

        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            ComputePositions();
        }

        public void ComputePositions()
        {
            int boardWidth = Stage.CurrentStage.Board.Width;
            int boardHeight = Stage.CurrentStage.Board.Height;
            int tileWidth = Stage.CurrentStage.Board.TileWidth;
            int boardRows = Stage.CurrentStage.Board.Rows;
            int boardColumns = Stage.CurrentStage.Board.Columns;

            // calculate dinamic tile size (depends on board size)
            tileSizeX = (int)((MAP_WIDTH * tileWidth) / boardWidth); // always integer
            tileSizeY = (int)((MAP_HEIGHT * tileWidth) / boardHeight);

            for (int r = 0; r < boardRows; r++)
            {
                for (int c = 0; c < boardColumns; c++)
                {
                    TileCell tileCell = Stage.CurrentStage.Board.Tiles[r, c];
                    if (tileCell != null && 
                        tileCell.Tile != null &&
                        tileCell.Tile.Sprite != null)
                    {
                        MinimapItem item = new MinimapItem();
                        item.type = 0;
                        if (tileCell.Tile.IsBreakable)
                            item.type = 1;
                        item.position = new Vector2((int)Position.X + c * tileSizeX, (int)Position.Y + r * tileSizeY);

                        items.Add(item);
                    }
                }
            }
        }

        public void SetCameraProjections()
        {
            // camera projections
            camW = ((int)Stage.CurrentStage.StageHUD._stageArea.Width * MAP_WIDTH) / Stage.CurrentStage.Board.Width;
            camH = (SnailsGame.GameSettings.ScreenHeight * MAP_HEIGHT) / Stage.CurrentStage.Board.Height;
        }

        private int ConvertToMinimapPointX(int posX)
        {
            return (posX * MAP_WIDTH) / Stage.CurrentStage.Board.Width;
        }

        private int ConvertToMinimapPointY(int posY)
        {
            return (posY * MAP_HEIGHT) / Stage.CurrentStage.Board.Height;
        }

        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            // set camera positions
            camX = ConvertToMinimapPointX((int)Stage.CurrentStage.Camera.Position.X);
            camY = ConvertToMinimapPointY((int)Stage.CurrentStage.Camera.Position.Y);
            camX += (int)Position.X + 1;
            camY += (int)Position.Y + 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //minimap background
            SnailsGame.DrawRectangleFilled(spriteBatch, new Rectangle((int)Position.X, (int)Position.Y, MAP_WIDTH, MAP_HEIGHT), Color.Black);

            foreach(MinimapItem item in items)
            {
                Color c = item.type == 0 ? Color.Gray : Color.LightGray;
                SnailsGame.DrawRectangleFilled(spriteBatch, new Rectangle((int)item.position.X, (int)item.position.Y, tileSizeX, tileSizeY), c);
            }

            //minimap frame
            SnailsGame.DrawRectangleFrame(spriteBatch, new Rectangle((int)Position.X, (int)Position.Y, MAP_WIDTH, MAP_HEIGHT), Color.Blue, 1);
            // camera
            SnailsGame.DrawRectangleFrame(spriteBatch, new Rectangle(camX, camY, camW, camH), Color.Red, 1);

            foreach (TwoBrainsGames.Snails.StageObjects.Snail snail in Stage.CurrentStage.Snails)
            {
                int x = ConvertToMinimapPointX((int)snail.Position.X) + (int)Position.X;
                int y = ConvertToMinimapPointY((int)snail.Position.Y) + (int)Position.Y - tileSizeY;
                SnailsGame.DrawRectangleFilled(spriteBatch, new Rectangle(x, y, 5, 5), Color.YellowGreen);
            }
        }
    }
}
