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

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class InformationSignSelectForm : ObjectSelectedBaseForm
    {
        Settings.InformationSignItem SelectedInformationSign
        {
            get
            {
                return this._lstSigns.SelectedItem as Settings.InformationSignItem;
            }
        }

        public InformationSignSelectForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InformationSignSelectForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.RefreshList();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshList()
        {
            this._lstSigns.Items.Clear();
            foreach (Settings.InformationSignItem item in Settings.InformationSignItems)
            {
                this._lstSigns.Items.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlImage_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (this.SelectedInformationSign != null)
                {
                    if (this.SelectedInformationSign.Thumbnail != null)
                    {
                        Rectangle rc = new Rectangle(0, 0,
                                                    this.SelectedInformationSign.Thumbnail.Width,
                                                    this.SelectedInformationSign.Thumbnail.Height);
                        e.Graphics.DrawImage(this.SelectedInformationSign.Thumbnail,
                                             0,
                                             0, rc, GraphicsUnit.Pixel);
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
        private void _lstSigns_SelectedIndexChanged(object sender, EventArgs e)
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
                if (this.SelectedInformationSign != null)
                {
                    ((InformationSign)(this.CurrentSelectedObject)).Copy(this.SelectedInformationSign.InformationSign);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }
    }
}
