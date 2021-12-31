using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Stages.Hints
{
    public class HintManager : IDataFileSerializable
    {
        private static Color GrayoutColor = new Color(0, 0, 0, 180);
        public List<Hint> Hints { get; private set; }
        public Stage Stage { get; private set; }
        public bool HintsVisible { get; set; }
        public int CurrentHintIndex { get; set; }
        public Hint CurrentHint 
        { 
            get 
            {
                if (this.CurrentHintIndex >= this.Hints.Count)
                {
                    return null;
                }

                return this.Hints[this.CurrentHintIndex]; 
            }
            set
            {
                if (value == null)
                {
                    this.CurrentHintIndex = -1;
                    return;
                }
                for (int i = 0; i < this.Hints.Count; i++)
                {
                    if (this.Hints[i] == value)
                    {
                        this.CurrentHintIndex = i;
                        return;
                    }
                }
                throw new SnailsException("Cannot set current Hint because the hint doesn't exist in the HintManager.");
            }

        }

        private Rectangle BackgroundRect
        {
            get
            {
                return new Rectangle((int)Stage.CurrentStage.Camera.UpperLeftScreenCorner.X,
                                     (int)Stage.CurrentStage.Camera.UpperLeftScreenCorner.Y,
                                     BrainGame.Viewport.Bounds.Width,
                                     BrainGame.Viewport.Bounds.Height);
            }
        }

        public HintManager(Stage stage)
        {
            this.Hints = new List<Hint>();
            this.Stage = stage;
            this.CurrentHintIndex = 0;
            this.HintsVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            foreach (Hint hint in this.Hints)
            {
                hint.Initialize();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            if (this.Hints.Count > 0)
            {
              /*  for (int i = 0; i <= this.CurrentHintIndex; i++)
                {
                    this.Hints[i].Update(gameTime);
                }*/
                this.Hints[this.CurrentHintIndex].Update(gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Hints.Count > 0)
            {
                Stage.CurrentStage.EndDraw();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.Draw(UIScreen.ClearTexture, BrainGame.Viewport.Bounds, GrayoutColor);
                spriteBatch.End();

                Stage.CurrentStage.BeginDraw();
                /*
                for (int i = 0; i <= this.CurrentHintIndex; i++)
                {
                    this.Hints[i].Draw(spriteBatch, (i == this.CurrentHintIndex));
                }*/
                this.Hints[this.CurrentHintIndex].Draw(spriteBatch, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncrementHint()
        {
            this.CurrentHintIndex++;
            if (this.CurrentHintIndex >= this.Hints.Count)
            {
                this.CurrentHintIndex = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideHint()
        {
            this.HintsVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowHint()
        {
            this.HintsVisible = true;
            for (int i = 0; i <= this.CurrentHintIndex; i++)
            {
                this.Hints[i].OnBeforeShow();
            }
        }

        #region IDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public void InitFromDataFileRecord(DataFileRecord record)
        {
            this.Hints.Clear();
            DataFileRecordList hintsRecords = record.SelectRecords("Hint");
            foreach (DataFileRecord hintRecord in hintsRecords)
            {
                this.Hints.Add(Hint.FromDataFileRecord(this, hintRecord));
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
        public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
        {
            DataFileRecord record = new DataFileRecord("Hints");
            foreach (Hint hint in this.Hints)
            {
                record.AddRecord(hint.ToDataFileRecord(context));
            }
            return record;

        }

        #endregion
    }
}
