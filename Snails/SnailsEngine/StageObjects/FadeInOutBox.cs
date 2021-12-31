using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    class FadeInOutBox : Box, ISwitchable, IDataFileSerializable
    {
        const float FADING_SPEED = 3f;

        public enum FadeInOutBoxState
        {
            FadedIn,
            FadedOut,
            FadingIn,
            FadingOut,
        }

        #region Vars
        private float _alpha;
        private Sample _fadeInSound;
        private Sample _fadeOutSound;
        #endregion

        #region Properties
        public FadeInOutBoxState State { get; set; }

        public bool IsFadedIn { get { return this.State == FadeInOutBoxState.FadedIn; } }
        public bool IsFadedOut { get { return this.State == FadeInOutBoxState.FadedOut; } }

        #endregion      

        public FadeInOutBox()
            : this(StageObjectType.FadeInOutBox)
        {
        }

        public FadeInOutBox(StageObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._fadeInSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.FADE_BOX_IN, this);
            this._fadeOutSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.FADE_BOX_OUT, this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.CurrentFrame = BrainGame.Rand.Next(this.Sprite.FrameCount);
        }


        /// <summary>
        ///
        /// </summary>
        public override void StageInitialize()
        {
            base.StageInitialize();
            if (this.IsFadedIn)
            {
                this._alpha = 1f;
                this.BoxDeployed(Stage.LoadingContext == Stage.StageLoadingContext.Gameplay, false, false);
            }
            else
            {
                this._alpha = 0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);

            switch (this.State)
            {
                case FadeInOutBoxState.FadingOut:
                    this._alpha -= (FADING_SPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
                    this.BlendColor = new Microsoft.Xna.Framework.Color(this._alpha, this._alpha, this._alpha, this._alpha);
                    if (this._alpha < 0)
                    {
                        this._alpha = 0;
                        this.State = FadeInOutBoxState.FadedOut;
                        Stage.CurrentStage.Board.RemoveTileAt(this.BoardX, this.BoardY);
                    }
                    break;

                case FadeInOutBoxState.FadingIn:
                    this._alpha += (FADING_SPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
                    this.BlendColor = new Color(this._alpha, this._alpha, this._alpha, this._alpha);
                    if (this._alpha > 1f)
                    {
                        this._alpha = 1f;
                        this.State = FadeInOutBoxState.FadedIn;
                    }
                    break; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            if (!this.IsFadedOut)
            {
                base.Draw(shadow);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SwitchChanged()
        {
            switch (this.State)
            {
                case FadeInOutBoxState.FadedOut:
                    if (Stage.CurrentStage.Board.GetTileAt(this.BoardY, this.BoardX) != null)
                    {
                        return; // Do nothing if there's already a tile at that position
                    }
                    this._fadeInSound.Play();
                    this.State = FadeInOutBoxState.FadingIn;
                    this.BoxDeployed(true, true, true);
                    break;

                case FadeInOutBoxState.FadedIn:
                    this.State = FadeInOutBoxState.FadingOut;
                    this._fadeOutSound.Play();
                   break;

                case FadeInOutBoxState.FadingIn:
                    this.State = FadeInOutBoxState.FadingIn;
                    break;
            }
        }

        #region ISwitchable Members

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOn()
        {
            this.SwitchChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchOff()
        {
            this.SwitchChanged();
        }


        public bool IsOn
        {
            get { return (this.State == FadeInOutBoxState.FadedIn); }
        }
        #endregion

        #region IDataFileSerializable Members
        /// <summary>
        ///
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.State = (FadeInOutBoxState)Enum.Parse(typeof(FadeInOutBoxState), record.GetFieldValue<string>("state", this.State.ToString()), true);
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
