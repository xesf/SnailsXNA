using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    /// <summary>
    /// This is a light that uses fire has a light source
    /// This maybe a candle, a torchlight etc
    /// </summary>
    class FlameLight : SingleLightEmitter
    {
        #region Constants
        const int BB_IDX_FLAME_POSITION = 0;
        #endregion

        string _flameSpriteRes;
        SpriteAnimation _flameAnimation;
        Vector2 _flamePosition;
        Vector2 _defaultScale;
        double _glowTime;

        public FlameLight()
            : base(StageObjectType.FlameLight)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            this._flameSpriteRes = ((FlameLight)other)._flameSpriteRes;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            string resManager = ResourceManager.RES_MANAGER_ID_TEMPORARY;
            // Bring the hammer
            if (this._flameSpriteRes.Contains(ThemeType.ThemeA.ToString()) ||
                this._flameSpriteRes.Contains(ThemeType.ThemeB.ToString()) ||
                this._flameSpriteRes.Contains(ThemeType.ThemeC.ToString()) ||
                this._flameSpriteRes.Contains(ThemeType.ThemeD.ToString()))
            {
                resManager = ResourceManagerIds.STAGE_THEME_RESOURCES;
            }

            this._flameAnimation = new SpriteAnimation(this._flameSpriteRes, resManager);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._flamePosition = this.TransformSpriteFrameBB(BB_IDX_FLAME_POSITION).ToBoundingSquare().Center;
            this._defaultScale = this.LightSource.Scale;
            this._glowTime = 50;
            this.LightSource.Position = this._flamePosition;
            this.LightSource.Color = new Color(1f, 0f, 0f, 0.2f);
            this._flameAnimation.RandomizeFrame();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this._flameAnimation.Update(gameTime);
            
            this._glowTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this._glowTime < 0)
            {
                this._glowTime = 50;
                Vector2 scaleDelta = new Vector2(BrainGame.RandomizeFloat(0f, 0.05f, 2),
                                                 BrainGame.RandomizeFloat(0f, 0.05f, 2));
                this.LightSource.Scale = this._defaultScale + scaleDelta;
                float r = BrainGame.RandomizeFloat(0.8f, 1f, 2);
                float g = BrainGame.RandomizeFloat(0.25f, 0.3f, 2);

                this.LightSource.Color = new Color(r, g, 0f, 0.2f);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            if (shadow)
            {
                this._flameAnimation.Draw(this._flamePosition + GenericConsts.ShadowDepth, this.ShadowColor, Stage.CurrentStage.SpriteBatch);
            }
            else
            {
                this._flameAnimation.Draw(this._flamePosition, Stage.CurrentStage.SpriteBatch);
            }
        }

        #region IDataFileSerializable Members
    
        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this._flameSpriteRes = record.GetFieldValue<string>("flameSpriteRes", this._flameSpriteRes);
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = base.ToDataFileRecord(context);

            if (context == ToDataFileRecordContext.StageDataSave)
            {
                record.AddField("flameSpriteRes", this._flameSpriteRes);
            }

            return record;
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
