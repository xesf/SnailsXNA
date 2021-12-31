using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages.HUD;

namespace TwoBrainsGames.Snails.ToolObjects
{
    class ToolEndMission : ToolObject
    {
        public enum EndMissionState
        {
            Start,
            Restart,
            EndMission
        }
        EndMissionState _state;
        Sprite _stateSprite;
        string [] _textLines;
        //string _iconText1;
        //string _iconText2;
        //Vector2 _iconText1Position;
        //Vector2 _iconText2Position;
        Vector2 _controllerHelpPosition;
        Vector2 _stateIconPosition;

        public EndMissionState State
        {
            set
            {
                this._state = value;
                this._textLines = null;
                switch (this._state)
                {
                    case ToolEndMission.EndMissionState.EndMission:
                        this._textLines = LanguageManager.GetMultiString("LBL_ICON_END_MISSION");
                        break;

                    case ToolEndMission.EndMissionState.Restart:
                        this._textLines = LanguageManager.GetMultiString("LBL_ICON_RESTART");
                        break;
                }
                this.ComputeTextPosition(this._textLines);
            }
        }
        public ToolEndMission()
            : base(ToolObjectType.EndMission)
        {
            this._withQuantity = false;
            this._state = EndMissionState.Restart;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
            this._spriteWhenUnselected = BrainGame.ResourceManager.GetSpriteStatic("spriteset/StageHUD", "ToolIcon");
            this._spriteWhenSelected = BrainGame.ResourceManager.GetSpriteStatic("spriteset/StageHUD", "ToolIconSelected");
            this.Sprite = this._spriteWhenUnselected;
            this._stateSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/StageHUD", "MissionToolStates");
            this._spriteShortcurKeys = BrainGame.ResourceManager.GetSpriteTemporary(SpriteResources.TOOLS_SHORTCUT_KEYS);
            this.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook", ResourceManager.ResourceManagerCacheType.Static);
            this.State = EndMissionState.Start;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetPosition(Vector2 pos)
        {
            base.SetPosition(pos);
            this.ComputeTextPosition(this._textLines);
            this._controllerHelpPosition = pos - new Vector2(5f, 5f);
            this._stateIconPosition = pos + new Vector2(30f, 10f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            this.Sprite.Draw(this.Position, this.SpriteBatch);
            this._stateSprite.Draw(this._stateIconPosition, (int)this._state, this.SpriteBatch);

            if (SnailsGame.GameSettings.UseGamepad)
            {
                StageHUD.SpriteXBoxHelp.Draw(this._controllerHelpPosition, StageHUD.XBOX_HELP_END_MISSION_FRAME_NR, this.SpriteBatch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Select()
        {
            this.Action(Vector2.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Action(Vector2 position)
        {
            switch (this._state)
            {
                case EndMissionState.Start:
                    Stage.CurrentStage.StartupEnded();
                    break;

                case EndMissionState.EndMission:
                    Stage.CurrentStage.EndMission();
                    break;

                case EndMissionState.Restart:
                    Stage.CurrentStage.RestartMission();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeTextPosition(string [] text)
        {
            /*if (text == null || text.Length == 0)
            {
                return;
            }

            this._iconText1 = text[0];
            this._iconText2 = null;
            if (text.Length == 1)
            {
                float w = this.Font.MeasureString(this._iconText1);
                this._iconText1Position = this.Position + new Vector2((this.Sprite.Frames[0].Width / 2) - (w / 2), 26f);
            }
            else
            {
                this._iconText2 = text[1];
                float w = this.Font.MeasureString(this._iconText1);
                this._iconText1Position = this.Position + new Vector2((this.Sprite.Frames[0].Width / 2) - (w / 2), 18f);

                w = this.Font.MeasureString(this._iconText2);
                this._iconText2Position = this.Position + new Vector2((this.Sprite.Frames[0].Width / 2) - (w / 2), 37f);
            }*/
        }
    }
}
