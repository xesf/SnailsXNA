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
        private void FadeInOutBoxPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._rbFadedIn.Checked = (this.Box.IsFadedIn);
                this._rbFadedOut.Checked = (this.Box.IsFadedOut);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FadeInOutBoxPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Box.State = (this._rbFadedIn.Checked ?
                                   FadeInOutBox.FadeInOutBoxState.FadedIn :
                                   FadeInOutBox.FadeInOutBoxState.FadedOut);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
