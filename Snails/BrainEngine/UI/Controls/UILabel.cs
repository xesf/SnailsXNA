using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    /// <summary>
    /// Don't use this class directly
    /// Use UITextFontLabel or UISpriteFontLabel
    /// </summary>
    public class UILabel : UIControl
    {
        public const char LINE_SEPARATOR = '|';
        #region Vars
        protected bool _autosize;
 	    private HorizontalTextAligment _horizontalAligment;
	    private VerticalTextAligment _verticalAligment;
        #endregion

        #region Properties
        public override string Text
        {
            get { return base.Text; }
            set 
            {
                base.Text = value;
            
                this.TextLines = new string[0];
                if (value != null)
                {
                  this.TextLines = value.Split(LINE_SEPARATOR);
                }
                if (base.Text == null)
                {
                    base.Text = "";
                }
                this.CalculateSize();
            }
        }

	    public string [] TextLines
        {
           get;
           private set;
        }

        public bool Autosize
        {
            get { return this._autosize; }
            set 
            {
                if (this._autosize != value)
                {
                    this._autosize = value;
                    this.CalculateSize();
                }
            }

        }

	    public HorizontalTextAligment HorizontalAligment
        {
            get
	        {
                return this._horizontalAligment;
            }
            set
            {
              if (this._horizontalAligment != value)
              {
                 this._horizontalAligment = value;
                 this.CalculateSize();
              }
           }
        }

	    public VerticalTextAligment VerticalAligment
        {
           get
	        {
              return this._verticalAligment;;
           }
           set
           {
              if (this._verticalAligment != value)
              {
                 this._verticalAligment = value;
                 this.CalculateSize();
              }
           }
        }

        public int LineCount 
        { 
            get 
            {
                return (this.TextLines == null? 0: this.TextLines.Length);
            } 
        }

        #endregion

        public UILabel(UIScreen screenOwner) :
            base(screenOwner)
        {
            this.AcceptControllerInput = false;
            this.Autosize = true;
            this.HorizontalAligment = HorizontalTextAligment.Left;
            this.VerticalAligment = VerticalTextAligment.Top;
            this.Text = "";
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void CalculateSize()
        { }

    }
}
