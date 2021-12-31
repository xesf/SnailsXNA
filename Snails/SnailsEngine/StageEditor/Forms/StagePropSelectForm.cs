using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;
using System.IO;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class StagePropSelectForm : ObjectSelectedBaseForm
    {
        StageProp SelectedStageProp
        {
            get
            {
                if (this._lstStageProps.SelectedItem == null)
                    return null;

                return (StageProp)this._lstStageProps.SelectedItem;
            }
        }

        public StagePropSelectForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PropSelectForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._lstStageProps.Items.Clear();
                foreach (KeyValuePair<string, StageProp.StagePropItem> propResource in StageProp.StagePropsItems)
                {
                    if (StageEditor.CurrentStageEdited.Stage.LevelStage.ThemeId == propResource.Value._theme)
                    {
                        this._lstStageProps.Items.Add(StageProp.Create(propResource.Key));
                    }
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlImage_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (this.SelectedStageProp != null)
                {
                    Microsoft.Xna.Framework.Rectangle frameRect = this.SelectedStageProp.Sprite.Frames[0].Rect;
                    Rectangle rc = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
                    e.Graphics.DrawImage(this.SelectedStageProp.Sprite.Image, 
                                         0,
                                         0, rc, GraphicsUnit.Pixel);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void _lstStageProps_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                this._pnlImage.Refresh();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedStageProp != null)
                {
                    ((StageProp)(this.CurrentSelectedObject)).Copy(this.SelectedStageProp);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
