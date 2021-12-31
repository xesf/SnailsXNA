using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// The laser beam, traces a line until it reaches a tile (a path) or a laser beam mirror
    /// Laser beams kill snails or can activate laser beam switches
    /// The beam can be activated/deactivated with switches and it can blink
    /// </summary>
    public class LaserBeam : MovingObject
    {
        #region Consts
        private const int BEAM_THICKNESS = 5;
        public const string LASER_BEAM_ID = "LASER_BEAM";
        #endregion

        public enum LaserBeamColor
        {
            Cyan,
            Magenta,
            Green
        }

        #region vars
        private OOBoundingBox _beamBs;
        private Rectangle _beamBs2;
        private BoundingSquare _quadtreeBeamBs;
        private Vector2 _beamOrigin;
        private Vector2 _beamEndPoint;
        private float _beamLength;
        private LaserBeam _nextBeam;
        private LaserBeamMirror _reflectingMirror;
        private LaserCannonBase _beamSource;
        private Sprite _laserSprite;
        private float _beamRotation;
        private ColorEffect _beamColorEffect;
        #endregion

        #region Properties
        public Vector2 BeamOrigin { get { return this._beamOrigin; } }
        public Vector2 BeamEndPoint { get { return this._beamEndPoint; } }
        public LaserBeamMirror ReflectingMirror { get { return this._reflectingMirror; } }
        public LaserBeam NextBeam { get { return this._nextBeam; } }
        public LaserBeamColor LaserColor { get; set;}

        public float BeamRotation
        {
            get { return this._beamRotation; }
            set
            {
                this._beamRotation = value;
                if (this._beamRotation > 180f)
                {
                    this._beamRotation -= 360f;
                }
                else
                    if (this._beamRotation < -180f)
                    {
                        this._beamRotation += 360f;
                    }
            }

        }
        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this._quadtreeBeamBs;
            }
        }

        public bool Visible
        {
            get { return this.IsVisible; }
            set
            {
                if (value != this.Visible)
                {
                    this.VisibilityChanged = true;
                }
                if (this.IsVisible == value)
                {
                    return;
                }
                if (value)
                {
                    this.DynamicFlags |= StageObjectDynamicFlags.IsVisible;
                }
                else
                {
                    this.DynamicFlags &= ~StageObjectDynamicFlags.IsVisible;
                    if (this._nextBeam != null)
                    {
                        this._nextBeam.Visible = false;
                    }
                }
            }
        }

        private bool VisibilityChanged { get; set; }
        #endregion

        public LaserBeam()
            : base(StageObjectType.LaserBeam)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._laserSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "Laser");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._beamEndPoint = this._beamOrigin = Vector2.Zero;
            this._beamColorEffect = new ColorEffect(Color.White, LaserBeam.GetXnaColor(this.LaserColor), 0.1f, true);
            this._beamColorEffect.Active = false;
            this.ComputeBeam();
        }

        /// <summary>
        /// 
        /// </summary>
        public static LaserBeam Create(LaserCannonBase laserSource)
        {
            LaserBeam beam = (LaserBeam)Stage.CurrentStage.StageData.GetObjectNoInitialize(LaserBeam.LASER_BEAM_ID);
            beam._beamSource = laserSource;
            beam.LaserColor = laserSource.LaserColor;
            beam.LoadContent();
            beam.Initialize();
            return beam;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            Stage.CurrentStage.AddObjectInRuntime(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Hide()
        {
            base.Hide();
            if (this._nextBeam != null)
            {
                this._nextBeam.Hide();
            }
        }

        /// <summary>
        /// Determine the size of the beam
        /// The beam with stretches until it hits a path or a laser beam mirror
        /// </summary>
        private void ComputeBeam()
        {
            if (this._nextBeam != null)
            {
                this._nextBeam.Hide();
            }
            Vector2 origin = this.Position;
            Vector2 endPoint = new Vector2(0, -999999);
            endPoint = origin + Mathematics.RotateVector(endPoint, this.BeamRotation);
            Vector2 p1;
            if (Stage.CurrentStage.Board.BoundingBox.IntersectsLine(origin, endPoint, out p1))
            {
                endPoint = p1;
            }

            this._beamEndPoint = endPoint;
            this._beamOrigin = origin;
            this._beamLength = (this._beamEndPoint - this._beamOrigin).Length();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeBeamBB()
        {
            Vector2 origin = Vector2.Zero;
            BoundingSquare bs = new BoundingSquare(origin, BEAM_THICKNESS, -this._beamLength);
            this._beamBs2 = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)BEAM_THICKNESS, (int)this._beamLength);

            this._beamBs = this.TransformBoundingBox(bs);
            this._quadtreeBeamBs = this.TransformBoundingBox(bs).ToBoundingSquare();

            this.RepositionObjectInQuadtree();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
            switch (listIdx)
            {
                case Stage.QUADTREE_SNAIL_LIST_IDX:
                    Snail snail = (Snail)obj;
                    if (snail.CheckCollisionWithLaserBeam(this))
                    {
                        snail.KillByLaser();
                    }
                    break;

                case Stage.QUADTREE_STAGEOBJ_LIST_IDX:
                    if (obj is LaserBeamMirror)
                    {
                        this.CollidedWithMirror((LaserBeamMirror) obj);
                    }
                    else
                    if (obj is LaserBeamSwitch)
                    {
                        this.CollidedWithSwitch((LaserBeamSwitch) obj);
                    }
                    
                    break;

                case Stage.QUADTREE_PATH_LIST_IDX:
                    BoardPathNode node = (BoardPathNode)obj;
                    Vector2 v;
                    if (Mathematics.LineLineIntersection(this._beamOrigin, this._beamEndPoint,
                                                         node.Value.P0, node.Value.P1, out v))
                    {
                   /*     this._beamEndPoint = v;
                        this._beamLength = (this._beamEndPoint - this._beamOrigin).Length();*/
                        this.CropBeam(v);
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CropBeam(Vector2 cropPoint)
        {
            this._beamEndPoint = cropPoint;
            this._beamLength = (this._beamEndPoint - this._beamOrigin).Length();
        }


        /// <summary>
        /// 
        /// </summary>
        private void CollidedWithMirror(LaserBeamMirror mirror)
        {
            
            Vector2 collidingPoint;
            if (!mirror.CollidesWithBeam(this, out collidingPoint) ||
                this._reflectingMirror == mirror) // This avoids colision with the laser beam start point and the mirror
            {
                return;
            }
            
            Vector2 reflectorCollidingPoint;
            //bool collided = mirror.BeamCollidesWithReflector(this, out reflectorCollidingPoint);
            if (mirror.BeamCollidesWithReflector(this, out reflectorCollidingPoint) == false)
            {
                this.CropBeam(collidingPoint);
                return;
            }
            this.CropBeam(reflectorCollidingPoint);
 
            // Ignore double mirror collisions to avoid infinite loops
            if (this._beamSource.CountBeamMirrorCollisions(mirror) > 0)
            {
                return;
            }

            // Make mirror rotation 0-180, because if we go above 180, the beam will reflect above the mirror
            float mirrorRotation = mirror.MirrorAbsoluteRotation < 0 ? mirror.MirrorAbsoluteRotation + 180 : mirror.MirrorAbsoluteRotation;
            // Ignore if mirror and beam are parellel (with a threshold -10 to 10 angle)
            if (Math.Abs(mirrorRotation - this.BeamRotation) > 10f)
            {

                if (this._nextBeam == null)
                {
                    this._nextBeam = LaserBeam.Create(this._beamSource);
                    this._nextBeam.Activate();
                }

                this._nextBeam.BeamRotation = -this.BeamRotation + (mirrorRotation * 2);
                this._nextBeam._reflectingMirror = mirror;
                this._nextBeam.Visible = true;
                this._nextBeam.Position = reflectorCollidingPoint;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CollidedWithSwitch(LaserBeamSwitch laserBeamSwitch)
        {
            Vector2 collidingPoint;
            if (laserBeamSwitch.OnLaserBeamCollided(this, out collidingPoint))
            {
                this.CropBeam(collidingPoint);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
   
            if (this.IsVisible == false)
            {
                return;
            }

            this.Rotation = this.BeamRotation;

            this.ComputeBeam();
            this.ComputeBeamBB();

            this.DoQuadtreeCollisions(Stage.QUADTREE_PATH_LIST_IDX);
            this.DoQuadtreeCollisions(Stage.QUADTREE_STAGEOBJ_LIST_IDX);
            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);

            this.ComputeBeamBB();

            if (this.IsVisible)
            {
                this._beamColorEffect.Update(gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
#if DEBUG    
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._beamBs.Draw(Color.LightGreen, Stage.CurrentStage.Camera.Position);
            }
#endif

            if (this.IsVisible)
            {
                this._laserSprite.Draw(this.Position, 0, this._beamBs2, _beamColorEffect.Color, this.BeamRotation + 180f, Stage.CurrentStage.SpriteBatch);
            }
        }

        /// <summary>
        /// Converts laser color to XNA color
        /// Statoic method to be used by LaserCannon, LaserBeam and LaserSwitch
        /// </summary>
        public static Color GetXnaColor(LaserBeamColor laserColor)
        {
            switch (laserColor)
            {
                case LaserBeam.LaserBeamColor.Cyan:
                    return new Color(26, 201, 255);
                case LaserBeam.LaserBeamColor.Green:
                    return new Color(95, 255, 30);
                case LaserBeam.LaserBeamColor.Magenta:
                    return Color.Magenta;
            }
            return Color.White;
        }
    }
}
