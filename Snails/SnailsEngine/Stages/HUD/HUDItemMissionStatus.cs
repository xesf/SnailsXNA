using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    class HUDItemMissionStatus : HUDItem
    {
        public const double MISSION_STATUS_BLINK_TIME = 400;
        Vector2 _missionStatusPosition;
        double _missionStatusBlink;
        bool _blinkVisible;
        string _statusText;
        Color _color;
        double _expirationTime; // in msecs. message will autohide when this time expires
        bool _expires;

        public HUDItemMissionStatus()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            this._missionStatusPosition = position + new Vector2(0f, 10f);
            this._width = 300f;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            if (this._visible)
            {
                this._missionStatusBlink += gameTime.ElapsedRealTime.TotalMilliseconds;
                if (this._missionStatusBlink > MISSION_STATUS_BLINK_TIME)
                {
                    this._missionStatusBlink -= MISSION_STATUS_BLINK_TIME;
                    this._blinkVisible = !this._blinkVisible;
                }

                if (this._expires)
                {
                    this._expirationTime -= gameTime.ElapsedRealTime.TotalMilliseconds;
                    if (this._expirationTime <= 0)
                    {
                        this._visible = false;
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this._blinkVisible && this._visible)
            {
                this._font.DrawString(spriteBatch, this._statusText, this._missionStatusPosition, new Vector2(1f, 1f), this._color);
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        public override void MissionStateChanged()
        {
            this._visible = false;
            this._missionStatusBlink = 0;
            this._blinkVisible = false;
            this._expirationTime = 0;
            this._expires = false;
            switch (Stage.CurrentStage.MissionState)
            {
                case Stage.MissionStateType.Starting:
                case Stage.MissionStateType.Running:
                    this._visible = true;
                    this._statusText = Formater.FormatGoalDescription(Stage.CurrentStage.LevelStage, true);
                    this._color = Colors.HudItem_GoalDescription;
                    this._expirationTime = 5000;
                    this._expires = (Stage.CurrentStage.MissionState == Stage.MissionStateType.Running);
                    this._blinkVisible = true;
                    break;
                
                case Stage.MissionStateType.Completed:
                    this._visible = true;
                    this._statusText = LanguageManager.GetString("MSG_MISSION_COMPLETED");
                    this._color = Colors.HudItem_MsgStatusCompleted;
                    break;

                case Stage.MissionStateType.Failed:
                    this._visible = true;
                    this._statusText = LanguageManager.GetString("MSG_MISSION_FAILED");
                    this._color = Colors.HudItem_MsgStatusFailed;
                    break;
            }
        }
    }
}
