using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Resources;
using Microsoft.Xna.Framework.Input;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UITextFontLabelInput : UITextFontLabel
    {
        UITimer _backTimer;  // delete text timer
        UITimer _backTimer2; // delete text timer
        int _maxLength = 16; // default max lenght

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public UITextFontLabelInput(UIScreen screenOwner, TextFont font) :
            this(screenOwner, font, string.Empty)
        { }

        /// <summary>
        /// 
        /// </summary>
        public UITextFontLabelInput(UIScreen screenOwner, TextFont font, string text) :
            base(screenOwner, font, text)
        {
            _backTimer = new UITimer(screenOwner, 75, true);
            _backTimer.OnTimer += backTimer_OnTimer;
            _backTimer.Enabled = true;

            _backTimer2 = new UITimer(screenOwner, 50, true);
            _backTimer2.OnTimer += backTimer2_OnTimer;
            _backTimer2.Enabled = false;

            this.Controls.Add(_backTimer);
            this.Controls.Add(_backTimer2);
        }

        public bool HasText()
        {
            return (!string.IsNullOrEmpty(base.Text));
        }

        private void backTimer_OnTimer(IUIControl sender)
        {
            if (this.ScreenOwner.InputController.Keyboard.IsKeyDown(Keys.Back))
            {
                if (!_backTimer2.Enabled)
                {
                    _backTimer2.Enabled = true;
                }
            }
        }

        private void backTimer2_OnTimer(IUIControl sender)
        {
            if (this.ScreenOwner.InputController.Keyboard.IsKeyDown(Keys.Back))
            {
                if (!string.IsNullOrEmpty(base.Text))
                {
                    base.Text = base.Text.Remove(base.Text.Length - 1, 1);
                }
            }
        }

        public override void Update(BrainGameTime gameTime)
        {
            base.Update(gameTime);

            // delete text
            if (this.ScreenOwner.InputController.Keyboard.IsKeyReleased(Keys.Back))
            {
                _backTimer.Enabled = false;
                _backTimer.Reset();
                _backTimer2.Enabled = false;
                _backTimer2.Reset();
            }
            else if (this.ScreenOwner.InputController.Keyboard.IsKeyPressed(Keys.Back))
            {
                if (!string.IsNullOrEmpty(base.Text))
                {
                    base.Text = base.Text.Remove(base.Text.Length - 1, 1);
                }
                _backTimer.Enabled = false;
                _backTimer.Reset();
            }
            else if (this.ScreenOwner.InputController.Keyboard.IsKeyDown(Keys.Back))
            {
                _backTimer.Enabled = true;
            }
            else
            {
                // write text
                Keys[] keys = this.ScreenOwner.InputController.Keyboard.GetPressedKeys();
                if (keys != null && keys.Length > 0)
                {
                    foreach (Keys k in keys)
                    {
                        ProcessKey(k);
                    }
                }
            }
        }

        /// <summary>
        /// If the key is allowed to be displayed or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool IsAllowed(Keys key)
        {
            int k = (int)key;
            return (IsNumber(k) || (k >= 65 && k <= 90) || k == 32); // 0-9 | A-Z | space | return
        }

        private bool IsNumber(Keys key)
        {
            int k = (int)key;
            return IsNumber(k);
        }

        private bool IsNumber(int k)
        {
            return (k >= 48 && k <= 57);
        }

        private void ProcessKey(Keys k)
        {
            if (base.Text.Length > _maxLength)
                return;

            // if char is allowed and is pressed (down and up)
            if (IsAllowed(k) && this.ScreenOwner.InputController.Keyboard.IsKeyPressed(k))
            {
                char key = (char)k; // get character value

                if (k == Keys.Space)
                {
                    base.Text += " ";
                }
                else
                {
                    bool shift = false;

                    // is shift key enabled
                    if (this.ScreenOwner.InputController.Keyboard.IsShiftDown)
                        shift = true;

                    if (!shift && !IsNumber(key))
                    {
                        key += (char)32;
                    }

                    base.Text += key.ToString();
                }
            }
        }
    }
}
