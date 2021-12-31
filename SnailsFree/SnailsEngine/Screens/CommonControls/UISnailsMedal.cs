using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnailsMedal : UISnailStamp
    {
       
        public enum MedalSizeType
        {
            Normal,
            Small,
            Tiny
        }

        #region Vars
        Snails.MedalType _face;
        MedalSizeType _medalSize;
        #endregion

        #region Properties
        public MedalSizeType MedalSize
        {
            get { return this._medalSize; }
            set
            {
                this._medalSize = value;
                this.UpdateSprite();
            }
        }

        public Snails.MedalType Face 
        {
            get
            {
                return this._face;
            }
            set
            {
                this._face = value;
                this.UpdateSprite();
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UISnailsMedal(UIScreen screenOwner, Snails.MedalType face)
            :
            base(screenOwner)
        {
            this.Face = face;
            this.ShowSample = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.MEDAL);
        }

      /// <summary>
      /// 
      /// </summary>
        private void UpdateSprite()
        {
          switch (this.Face)
          {
            case Snails.MedalType.Bronze:
              switch (this.MedalSize)
              {
                case MedalSizeType.Normal:
                  this.SetSprite("BronzeMedal");
                  break;
                case MedalSizeType.Small:
                  this.SetSprite("BronzeMedalSmall");
                  break;
                case MedalSizeType.Tiny:
                  this.SetSprite("BronzeMedalTiny");
                  break;
              }
              break;

            case Snails.MedalType.Silver:
               switch (this.MedalSize)
              {
                case MedalSizeType.Normal:
                  this.SetSprite("SilverMedal");
                  break;
                case MedalSizeType.Small:
                  this.SetSprite("SilverMedalSmall");
                  break;
                case MedalSizeType.Tiny:
                  this.SetSprite("SilverMedalTiny");
                  break;
              }
              break;

            case Snails.MedalType.Gold:
              switch (this.MedalSize)
              {
                case MedalSizeType.Normal:
                  this.SetSprite("GoldMedal");
                  break;
                case MedalSizeType.Small:
                  this.SetSprite("GoldMedalSmall");
                  break;
                case MedalSizeType.Tiny:
                  this.SetSprite("GoldMedalTiny");
                  break;
              }
              break;
          }
        }

        private void SetSprite(string spriteName)
        {
            this._imgImage.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", spriteName);
        }
    }
}
