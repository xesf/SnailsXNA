using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Debugging;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Resources;
using System;
using TwoBrainsGames.Snails.Player;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// Explosion is an object just like the others, and can be used by any object that explodes
    /// Explosions can kill object and tiles
    /// -Object collisions are tested using the Quadree. The BB to use is defined in the QuadtreeCollisionBB override
    /// -Tiles collisions are tested in the update method by a simple BB->tile collision test
    /// 
    /// BB to test can have 3 sizes : small, medium and big
    /// This enum is just the index to the explosion sprite colision BB (defined in the animation editor)
    /// Tiles explosion with a small BB only destroys the tile that is in the same spot of the explosion (dynamite boxes for instance)
    /// </summary>
    public class Explosion : StageObject
    {

             
        // This does not affect the explosion size. It only affects the explosion collision boxes
        public enum ExplosionSize
        {
            Small,
            Medium
        }

        public enum ExplosionType
        {
            Dynamite,
            Tile
        }

        public enum ExplosionRadiusType
        {
            Circle,
            Square
        }

        private enum StatusType
        {
            Exploding,
            Smoking // Will not kill objects when in this state
        }

        [Flags]
        public enum ObjectTypeAffected
        {
            Tiles = 0x01,
            Snails = 0x02,
            Objects = 0x04,
            All = Tiles | Snails | Objects
        }

        #region Constants
        public const string ID = "EXPLOSION";

        public const int EXPLOSION_TIME = 1300;
        public const int RUMBLE_SHAKE_TIME = 750;
        public const int CAMERA_SHAKE_TIME = 800;
        public const int CAMERA_SHAKE_TIME_LIQUID = 500;
        public const int CAMERA_SHAKE_STRENGHT = 10;
        public const int CAMERA_SHAKE_STRENGHT_LIQUID = 3;
        public const float EXPLOSION_LAYER_DEPTH = 0.8f;

        public const int SPRITE_BB_IDX_SMALL_EXPLOSION = 0; // This is a smaller BB. The BB is smaller then a tile
        public const int SPRITE_BB_IDX_BIG_EXPLOSION = 1; // This a a bigger BB. This explosion hits adjacent tiles
        #endregion

        #region Members/Properties
        private int _countSnailsExploded = 0;

        private int CameraShakeStrength
        {
            get { return (this.IsUnderLiquid ? CAMERA_SHAKE_STRENGHT_LIQUID : CAMERA_SHAKE_STRENGHT); }
        }
        private int CameraShakeTime
        {
            get { return (this.IsUnderLiquid ? CAMERA_SHAKE_TIME_LIQUID : CAMERA_SHAKE_TIME); }
        }
        protected ExplosionType _typeExplosion;

        protected Sample []_explosionSample;
        protected Sample _underWaterExplosionSample;
        protected Sample _soundToPlay;
        private BoundingCircle _tilesCollisionBS;
        private BoundingSquare _tilesCollisionBB; // This will store the BB that contains the BS, will be used to determine
                                                 // which tiles will be tested for collision
        // The same as the above but for objects
        private BoundingCircle _objCollisionBS;
        private BoundingSquare _objCollisionBB; // This will store the BB that contains the BS, will be used to determine
        public BoundingSquare ObjCollisionBB { get { return this._objCollisionBB; } }

        // which tiles will be tested for collision

        private bool Exploded { get; set; }
        public ExplosionSize TileKillBBSize  { get; set; }  
        public ExplosionSize ObjectKillBBSize  { get; set; }
        public ObjectTypeAffected AffectedObjects { get; set; }
        public ExplosionRadiusType RadiusType { get; set; }
        public bool DestroyUnbreakbleTiles { get; set; }
        public bool ThrowParticles{ get; set; }
        private StatusType _explosionStatus;
        private Sprite _smokeSprite;
        public Sprite SmokeSprite
        {
            get { return this._smokeSprite; }
            set { this._smokeSprite = value; }
        }
        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this._objCollisionBB;
            }
        }

        public StageObject ExplosionSource { get; set;}
        #endregion        

        /// <summary>
        /// 
        /// </summary>
        public Explosion()
            : base(StageObjectType.Explosion)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._explosionSample = new Sample[3];
            this._explosionSample[0] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.EXPLOSION_1, this);
            this._explosionSample[1] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.EXPLOSION_2, this);
            this._explosionSample[2] = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.EXPLOSION_3, this);

            this._underWaterExplosionSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.EXPLOSION_UNDERWATER, this);
            this._smokeSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "ExplosionSmoke");
        }

        /// <summary>
        /// 
        /// </summary>
        private Sample GetSoundToPlay()
        {
            if (this.IsUnderLiquid)
            {
                return this._underWaterExplosionSample;
            }
            
            return this._explosionSample[BrainGame.Rand.Next(this._explosionSample.Length)];
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            _explosionSample = (other as Explosion)._explosionSample;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BoundingSquare GetObjCollisionBB()
        {
            return this.Sprite.BoundingBoxes[(int)this.TileKillBBSize].Transform(this.Position);
        }

        /// <summary>
        /// Setup the explosion object
        /// </summary>
        public override void OnAddedToStage()
        {
            // Transform the collision BS to the object position
            // Pre compute this because the explosion position will not change
            if (this.RadiusType == ExplosionRadiusType.Circle)
            {
                this._tilesCollisionBS = this.TransformBoundingCircle(this.Sprite._boundingSpheres[(int)this.TileKillBBSize]);
                this._tilesCollisionBB = this._tilesCollisionBS.GetContainingSquare();

                // The same as the above but for objects
                this._objCollisionBS = this.TransformBoundingCircle(this.Sprite._boundingSpheres[(int)this.ObjectKillBBSize]);
                this._objCollisionBB = this._objCollisionBS.GetContainingSquare();
            }
            else
            {
                this._tilesCollisionBB = this.Sprite.BoundingBoxes[(int)this.TileKillBBSize].Transform(this.Position);
                this._objCollisionBB = this.GetObjCollisionBB();
            }

            this.RepositionObjectInQuadtree();
            Stage.CurrentStage.SendExplosionNotification(this);
        }

      
        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            bool snailWasKilled = false;
            StageObject stageObj = (StageObject) obj;
            if (stageObj.CanDieWithExplosions)
            {
                if (!stageObj.CanDieWithAnyTypeOfExplosion)
                {
                    if ((this.AffectedObjects & ObjectTypeAffected.Objects) != ObjectTypeAffected.Objects &&
                          !(stageObj is Snail))
                    {
                        return;
                    }

                    if ((this.AffectedObjects & ObjectTypeAffected.Snails) != ObjectTypeAffected.Snails &&
                          (stageObj is Snail))
                    {
                        return;
                    }
                }


                // Make a second test, check if the explosion BS collides with the object
                if (this.RadiusType == ExplosionRadiusType.Circle)
                {
                    if (this._objCollisionBS.Collides(stageObj.QuadtreeCollisionBB))
                    {
                        (obj as StageObject).KillByExplosion(this);
                        snailWasKilled = true;
                    }
                }
                else
                {
                    if (this._objCollisionBB.Collides(stageObj.QuadtreeCollisionBB))
                    {
                        (obj as StageObject).KillByExplosion(this);
                        snailWasKilled = true;
                    }
                }

                if (snailWasKilled)
                {
                    if (stageObj is Snail &&
                         !((Snail)stageObj).IsEvilSnail &&
                        this.ExplosionSource is Dynamite)
                    {
                        _countSnailsExploded++;
                        //int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Kill50WithASingleDynamite).Quantity;
                        if (_countSnailsExploded >= Achievements.Kill50WithASingleDynamite_Quantity)
                        {
                            SnailsGame.AchievementsManager.Notify((int)AchievementsType.Kill50WithASingleDynamite);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this._explosionStatus == StatusType.Smoking)
            {
                return; // Smoke does not kill objects
            }

            if (this.Exploded)
            {
                this.DoQuadtreeCollisions(Stage.QUADTREE_STAGEOBJ_LIST_IDX);
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
                return;
            }

            if ((this.AffectedObjects & ObjectTypeAffected.Tiles) == ObjectTypeAffected.Tiles)
            {
                this.CheckCollisionWithTiles();
            }

            // Camera shake
            Stage.CurrentStage.Camera.Shake(this.CameraShakeTime, this.CameraShakeStrength);

            if (SnailsGame.GameSettings.WithRumbble)
            {
                Stage.CurrentStage.Rumble.AddEffect(RUMBLE_SHAKE_TIME, 1, 1);
            }

            if (!this.Exploded)
            {
                _soundToPlay = this.GetSoundToPlay();
                _soundToPlay.Play();
            }
            this.Exploded = true;
        }

        /// <summary> 
        /// 
        /// </summary>
        private void CheckCollisionWithTiles()
        {
            // Check colisions between the tiles and the explosion
            // First step, determine the tiles that are inside the explosion tile collision BB
            TileCellCoords ulCoords = Stage.CurrentStage.Board.GetTileCellCoordsAt(this._tilesCollisionBB.UpperLeft);
            TileCellCoords lrCoords = Stage.CurrentStage.Board.GetTileCellCoordsAt(this._tilesCollisionBB.LowerRight);

            if (ulCoords.ColIndex < 0) ulCoords.ColIndex = 0;
            if (ulCoords.RowIndex < 0) ulCoords.RowIndex = 0;

            // Next, loop thru all those tiles and do a second test to determine if the tiles collide with the BS
            for (int col = ulCoords.ColIndex; (col <= lrCoords.ColIndex && col < Stage.CurrentStage.Board.Columns); col++)
            {
                for (int row = ulCoords.RowIndex; (row <= lrCoords.RowIndex && row < Stage.CurrentStage.Board.Rows); row++)
                {
                    if (Stage.CurrentStage.Board.Tiles[row, col] != null &&
                        Stage.CurrentStage.Board.Tiles[row, col].Tile != null &&
                        (Stage.CurrentStage.Board.Tiles[row, col].Tile.IsBreakable ||
                         this.DestroyUnbreakbleTiles))
                    {
                        // Compute tile BB
                        BoundingSquare bb = new BoundingSquare(new Vector2(col * Stage.TILE_WIDTH, row * Stage.TILE_HEIGHT),
                                                                                    Stage.TILE_WIDTH, Stage.TILE_HEIGHT);
                        if (this.RadiusType == ExplosionRadiusType.Circle)
                        {
                            // Check collision with BS
                            if (this._tilesCollisionBS.Collides(bb))
                            {
                                if (this.ThrowParticles)
                                {
                                    Stage.CurrentStage.Board.Tiles[row, col].Break();
                                }
                                Stage.CurrentStage.Board.RemoveTileAt(col, row);
                             }
                        }
                        else
                        {
                            // Check collision with BS
                            if (this._tilesCollisionBB.Collides(bb))
                            {
                                if (this.ThrowParticles)
                                {
                                    Stage.CurrentStage.Board.Tiles[row, col].Break();
                                }
                                Stage.CurrentStage.Board.RemoveTileAt(col, row);
                             }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
            switch(this._explosionStatus)
            {
                case StatusType.Exploding:
                    this._explosionStatus = StatusType.Smoking;
                    this.CurrentFrame = 0;
                    this.Sprite = this._smokeSprite;
                    break;
                case StatusType.Smoking:
                    this.DisposeFromStage();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetUnderLiquid(bool underLiquid)
        {
            this.DrawInForeground = !underLiquid;
        }

#if DEBUG
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                if (this.RadiusType == ExplosionRadiusType.Circle)
                {
                    this._tilesCollisionBS.Draw(Color.Purple, Stage.CurrentStage.Camera.Position);
                    this._objCollisionBS.Draw(Color.Purple, Stage.CurrentStage.Camera.Position);
                }
                else
                {
                    this._tilesCollisionBB.Draw(Color.Purple, Stage.CurrentStage.Camera.Position);
                    this._objCollisionBB.Draw(Color.Purple, Stage.CurrentStage.Camera.Position);
                }
            }
        }
#endif
    }

  }