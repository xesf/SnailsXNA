using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    public partial class FadeInOutBoxPropsForm : ObjectPropsBaseForm
    {
        private FadeInOutBox Box
        {
            get { return (FadeInOutBox)this.GameObject; }
        }
        public FadeInOutBoxPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._rbFadedIn.Checked = (this.Box.IsFadedIn);
            this._rbFadedOut.Checked = (this.Box.IsFadedOut);
            this._chkAutofade.Checked = this.Box.AutoFade;
            this._numFadeInTime.Value = this.Box.FadeInTime;
            this._numFadeOutTime.Value = this.Box.FadeOutTime;
            this._numInitialDelay.Value = this.Box.InitialDelay;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Box.State = (this._rbFadedIn.Checked ?
                        FadeInOutBox.FadeInOutBoxState.FadedIn :
                        FadeInOutBox.FadeInOutBoxState.FadedOut);
            this.Box.AutoFade = this._chkAutofade.Checked;
            this.Box.FadeInTime = (int)this._numFadeInTime.Value;
            this.Box.FadeOutTime = (int)this._numFadeOutTime.Value;
            this.Box.InitialDelay = (int)this._numInitialDelay.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.EnableButtons();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            } 
        }

        private void EnableButtons()
        {
            this._gbAutofade.Enabled = this._chkAutofade.Checked;
        }

    }
}
