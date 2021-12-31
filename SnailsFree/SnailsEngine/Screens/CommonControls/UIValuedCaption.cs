using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
	class UIValuedCaption: UIControl
	{
		const int TOTAL_VALUE_COUNTER_TIME = 1000;
		const int COUNTER_TIMER = 50;

		enum ValueType
		{
			None,
			Int,
			Time,
			String
		}

		public enum CaptionMode
		{
			Simple,                 // TEXT            VAL
			Fraction,               // TEXT            VAL / FRACTION
			Multiplier,             // TEXT            VAL * MULTIPLIER
			Fulltime,               // TEXT            mm:ss,mmm
			FulltimeHours           // TEXT            hh:mm:ss,mmm
		}

		public enum ValueAlignmentMode
		{
			Right,
			Left
		}

		#region Members
		UICaption _capText;
		UICaption _capValue;
		UITimer _tmrTimer;
		object _value;
		Sample _pointIncSample;
		ValueAlignmentMode _captionAlignment;
		float _captionSpacing;
		#endregion


		// Captions support three formats:
		//   TEXT        VAL
		//   TEXT        VAL / FRACTION     ->For instance    SNAILS DELIVERED       5/10
		//   TEXT        VAL * MULTIPLIER   ->For instance    SNAILS BONUS           5*10

		public Color ValueColor 
		{
			get { return this._capValue.BlendColor; }
			set {
				this._capValue.BlendColor = value;
				if (this._capValue.ShowEffect is SquashEffect) // Set the color on the effect or else object will lose it's color
				{
					((SquashEffect)this._capValue.ShowEffect).BlendColor = value;
				}
			}
		}
		public Color CaptionColor
		{
			get { return this._capText.BlendColor; }
			set {
				this._capText.BlendColor = value;
				if (this._capText.ShowEffect is SquashEffect)
				{
					((SquashEffect)this._capText.ShowEffect).BlendColor = value;
				}
			}
		}
		public object Value 
		{
			get { return this._value; }
			set
			{
				this._value = value;
				if (value is int)
				{
					this.ValType = ValueType.Int;
					this.ValueCounter = 0;

					if (this.AutoComputeIncrement)
					{
						this.Increment = (int)Math.Ceiling((double)((double)(int)this.Value * COUNTER_TIMER / TOTAL_VALUE_COUNTER_TIME));
						if (this.Increment <= 0)
						{
							this.Increment = 1;
						}
					}
				}
				else
					if (value is TimeSpan)
					{
						this.ValType = ValueType.Time;
						this.ValueCounter = new TimeSpan(0, 0, 0);

						if (this.AutoComputeIncrement)
						{
							this.Increment = (int)Math.Ceiling((((TimeSpan)this.Value).TotalSeconds * COUNTER_TIMER / TOTAL_VALUE_COUNTER_TIME));
							if (this.Increment <= 0)
							{
								this.Increment = 1;
							}
						}
					}
					else
						if (value is string)
						{
							this.ValType = ValueType.String;
							this.ValueCounter = value;
						}

			}
		}
		public object FractionValue { get; set; }
		public int MultiplierValue { get; set; }
		object ValueCounter { get; set; } // Used to control the value when it is incrementing
		ValueType ValType { get; set; }
		public CaptionMode Mode { get; set; }
		public int Increment { get; set; }
		public bool AnimateValue { get; set; }
		public bool AutoComputeIncrement { get; set; }

		public override string Text
		{
			get { return this._capText.Text; }
			set { this._capText.Text = value; }
		}

		public Size CaptionSize
		{
			get { return this._capText.Size; }
		}

		public Vector2 ValuePosition
		{
			get { return (this.Position + this._capValue.Position); }
			set { this._capValue.Position = value; }
		}

		public ValueAlignmentMode CaptionAlignment
		{
			get
			{
				return this._captionAlignment;
			}
			set
			{
				this._captionAlignment = value;
				this.Refresh();
			}
		}

		public float CaptionSpacing
		{
			get
			{
				return this._captionSpacing;
			}
			set
			{
				this._captionSpacing = value;
				this.Refresh();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public UIValuedCaption(UIScreen screenOwner, string text, object value, Color color, Color valColor, UICaption.CaptionStyle style, float width, bool playShowSound) :
		base(screenOwner)
		{
			this.Initialize (screenOwner, text, value, color, valColor,  style, width, playShowSound, true);
		}

		/// <summary>
		/// 
		/// </summary>
		public UIValuedCaption(UIScreen screenOwner, string text, object value, Color color, Color valColor, UICaption.CaptionStyle style, float width, bool playShowSound, bool withShowEffect) :
		base(screenOwner)
		{
			this.Initialize (screenOwner, text, value, color, valColor,  style, width, playShowSound, withShowEffect);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Initialize(UIScreen screenOwner, string text, object value, Color color, Color valColor, UICaption.CaptionStyle style, float width, bool playShowSound, bool withShowEffect)
		{
			this.AnimateValue = true;
			// Timer
			this._tmrTimer = new UITimer(screenOwner, COUNTER_TIMER, true);
			this._tmrTimer.OnTimer += new UIEvent(_tmrTimer_OnTimer);
			this.Controls.Add(this._tmrTimer);
			this.Value = value;

			// Caption
			this._capText = new UICaption(screenOwner, "", color, style);
			this._capText.TextResourceId = text;
			this._capText.OnShow += new UIEvent(_capText_OnShow);
			if (withShowEffect) {
				this._capText.ShowEffect = new SquashEffect (0.9f, 4.0f, 0.04f, this._capText.BlendColor, this._capText.Scale);
			}
			this.Controls.Add(this._capText);

			// Value
			this._capValue = new UICaption(screenOwner, (this.Value != null ? this.Value.ToString() : ""), valColor, style);
			this._capValue.ParentAlignment = BrainEngine.UI.AlignModes.Right;
			if (withShowEffect) {
				this._capValue.ShowEffect = new SquashEffect(0.9f, 4.0f, 0.04f, this._capValue.BlendColor, this._capValue.Scale);
			}
			this.Controls.Add(this._capValue);

			this.Size = new Size(width, this._capText.Size.Height);
			this.Increment = 1;
			this.AcceptControllerInput = false;
			this.AutoComputeIncrement = true;
			this.Refresh();
			this._pointIncSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.POINT_INCREMENT);
			if (playShowSound)
			{
				this.ShowSoundEffect = BrainGame.ResourceManager.GetSampleStatic(AudioTags.CAPTION_SHOW);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Update(BrainEngine.BrainGameTime gameTime)
		{
			base.Update(gameTime);
			if (this.AnimateValue == false)
			{
				this.ValueCounter = this.Value;
			}
			switch (this.ValType)
			{
			case ValueType.Int:
				switch (this.Mode)
				{
				case CaptionMode.Simple:
					this._capValue.Text = string.Format("{0}", this.ValueCounter);
					break;
				case CaptionMode.Fraction:
					this._capValue.Text = string.Format("{0}/{1}", this.ValueCounter, this.FractionValue);
					break;
				case CaptionMode.Multiplier:
					this._capValue.Text = string.Format("{0}X{1}", this.ValueCounter, this.MultiplierValue);
					break;
				}
				break;

			case ValueType.Time:
				TimeSpan valueCounter = (TimeSpan)this.ValueCounter;
				TimeSpan ts = valueCounter;

				switch (this.Mode)
				{
				case CaptionMode.Simple:

					string capStr = string.Format("{0:00}:{1:00}", (ts.Hours * 60) + ts.Minutes, ts.Seconds);
					if (ts.Hours > 0) // fix displayed time (we still keep the original timer
					{
						capStr = "60:00";
					}
					this._capValue.Text = capStr;
					break;
				case CaptionMode.Fraction:
					throw new SnailsException("Not implemented");
				case CaptionMode.Multiplier:
					this._capValue.Text = string.Format("{0}X{1}", (int)ts.TotalSeconds, this.MultiplierValue);
					break;
				case CaptionMode.Fulltime:
					this._capValue.Text = string.Format("{0:00}:{1:00},{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);
					break;
				case CaptionMode.FulltimeHours:
					this._capValue.Text = string.Format("{0:00}:{1:00}:{2:00},{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
					break;
				}
				break;
			case ValueType.String:
				this._capValue.Text = (string)this.Value;
				break;
			}

		}


		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			this._tmrTimer.Enabled = false;
		}


		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			switch (this.CaptionAlignment)
			{
			case ValueAlignmentMode.Right:
				this._capValue.ParentAlignment = AlignModes.Right;
				break;

			case ValueAlignmentMode.Left:
				this._capValue.ParentAlignment = AlignModes.None;
				this._capValue.Position = this._capText.Position + new Vector2(this._capText.Width + this._captionSpacing, 0f);
				break;
			}
		}

		/// <summary>
		/// QuickShow show the label whitout counters
		/// </summary>
		public void QuickShow()
		{
			this.ValueCounter = this.Value;
			this.Show();
		}


		/// <summary>
		/// 
		/// </summary>
		void _capText_OnShow(IUIControl sender)
		{
			if (this.AnimateValue)
			{
				this._tmrTimer.Enabled = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void _tmrTimer_OnTimer(IUIControl sender)
		{
			bool ended = false;

			switch (this.ValType)
			{
			case ValueType.Int:
				int val = (int)this.ValueCounter;
				val += this.Increment;
				if (val > (int)this.Value)
				{
					val = (int)this.Value;
					ended = true;
				}
				this.ValueCounter = val;
				break;

			case ValueType.Time:
				TimeSpan time = (TimeSpan)this.ValueCounter;
				time = time.Add(new TimeSpan(0, 0, this.Increment));
				if (time > (TimeSpan)this.Value)
				{
					time = (TimeSpan)this.Value;
					ended = true;
				}
				this.ValueCounter = time;
				break;

			default:
				ended = true;
				break;
			}

			if (ended)
			{
			this._tmrTimer.Enabled = false;
			this.InvokeOnShow();
			_pointIncSample.Stop();
			}
			else
			{
				_pointIncSample.Play(true);
			}
		}
	}
}
