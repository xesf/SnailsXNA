using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Localization;
using System.Drawing;

namespace TwoBrainsGames.Snails.Winforms
{
    public partial class ScreenModeForm : Form
    {
        public ScreenModeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnWindow_Click(object sender, EventArgs e)
        {
            SnailsGame.GameSettings.IsFullScreen = false;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnFullscreen_Click(object sender, EventArgs e)
        {
            SnailsGame.GameSettings.IsFullScreen = true;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ScreenModeForm_Load(object sender, EventArgs e)
        {
            this._picSnails.Image = (System.Drawing.Image)new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().
                                                GetManifestResourceStream("TwoBrainsGames.Snails.Winforms.snails-logo.png"));

            this._btnFullscreen.Text = LanguageManager.GetString("BTN_FULLSCREEN");
            this._btnWindow.Text = LanguageManager.GetString("BTN_WINDOWED");
            this._lblMessage.Text = LanguageManager.GetString("LBL_SEL_SCR_MODE");
            this._lblInfo.Text = LanguageManager.GetString("LBL_MODE_CHANGE");

        }
    }
}
