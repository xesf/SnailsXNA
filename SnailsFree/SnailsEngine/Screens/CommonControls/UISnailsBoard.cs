using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Effects;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
	class UISnailsBoard : UIControl
	{
		public enum BoardType
		{
			LightWoodMedium,
			LightWoodLongNarrow,
			LeafsMedium,
			LightWoodMediumLong
		}

		#region Vars
		private BoardType _type;
		private Sample _hideSample;
		private Sample _showSample;
		#endregion

		#region Properties
		private UIImage _imgBackground;
		public UIImage BoardImage { get { return this._imgBackground; } }
		public BoardType Type 
		{
			get { return this._type; }
			set
			{
				this._type = value;
				switch (this._type)
				{
				case BoardType.LightWoodMedium:
					this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards/LightWoodMedium");
					break;
				case BoardType.LightWoodLongNarrow:
					this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards/LightWoodLongNarrow");
					break;
				case BoardType.LeafsMedium:
					this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/ingame-elements-1/LeafsMedium");
					break;
				case BoardType.LightWoodMediumLong:
					this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards/LightWoodMediumLong");
					break;

				}
				this.Size = this._imgBackground.Size;
			}
		}

		public UIImage ImgBackground
		{
			get { return _imgBackground; }
			set { _imgBackground = value; }
		}

		public Vector2 ImagePosition {
			get {
				return this._imgBackground.Position;
			}
			set {
				this._imgBackground.Position = value;
			}
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public UISnailsBoard(UIScreen screenOwner, BoardType type) :
		base(screenOwner)
		{
			// Background image
			this._imgBackground = new UIImage(screenOwner);
			this._imgBackground.ParentAlignment = BrainEngine.UI.AlignModes.Horizontaly;
			this.Controls.Add(this._imgBackground);

			// 
			this.ShowEffect = new SquashEffect(0.85f, 4.0f, 0.03f, this.BlendColor, this.Scale);
			this.HideEffect = new PopOutEffect(new Vector2(1.2f, 1.2f), 6.0f);

			this.OnHideBegin += new UIEvent(UISnailsBoard_OnHideBegin);
			this.OnShowBegin += new UIEvent(UISnailsBoard_OnShowBegin);
			this.Type = type;

			this._hideSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BOARD_HIDDEN);
			this._showSample = BrainGame.ResourceManager.GetSampleStatic(AudioTags.BUTTON_SHOWN);
		}


		void UISnailsBoard_OnShowBegin(IUIControl sender)
		{
			this._showSample.Play();
		}

		void UISnailsBoard_OnHideBegin(IUIControl sender)
		{
			this._hideSample.Play();
		}

	}
}
