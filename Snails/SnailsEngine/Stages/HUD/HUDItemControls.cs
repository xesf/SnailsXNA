using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.UI.Controls;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    // Pause, Play, Fast forward, restart controls
    class HUDItemControls : HUDItem, ICursorInteractable
    {
        const float MARGIN = 10f;
        const float ITEM_SPACING = 20f;
        const float MARGIN_TOP = 5f;
        private static Color DefaultIconColor = Colors.StageHUDInfoColor;
        private static Color SelectedIconColor = new Color(125, 125, 20);

        protected enum ControlButtonType
        {
            Pause,
            Play,
            FastForward,
            Restart,
            EndStage
        }

        class ControlButton
        {
            public delegate void ControlButtonEvent();
            public event ControlButtonEvent OnSelect;

            public Vector2 _position;
            public ControlButtonType _buttonType;
            public bool _visible;
            public BoundingCircle _bsButton;
            public Color _color;
            public Color _defaultColor;
            UITimer _timer;
            public GameplayInput.GamePlayButtons _hotKey;

            public ControlButton(ControlButtonType buttonType, Color color, ControlButtonEvent onSelectMethod, GameplayInput.GamePlayButtons hotKey)
            {
                this._buttonType = buttonType;
                this._visible = true;
                this._bsButton = new BoundingCircle();
                this._color = color;
                this._defaultColor = color;
                this._timer = new UITimer(null, 50, false);
                this._timer.Enabled = false;
                this._timer.OnTimer += new UIControl.UIEvent(_timer_OnTimer);
                this.OnSelect += new ControlButtonEvent(onSelectMethod);
                this._hotKey = hotKey;
            }

            /// <summary>
            /// 
            /// </summary>
            void _timer_OnTimer(IUIControl sender)
            {
                this._color = HUDItemControls.DefaultIconColor;
                this.OnSelect();
            }

            public void Select()
            {
                this._timer.Reset();
                this._timer.Enabled = true;
                this._color = HUDItemControls.SelectedIconColor;
            }

            public void Update(BrainGameTime gameTime)
            {
                if ((Stage.CurrentStage.Input.GameButtons & this._hotKey) == this._hotKey && 
                     this._hotKey != GameplayInput.GamePlayButtons.None)
                {
                    this.Select();
                }
                if (this._timer.Enabled)
                {
                    this._timer.Update(gameTime);
                }
            }
        }

 
//        Sprite _spriteButton;
        Sprite _spriteIcon;
        List<ControlButton> _buttons;
        Vector2 _itemSize;
        ControlButton _warpButton;
        ControlButton _endStageButton;
        float _scale; // Depends on platform
        bool _isInsideControl;
        BoundingSquare _bbController; // The bounding box for the control

        /// <summary>
        /// 
        /// </summary>
        public override void StageAreaChanged(BoundingSquare newStageArea)
        {
            this._itemSize = new Vector2(this._spriteIcon.Frames[0].Width * this._scale, this._spriteIcon.Frames[0].Height * this._scale);
            this._position = new Vector2((MARGIN * this._scale), newStageArea.Bottom - this._itemSize.Y - (MARGIN * this._scale));
            Vector2 position = this._position;

            for (int i = 0; i < this._buttons.Count; i++)
            {
                this._buttons[i]._position = position + new Vector2(0f, MARGIN_TOP);
                this._buttons[i]._bsButton = this._spriteIcon._boundingSpheres[0].Transform(position);
                this._buttons[i]._bsButton._radius *= this._scale;
                position += new Vector2(this._itemSize.X + (ITEM_SPACING * this._scale), 0f);
            }

            this.UpdateControllerBB();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoadContent()
        {
//            this._spriteButton = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "GameplayControlsButton");
            this._spriteIcon = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "GameplayControls");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize(Vector2 position)
        {
            this._buttons = new List<ControlButton>();

            this._buttons.Add(new ControlButton(ControlButtonType.Restart, HUDItemControls.DefaultIconColor, this.Button_Restart, GameplayInput.GamePlayButtons.RestartStage));
            this._buttons.Add(new ControlButton(ControlButtonType.Pause, HUDItemControls.DefaultIconColor, this.Button_PauseGame, GameplayInput.GamePlayButtons.Pause));
            this._warpButton = new ControlButton(ControlButtonType.FastForward, HUDItemControls.DefaultIconColor, this.Button_FastForward, GameplayInput.GamePlayButtons.TimeWarp);
            this._buttons.Add(this._warpButton);
            this._endStageButton = new ControlButton(ControlButtonType.EndStage, HUDItemControls.DefaultIconColor, this.Button_EndMission, GameplayInput.GamePlayButtons.None);
            this._endStageButton._visible = false;
            this._buttons.Add(this._endStageButton);

            this._scale = 1f;
            if (SnailsGame.GameSettings.PresentationMode == Configuration.GameSettings.PresentationType.LD)
            {
//                this._scale = 0.8f;
            }

            this._isInsideControl = false;
            this.UpdateControllerBB();

        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateControllerBB()
        {
            float top = this._buttons[0]._bsButton._center.Y - this._buttons[0]._bsButton._radius;
            float left = this._buttons[0]._bsButton._center.X - this._buttons[0]._bsButton._radius;
            float right = 0;
            float bottom = 0;

            foreach(ControlButton button in this._buttons)
            {
                if (button._visible)
                {
                    if (right < button._bsButton._center.X + button._bsButton._radius)
                    {
                        right = button._bsButton._center.X + button._bsButton._radius;
                    }
                    if (bottom < button._bsButton._center.Y + button._bsButton._radius)
                    {
                        bottom = button._bsButton._center.Y + button._bsButton._radius;
                    }
                }
            }
            this._bbController = new BoundingSquare(new Vector2(left, top), new Vector2(right, bottom) );
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);
            this._isInsideControl = this._bbController.Contains(Stage.CurrentStage.Input.MotionPosition);
            if (this._isInsideControl && !Stage.CurrentStage.Cursor.IsInteractingWithObject)
            {
                Stage.CurrentStage.Cursor.SetInteractingObject(this);
            }

            foreach (ControlButton button in this._buttons)
            {
                if (button._visible)
                {
                    button.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UIScreen.ClearTexture, this._bbController.ToRect(), new Color(0, 0, 0, 150));

            foreach (ControlButton button in this._buttons)
            {
                if (button._visible)
                {
                    this._spriteIcon.Draw(button._position, (int)button._buttonType, 0f, SpriteEffects.None, button._color, this._scale, spriteBatch);
                }
            }
#if DEBUG
            if (SnailsGame.Settings.ShowBoundingBoxes)
            {
                this._bbController.Draw(Color.Red, Vector2.Zero);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void Button_PauseGame()
        {
            Stage.CurrentStage.PauseGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Button_Restart()
        {
            Stage.CurrentStage.RestartMission();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Button_EndMission()
        {
            Stage.CurrentStage.EndMission();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Button_FastForward()
        {
            this.ToggleTimeWarp();
        }


        /// <summary>
        /// 
        /// </summary>
        public void ToggleTimeWarp()
        {
            Stage.CurrentStage.ToggleTimeWarp();

            
            if (Stage.CurrentStage.InTimeWarp)
            {
                this._warpButton._buttonType = ControlButtonType.Play;
            }
            else
            {
                this._warpButton._buttonType = ControlButtonType.FastForward;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void MissionStateChanged()
        {
            if (Stage.CurrentStage.MissionState == Stage.MissionStateType.Completed)
            {
                if (Stage.CurrentStage.Stats.NumSnailsActive + Stage.CurrentStage.Stats.NumSnailsToRelease> 0)
                {
                    this._endStageButton._visible = true;
                    this.UpdateControllerBB();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ControlButton GetButton(Vector2 position)
        {
            for (int i = 0; i < this._buttons.Count; i++)
            {
                if (this._buttons[i]._bsButton.Contains(Stage.CurrentStage.Input.MotionPosition) && this._buttons[i]._visible)
                {
                    return this._buttons[i];
                }
            }

            return null;
        }

        #region ICursorInteractable Members

        public StageCursor.CursorType QueryCursor()
        {
            return StageCursor.CursorType.Select;
        }

        public bool QueryInterating()
        {
            return (this._isInsideControl);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionPressed(Vector2 cursorPos)
        {
            ControlButton button = this.GetButton(Stage.CurrentStage.Input.MotionPosition);
            if (button != null)
            {
                button.Select();
            }     
        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionReleased()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void CursorActionSelected()
        {
          
        }

        public bool CanAcceptCursorInteraction
        {
            get { return this._visible == true; }
        }
        #endregion
    }
}
