using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Controls;

namespace TwoBrainsGames.BrainEngine.UI
{
    public enum UnitsType
    {
        Point, // 1 part in 10.000 - Use this unit type to draw resolution independent Screens
        // 5.000 x 5.0000 will always be center of the screen don't matter the resolution
        Pixel
    }

    [Flags]
    public enum AlignModes
    {
        None,
        Horizontaly = 0x01,
        Vertically = 0x02,
        HorizontalyVertically = Horizontaly | Vertically,
        Right = 0x04,
        Bottom = 0x08,
        Top = 0x100,
        Left = 0x200,

    }

    public enum TextAlignmentModes
    {
        Left,
        Right
    }


    [Flags]
    public enum PivotAlignModes
    {
        UpperLeft,
        Center,
        Custom
    }

    public struct Size
    {
        public float Width;
        public float Height;

        public Size(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public Size(Vector2 vector)
        {
            this.Width = vector.X;
            this.Height = vector.Y;
        }

        public static Size Zero { get { return new Size(0, 0); } }
        public override string ToString()
        {
            return string.Format("{0},{1}", this.Width, this.Height);
        }
    }

    public enum ImageSizeMode
    {
        None,
        Stretch,
        Center,
        Autosize,
        HorizontalCenter
    }

    public enum CursorModes
    {
        Free,           // A cursor that behaves like a mouse
        SnapToControl   // The cursor snaps to the controls
    }

    public class Margin
    {
        private float _left;
        private float _top;
        private float _right;
        private float _bottom;
        private UIControl _owner;

        public float Left
        {
            get { return this._left; }
            set 
            {
                if (this._left != value)
                {
                    this._left = value;
                    this._owner.MarginChanged();
                }
            }
        }
        public float Top
        {
            get { return this._top; }
            set
            {
                if (this._top != value)
                {
                    this._top = value;
                    this._owner.MarginChanged();
                }
            }
        }
        public float Right
        {
            get { return this._right; }
            set
            {
                if (this._right != value)
                {
                    this._right = value;
                    this._owner.MarginChanged();
                }
            }
        }
        public float Bottom
        {
            get { return this._bottom; }
            set
            {
                if (this._bottom != value)
                {
                    this._bottom = value;
                    this._owner.MarginChanged();
                }
            }
        }

        public Margin(UIControl owner)
        {
            this._owner = owner;
        }

        public Margin(float left, float top, float right, float bottom, UIControl owner )
        {
            this._left = left;
            this._top = top;
            this._right = right;
            this._bottom = bottom;
            this._owner = owner;
        }

        public void Clear()
        {
            this.Left = this.Top = this.Bottom = this.Right = 0;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", this.Left, this.Top, this.Right, this.Bottom);
        }
    }

    public enum MenuItemStyle
    {
        Image,
        Text,
        ImageAndText
    }

    public enum MenuItemPlacement
    {
        Horizontal, // Items are placed horzontally
        Vertical,   // Items are placed vertically
        Free        // Items in the menu have free placement, they control their position
    }


    public enum SliderOrientation
    {
        Horizontal,
        Vertical
    }

    public enum ScreenBackgroudImageMode
    {
        Normal,
        FitToScreen
    }

    public enum HorizontalTextAligment
    {
        Left,
        Right,
        Center
    }

    public enum VerticalTextAligment
    {
        Top,
        Bottom,
        Center
    }

    [Flags]
    public enum SnapDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        All = Left | Right | Up| Down
    }
}
