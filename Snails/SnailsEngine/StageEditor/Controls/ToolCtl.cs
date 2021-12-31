using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.ToolObjects;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    public partial class ToolCtl : UserControl
    {
        public event EventHandler ToolQuantityChanged;
        ToolObject _Tool;

        /// <summary>
        /// 
        /// </summary>
        public ToolObject Tool
        {
            get { return this._Tool; }
            set 
            {
                if (this._Tool != value)
                {
                    this._Tool = value;
                    if (this._Tool != null)
                    {
                        Rectangle rc = this.GetToolRect();
                        this._pnlTool.Width = rc.Width;
                        this._pnlTool.Height = rc.Height;
                        this.Width = (this._pnlTool.Left * 2) + this._pnlTool.Width;
                  //      this._udQuantity.Top = this._pnlTool.Top + this._pnlTool.Height + 5;
                  //      this._udQuantity.Left = (this.Width / 2) - (this._udQuantity.Width / 2);
                        this.Refresh();
                        this._udQuantity.Value = this._Tool.Quantity;
                    }
                }
            }
        }

        public int Quantity 
        {
            set 
            { 
                if (this.Tool == null)
                    return;

                this.Tool.Quantity = value;
                this._udQuantity.Value = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ToolCtl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawCharCallBack(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 position, TextFontChar charToPrint, Microsoft.Xna.Framework.Color color, object param, Microsoft.Xna.Framework.Vector2 scale)
        {
            Graphics graphics = (Graphics)param;
            if (this.Tool.Font.Image == null)
                this.Tool.Font.UpdateImage();
            Rectangle rc = new Rectangle(charToPrint.Rectangle.Left,
                                         charToPrint.Rectangle.Top,
                                         charToPrint.Rectangle.Width, charToPrint.Rectangle.Height);


            graphics.DrawImage(this.Tool.Font.Image,
								(int)position.X + (int)this.Tool.Sprite.BoundingBox.UpperLeft.X,
								(int)position.Y + (int)this.Tool.Sprite.BoundingBox.UpperLeft.Y, 
                                rc, GraphicsUnit.Pixel);

        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlTool_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (this.Tool == null)
                    return;
#if STAGE_EDITOR
                e.Graphics.DrawImage(this.Tool._iconSprite.Image, 0, 0, this.GetToolRect(), GraphicsUnit.Pixel);
#endif
        //        this.Tool.Font.DrawStringWithCallback(null, this.Tool.Quantity.ToString(), new Microsoft.Xna.Framework.Vector2(0, 0), Microsoft.Xna.Framework.Color.White, this.DrawCharCallBack, e.Graphics, new Microsoft.Xna.Framework.Vector2(1.0f, 1.0f));
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Rectangle GetToolRect()
        {
            if (this.Tool == null)
                return new Rectangle(0, 0, 0, 0);

            Microsoft.Xna.Framework.Rectangle frameRect = this.Tool._iconSprite.Frames[Tool.CurrentFrame].Rect;
            return new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _udQuantity_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Tool != null)
                {
                    this.Tool.Quantity = (int)this._udQuantity.Value;
                    this.Refresh();

                    if (this.ToolQuantityChanged != null)
                    {
                        this.ToolQuantityChanged(this, new EventArgs());
                    }
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }
    }
}
