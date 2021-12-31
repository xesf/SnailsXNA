using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace FontEdit
{
    [DefaultPropertyAttribute("ImageId")]
    public class CFont
    {
        public struct FontData
        {
            public char? Character;
            public Rectangle Rect;
            public int Spacing;
            public int SpacingAfter;
        }
        private FontData[] _Characters;
        [BrowsableAttribute(false)]
        public FontData[]   Characters
        {
            set
            {
                this._Characters = value;
            }
            get
            {
                return this._Characters;
            }
        }

        private string _ImageFilename;
        [CategoryAttribute("Project Properties"), ReadOnlyAttribute(true)]
        public string ImageFilename
        {
            get
            {
                return this._ImageFilename;
            }
            set
            {
                this._ImageFilename = value;
            }
        }

        [CategoryAttribute("Project Properties"), ReadOnlyAttribute(false)]
        public string IngameExportFile { get; set; }

        private string _FontEditImageFilename;
        [CategoryAttribute("Project Properties"), ReadOnlyAttribute(true)]
        public string FontEditImageFilename
        {
            get
            {
                return this._FontEditImageFilename;
            }
            set
            {
                this._FontEditImageFilename = value;
            }

        }

        private int _SpaceWidth;
        [CategoryAttribute("Font Properties")]
        public int SpaceWidth
        {
            get { return _SpaceWidth; }
            set { _SpaceWidth = value; }
        }

        private int _BetweenCharsWidth;
        [CategoryAttribute("Font Properties")]
        public int BetweenCharsWidth
        {
            get { return _BetweenCharsWidth; }
            set { _BetweenCharsWidth = value; }
        }

		private int _LineHeight;
        [CategoryAttribute("Font Properties")]
		public int LineHeight
		{
			get { return _LineHeight; }
			set { _LineHeight = value; }
		}

        private int _ImageWidth;
        [BrowsableAttribute(false)]
        public int ImageWidth
        {
            get { return _ImageWidth; }
            set { _ImageWidth = value; }
        }

        private int _ImageHeight;
        [BrowsableAttribute(false)]
        public int ImageHeight
        {
            get { return _ImageHeight; }
            set { _ImageHeight = value; }
        }

        private int _NumCharsWidth;
        [CategoryAttribute("Font Properties"), ]
        public int NumCharsWidth
        {
            get { return _NumCharsWidth; }
            set { _NumCharsWidth = value; }
        }

        private int _NumCharsHeight;
        [CategoryAttribute("Font Properties")]
        public int NumCharsHeight
        {
            get { return _NumCharsHeight; }
            set { _NumCharsHeight = value; }
        }

        private string _ImageId;
        [CategoryAttribute("Project Properties")]
        public string ImageId
        {
            get
            {
                return this._ImageId;
            }
            set
            {
                this._ImageId = value;
            }
        }
        public CFont()
        {
            this._Characters = new FontData[0];
            _BetweenCharsWidth = 1;
            _SpaceWidth = 16;
            _LineHeight = 45;
            _NumCharsWidth = 16; // characters per row
            _NumCharsHeight = 16; // characters per column
        }

        public void GenerateDefaultFrames(int imgWidth, int imgHeight, int charsWidth, int charsHeight)
        {
            int pixelWidth = imgWidth / charsWidth;
            int pixelHeight = imgHeight / charsHeight;

            int numChars = charsWidth * charsHeight;
            if (this._Characters.Length == 0) // new character frames
            {
                this._Characters = new FontData[numChars];
                for (int i = 0; i < charsHeight; i++)
                {
                    for (int j = 0; j < charsWidth; j++)
                    {
                        if (j + i * charsWidth >= this._Characters.Length)
                            continue;
                        this._Characters[j + i * charsWidth].Character = null;
                        this._Characters[j + i * charsWidth].Rect = new Rectangle(j * pixelWidth, i * pixelHeight, pixelWidth, pixelHeight);
                    }
                }
            }
            else // current character frames
            {
                for (int i = 0; i < charsHeight; i++)
                {
                    for (int j = 0; j < charsWidth; j++)
                    {
                        if (j + i * charsWidth >= this._Characters.Length)
                            continue;
                        this._Characters[j + i * charsWidth].Rect = new Rectangle(j * pixelWidth, i * pixelHeight, pixelWidth, pixelHeight);
                    }
                }
            }
        }
    }
}
