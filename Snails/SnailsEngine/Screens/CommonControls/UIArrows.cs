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

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIArrow : UIControl
    {
        public enum ArrowType
        { 
            Up,
            Down,
            Left,
            Right,
        }

        public enum ArrowSize
        {
            Small,
            Medium
        }

        #region Constants
        public const string SPRITE_PATH = "spriteset/menu-elements-1/";
        public const string SPRITE_UP = SPRITE_PATH + "ArrowUp";
        public const string SPRITE_DOWN = SPRITE_PATH + "ArrowDown";
        public const string SPRITE_LEFT = SPRITE_PATH + "ArrowLeft";
        public const string SPRITE_RIGHT = SPRITE_PATH + "ArrowRight";
        public const string SPRITE_SMALL = "Small";
        #endregion

        #region Members
        protected UIPanel _container;
        protected UIImage _arrow;  
        protected ArrowType _type;
        protected ArrowSize _size = ArrowSize.Small;
        //private HooverEffect _outsideEffect;
        #endregion

        #region Properties
        public ArrowType Orientation
        {
            get { return this._type; }
            set
            {
                this._type = value;
                string resImg = string.Empty;
                switch (this._type)
                {
                    case ArrowType.Up:
                        resImg = SPRITE_UP;
                        break;
                    case ArrowType.Down:
                        resImg = SPRITE_DOWN;
                        break;
                    case ArrowType.Left:
                        resImg = SPRITE_LEFT;
                        break;
                    case ArrowType.Right:
                        resImg = SPRITE_RIGHT;
                        break;
                }

                if (_size == ArrowSize.Small)
                {
                    resImg += SPRITE_SMALL;
                }

                this._arrow.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(resImg);
                this._container.Size = this._arrow.Size;
                this.Size = this._arrow.Size;
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;

                if (this._container != null)
                {
                    this._container.Visible = value;
                }
                if (this._arrow != null)
                {
                    this._arrow.Visible = value;
 
                }
            }
        }
        #endregion

        public UIArrow(UIScreen screenOwner, ArrowType type, ArrowSize size) :
            this(screenOwner, type, size, null)
        { 
            
        }

        public UIArrow(UIScreen screenOwner, ArrowType type, ArrowSize size, HooverEffect effect) :
            base(screenOwner)
        {
            //_outsideEffect = effect;
            this.AcceptControllerInput = false; // by default don't accept actions in the control
                    
            this._container = new UIPanel(screenOwner);
            this._container.ParentAlignment = BrainEngine.UI.AlignModes.HorizontalyVertically;
            this.Controls.Add(this._container);

            this._arrow = new UIImage(screenOwner);
            this._container.Controls.Add(this._arrow);

            this._container.Size = this._arrow.Size;
            this.Orientation = type;
        }


        public void DoHoover(Vector2 pos)
        {
            if (this.Orientation == ArrowType.Left ||
                this.Orientation == ArrowType.Up)
            {
                this._arrow.Offset = pos;
            }
            else
            {
                this._arrow.Offset = -pos;
            }
        }

        public override void Draw()
        {
            base.Draw();
        }

    }
}
