using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Player
{
    public delegate int Callback();
    
    public class AchievementsManager
    {
        const int DISPLAY_TIME = 5000;

        private int posX = -1;
        private int posY = -1;
        private string difficultyRes = null;
        private string ballonRes = null;
        private string textFontRes = null;
        private string playSoundRes = null;

        private double _ellapsedDisplayTime = 0;
        private int queueIdx = -1;

        private Dictionary<int, BrainAchievement> _listAchievementsType = new Dictionary<int, BrainAchievement>();
        private Dictionary<int, Delegate> _eventTable = new Dictionary<int, Delegate>();
        private List<BrainAchievement> _queueAchivements = new List<BrainAchievement>();

        public Dictionary<int, BrainAchievement> Achievements { get { return this._listAchievementsType; } }

        public bool HasAchievementsInQueue
        {
            get { return _queueAchivements.Count > 0; }
        }

        public AchievementsManager()
        { 

        }

        public void Update(BrainGameTime gameTime)
        {
            if (HasAchievementsInQueue)
            {
                if (queueIdx == -1)
                {
                    queueIdx = 0;
                    _queueAchivements[queueIdx].Show();
                }

                _ellapsedDisplayTime += gameTime.ElapsedRealTime.TotalMilliseconds;
                if (_ellapsedDisplayTime > DISPLAY_TIME)
                {
                    _ellapsedDisplayTime = 0;
                    _queueAchivements[queueIdx].Hide();

                    if (queueIdx < _queueAchivements.Count - 1)
                    {
                        queueIdx++;
                        _queueAchivements[queueIdx].Show();
                    }
                    else
                    {
                        queueIdx = -1;
                    }
                }

                // reset queue
                if (queueIdx == -1)
                {
                    for (int i = 0; i < _queueAchivements.Count; i++)
                    {
                        if (!_queueAchivements[i].CanBeDisplayed)
                        {
                            _queueAchivements.RemoveAt(i);
                            i--;
                        }
                    }
                    //_queueAchivements.RemoveAll(delegate(BrainAchievement match) { return !match.CanBeDisplayed; });
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasAchievementsInQueue &&
                queueIdx != -1)
            {
                _queueAchivements[queueIdx].Draw(spriteBatch);
            }
        }

        public void QueueAchievement(int eventType)
        {
            if (_listAchievementsType.ContainsKey(eventType))
            {
                BrainAchievement achiev = _listAchievementsType[eventType];
                _queueAchivements.Add(achiev);
                Unregister(achiev.EventType);
            }
        }

        public BrainAchievement GetAchievement(int eventType)
        {
            if (_listAchievementsType.ContainsKey(eventType))
            {
                return _listAchievementsType[eventType];
            }
            return null;
        }

        public void Register(int eventType, Callback handler)
        {
            // Create an entry for this event type if it doesn't already exist.
            if (!_eventTable.ContainsKey(eventType))
            {
                _eventTable.Add(eventType, null);
            }
            // Add the handler to the event.
            _eventTable[eventType] = (Callback)_eventTable[eventType] + handler;
        }

        public void Unregister(int eventType)
        {
            // Only take action if this event type exists.
            if (_eventTable.ContainsKey(eventType))
            {
                // Remove the event handler from this event.
                _eventTable[eventType] = null; //(Callback)_eventTable[eventType] - handler;
                _eventTable.Remove(eventType);
            }
        }

        public void Notify(int eventType)
        {
            Delegate d;
            // Invoke the delegate only if the event type is in the dictionary.
            if (_eventTable.TryGetValue(eventType, out d))
            {
                // Take a local copy to prevent a race condition if another thread
                // were to unsubscribe from this event.
                Callback callback = (Callback)d;

                // Invoke the delegate if it's not null.
                if (callback != null)
                {
                    callback();
                }
            }
        }

        public int Verify(int eventType)
        {
            Delegate d;
            // Invoke the delegate only if the event type is in the dictionary.
            if (_eventTable.TryGetValue(eventType, out d))
            {
                // Take a local copy to prevent a race condition if another thread
                // were to unsubscribe from this event.
                Callback callback = (Callback)d;

                // Invoke the delegate if it's not null.
                if (callback != null)
                {
                    return callback();
                }
            }
            return -1;
        }

        public void InitFromDataFileRecord(DataFileRecord record)
        {
            posX = record.GetFieldValue<int>("posX");
            posY = record.GetFieldValue<int>("posY");
            difficultyRes = record.GetFieldValue<string>("difficultyRes");
            ballonRes = record.GetFieldValue<string>("ballonRes");
            textFontRes = record.GetFieldValue<string>("textFontRes");
            playSoundRes = record.GetFieldValue<string>("playSoundRes");

            DataFileRecordList achievTypeRecords = record.SelectRecords("Achievements\\Achievement");
            foreach (DataFileRecord achievRecord in achievTypeRecords)
            {
                BrainAchievement achiev = new BrainAchievement(difficultyRes, ballonRes, textFontRes, playSoundRes);
                achiev.InitFromDataFileRecord(achievRecord);
                achiev.LoadContent();
                achiev.Position = new Vector2(posX, posY);
                _listAchievementsType.Add(achiev.EventType, achiev);
            }
        }

        internal void Load(string asset)
        {
            DataFileRecord achievRootRecord = BrainGame.ResourceManager.Load<DataFileRecord>(asset, Resources.ResourceManager.ResourceManagerCacheType.Static);
            this.InitFromDataFileRecord(achievRootRecord);
        }
    }
}
