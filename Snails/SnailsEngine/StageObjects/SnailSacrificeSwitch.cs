using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// A switch that is activated after X snails have entered the switch
    /// The swith kills the snail
    /// 
    /// Storage - Is the area where the soop is stored (the glass container)
    /// </summary>
    public partial class SnailSacrificeSwitch : Switch
    {
        #region Constants
        public const string ID = "SNAIL_SACRIFICE";
        const int BB_IDX_TUBE = 1;
        const int BB_IDX_DISCARD = 2;
        const int BB_IDX_SUCK_TUBE1 = 3;
        const int BB_IDX_SUCK_TUBE2 = 4;
        const int BB_IDX_SUCK_TUBE3 = 5;
        const int BB_IDX_SUCK_TUBE4 = 6;
        const int BB_IDX_SUCK_TUBE5 = 7;
        const int BB_IDX_SUCK = 8;
        const int BB_IDX_BUTTONS = 9;
        const int BB_IDX_FLOW = 10;
        const int BB_IDX_STORAGE = 11;
        const int BB_IDX_SOOP = 12;
        const int BB_IDX_RED_LIGHT = 13;
        const int BB_IDX_GREEN_LIGHT = 14;
        const int BB_IDX_SNAIL = 15;
        const int BB_IDX_SHELL = 16;
        const int BB_IDX_SNAIL_COLLISION_RIGHT = 17;
        const int BB_IDX_SNAIL_COLLISION_LEFT = 18;
        const int BB_IDX_SNAIL_COUNTER = 19;

        const int SOOP_HEIGHT_PER_SNAIL = 7;

        #endregion
     
        #region Members
        public int SnailsToSacrifice { get; set; }

        int _numSacrifices;
        string _numSacrificesString;
        SpriteAnimationQueue _animations;
        SpriteAnimationQueueItem _soopAnimation;
        SpriteAnimationQueueItem _snailAnimation;

        Vector2 _storagePosition;
        Sprite _storageSprite;

        BoundingSquare _bsSoop;
        Rectangle _rcSoop;
        Sprite _soopSprite;

        Sprite _redLightSprite;
        Vector2 _redLightPosition;
        Sprite _greenLightSprite;
        Vector2 _greenLightPosition;

        BoundingSquare _bsSnailCollisionRight;
        BoundingSquare _bsSnailCollisionLeft;

        Snail _currentSnailSooped;
        TextFont _font;
        Rectangle _rcCounter;

        SpriteAnimation _onTextAnimation;

        Sample _switchSound;

        #endregion

        bool IsSuckingSnail { get { return (this._animations.Active == true); } }

        /// <summary>
        /// 
        /// </summary>
        public SnailSacrificeSwitch()
            : base(StageObjectType.SnailSacrifice)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            // All this could go in a content file, not going to do that at this moment, maybe later
            // if the need rises again

            Vector2 tubePosition = this.Sprite.BoundingBoxes[BB_IDX_TUBE].UpperLeft;
            Vector2 discardPosition = this.Sprite.BoundingBoxes[BB_IDX_DISCARD].UpperLeft;
            Vector2 suckTube1Position = this.Sprite.BoundingBoxes[BB_IDX_SUCK_TUBE1].UpperLeft;
            Vector2 suckTube2Position = this.Sprite.BoundingBoxes[BB_IDX_SUCK_TUBE2].UpperLeft;
            Vector2 suckTube3Position = this.Sprite.BoundingBoxes[BB_IDX_SUCK_TUBE3].UpperLeft;
            Vector2 suckTube4Position = this.Sprite.BoundingBoxes[BB_IDX_SUCK_TUBE4].UpperLeft;
            Vector2 suckTube5Position = this.Sprite.BoundingBoxes[BB_IDX_SUCK_TUBE5].UpperLeft;
            Vector2 suckPosition = this.Sprite.BoundingBoxes[BB_IDX_SUCK].UpperLeft;
            Vector2 snailPosition = this.Sprite.BoundingBoxes[BB_IDX_SNAIL].UpperLeft;
            Vector2 buttonsPosition = this.Sprite.BoundingBoxes[BB_IDX_BUTTONS].UpperLeft;
            Vector2 flowPosition = this.Sprite.BoundingBoxes[BB_IDX_FLOW].UpperLeft;

            this._animations = new SpriteAnimationQueue(false, false);
            this._snailAnimation = this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Snail", ResourceManager.RES_MANAGER_ID_TEMPORARY), snailPosition, false, false);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SnailSucking", ResourceManager.RES_MANAGER_ID_TEMPORARY), suckPosition, true, true, AudioTags.SNAILS_SWITCH_SUCK, this);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube1", ResourceManager.RES_MANAGER_ID_TEMPORARY), suckTube1Position, true, true);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube2", ResourceManager.RES_MANAGER_ID_TEMPORARY), suckTube2Position, true, true);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube3", ResourceManager.RES_MANAGER_ID_TEMPORARY), suckTube3Position, true, true);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube4", ResourceManager.RES_MANAGER_ID_TEMPORARY), suckTube4Position, true, true);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube5", ResourceManager.RES_MANAGER_ID_TEMPORARY), suckTube5Position, true, true);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Lights", ResourceManager.RES_MANAGER_ID_TEMPORARY), buttonsPosition, true, true, AudioTags.SNAILS_SWITCH_PROCESSING, this, this.ButtonsAnimationEnded);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/ShellDiscard", ResourceManager.RES_MANAGER_ID_TEMPORARY), discardPosition, true, false, AudioTags.SNAIL_THROW, this);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Tube", ResourceManager.RES_MANAGER_ID_TEMPORARY), tubePosition, true, false);
            this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Flow", ResourceManager.RES_MANAGER_ID_TEMPORARY), flowPosition, false, false, AudioTags.SNAILS_SWITCH_SOUP, this);
            // Use a var for this animation because the position will change depending on the snails sooped
            this._soopAnimation = this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SoopWaves", ResourceManager.RES_MANAGER_ID_TEMPORARY), Vector2.Zero, false, true);
            
            this._animations.Reset();

            this._animations.OnAnimationEnded += new SpriteAnimationQueue.AnimationEndedHandler(_animations_OnAnimationEnded);

            // Storage
            this._storagePosition = this.Sprite.BoundingBoxes[BB_IDX_STORAGE].UpperLeft;
            this._storageSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/Storage");

            // Soop
            this._bsSoop = this.Sprite.BoundingBoxes[BB_IDX_SOOP];
            this._soopSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/Soop");
            
            // ON text
            this._onTextAnimation = new SpriteAnimation("spriteset/snails-switch/On", ResourceManager.RES_MANAGER_ID_TEMPORARY);

            // Red light
            this._redLightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/RedLight");
            this._redLightPosition = this.Sprite.BoundingBoxes[BB_IDX_RED_LIGHT].UpperLeft;

            // Green light
            this._greenLightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/GreenLight");
            this._greenLightPosition = this.Sprite.BoundingBoxes[BB_IDX_GREEN_LIGHT].UpperLeft;

            this.ComputeSoopRect();

            this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/snails-switch", ResourceManager.ResourceManagerCacheType.Temporary);

            this._switchSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.SWITCH, this);

            this.UpdateCounterString();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._bsSnailCollisionRight = this.TransformSpriteFrameBB(BB_IDX_SNAIL_COLLISION_RIGHT).ToBoundingSquare();
            this._bsSnailCollisionLeft = this.TransformSpriteFrameBB(BB_IDX_SNAIL_COLLISION_LEFT).ToBoundingSquare();
            this._rcCounter = this.TransformSpriteFrameBB(BB_IDX_SNAIL_COUNTER).ToRect();
            this._onTextAnimation.Position = new Vector2(this._rcCounter.Left, this._rcCounter.Top);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSnailCollided(Snail snail)
        {
            if (this.IsOn || this.IsSuckingSnail || !snail.CanBeSuckedBySwitch)
            {
                return;
            }

           
            if (snail.Direction == MovingObject.WalkDirection.Clockwise)
            {
                if (!snail.CheckCollisionWithHead(this._bsSnailCollisionRight))
                {
                    return;
                }
                this._snailAnimation._animation._spriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            }
            else
            {
                if (!snail.CheckCollisionWithHead(this._bsSnailCollisionLeft))
                {
                    return;
                }
                this._snailAnimation._animation._spriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            // Switch on will happen only when the animation ends
            this._animations.Activate();
            snail.KillBySacrificeSwitch();

            this._currentSnailSooped = snail;

            this._numSacrifices++;
            this.UpdateCounterString();
            if (this._numSacrifices >= this.SnailsToSacrifice)
            {
                this.SwitchOn();
                this._switchSound.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void ButtonsAnimationEnded()
        {
            Prop shellProp = Prop.CreateShell(this.Position + this.Sprite.BoundingBoxes[BB_IDX_SHELL].UpperLeft);
            shellProp.DrawInForeground = true;
            Stage.CurrentStage.AddObjectInRuntime(shellProp);
            shellProp.ProjectWithRotation(Mathematics.RandomizeVector(70, 110), 50f, 30f);
            this._currentSnailSooped.DisposeFromStage();
        }

        /// <summary>
        /// 
        /// </summary>
        void _animations_OnAnimationEnded()
        {

            this.ComputeSoopRect();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this._animations.Update(gameTime);

            if (this.IsOn)
            {
                this._onTextAnimation.Update(gameTime);
            }
            else
            {
                this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            Vector2 offset = Vector2.Zero;
            Color color = this.BlendColor;
            Rectangle rc = this._rcSoop;
            if (shadow)
            {
                offset += GenericConsts.ShadowDepth;
                color = this.ShadowColor;
                rc.X += (int)offset.X;
                rc.Y += (int)offset.Y;
            }
            this._animations.Draw(this.Position + offset, color, Stage.CurrentStage.SpriteBatch);
            
            // Soop
            this._soopSprite.Draw(rc, 0, color, Stage.CurrentStage.SpriteBatch);

            // Storage
            this._storageSprite.Draw(this.Position + this._storagePosition + offset, 0, color,  Stage.CurrentStage.SpriteBatch);

            // Lights
            if (this.IsOff)
            {
                this._redLightSprite.Draw(this.Position + this._redLightPosition, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._greenLightSprite.Draw(this.Position + this._greenLightPosition, Stage.CurrentStage.SpriteBatch);
            }

            if (!this.IsOn)
            {
                this._font.DrawString(Stage.CurrentStage.SpriteBatch, this._numSacrificesString, this._rcCounter, TextFont.TextHorizontalAlign.Center);
            }
            else
            {
                this._onTextAnimation.Draw(Stage.CurrentStage.SpriteBatch);           
            }
         }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeSoopRect()
        {
            float height = (this._numSacrifices * SOOP_HEIGHT_PER_SNAIL);
            this._rcSoop = new Rectangle((int)(this.Position.X + this._bsSoop.Left),
                                         (int)(this.Position.Y + (this._bsSoop.Top + this._bsSoop.Height - height)),
                                         (int)this._bsSoop.Width, (int)(height));

            this._soopAnimation._position = new Vector2(this._bsSoop.Left, this._rcSoop.Top - this.Position.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCounterString()
        {
            this._numSacrificesString = (this.SnailsToSacrifice - this._numSacrifices).ToString();
        }

        #region IDataFileSerializable Members   
        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);
            switch (context)
            {
                case ToDataFileRecordContext.StageSave:
                    record.AddField("snailsToSacrifice", (int)this.SnailsToSacrifice);
                    break;
            }
            return record;
        }

   
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.SnailsToSacrifice = record.GetFieldValue<int>("snailsToSacrifice", this.SnailsToSacrifice);
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
