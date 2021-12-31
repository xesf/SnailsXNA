using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// A trigger that is activated/deactivated by a snail
    /// IThsi switch can be turned on and turned off
    /// </summary>
    public class SnailTriggerSwitch : Switch
    {
        #region Constants
        public const string ID = "TRIGGER_SWITCH";

        const int BB_IDX_HANDLE = 1;
        const int BB_IDX_LIGHT = 2;

        const float HANDLE_ANGLE = 45f;
        const float HANDLE_SPEED = 30f;

        #endregion

        enum SnailTriggerSwitchState
        {
            Idle,
            SwitchingOn,
            SwitchingOff
        }
        #region Vars
        private Sprite _handleSprite;
        private Sprite _lightSprite;

        private Sample _leverMovingSound;
        private Sample _leverAcivatingSound;

        private Vector2 _handlePosition;
        private Vector2 _lightPosition;

        private SnailTriggerSwitchState _switchState;
        private float _handleRotation;
        private int _handleDirection;

        List<Snail> _snailsInsideSwitch; // use to control the snails that are "inside" the switch
                                         // This is used to avoid ping-pong switch activation when
                                         // two or more snails are inside the switch

        // Returns the direction needed in order to activate the switch
        // This depends on the current state on/off of the switch and if it is flipped or not
        MovingObject.WalkDirection SwitchActivationDirection
        {
            get 
            {
                if (this.IsHorizontallyFlipped)
                {
                    return (this.IsOn ? MovingObject.WalkDirection.Clockwise : MovingObject.WalkDirection.CounterClockwise);
                }
                else
                {
                    return (this.IsOff ? MovingObject.WalkDirection.Clockwise : MovingObject.WalkDirection.CounterClockwise);
                }
            }
        }
        #endregion


        public SnailTriggerSwitch()
            : this(StageObjectType.TriggerSwitch)
        { }

        public SnailTriggerSwitch(StageObjectType type)
            : base(type)
        {
            this._snailsInsideSwitch = new List<Snail>();
        }

        public override void Copy(StageObject other)
        {
            base.Copy(other);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._handleSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/simple-switch/Handle");
            this._lightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/simple-switch/Lights");

            this._leverMovingSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SWITCH_LEVER_MOVING, this);
            this._leverAcivatingSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SWITCH_LEVER_ACTIVATING, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._crateCollisionBB = this.GetCurrentFrameRectTransformed();
            this._handlePosition = this.TransformSpriteFrameBB(BB_IDX_HANDLE).ToBoundingSquare().UpperLeft;
            this._lightPosition = this.TransformSpriteFrameBB(BB_IDX_LIGHT).ToBoundingSquare().UpperLeft;
            if (this.IsOff)
            {
                this._handleRotation = -HANDLE_ANGLE;
                this._handleDirection = 1;
            }
            else
            {
                this._handleRotation = HANDLE_ANGLE;
                this._handleDirection = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            if (this._switchState != SnailTriggerSwitchState.Idle)
            { 
                // Do collisions even the switch is working, this will track colliding snails
                // with the switch when it is working
                // This snails should only be able to activate the switch if they exit the switch and the re-enter

                // Move the handle
                this._handleRotation += ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 100) * this._handleDirection * HANDLE_SPEED;


                // If the handle has reached the maximum angle, turn the switch on/off
                if (Math.Abs(this._handleRotation) > HANDLE_ANGLE)
                {
                    this._handleRotation = (HANDLE_ANGLE * this._handleDirection);
                    this._handleDirection *= -1;
                    if (this._switchState == SnailTriggerSwitchState.SwitchingOff)
                    {
                        this.SwitchOff();
                    }
                    else
                    {
                        this.SwitchOn();
                    }
                    this._leverAcivatingSound.Play();
                    this._switchState = SnailTriggerSwitchState.Idle;
                }

            }
            this.UpdateSnailsInsideSwitchList();
        }
        
        /// <summary>
        /// Remove all non-colliding snails from the snails inside list
        /// </summary>
        private void UpdateSnailsInsideSwitchList()
        {
            for (int i = 0; i < this._snailsInsideSwitch.Count; i++)
            {
                if (!this.CollidesWithSnail(this._snailsInsideSwitch[i]))
                {
                    this._snailsInsideSwitch.Remove(this._snailsInsideSwitch[i]);
                    i--;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool CollidesWithSnail(Snail snail)
        {
            return snail.CheckCollisionWithHead(this.SnailCollisionBB);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            float angle = this._handleRotation;
            if (this.IsHorizontallyFlipped)
            {
                angle *= -1;
            }

            Vector2 offset = Vector2.Zero;
            Color color = this.BlendColor;
            if (shadow)
            {
                offset += GenericConsts.ShadowDepth;
                color = this.ShadowColor;
            }

            this._handleSprite.Draw(this._handlePosition + offset, 0, angle + this.Rotation, this.SpriteEffect, color, 1f, Stage.CurrentStage.SpriteBatch);
            base.Draw(shadow);
            this._lightSprite.Draw(this._lightPosition, (int)this.State, Stage.CurrentStage.SpriteBatch);

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSnailCollided(Snail snail)
        {

            if (this._switchState == SnailTriggerSwitchState.Idle &&
                snail.CanActivateSwitch &&
                snail.Direction == this.SwitchActivationDirection &&
                this.CollidesWithSnail(snail) && // Make a second test with the snail head
                !this._snailsInsideSwitch.Contains(snail))
            {
                this._leverMovingSound.Play();

                if (this.IsOff)
                {
                    this._switchState = SnailTriggerSwitchState.SwitchingOn;
                }
                else
                {
                    this._switchState = SnailTriggerSwitchState.SwitchingOff;

                }
            }

            // If the switch is working, add colliding snails to a temporary list
            // Snails in this list should not activate the switch when the switch turns to idle
            // This will avoid a ping-pong effect with the switch
            if (this._switchState != SnailTriggerSwitchState.Idle &&
                !this._snailsInsideSwitch.Contains(snail))
            {
                this._snailsInsideSwitch.Add(snail);
            }
        }

        /// <summary>
        /// See virtual method for details
        /// </summary>
        public override bool CrateToolIsValid(BoundingSquare crateBs)
        {
            return !(crateBs.Collides(this._crateCollisionBB));
        }

        #region IDataFileSerializable Members
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.State = (SwitchState)Enum.Parse(typeof(SwitchState), record.GetFieldValue<string>("state", SwitchState.On.ToString()), true);
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
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            record.AddField("state", this.State.ToString());
            return record;
        }
        #endregion
    }
}
