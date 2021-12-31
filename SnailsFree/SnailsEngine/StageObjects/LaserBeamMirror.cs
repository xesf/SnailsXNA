using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.StageObjects
{
    public class LaserBeamMirror : MovingObject, ISnailsDataFileSerializable, ICursorInteractable, IRotationControllable
    {
        #region Consts
        public const float MAX_ROTATION = 160;
        public const float MIN_ROTATION = 20;

        // Mirror sprite
        private const int MIRROR_BS_IDX = 0;           // If a beam hits this BB, it will stop at the mirror
        private const int MIRROR_REFLECTOR_BS_IDX = 1; // but will only reflect if it hits this BB (smaller and only a thin line)
        private const int CRATE_COLLISION_BB_IDX = 2;

        // Base sprite
        private const int MIRROR_POSITION_BS_IDX = 0;
        private const int CONTROLLER_IDX = 1;

        #endregion

        #region Vars
        private OOBoundingBox _mirrorOOBB;
        private BoundingSquare _mirrorBB;

        private OOBoundingBox _mirrorReflectorBB;
        private float _mirrorRotation;
        private Vector2 _mirrorReflectorPosition;
        private Sprite _mirrorSprite;
        private RotationController _controller;
        #endregion

        #region Properties
        public Vector2 MirrorVector
        {
            get { return (this._mirrorOOBB.P1 - this._mirrorOOBB.P0); }
        }

        public override BoundingSquare QuadtreeCollisionBB
        {
            get
            {
                return this._mirrorBB;
            }
        }

        public Vector2 ReflectorCenter { get; set; }

        // Returns the mirror rotation taking into account the base rotation
        public float MirrorAbsoluteRotation { get; set;}

        public float MirrorRotation 
        { 
            get { return this._mirrorRotation; }
            set
            {
                this._mirrorRotation = value;
                if (this._mirrorRotation > 180f)
                {
                    this._mirrorRotation = 180f;
                }
                else
                if (this._mirrorRotation < -180f)
                {
                    this._mirrorRotation = -180f;
                }
            }

        }
        #endregion

        public LaserBeamMirror()
            : base(StageObjectType.LaserBeamMirror)
        {
            this.MirrorRotation = 90f; // Horizontal by default
            this._controller = new RotationController(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._controller.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._mirrorSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "Mirror");
            this.UpdateMirror();
            this._controller.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateMirror()
        {
            if (this._mirrorRotation > MAX_ROTATION)
            {
                this._mirrorRotation = MAX_ROTATION;
            }
            if (this._mirrorRotation < MIN_ROTATION)
            {
                this._mirrorRotation = MIN_ROTATION;
            }

            this.MirrorAbsoluteRotation = this._mirrorRotation + Mathematics.ConvertAngleTo180(this.Rotation);
            this._mirrorReflectorPosition = Mathematics.TransformVector(this.Sprite.BoundingBoxes[MIRROR_POSITION_BS_IDX].Center, this.Rotation, this.Position);
            this._mirrorOOBB = this._mirrorSprite.BoundingBoxes[MIRROR_BS_IDX].Transform(this.MirrorAbsoluteRotation, this._mirrorReflectorPosition);
            this._mirrorBB = this._mirrorOOBB.ToBoundingSquare();
            this._mirrorReflectorBB = this._mirrorSprite.BoundingBoxes[MIRROR_REFLECTOR_BS_IDX].Transform(this.MirrorAbsoluteRotation, this._mirrorReflectorPosition);

            this.ReflectorCenter = this._mirrorReflectorBB.GetCenter();
            this._controller.SetPosition(this.Sprite.BoundingBoxes[CONTROLLER_IDX].UpperLeft);
       }
        
        /// <summary>
        /// Returns the point of collision between a beam and the mirror
        /// </summary>
        public bool CollidesWithBeam(LaserBeam beam, out Vector2 p)
        {
            return (this._mirrorOOBB.IntersectsLine(beam.BeamOrigin, beam.BeamEndPoint, out p) != OOBoundingBox.CollidingSegment.None);
        }

        /// <summary>
        /// Returns the point of collision between a beam and the mirror
        /// </summary>
        public bool BeamCollidesWithReflector(LaserBeam beam, out Vector2 collidingPoint)
        {
            return (this._mirrorReflectorBB.IntersectsLine(beam.BeamOrigin, beam.BeamEndPoint, out collidingPoint) == OOBoundingBox.CollidingSegment.P3P0);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            // Check if the player cursor is inside the controller area
            if (Stage.CurrentStage.Cursor.IsInteractingWithObject == false &&
                this.QueryInterating())
            {
                Stage.CurrentStage.Cursor.SetInteractingObject(this);
            }
            this._controller.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            if (!shadow)
            {
                this._mirrorSprite.Draw(this._mirrorReflectorPosition, 0, this.MirrorRotation + this.Rotation, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, this.BlendColor, 1f, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._mirrorSprite.Draw(this._mirrorReflectorPosition + GenericConsts.ShadowDepth, 0, this.MirrorRotation + this.Rotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
            }
            base.Draw(shadow);
            this._controller.Draw();
            #if DEBUG
            if (SnailsGame.GameSettings.ShowBoundingBoxes)
            {
                this._mirrorReflectorBB.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
            }
            #endif
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void ForegroundDraw()
        {
            base.ForegroundDraw();
            this._controller.DrawForeground();
        }

        #region ICursorInteractable Members
        /// <summary>
        /// 
        /// </summary>
        public StageCursor.CursorType QueryCursor()
        {
             return this._controller.QueryCursor();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool QueryInterating()
        {
            return this._controller.QueryInterating();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionPressed(Vector2 cursorPos)
        {
            this._controller.CursorActionPressed(cursorPos);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionReleased()
        {
            this._controller.CursorActionReleased();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionSelected()
        {
        }
        #endregion

        #region IRotationControllable Members

        public StageObject ControlledObject { get { return this; } }

        public bool ControllerValueChanged(float value)
        {
            float prevRotation = this.MirrorRotation;
            this.MirrorRotation -= value;
            this.UpdateMirror();


            return (prevRotation != this.MirrorRotation);
        }


        #endregion

        #region ISnailsDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            record.AddField("mirrorRotation", this.MirrorRotation);
            return record;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.MirrorRotation = record.GetFieldValue<float>("mirrorRotation", this.MirrorRotation);
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
