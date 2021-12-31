using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.ToolObjects
{
    public class ToolSalt : ToolObject
    {
        #region Consts
        public const string ID = "TOOL_SALT";
        #endregion

        BoundingSquare _bsHotStop;
        Sprite _spriteHotSpot;
        float _hotSpotRotation;
 
        public ToolSalt()
            : base(ToolObjectType.Salt)
        {

        }



        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this.ObjectSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/player-cursor", "SaltCursor");
            this._spriteHotSpot = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SaltCursorHotSpot");
            this._bsHotStop = this.ObjectSprite.BoundingBox;
            this._hotSpotRotation = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Action(Vector2 position)
        {

            base.Action(position);
            // Determine if the user clicked on an edge on the tile
            // The BoundingBox on the salt tool sprite is used to test the colision

            // The x, y point that enters the function is the mouse click position
            // We have to compute the position of the sprite BB using the x,y mouse position
            // This is done adding the x,y position to the Sprite BB
            // Then we test the intersection with the paths and the computed BB
            // 
            // The quadtree is used to test only a subset of the paths
            BoundingSquare bs = this.ComputeHotSpot(position.X, position.Y);
            BoardPathNode node = this.GetNearestClickedPath(bs, position);
            if (node != null)
            {
                switch (node.Value.WallType)
                {
                    case PathSegment.SegmentType.Floor:
                        this.AddSalt((bs.Left + (bs.Width / 2)), node.Value.P0.Y, Salt.SaltPosition.Floor, node);
                        break;
                    case PathSegment.SegmentType.LeftWall:
                        this.AddSalt(node.Value.P0.X, (bs.Top + (bs.Height / 2)), Salt.SaltPosition.Left, node);
                        break;
                    case PathSegment.SegmentType.RightWall:
                        this.AddSalt(node.Value.P0.X, (bs.Top + (bs.Height / 2)), Salt.SaltPosition.Right, node);
                        break;
                    case PathSegment.SegmentType.Ceiling:
                        this.AddSalt((bs.Left + (bs.Width / 2)), node.Value.P0.Y, Salt.SaltPosition.Ceiling, node);
                        break;
                }
            }
        }

        /// <summary>
        /// Adds salt to a tile edge (pos determines the edge)
        /// </summary>
        private void AddSalt(float x, float y, Salt.SaltPosition pos, BoardPathNode node)
        {
            StageObject obj = Stage.CurrentStage.StageData.GetObject(Salt.ID);
            Salt saltObj = obj as Salt;
            saltObj.PlaceOnPath(x, y, pos, node);
            
            Stage.CurrentStage.AddObjectInRuntime(saltObj);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsValidAtPosition(Microsoft.Xna.Framework.Vector2 position)
        {
            BoundingSquare bs = this.ComputeHotSpot(position.X, position.Y);
            List<IQuadtreeContainable> paths = Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bs, Stage.QUADTREE_PATH_LIST_IDX);

            return (paths.Count > 0);
        }

        /// <summary>
        /// Returns the BB for the salt hot spot
        /// </summary>
        private BoundingSquare ComputeHotSpot(float x, float y)
        {
            return new BoundingSquare(new Vector2(x + this._bsHotStop.UpperLeft.X, y + this._bsHotStop.UpperLeft.Y), this._bsHotStop.Width, this._bsHotStop.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            this._hotSpotRotation += 0.03f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DrawCursor(Vector2 position, bool enabled)
        {
         //   this.ObjectSprite.Draw(position, 0, Color.White, Levels.CurrentLevel.SpriteBatch);
            if (enabled)
            {
                this._spriteHotSpot.Draw(position + new Vector2(this._bsHotStop.Left + (this._bsHotStop.Width / 2),
                                                            this._bsHotStop.Top + (this._bsHotStop.Height / 2)),
                                                            0, this._hotSpotRotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
            }
                //            this.Tool.ObjectSprite.Draw(new Vector2(X - Stage.CurrentStage.Camera.Position.X, Y - Stage.CurrentStage.Camera.Position.Y), 0, TOOL_OPACITY, Levels.CurrentLevel.SpriteBatch);


             //  Vector2 pos = new Vector2(-position.X, -position.Y);
             //  this.ObjectSprite.BoundingBox.Draw(Levels.CurrentLevel.SpriteBatch, Color.Red, pos);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetCursorOnBoard(bool enabled)
        {
            if (enabled)
            {
                SnailsGame.GameCursor.SetCursor(GameCursors.Salt);
            }
            else
            {
                SnailsGame.GameCursor.SetCursor(GameCursors.SaltForbidden);
            }
        }

    }
}
