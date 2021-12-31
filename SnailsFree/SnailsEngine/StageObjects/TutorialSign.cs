using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.Snails.StageObjects
{
    class TutorialSign : StageObject, ICursorInteractable
    {
        private Sprite _signSprite;
        private Vector2 _signPosition;
        private BoundingCircle _bsSelectArea;
        private ScaleEffect _scaleEffect;
        public int [] TutorialTopics { get; private set; } 
        public string TopicsString { get; set; } // Comma separated string with the ids do show

        /// <summary>
		/// 
		/// </summary>
        public TutorialSign()
            : base(StageObjectType.TutorialSign)
        {
        }

        public TutorialSign(TutorialSign other)
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
            TutorialSign otherSign = other as TutorialSign;
            this.TopicsString = otherSign.TopicsString;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            this._signSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Sign");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._signPosition = this.TransformSpriteFrameBB(0).GetCenter();
            this._scaleEffect = new ScaleEffect(new Vector2(1.1f, 1.1f), 0.5f, new Vector2(0.93f, 0.93f), true);
            this._bsSelectArea = this._signSprite._boundingSpheres[0].Transform(this._signPosition);

            if (!string.IsNullOrEmpty(this.TopicsString))
            {
                string[] topicsList = this.TopicsString.Split(',');
                this.TutorialTopics = new int[topicsList.Length];
                for (int i = 0; i < TutorialTopics.Length; i++)
                {
                    this.TutorialTopics[i] = Convert.ToInt32(topicsList[i]);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this._scaleEffect.Update(gameTime);
            // Check if the player cursor is inside the controller area
            if (Stage.CurrentStage.Cursor.IsInteractingWithObject == false &&
                Stage.CurrentStage.Input.IsActionPressed)
            {
                if (this.QueryCursorInsideInteractingZone())
                {
                    Stage.CurrentStage.Cursor.SetInteractingObject(this);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw(bool shadow)
        {
            base.Draw(shadow);
            this._signSprite.Draw(this._signPosition, 0, 0f, SpriteEffects.None, Color.White, this._scaleEffect.Scale.X, Stage.CurrentStage.SpriteBatch);
#if DEBUG
            if (SnailsGame.Settings.ShowBoundingBoxes)
            {
                this._bsSelectArea.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private bool QueryCursorInsideInteractingZone()
        {
            return (this._bsSelectArea.Contains(Stage.CurrentStage.Cursor.Position));
        }

        #region ICursorInteractable Members

        public StageCursor.CursorType QueryCursor()
        {
            return StageCursor.CursorType.Select;
        }

        public bool QueryInterating()
        {
            return this.QueryCursorInsideInteractingZone();
        }

        public void CursorActionPressed(Vector2 cursorPos)
        {
                
        }

        public void CursorActionReleased()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionSelected()
        {
            if (this.TutorialTopics != null)
            {
                Stage.CurrentStage.ShowTutorialTopics(this.TutorialTopics, true);
            }    
        }

        #endregion

        #region ISnailsDataFileSerializable Members

        /// <summary>
        /// 
        /// </summary>
        public override void InitFromDataFileRecord(DataFileRecord record)
        {
            base.InitFromDataFileRecord(record);
            this.TopicsString = record.GetFieldValue<string>("tutorialTopics");
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
            record.AddField("tutorialTopics", TopicsString);
            return record;
        }
        #endregion        
    
    
    }
}
