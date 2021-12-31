using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.Tutorials
{
    /// <summary>
    /// The tutorial is organized by topics. Each topic represents a pop-up tutorial
    /// A TutorialTopic can have more then one line and each line can have more then 1 tutorial item
    /// A TutorialItem can be text or image (TutorialText and TutorialImage)
    /// </summary>
    public class Tutorial : IBrainComponent, ISnailsDataFileSerializable, IAsyncOperation
    {

        public List<TutorialTopic> Topics { get { return this._topics; } }
        private List<TutorialTopic> _topicQueue; // Topics are queued for reading (topics stay in a queue waiting for the current topic do be closed)
        private List<TutorialTopic> _topics;
        private TutorialTopic _currentTopic;

        public bool _loaded;
       
        public bool TopicVisible
        {
            get { return (this._currentTopic != null && this._currentTopic.IsOpen); }
        }

        public Tutorial()
        {
            this._topicQueue = new List<TutorialTopic>();
        }


        /// <summary>
        /// 
        /// </summary>
        public static Tutorial FromDataFileRecord(DataFileRecord record)
        {
            Tutorial tutorial = new Tutorial();
            tutorial.InitFromDataFileRecord(record);
            return tutorial;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TutorialTopic> GetTopics(int [] topicsIds)
        {
            List<TutorialTopic> topics = new List<TutorialTopic>();
            foreach (int topicId in topicsIds)
            {
                topics.Add(this.GetTopic(topicId));
            }
            return topics;
        }

        /// <summary>
        /// 
        /// </summary>
        public TutorialTopic GetTopic(int topicId)
        {
            TutorialTopic topic = this._topics.Find(
                delegate(TutorialTopic t)
                {
                    return t.TopicId == topicId;
                }
                );
            if (topic == null)
            {
                throw new SnailsException("Topic with id [" + topicId.ToString() + "] not found.");
            }

            return topic;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowTopic(int topicId, bool showIfAlreadyViewed)
        {
           if (SnailsGame.ProfilesManager.CurrentProfile != null &&
               SnailsGame.ProfilesManager.CurrentProfile.IsTutorialTopicRead(topicId) &&
               showIfAlreadyViewed == false)
           {
             return;
           }

           TutorialTopic topic = this.GetTopic(topicId);

           // Check the stageId on the topic. If it's diferent then the current stage, just ignore it
           if (!string.IsNullOrEmpty(topic._stageId) && topic._stageId != Stage.CurrentStage.LevelStage.StageId)
           {
               return;
           }

           // If there's no current topic, show it
           if (this._currentTopic == null)
           {
                this.ShowTopic(topic);
           }
           else // Add to the queue of topics if not already in the queue
           {
               if (!topic._inQueue)
               {
                   this._topicQueue.Add(topic);
                   topic._inQueue = true;
               }
           }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowTopic(TutorialTopic topic)
        {
		    this._currentTopic = topic;
		    this._currentTopic.Show();
        }

        #region IBrainComponent
      
        /// <summary>
        /// 
        /// </summary>
        public Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            this._currentTopic = null;
            this._topicQueue.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            // Only load tutorial topic content when needed. This was moved to TutorialTopic.Show
          /*  foreach (TutorialTopic topic in _topics)
            {
		        topic.LoadContent();
            }
            */
            this._loaded = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
	       if (this._currentTopic != null)
           {
              this._currentTopic.Update(gameTime);
              if (this._currentTopic.IsClosed)
              {
                  this._currentTopic = null;
                  if (this._topicQueue.Count > 0)
                  {
                     this.ShowTopic(this._topicQueue[0]);
                     this._topicQueue.RemoveAt(0);
                  }
              }
           }            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this._currentTopic != null)
            {
                this._currentTopic.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadContent()
        {
            
        }
        #endregion

        #region IDataFileSerializable
        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this._topics = new List<TutorialTopic>();
            DataFileRecordList tutorialTopicRecords = record.SelectRecords("TutorialTopic");
            foreach (DataFileRecord topicRecord in tutorialTopicRecords)
            {
                TutorialTopic topic = TutorialTopic.FromDataFileRecord(topicRecord, this);
                // This runs when the content compiles, SnailsGame.Settings might be null
                if (SnailsGame.Instance != null && SnailsGame.Settings != null &&
                    (topic.Platforms & SnailsGame.Settings.Platform) != SnailsGame.Settings.Platform)
                {
                    continue; // filter by platform
                }
                this._topics.Add(topic);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord()
        {
            return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Tutorial");
            foreach (TutorialTopic topic in this._topics)
            {
                record.AddRecord(topic.ToDataFileRecord());
              }

            return record;
        }
        #endregion

        #region IAsyncOperation
        /// <summary>
        /// 
        /// </summary>
        public void BeginLoad()
        {
            this.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        public object AsyncLoadingParams
        {
            set {  }
        }
        #endregion
    }
}
