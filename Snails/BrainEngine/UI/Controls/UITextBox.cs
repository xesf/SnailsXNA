using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UITextBox : UITextFontLabel
    {
        public string _keyboardInputTitleResourceId;
        public string _keyboardInputDescriptionResourceId;

        public int InputLength { get; set; }
        public bool UsePasswordChar { get; set; }
        public string KeyboardInputTitle { get; set; }
        public string KeyboardInputDescription { get; set; }

        public virtual string KeyboardInputTitleResourceId
        {
            get
            {
                return this._keyboardInputTitleResourceId;
            }
            set
            {
                this._keyboardInputTitleResourceId = value;
                this.KeyboardInputTitle = LanguageManager.GetString(value);
            }
        }

        public virtual string KeyboardInputDescriptionResourceId
        {
            get
            {
                return this._keyboardInputDescriptionResourceId;
            }
            set
            {
                this._keyboardInputDescriptionResourceId = value;
                this.KeyboardInputDescription = LanguageManager.GetString(value);
            }
        }
        public UITextBox(UIScreen screenOwner, TextFont font, int inputLength) :
            base(screenOwner, font)
        {
            this.OnAccept += new UIEvent(UITextBox_OnAccept);
            this.BackgroundColor = new Color(0, 0, 0, 100);
            this.InputLength = inputLength;
            this.Size = new Size(2000f, 500f);
            this.AcceptControllerInput = true;
            this.Autosize = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void UITextBox_OnAccept(IUIControl sender)
        {
#if WP7 
            Guide.BeginShowKeyboardInput(PlayerIndex.One,
                   this.KeyboardInputTitle,
                   this.KeyboardInputDescription,
                   "",
                   ar => ((UITextBox)ar.AsyncState).Text = Guide.EndShowKeyboardInput(ar), this, UsePasswordChar);
#endif
        }
        
        /// <summary>
        /// 
        /// </summary>
        internal override void InternalLanguageChanged()
        {
            base.InternalLanguageChanged();
            if (!string.IsNullOrEmpty(this.TextResourceId))
            {
                this.KeyboardInputTitle = LanguageManager.GetString(this.KeyboardInputTitleResourceId);
            }
            if (!string.IsNullOrEmpty(this.TextResourceId))
            {
                this.KeyboardInputDescription = LanguageManager.GetString(this.KeyboardInputDescriptionResourceId);
            }
        }
    }
}
