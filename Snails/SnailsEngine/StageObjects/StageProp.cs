using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using System.IO;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.StageObjects
{
    // Prop represents an object that does nothing. It's just used to visualy
    // enrich the game = adereço
    // StageProps do nothing on update, so they can't even be animated
    class StageProp : StageObject, ISnailsDataFileSerializable
    {
        public const string ID = "STAGE_PROP";

        public struct StagePropItem
        {
            public string _id;
            public string _spriteResource;
            public bool _foreground;
            public ThemeType _theme;
            public bool _autoHideOnAnimationEnd;
            public bool _redisplay; // Prop should be redisplayed after hidding
            public bool _randomizeShowDelay; // This will allow for a show randomize time, used in the goldmines in the gold blink for instance
            public int _showDelay;
            public string _displayDelayStr;
            public bool _randomizeFirstFrame;
            public string _soundRes;

            public StagePropItem(string id, string spriteRes, bool foreground, ThemeType theme, bool autoHideOnAnimationEnd,
                                 int showDelay, bool randomizeShowDelay, bool redisplay, string displayDelayStr,
                                 bool randomizeFirstFrame, string soundRes)
            {
                this._id = id;
                this._spriteResource = spriteRes;
                this._foreground = foreground;
                this._theme = theme;
                this._autoHideOnAnimationEnd = autoHideOnAnimationEnd;
                this._showDelay = showDelay;
                this._randomizeShowDelay = randomizeShowDelay;
                this._redisplay = redisplay;
                this._displayDelayStr = displayDelayStr;
                this._randomizeFirstFrame = randomizeFirstFrame;
                this._soundRes = soundRes;
            }
        }

        public static Dictionary<string, StagePropItem> StagePropsItems { get; set; }
        public string PropId { get; set; }
        public ThemeType Theme { get; set; }
        private StagePropItem _properties;
        private double _ellapsedHideTime;
        private Sample _sound;
        /// <summary>
		/// 
		/// </summary>
        public StageProp()
            : base(StageObjectType.StageProp)
        {
        }

        public StageProp(StageProp other)
            : base(other)
        {
            Copy(other);

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy(StageObject other)
        {
            base.Copy(other);
            StageProp otherProp = other as StageProp;
            this.PropId = otherProp.PropId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            if (StageProp.StagePropsItems != null && this.PropId != null)
            {
#if DEBUG
                if (!StageProp.StagePropsItems.ContainsKey(this.PropId))
                {
                    throw new SnailsException("StageProp with id [" + PropId + "] not found in StageProps list.");
                }
#endif
                string resName = StageProp.StagePropsItems[this.PropId]._spriteResource;
                this.ResourceId = BrainPath.GetDirectoryName(resName);
                this.SpriteId = BrainPath.GetFileName(resName);
                if (!string.IsNullOrEmpty(StageProp.StagePropsItems[this.PropId]._soundRes))
                {
                    this._sound = BrainGame.ResourceManager.GetSampleTemporary(StageProp.StagePropsItems[this.PropId]._soundRes);
                }
            }
            
            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.DrawInForeground = StageProp.StagePropsItems[this.PropId]._foreground;
            this.Theme = StageProp.StagePropsItems[this.PropId]._theme;
            this._properties = StageProp.StagePropsItems[this.PropId];
            this._ellapsedHideTime = 0;
            if (this._properties._showDelay > 0)
            {
                this.Hide();
                this.SetupShowTimer();
            }
            if (this._properties._randomizeFirstFrame)
            {
                this.CurrentFrame = BrainGame.Rand.Next(this.Sprite.FrameCount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnLastFrame()
        {
          if (this._properties._autoHideOnAnimationEnd)
          {
             this.Hide();
             if (this._properties._redisplay)
             {
                 this.SetupShowTimer();
             }
          }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            if (this.IsVisible == false && this._properties._redisplay)
            {
                this._ellapsedHideTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this._ellapsedHideTime < 0)
                {
                    this.Show();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Show()
        {
            if (this._sound != null)
            {
                this._sound.Play();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void SetupShowTimer()
        {
            this._ellapsedHideTime = this._properties._showDelay;
            if (this._properties._randomizeShowDelay)
            {
                this._ellapsedHideTime = BrainGame.Rand.Next(this._properties._showDelay);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            if (this.PropId == null)
            {
                return base.ToString();
            }
            return (this.PropId);
        }


        public static StageProp Create(string id)
        {
            StagePropItem propItem = StageProp.StagePropsItems[id];
            StageProp prop = new StageProp();
            prop.Id = StageProp.ID;
            prop.PropId = id;
            prop.SpriteId = BrainPath.GetFileName(propItem._spriteResource);
            prop.ResourceId = BrainPath.GetDirectoryName(propItem._spriteResource);
            prop.DrawInForeground = propItem._foreground;
            prop.Theme = propItem._theme;
            prop._properties = propItem;
            prop.LoadContent();

            return prop;
        }

  

        #region ISnailsDataFileSerializable Members

        /// <summary>
                /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.PropId = record.GetFieldValue<string>("propId");

            // StageProps sprite resources
            // This data is not stored when StageProp is saved to a Stage, this is only saved on StageData
            DataFileRecordList stagePropRecords = record.SelectRecords("Props\\Prop");
            if (stagePropRecords != null && stagePropRecords.Count > 0)
            {
                StageProp.StagePropsItems = new Dictionary<string, StagePropItem>();
                foreach (DataFileRecord stagePropRec in stagePropRecords)
                {
                    string id = stagePropRec.GetFieldValue<string>("id");
                    string resName = stagePropRec.GetFieldValue<string>("res") + "/" + stagePropRec.GetFieldValue<string>("sprite");
                    bool foreground = stagePropRec.GetFieldValue<bool>("foreground", false);
                    ThemeType theme = (ThemeType)Enum.Parse(typeof(ThemeType), stagePropRec.GetFieldValue<string>("theme"), true);
                    bool autoHideOnAnimationEnd = stagePropRec.GetFieldValue<bool>("autoHideOnAnimationEnd", false);
                    bool redisplay = stagePropRec.GetFieldValue<bool>("redisplay", false);
                    string showDelayStr = stagePropRec.GetFieldValue<string>("displayDelay", null);
                    string soundRes = stagePropRec.GetFieldValue<string>("soundRes", null);
                    bool randomizeShowDelay = false;
                    int showDelay = 0;
                    if (showDelayStr != null)
                    {
                   //     throw new ApplicationException(showDelayStr);
                        // A despachar... always put "rand:" not "rand :" or "Rand:" etc...
                        if (showDelayStr.Contains("rand:"))
                        {
                            showDelay = Convert.ToInt32(showDelayStr.Substring(showDelayStr.IndexOf(":") + 1));
                            randomizeShowDelay = true;
                        }
                        else
                        {
                            showDelay = Convert.ToInt32(showDelayStr);
                        }
                    }

                    bool randomizeFirstFrame = stagePropRec.GetFieldValue<bool>("randomizeFristFrame", false);
                    
                    StageProp.StagePropsItems.Add(id, new StagePropItem(id, resName, foreground, theme, autoHideOnAnimationEnd, showDelay, randomizeShowDelay, redisplay, showDelayStr, randomizeFirstFrame, soundRes));
                }
            }
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
            record.AddField("propId", this.PropId);
            
            if (context == ToDataFileRecordContext.StageDataSave)
            {
                // Sprite list
                DataFileRecord spritesRecord = record.AddRecord("Props");
                foreach (KeyValuePair<string, StagePropItem> sprite in StageProp.StagePropsItems)
                {
                    DataFileRecord spriteRecord = new DataFileRecord("Prop");
                    spriteRecord.AddField("id", sprite.Key);
                    spriteRecord.AddField("res", BrainPath.GetDirectoryName(sprite.Value._spriteResource));
                    spriteRecord.AddField("sprite", BrainPath.GetFileName(sprite.Value._spriteResource));
                    spriteRecord.AddField("foreground", sprite.Value._foreground);
                    spriteRecord.AddField("theme", sprite.Value._theme.ToString());
                    spriteRecord.AddField("autoHideOnAnimationEnd", sprite.Value._autoHideOnAnimationEnd);
                    spriteRecord.AddField("redisplay", sprite.Value._redisplay);
                    spriteRecord.AddField("displayDelay", sprite.Value._displayDelayStr);
                    spriteRecord.AddField("randomizeFristFrame", sprite.Value._randomizeFirstFrame);
                    spriteRecord.AddField("soundRes", sprite.Value._soundRes);
                    spritesRecord.AddRecord(spriteRecord);
                }
            }
            return record;
        }
        #endregion        
    }
}
