using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.BrainEngine.Graphics;
using LevelEditor;
using System.Drawing;
using TwoBrainsGames.Snails.StageEditor.ToolboxItems;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    public partial class ImageListxItem : UserControl
    {
        protected const int LEFT_MARGIN = 10;
        protected const int TOP_MARGIN = 10;

        public event EventHandler ItemClicked;

        private IToolboxItem _toolboxItem;

        public Sprite Sprite 
        { 
            get { return (this.ToolboxItem == null? null : this.ToolboxItem.Sprite); } 
        }
        public int FrameNr 
        { 
            get { return (this.ToolboxItem == null? 0 : this.ToolboxItem.SpriteFrameNr); } 
        }

        protected IToolboxItem ToolboxItem 
        {
            get { return this._toolboxItem; } 
            set
            {
                this._toolboxItem = value;
                this.Width = this.ItemWidth + (ImageListxItem.LEFT_MARGIN * 2);
                this.Height = this.ItemHeight + (ImageListxItem.TOP_MARGIN * 2);
/*                if (this._toolboxItem != null)
                {
                    if (this._toolboxItem.Thumbnail != null)
                    {
                        this.Width = this._toolboxItem.Thumbnail.Width + (ImageListxItem.LEFT_MARGIN * 2);
                        this.Height = this._toolboxItem.Thumbnail.Width + (ImageListxItem.TOP_MARGIN * 2);
                    }
                    else
                    {
                        if (this._toolboxItem.Sprite != null &&
                            this._toolboxItem.Sprite.Frames.Length > this._toolboxItem.SpriteFrameNr)
                        {
                            this.Width = this._toolboxItem.Sprite.Frames[this._toolboxItem.SpriteFrameNr].Rect.Width + (ImageListxItem.LEFT_MARGIN * 2);
                            this.Height = this._toolboxItem.Sprite.Frames[this._toolboxItem.SpriteFrameNr].Rect.Height + (ImageListxItem.TOP_MARGIN * 2);
                        }
                    }
                }*/
            }
        }
        public bool _Selected;

        public bool Selected
        {
            get
            {
                return this._Selected;
            }
            set
            {
                if (this._Selected != value)
                {
                    this._Selected = value;
                    this.Refresh();

                }
            }
        }

        public int ItemWidth 
        {
            get 
            {
                if (this._toolboxItem != null)
                {
                    if (this._toolboxItem.Thumbnail != null)
                    {
                        return this._toolboxItem.Thumbnail.Width;
                    }
                    else
                    {
                        if (this._toolboxItem.Sprite != null &&
                            this._toolboxItem.Sprite.Frames.Length > this.FrameNr)
                        {
                            return this._toolboxItem.Sprite.Frames[this.FrameNr].Rect.Width;
                        }
                    }
                }    
                return 0;
            }
        }

        public int ItemHeight
        {
            get 
            {
                if (this._toolboxItem != null)
                {
                    if (this._toolboxItem.Thumbnail != null)
                    {
                        return this._toolboxItem.Thumbnail.Height;
                    }
                    else
                    {
                        if (this._toolboxItem.Sprite != null &&
                            this._toolboxItem.Sprite.Frames.Length > this.FrameNr)
                        {
                            return this._toolboxItem.Sprite.Frames[this.FrameNr].Rect.Height;
                        }
                    }
                }    
                return 0;
            }
        }

        protected Rectangle FrameRect
        {
            get
            {
                if (this._toolboxItem != null)
                {
                    if (this._toolboxItem.Thumbnail != null)
                    {
                        return new Rectangle(0, 0, this.ItemWidth, this.ItemHeight);
                    }
                    else
                    {
                        if (this._toolboxItem.Sprite != null &&
                            this._toolboxItem.Sprite.Frames.Length > this._toolboxItem.SpriteFrameNr)
                        {
                            Microsoft.Xna.Framework.Rectangle frameRect = this.Sprite.Frames[this.FrameNr].Rect;
                            return new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
                        }
                    }
                }
                return new Rectangle(0, 0, 32, 32);
            }
        }
        public ImageListxItem()
        {
            InitializeComponent();
            this.BackColor = UserSettings.ToolItemBackColor;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ImageListxItem_Paint(object sender, PaintEventArgs e)
        {
            try
            {

                if (this.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(195, 225, 255)),
                                                this.ClientRectangle);

                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(50, 180, 255)),
                                             this.ClientRectangle.Left,
                                             this.ClientRectangle.Top,
                                             this.ClientRectangle.Width - 1,
                                             this.ClientRectangle.Height - 1);

                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 220, 220)), this.ClientRectangle);
                }

                if (UserSettings.ShowImages)
                {
                   
                    if (this.ToolboxItem == null || this.ToolboxItem.ThumbnailType == ToolboxItems.ToolboxItem.ThumbnailSourceType.Sprite)
                    {
                        e.Graphics.DrawImage(this.Sprite.Image, ImageListxItem.LEFT_MARGIN, ImageListxItem.TOP_MARGIN,
                            this.FrameRect, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        e.Graphics.DrawImage(this.ToolboxItem.Thumbnail, ImageListxItem.LEFT_MARGIN, ImageListxItem.TOP_MARGIN, this.ToolboxItem.Thumbnail.Width, this.ToolboxItem.Thumbnail.Height);
                    }

                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ImageListxItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.OnItemClicked();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnItemClicked()
        {
            if (this.ItemClicked != null)
            {
                this.ItemClicked(this, new EventArgs());
            }
        }

    }
}
