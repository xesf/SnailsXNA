using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    public enum StageObjectType
    {
        None = 0,
        Snail = 1,
        StageEntrance = 2,
        StageExit = 3,
        Apple = 4,
        Vitamin = 5,
        SnailCounter = 6,
        Dynamite = 7,
        Box = 8,
        Copper = 9,
        CollisionTester = 10,
        WalkTester = 11,
        Explosion = 12,
        Trampoline = 13,
        Spikes = 14,
        TriggerSwitch = 15, 
        TeleportEntrance = 16,  // TODO
        TeleportExit = 17,      // TODO
		PickableObject = 18,
        Prop = 19,
        Fire = 20,
        Salt = 21,
        DynamiteBox = 22,
        DynamiteBoxTriggered = 23,
        SnailSacrifice = 24,
        SnailKing = 25,
        PopUpBox = 26,
        Lamp = 27,
        FlameLight = 28,
        Water = 29,
        Crystal = 30,
        StageProp = 31,
        InformationSign = 32,
        SnailShell = 33,
        FadeInOutBox = 34,
        C4 = 35,
        LaserBeam = 36,
        LaserBeamMirror = 37,
        ControllableLaserCannon = 38,
        LaserBeamSwitch = 39,
        Acid = 40,
        Lava = 41,
        WaterBubble = 42,
        LiquidPump = 43,
        LiquidPipe = 44,
        LiquidTap = 45,
        EvilSnail = 46,
        FixedLaserCannon = 47,
        DirectionalBox = 48,
        TutorialSign = 49,
        DynamiteBoxCounted = 50,
        Slime = 51,
        Last = Slime,
    }

    [Flags]
    public enum StageObjectStaticFlags
    {
        None = 0x00,
        CanFall = 0x01,
        CanCollide = 0x02,
        CanHoover = 0x04,
        CanWalk = 0x08,
        CanWalkOnWalls = 0x10,
        CanDieWithExplosions = 0x20,
        CanDie = 0x40,
        CanDieWithFire = 0x80,
        CanDieWithAnyTypeOfExplosion = 0x100, // Explosions may affect, snails, objects or tails, if object has this flag it will die with any explosion
        CanDieWithCrates = 0x400,
        IgnoreLiquidCollisions = 0x800 
    }

    [Flags]
    public enum StageObjectDynamicFlags
    {
        None = 0x00,
        IsVisible = 0x01,
        IsVitaminized = 0x02,
        IsFalling = 0x04,
        IsDead = 0x08,
        IsDisposed = 0x10,
        IsEating = 0x20,
        IsUnderLiquid = 0x40
    }

    public partial class StageObject : Object2D, ISnailsDataFileSerializable, IQuadtreeContainable
    {
        #region Constants
        private Color DEFAULT_COLOR = Color.White;

        // Effects ID in the effect blender
        public const int TRANSF_EFFECT_GRAVITY = 2;
        public const int TRANSF_EFFECT_HOOVER = 1; // Hoover effect when objects are on the ground (applies when CanHoover=true)
        public const int TRANSF_EFFECT_LIQUID_GRAVITY = 3;
        public const int TRANSF_EFFECT_ROTATION = 4;
        
        public const float OBJ_HOOVER_SPEED = 0.06f;
        public const float OBJ_HOOVER_POWER = 0.3f;
        public const string SPRITE_DEATH_BY_FIRE = "FireDeath";
        public const string SPRITE_DEATH_BY_SPIKES = "SpikesDeath";
        #endregion

        #region Members
        private StageObjectType _type;
        private bool _canAnimate;
        protected List<StageObject> _linkedObjects;
        protected string _linkString;
		public bool DrawInForeground;
        public StageObjectStaticFlags StaticFlags;
		public StageObjectDynamicFlags DynamicFlags;
		public bool IsStageObject;
		public Vector2 PreviousPosition;
		protected bool Killed;
        protected Matrix _rotationZMatrix = Matrix.Identity;
        private bool _faddingOut;
        private float _fadeAlpha;
        private float _fadeSpeed;
        private Color _shadowColor;
        //private Dictionary<string, SpriteData> _sprites;
        protected Liquid _inLiquidRef; // Reference to the liquid where the object currently is
        protected BoundingSquare _crateCollisionBB; // For tests between spikes and crates
        private int _crateCollisionBBIdx;

        private string _deathFireSpriteRes;
        private Sprite _deathFireSprite;
        protected bool _projectWhenKilledByCrate;
        private bool _preLoad;
        protected Sample _killedByFire;
        protected bool _autoDisposeIfDeadOnAnimationEnd; // Big name, I'm mad with an annoying bug
        public string _contentManagerId;
        #endregion

        #region Properties
        public bool PreLoad { get { return this._preLoad; } }
        protected Color ShadowColor
        {
            get
            {
                if (this._colorChanged)
                {
                    // This takes into account the alpha of the color
                    // The more transparent the object weaker the shadow
                    Vector4 vShadow = Stage.CurrentStage.ShadowColor.ToVector4();
                    Vector4 vColor = this.BlendColor.ToVector4();
                    this._shadowColor = new Color(new Vector4(vShadow.X, vShadow.Y, vShadow.Z, vShadow.W * vColor.W));
                }
                return this._shadowColor;
            }

        }

        public virtual BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this.AABoundingBox;
            }
        }

        public StageObjectType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                if (this._rotation != value)
                {
                  base.Rotation = value;
                  this._rotationZMatrix = Matrix.CreateRotationZ(this._rotationInRad);
                }
            }
        }

        public bool IsSnail
        {
            get
            {
                return (this.Type == StageObjectType.Snail ||
                        this.Type == StageObjectType.SnailKing ||
                        this.Type == StageObjectType.EvilSnail);
            }
        }

        public bool UseQuadtree { get { return this.CanCollide; } }

        // Properties for StaticFlags . This could be acomplished with a single method call, but for code readability and speed
        // each flag has it's own method. Another advantage is that we can easely change the way this props are calculated whithout breaking the code
        public bool CanFall { get { return ((this.StaticFlags & StageObjectStaticFlags.CanFall) == StageObjectStaticFlags.CanFall); } }
        public bool CanCollide { get { return ((this.StaticFlags & StageObjectStaticFlags.CanCollide) == StageObjectStaticFlags.CanCollide); } }
        public bool CanHoover { get { return ((this.StaticFlags & StageObjectStaticFlags.CanHoover) == StageObjectStaticFlags.CanHoover); } }
        public bool CanWalk { get { return ((this.StaticFlags & StageObjectStaticFlags.CanWalk) == StageObjectStaticFlags.CanWalk); } }
        public bool CanWalkOnWalls { get { return ((this.StaticFlags & StageObjectStaticFlags.CanWalkOnWalls) == StageObjectStaticFlags.CanWalkOnWalls); } }
        public bool CanDieWithExplosions { get { return ((this.StaticFlags & StageObjectStaticFlags.CanDieWithExplosions) == StageObjectStaticFlags.CanDieWithExplosions); } }
        public bool CanDie { get { return ((this.StaticFlags & StageObjectStaticFlags.CanDie) == StageObjectStaticFlags.CanDie); } }
        public bool CanDieWithFire { get { return ((this.StaticFlags & StageObjectStaticFlags.CanDieWithFire) == StageObjectStaticFlags.CanDieWithFire); } }
        public bool CanDieWithAnyTypeOfExplosion { get { return ((this.StaticFlags & StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion) == StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion); } }
        public bool CanDieWithCrates { get { return ((this.StaticFlags & StageObjectStaticFlags.CanDieWithCrates) == StageObjectStaticFlags.CanDieWithCrates); } }
        public bool IgnoreLiquidCollisions { get { return ((this.StaticFlags & StageObjectStaticFlags.IgnoreLiquidCollisions) == StageObjectStaticFlags.IgnoreLiquidCollisions); } }

        // Properties for StaticFlags. The same comment stated previous applies here too
        public bool IsVisible { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsVisible) == StageObjectDynamicFlags.IsVisible); } }
        public bool IsVitaminized { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsVitaminized) == StageObjectDynamicFlags.IsVitaminized); } }
        public bool IsFalling { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsFalling) == StageObjectDynamicFlags.IsFalling); } }
        public bool IsDead { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsDead) == StageObjectDynamicFlags.IsDead); } }
        public bool IsDisposed { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsDisposed) == StageObjectDynamicFlags.IsDisposed); } }
        public bool IsUnderLiquid { get { return ((this.DynamicFlags & StageObjectDynamicFlags.IsUnderLiquid) == StageObjectDynamicFlags.IsUnderLiquid); } }
        public bool IsUnderWater { get { return (this._inLiquidRef is Water); } }


        public bool IgnorePathCollisions { get; protected set; }

        public override BoundingSquare SoundEmmiterBoundingBox
        {
            get { return this.QuadtreeCollisionBB; }
        }
        #endregion

        public StageObject()
        {
            StaticFlags = StageObjectStaticFlags.None;
            DynamicFlags = StageObjectDynamicFlags.IsVisible;
            this.SpriteDrawOffset = new Vector2();
            this.BlendColor = DEFAULT_COLOR;
            this._shadowColor = Colors.IngameShadows;
            this._crateCollisionBBIdx = -1;
            this._autoDisposeIfDeadOnAnimationEnd = true;
            this._contentManagerId = ResourceManager.RES_MANAGER_ID_TEMPORARY;
//            this._sprites = new Dictionary<string, SpriteData>();
        }

        public StageObject(StageObjectType type) :
            this()
        {
            _type = type;
            _linkedObjects = new List<StageObject>();

            _canAnimate = true;
        }

        public StageObject(StageObject other)
            : base(other)
        {
            Copy(other);
        }

        public virtual void Copy(StageObject other)
        {
            base.Copy(other);

            this._type = other._type;
            this.StaticFlags = other.StaticFlags;
            this.DynamicFlags = other.DynamicFlags;
			this.DrawInForeground = other.DrawInForeground;
            this.IgnorePathCollisions = other.IgnorePathCollisions;
            this._canAnimate = other._canAnimate;
            this._faddingOut = other._faddingOut;
            this.BlendColor = other.BlendColor;
            this._crateCollisionBBIdx = other._crateCollisionBBIdx;
            this._deathFireSpriteRes = other._deathFireSpriteRes;
            this._projectWhenKilledByCrate = other._projectWhenKilledByCrate;
            this._contentManagerId = other._contentManagerId;
            this._linkedObjects = new List<StageObject>();
            foreach (StageObject obj in this._linkedObjects)
            {
                this._linkedObjects.Add(new StageObject(obj));
            }

//            this._sprites = other._sprites;
        }

        public virtual StageObject Clone()
        {
            StageObject obj;

            obj = StageObjectFactory.Create(this.Type);
            obj.Copy(this);

            return obj;
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadContent()
        {
            if (this.SpriteId != null && this.ResourceId != null)
            {
                this.Sprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + this.SpriteId, this._contentManagerId);
            }
            if (this.CanDieWithFire)
            {
                this._deathFireSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._deathFireSpriteRes);
            }
            this._killedByFire = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.OBJECT_KILLED_FIRE, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {
            this._faddingOut = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void HintInitialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void StageStartupPhaseEnded()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCrateCollisionBoundingBox()
        {
            if (this._crateCollisionBBIdx != -1)
            {
                this._crateCollisionBB = this.TransformSpriteFrameBB(this._crateCollisionBBIdx).ToBoundingSquare();
            }
        }

        /// <summary>
        /// Stage initialize is only called when a object is initialized on stage loading
        /// This is usefull in objects that need to add paths to the stage after loading (Triggered boxes for instance)
        /// </summary>
        public virtual void StageInitialize()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void AfterBoardInitialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void RepositionObjectInQuadtree()
        {
            if (this.Quadtree != null)
            {
                Stage.CurrentStage.Board.RepositionObjectInQuadtree(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Hide()
        {
            this.DynamicFlags  &= ~StageObjectDynamicFlags.IsVisible;
            this.SpriteAnimationActive = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Show()
        {
            this.DynamicFlags |= StageObjectDynamicFlags.IsVisible;
            this.SpriteAnimationActive = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (!this.IsVisible)
            {
                return;
            }
            // Fade out effect
            if (this._faddingOut)
            {
                float factor = (this._fadeSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                this._fadeAlpha -= factor;
                if (this._fadeAlpha < 0)
                {
                    this.DisposeFromStage();
                    return;
                }
                this.BlendColor = new Color(this._fadeAlpha, this._fadeAlpha, this._fadeAlpha, this._fadeAlpha);
            }

            if (this.Killed) // This is a major martelada. This was supposed to be a temporary fix, but the truth is because I didn't commented this crap
							 // Now I don't remember why I had to do this. Anyways, use this.Kill() to kill objects
            {
                this.DynamicFlags = StageObjectDynamicFlags.IsVisible | StageObjectDynamicFlags.IsDead;
                this.EffectsBlender.Clear();
                this.EffectsBlender.Add(new GravityEffect(SnailsGame.GameSettings.Gravity, 0f));
                this.Killed = false;
                return;
            }

            // I think this could be moved to MovingObject, but better not at least for now
            if (this.Quadtree != null)
            {
                // Check if the object is out of the board bounds
                if (this.Quadtree.IsObjectInBounds(this) == false)
                {
                    if (this is Snail && !this.IsDead)
                    {
                        SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage++;
                    }
                    // Object is out of bounds, remove it from game
                    Stage.CurrentStage.RemoveObject(this);
                }
            }
            else // Objects may not be in the quadtree (in the case when a snail dies and falls for instance)
            {
                if (Stage.CurrentStage.Board.IsObjectInBounds(this) == false)
                {
                    // Only remove objects that are to the bottom of the board
                    // this will allow objects to get out of the board on the top, and then come back down
                    // Object is out of bounds, remove it from game
                    if (this.AABoundingBox.Top > Stage.CurrentStage.Board.BoundingBox.Bottom)
                    {
                        if (this is Snail && !this.IsDead && !((Snail)this).IsEvilSnail)
                        {
                            SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage++;
                        }
                        Stage.CurrentStage.RemoveObject(this);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnStageRemoved()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void AfterUpdate(BrainGameTime gameTime)
        {
            
            if (this.IsDead == false)
            {
                if (this.BoundingBoxChanged && this.IsDisposed == false)
                {
                    this.RepositionObjectInQuadtree();
                }

                if (this.IsUnderLiquid &&
                    !this.CheckCollisionWithLiquid(this._inLiquidRef))
                {
                    this.OnExitLiquid();
                }
            }
            else
            {
                if (this.Quadtree != null)
                {
                    this.Quadtree.RemoveObject(this);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
        }

        /// <summary>
        /// This was commented and was uncommented again
        /// This is because apples and others were not being removed after a kill fy fire animation
        /// I know there's a situation when this cannot be done, I just don't know which it is...
        /// If that ug rises again, at least now there's a comment
        /// </summary>
        public override void OnLastFrame()
        {
            // If animation ended and the object is dead, probably the animation is a death animation
            // Just dispose the object from the stage
            if (this.IsDead && this._autoDisposeIfDeadOnAnimationEnd)
            {
                this.DisposeFromStage();
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual void Kill()
        {
            if (CanDie)
            {
                this.Killed = true;

                if (this.IsSnail)
                {
                    this.MoveToForeground();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void KillByTouchingDeadlyLiquid()
        {
            this.KillByFire();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void KillByExplosion(Explosion exp)
        {
            this.KillMe();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void KillByFire()
        {

            this._killedByFire.Play();
            this.SpriteAnimationActive = true;
            this.Sprite = this._deathFireSprite;
            this.CurrentFrame = 0;
            this.UpdateBoundingBox();
            this.KillMe();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void KillMe()
        {
            if (this.CanDie)
            {
                this.StaticFlags = StageObjectStaticFlags.None;
                this.DynamicFlags = StageObjectDynamicFlags.IsDead | StageObjectDynamicFlags.IsVisible;
                this.EffectsBlender.DisableAll();
                this.MoveToForeground();
            }
        }


        /// <summary>
        /// Explode o objecto.
        /// É adicionado um objecto explosão ao stage e o objecto é removido do stage
        /// If null, default explosion sprite will be used (usefull for boxes implosion that have a diferent sprite)
        /// </summary>
        public Explosion Explode(Explosion.ExplosionSize tileKillBBsize, 
                            Explosion.ExplosionSize objKillBBSize, 
                            Explosion.ExplosionRadiusType radiusType, 
                            Explosion.ObjectTypeAffected objectsAffected,
                            Vector2 explosionCenter,
                            bool destroyUnbreakbleTiles,
                            Sprite explosionSprite,
                            Sprite explosionEndSprite,
                            bool dispose,
                            bool isUnderLiquid) 
        {
            // Add explosion object
            Explosion expObj = (Explosion)Stage.CurrentStage.StageData.GetObject(Explosion.ID);
            expObj.Position = explosionCenter;
            expObj.TileKillBBSize = tileKillBBsize;
            expObj.ObjectKillBBSize = objKillBBSize;
            expObj.RadiusType = radiusType;
            expObj.AffectedObjects = objectsAffected;
            expObj.DestroyUnbreakbleTiles = destroyUnbreakbleTiles;
            if (explosionEndSprite != null)
            {
                expObj.SmokeSprite = explosionEndSprite;
            }
            expObj.ThrowParticles = (!this.IsUnderLiquid);
            expObj.SetUnderLiquid(isUnderLiquid);
            expObj.ExplosionSource = this;

            if (explosionSprite != null)
            {
                expObj.Sprite = explosionSprite;
            }
            Stage.CurrentStage.AddObjectInRuntime(expObj);
            // Remove self
            if (dispose)
            {
                Stage.CurrentStage.DisposeObject(this);
            }
            return expObj;
        }

        /// <summary>
        /// Move o objecto para a lista de objectos a desenhar no foreground. Os objectos foreground são desenhados sobre os tiles
        /// </summary>
        protected void MoveToForeground()
        {
            // Move object to the foreground layer
            this.DrawInForeground = true;
            Stage.CurrentStage.BackgroundObjectsDrawList.Remove(this);
            Stage.CurrentStage.ForegroundObjectsDrawList.Add(this); 
        }

        /// <summary>
        /// 
        /// </summary>
        protected void MoveToBackground()
        {
            // Move object to the background layer
            this.DrawInForeground = false;
            Stage.CurrentStage.BackgroundObjectsDrawList.Add(this);
            Stage.CurrentStage.ForegroundObjectsDrawList.Remove(this);
        }

        /// <summary>
        /// Allows an object to Snap on board
        /// </summary>
        public void SnapIt()
        {
            this.BoardX = this.BoardX;
            this.BoardY = this.BoardY;
        }

       

        #region IBoardCoordinates Members

        public int BoardX
        {
            get { return (int)(X / Stage.CurrentStage.Board.TileWidth); }
            set { X = value * Stage.CurrentStage.Board.TileWidth; }
        }

        public int BoardY
        {
            get { return (int)(Y / Stage.CurrentStage.Board.TileHeight); }
            set { Y = value * Stage.CurrentStage.Board.TileHeight; }
        }

        #endregion

        /// <summary>
        /// Transforma o rect do frame corrente de this.Sprite para a posição do objecto
        /// </summary>
        public BoundingSquare GetCurrentFrameRectTransformed()
        {
            Microsoft.Xna.Framework.Rectangle curFrameRect = this.Sprite.Frames[this.CurrentFrame].Rect;
            Microsoft.Xna.Framework.Rectangle rc =
            new Microsoft.Xna.Framework.Rectangle
                                   ((int)(this.Position.X - this.Sprite.Offset.X),
                                    (int)(this.Position.Y - this.Sprite.Offset.Y),
                                     curFrameRect.Width,
                                     curFrameRect.Height);

            return new BoundingSquare(rc);
        }

        /// <summary>
        /// 
        /// </summary>
        public string BuildLinkString()
        {
            string link = "";
            for (int i = 0; i < this.LinkedObjects.Count; i++)
            {
                link += this.LinkedObjects[i].UniqueId;
                if (i + 1 != this.LinkedObjects.Count)
                    link += ";";
            }

            return link;
        }

 #if DEBUG&& !RETAIL        

        /// <summary>
        /// 
        /// </summary>
        protected void DrawQuadtreeNode()
        {
            if (this.DrawQuadtree && this.QuadtreeNodes != null)
            {
                foreach (QuadtreeNode node in this.QuadtreeNodes)
                {
                    node.DrawNode(Color.LightGreen, SnailsGame.DebugFont, Stage.CurrentStage.SpriteBatch);
                }
            }
        }
       /// <summary>
        /// 
        /// </summary>
        protected void DrawObjectId()
        {
            this.DrawObjectId(DEFAULT_COLOR);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawObjectId(Color color)
        {

            if (SnailsGame.GameSettings.ShowObjectIds)
            {
                if (this.UniqueId != null)
                {
                    SnailsGame.SpriteBatch.DrawString(SnailsGame.ObjectsIdFont,
                                this.UniqueId, this.Position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
                }
            }

        }
#endif
        /// <summary>
        /// What follows is hard to explain
        /// This is a specific draw sprite method to be used when drawing a sprite relative to a parent sprite
        /// A good is example is the crown in the snail king
        /// The theory:
        /// 1st) Place the child sprite in the position relative to the parent
        /// 2st) Take the vector that goes from the origin to the position computed in 1 and rotate it with the parent rotation value 
        /// 3st) The parent and child rotation are added and used in the spriteBatch call
        /// 
        /// The flipped works the same way, with the exception that the child offset and pivo have to be flipped
        /// 
        /// This method is too specific for the engine, I think it belongs in the snail project
        /// </summary>
        public void DrawParentChild(bool shadow, Sprite parentSprite, Sprite childSprite)
        {
            int childFrameNr = this.CurrentFrame;
            if (childFrameNr >= childSprite.Frames.Length)
            {
                childFrameNr = childSprite.Frames.Length-1;
            }
            if ((this.SpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
            {

                Frame frame = childSprite.Frames[childFrameNr];
                Frame parentFrame = parentSprite.Frames[this.CurrentFrame];

                // This have to be fliped because are drawing with horizontal flip
                // This could be all precomputed when the current frame changed - optimization possible here
                float rotationRadInverted = this._rotationInRad + ((MathHelper.Pi * 2) - frame._rotationInRads);
                Vector2 offsetInverted = new Vector2(parentSprite.Frames[this.CurrentFrame].Rect.Width - frame._offset.X, frame._offset.Y);
                Vector2 parentOffsetInverted = new Vector2(parentFrame.Rect.Width - parentSprite.Offset.X, parentSprite.Offset.Y);
                Vector2 pos = offsetInverted - parentOffsetInverted;

                pos = this.Position + Vector2.Transform(pos, this._rotationZMatrix);
                pos = new Vector2((int)pos.X, (int)pos.Y); // Half pixels make the texture blur, so remove them
                Color color = DEFAULT_COLOR;
                if (shadow)
                {
                    pos += GenericConsts.ShadowDepth;
                    color = this.ShadowColor;
                }
                this.Sprite.Draw(pos, frame.Rect, rotationRadInverted, this.SpriteEffect, frame._pivotHorizFlipped, color, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                // This could be all precomputed when the current frame changed - optimization possible here
                Frame frame = childSprite.Frames[childFrameNr];
                float rotationRad = this._rotationInRad + frame._rotationInRads;
                // Compute the vector from the origin to the child position
                Vector2 pos = frame._offset - parentSprite.Offset;
                // Rotate the vector and add the parent position
                pos = this.Position + Vector2.Transform(pos, this._rotationZMatrix);
                pos = new Vector2((int)pos.X, (int)pos.Y); // Half pixels make the texture blur, so remove them
                Color color = DEFAULT_COLOR;
                if (shadow)
                {
                    pos += GenericConsts.ShadowDepth;
                    color = this.ShadowColor;
                }
                this.Sprite.Draw(pos, frame.Rect, rotationRad, this.SpriteEffect, frame._pivot, color, Stage.CurrentStage.SpriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw(bool shadow)
        {
            if (!this.IsVisible)
            {
                return;
            }
            if (!shadow)
            {
                base.Draw(Stage.CurrentStage.SpriteBatch, this.Position, this.BlendColor);
            }
            else
            {
                base.Draw(Stage.CurrentStage.SpriteBatch, this._position + GenericConsts.ShadowDepth, this.ShadowColor);
            }

#if DEBUG
            if (BrainGame.Settings.ShowBoundingBoxes)
            {
                if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox) // By frame bb
                {
                    // Multiple BB are not updated automatically, so we have to do that here. Only do this for debug purposes
                    for (int i = 0; i < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length; i++)
                    {
                        BoundingSquare bs = this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[i];
                        this.TransformBoundingBox(bs).ToBoundingSquare().Draw(Color.Red, SnailsGame.Instance.ActiveCamera.Position);
                    }
                }
               
                // Sprite bbs
				if (this.Sprite.BoundingBoxes.Length > 0)
				{
					// Multiple BB are not update automatically, so we have to that here. Only do this for debug purposes
					for(int i = 0; i <this.Sprite.BoundingBoxes.Length; i++)
					{
                        this.GetBoundingBoxTransformed(i).ToBoundingSquare().Draw(Color.Red, SnailsGame.Instance.ActiveCamera.Position);
					}
				}
					

                // Boundoing circle
                if (this.Sprite._boundingSpheres != null)
                {
                    for (int i = 0; i < this.Sprite._boundingSpheres.Length; i++)
                    {

                        this.TransformBoundingCircle(this.Sprite._boundingSpheres[i]).Draw(Color.Red, SnailsGame.Instance.ActiveCamera.Position);
                    }
                }

                // Invalid crate area
                if (this._crateCollisionBBIdx != -1)
                {
                    this._crateCollisionBB.Draw(Color.Blue, SnailsGame.Instance.ActiveCamera.Position);
                }
            }

            if (BrainGame.Settings.ShowSpriteFrame)
            {
                // Sprite frame
                Rectangle rc = this.Sprite.Frames[this.CurrentFrame].Rect;
                BoundingSquare bs1 = new BoundingSquare(-this.Sprite.Offset, rc.Width, rc.Height);
                bs1 = this.TransformBoundingBox(bs1).ToBoundingSquare();
                bs1.Draw(Color.Orange, SnailsGame.Instance.ActiveCamera.Position);               
            }
#endif

#if DEBUG && !RETAIL
            this.DrawObjectId();
            this.DrawQuadtreeNode();
#endif
        }

        /// <summary>
        /// Method called on objects that need to draw on top of tiles
        /// </summary>
        public virtual void ForegroundDraw()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void HintDraw(Color color)
        {
        }

        /// <summary>
        /// Remove o objecto do stage
        /// </summary>
        public virtual void DisposeFromStage()
        {
            Stage.CurrentStage.DisposeObject(this);
        }

        /// <summary>
        /// Objects recieve this notification when an explosion happens
        /// </summary>
        public virtual void OnExplosion(Explosion explosion)
        {
        }

        /// <summary>
        /// Occurs when the object enters the stage
        /// This event is not launched when objects enter the stage in load time
        /// The event only happens when objects enter the stage after gameplay has started
        public virtual void OnAddedToStage()
        {
        }

        /// <summary>
        /// Used to stop all samples from an object - For exapmle when we remove it from Stage, we must stop every playing object sample
        /// </summary>
        public virtual void StopSamples()
        { }

        /// <summary>
        /// Fades the object out, when the fade ends the object is disposed from stage
        /// </summary>
        public void FadeOut(float speed)
        {
            this._faddingOut = true;
            this._fadeSpeed = (speed / 1000f); // This allows the calls to FadeOut to not use a very small number
            this.BlendColor = DEFAULT_COLOR;
            this._fadeAlpha = 1f;
        }

        /// <summary>
        /// This event runs when the object enters a liquid for the first time
        /// </summary>
        public virtual void OnEnterLiquid(Liquid liquid)
        {
            this.DynamicFlags |= StageObjectDynamicFlags.IsUnderLiquid;
            this._inLiquidRef = liquid;
        }

        /// <summary>
        /// This event runs when the object enters a liquid for the first time
        /// </summary>
        public virtual void OnExitLiquid()
        {
            this.DynamicFlags &= ~StageObjectDynamicFlags.IsUnderLiquid;
            this._inLiquidRef = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LineCollides(Vector2 p0, Vector2 p1)
        {
            Vector2 v;
            return this.AABoundingBox.IntersectsLine(p0, p1, out v);
        }

        public bool CanAcceptCursorInteraction
        {
            get
            {
                return (Stage.CurrentStage.State == Stage.StageState.Playing);
            }
        }

        #region ICollidable Members


        /// <summary>
        /// 
        /// </summary>
        public bool Collides(StageObject obj)
        {
#if DEBUG
            SnailsGame.DebugInfo.CollisionCounter.Counter += 1.0f;
#endif
            return this.AABoundingBox.Collides(obj.AABoundingBox);
        }


        public bool Collides(Tile tile, Vector2 pos)
        {
            BoundingSquare bb = this.Sprite.FlipCollisionBox(this.SpriteEffect);
            OOBoundingBox BoundingBox = bb.Transform(0, pos + this.Sprite.Offset);

            Vector2 vInvPos = -this.Position;
            OOBoundingBox bbObjectInv = BoundingBox.Transform(vInvPos, 0);

            if (bbObjectInv.Collides(this.Sprite.BoundingBox))
            {
                return true;
            }

            vInvPos = -pos;
            bbObjectInv = this.BoundingBox.Transform(vInvPos, 0);

            if (bbObjectInv.Collides(tile.Sprite.BoundingBox))
            {
                return true;
            }

            return false;
        }

        public virtual void OnCollide(StageObject obj)
        {
        }

        #endregion

        #region IGameObjectLinkable Members
        public bool WithLinks { get { return (this.LinkedObjects.Count > 0); } }

        public string LinkString
        {
            get { return _linkString; }
            set { _linkString = value; }
        }

        public List<StageObject> LinkedObjects
        {
            get
            {
                return _linkedObjects;
            }
        }

        public virtual void AddLinkedObject(StageObject obj)
        {
            this._linkedObjects.Add(obj);
        }

        public virtual void SetLinkedObjects(List<StageObject> objList)
        {
            this._linkedObjects = objList;
        }

        public void RemoveLinkedObject(StageObject obj)
        {
            this._linkedObjects.Remove(obj);
        }
        public void ClearLinkedObjects()
        {
            this._linkedObjects.Clear();
        }
        public bool IsLinkedTo(StageObject obj)
        {
            return this._linkedObjects.Contains(obj);
        }
        public void LinkTo(StageObject obj)
        {
            this.ClearLinkedObjects();
            if (obj != null)
            {
                this.AddLinkedObject(obj);
            }
        }
        #endregion

        #region IDataFileSerializable Members

        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);

            string type = record.GetFieldValue<string>("type", null);
            if (!string.IsNullOrEmpty(type))
            {
                _type = (StageObjectType)Enum.Parse(typeof(StageObjectType), type, true);
            }

            this.UniqueId = record.GetFieldValue<string>("uid", this.UniqueId);
            this._preLoad = record.GetFieldValue<bool>("preLoad", false);
            this.IsStageObject = record.GetFieldValue<bool>("stageObject", this.IsStageObject);
            this._linkString = record.GetFieldValue<string>("linkTo", _linkString);
            this.DrawInForeground = record.GetFieldValue<bool>("drawInForeground", this.DrawInForeground);
            this.IgnorePathCollisions = record.GetFieldValue<bool>("ignorePathCollisions", this.IgnorePathCollisions);
            this._crateCollisionBBIdx = record.GetFieldValue<int>("crateCollisionBBIdx", this._crateCollisionBBIdx);
            FlagsType flags = record.GetFieldValue<FlagsType>("staticFlags", FlagsType.Zero);
            if (flags.Value != 0)
            {
                this.StaticFlags = (StageObjectStaticFlags)Enum.Parse(typeof(StageObjectStaticFlags), flags.ToString(), true);
            }

            flags = record.GetFieldValue<FlagsType>("dynamicFlags", FlagsType.Zero);
            if (flags.Value != 0)
            {
                this.DynamicFlags = (StageObjectDynamicFlags)Enum.Parse(typeof(StageObjectDynamicFlags), flags.ToString(), true);
            }
         
            float x = record.GetFieldValue<float>("posX", 0.0f);
            float y = record.GetFieldValue<float>("posY", 0.0f);
            this.Position = new Vector2(x, y);

            this.BlendColor = record.GetFieldValue<Color>("color", this.BlendColor);
            this._deathFireSpriteRes = record.GetFieldValue<string>("deathFireSpriteRes", SpriteResources.FIRE_DEATH_ANIMATION);
            this._projectWhenKilledByCrate = record.GetFieldValue<bool>("projectWhenKilledByCrate", true);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord();

            switch (context)
            {
                case ToDataFileRecordContext.StageDataSave:
                    record.AddField("canAnimate", this._canAnimate);
                    record.AddField("drawInForeground", this.DrawInForeground);
                    record.AddField("staticFlags", (int)this.StaticFlags);
                    record.AddField("dynamicFlags", (int)this.DynamicFlags);
                    record.AddField("stageObject", this.IsStageObject);
                    record.AddField("ignorePathCollisions", this.IgnorePathCollisions);
                    record.AddField("crateCollisionBBIdx", this._crateCollisionBBIdx);
                    record.AddField("deathFireSpriteRes", this._deathFireSpriteRes);
                    record.AddField("projectWhenKilledByCrate", this._projectWhenKilledByCrate);
                    record.AddField("preLoad", this._preLoad);
                    break;

                case ToDataFileRecordContext.StageSave:
                    record.RemoveField("res"); // This are not needed because this values will come from StageData
                    record.RemoveField("sprite");
                    record.RemoveField("frame");
                    break;
            }
            record.AddField("type", (int)this._type);
            record.AddField("uid", this.UniqueId);
            this._linkString = this.BuildLinkString();
            record.AddField("linkTo", this._linkString);
            record.AddField("posX", this.Position.X);
            record.AddField("posY", this.Position.Y);
            if (this.BlendColor != DEFAULT_COLOR)
            {
                record.AddField("color", this.BlendColor);
            }
            return record;
        }
        #endregion



        #region IQuadtreeContainable Members


        public Quadtree Quadtree { get; set; }
        public List<QuadtreeNode> QuadtreeNodes { get; set; }
        public int ObjectListIdx { get; set; }
        public bool ShouldTestCollisions { 
            get { 
                return (this.IsDead == false && this.IsVisible == true && this.CanCollide); 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsContained(QuadtreeNode node)
        {
            return node.BoundingBox.Contains(this.QuadtreeCollisionBB);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(IQuadtreeContainable obj)
        {
#if DEBUG
            SnailsGame.DebugInfo.CollisionCounter.Counter++;
#endif
            // First test: Test collisions with the sprite master BB (Animation BB)
            BoundingSquare bs = this.QuadtreeCollisionBB;

            if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
            {
                bs = this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBox).ToBoundingSquare();
            }
            return (obj.Collides(bs));      
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(BoundingSquare bb)
        {
            // First test: Test collisions with the sprite master BB (Animation BB)
            BoundingSquare bs = this.QuadtreeCollisionBB;

            if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
            {
                bs = this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBox).ToBoundingSquare();
            }

            return bs.Collides(bb);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool CheckCollisionWithLiquid(Liquid liquid)
        {
            return this.Collides(liquid.QuadtreeCollisionBB);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoQuadtreeCollisions(int listIdx)
        {
            if (this.Quadtree != null)
            {
                this.Quadtree.DoCollisions(this, listIdx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(Vector2 p)
        {
            return this.QuadtreeCollisionBB.Contains(p);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(Vector2 p0, Vector2 p1)
        {
            Vector2 v;
            return this.QuadtreeCollisionBB.IntersectsLine(p0, p1, out v);
        }
        /// <summary>
        /// Returns true if a crate (box) can be placed at the given position 
        /// This is used by the stage cursor 
        /// Objects may have specific bounding boxes for this puprpose thus the virtual method
        /// By default crates are valid
        /// </summary>
        public virtual bool CrateToolIsValid(BoundingSquare crateBs)
        {
            if (this._crateCollisionBBIdx != -1)
            {
                return !(crateBs.Collides(this._crateCollisionBB));
            }
            return true;
        }

        public string FormatStringToDumpFile(QuadtreeNode currentNode)
        {
            return string.Format("   obj [{0}] [{1}] pos[{2}  {3}] prev_pos[{4}  {5}] [Contained in the node: {6}] bb[{7} {8} {9} {10} , w: {11} h: {12}] se[{13}]",
                        this.UniqueId,
                        this.GetType().Name,
                        this.Position.X, this.Position.Y,
                        this.PreviousPosition.X, this.PreviousPosition.Y,
                        this.Collides(currentNode.BoundingBox),
                        this.QuadtreeCollisionBB.UpperLeft.X, this.QuadtreeCollisionBB.UpperLeft.Y,
                        this.QuadtreeCollisionBB.LowerRight.X, this.QuadtreeCollisionBB.LowerRight.Y,
                        this.QuadtreeCollisionBB.Width, this.QuadtreeCollisionBB.Height,
                        this.SpriteEffect.ToString());
        }

        #endregion

    }

}
