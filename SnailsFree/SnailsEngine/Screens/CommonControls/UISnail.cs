using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UISnail : UIControl
    {
        enum SnailDirection
        {
            Clowckwise = 1,
            CounterClockwise = -1
        }

        enum SnailWalkEdge
        {
             Bottom,
             Right,
             Top,
             Left
       }

        enum SnailState
        {
            Walking,
            Turning,
            Falling
        }
        #region Vars
        private SpriteAnimation _snailAnim;
        private SnailWalkEdge _walkEdge;
        private SnailDirection _direction;
        private Sprite _spriteWalk;
        private Sprite _spriteTurn;
        private SnailState _state;
        private float _snailWidth;
        private float _snailHeight;
        #endregion

        #region Properties
        private SnailState State 
        {
            get { return this._state; }
            set
            {
                this._state = value;
                switch(this._state)
                {
                    case SnailState.Turning:
                        this._snailAnim.Sprite = this._spriteTurn;
                        break;
                    case SnailState.Walking:
                        this._snailAnim.Sprite = this._spriteWalk;
                        break;
                    case SnailState.Falling:
                        this._snailAnim.Sprite = this._spriteWalk;
                        this.Effect = new GravityEffect(SnailsGame.GameSettings.Gravity, 0f);
                        this.Rotation = 0; 
                        break;

                }
                this._snailAnim.CurrentFrame = 0;
            }
        }
        public float Speed { get; set; }
        private SnailDirection Direction 
        {
            get { return this._direction; }
            set
            {
                this._direction = value;
                this._snailAnim._spriteEffect = (this._direction == SnailDirection.Clowckwise ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            }
        }

        private SnailWalkEdge WalkEdge 
        {
            get { return this._walkEdge; }
            set
            {
                this._walkEdge = value;
                switch (this._walkEdge)
                {
                    case SnailWalkEdge.Bottom:
                        this.Rotation = 0.0f;
                        this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailWidth, this._snailHeight)));  
                        this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height);
                        break;
                    case SnailWalkEdge.Left:
                        this.Rotation = 90.0f;
                        this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailHeight, this._snailWidth)));  
                        this.Position = new Microsoft.Xna.Framework.Vector2(0.0f, this.Position.Y);
                        break;
                    case SnailWalkEdge.Top:
                        this.Rotation = 180.0f;
                        this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailWidth, this._snailHeight)));  
                        this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, 0.0f);
                        break;
                    case SnailWalkEdge.Right:
                        this.Rotation = 270.0f;
                        this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailHeight, this._snailWidth)));  
                        this.Position = new Microsoft.Xna.Framework.Vector2(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width, this.Position.Y);
                        break;
                }
               
            }

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public UISnail(UIScreen ownerScreen) :
            base(ownerScreen)
        {
            this._spriteWalk = BrainGame.ResourceManager.GetSpriteStatic("spriteset/anim-snails", "SnailWalk");
            this._spriteTurn = BrainGame.ResourceManager.GetSpriteStatic("spriteset/anim-snails", "SnailTurnUp");
            this._snailAnim = new SpriteAnimation(this._spriteWalk);
            this._snailWidth = this._spriteWalk.Frames[0].Width;
            this._snailHeight = this._spriteWalk.Frames[0].Height;

            this.State = SnailState.Walking;
            this.Speed = ((BrainGame.Rand.Next(5)) + 10) * 10;
            this.Direction = (BrainGame.Rand.Next(2) == 1? SnailDirection.Clowckwise : SnailDirection.CounterClockwise);
            this.WalkEdge = (SnailWalkEdge)(BrainGame.Rand.Next(4));

            switch (this.WalkEdge)
            {
                case SnailWalkEdge.Bottom:
                case SnailWalkEdge.Top:
                    this.Position = new Microsoft.Xna.Framework.Vector2(BrainGame.Rand.Next(UIScreen.MAX_SCREEN_WITDH_IN_POINTS), this.Position.Y);
                     break;
                case SnailWalkEdge.Left:
                case SnailWalkEdge.Right:
                    this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, BrainGame.Rand.Next(UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS));
                 break;
            }

            //this.BackgroundColor = Color.Blue;
            this.DropShadow = true;
            this.ShadowDistance = new Vector2(3.0f, 3.0f);
            this.AcceptControllerInput = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            this._snailAnim.Update(gameTime);

            switch (this.State)
            {
                case SnailState.Walking:
                    this.UpdateWalk(gameTime);
                    break;
                case SnailState.Turning:
                    this.UpdateTurn(gameTime);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Kill()
        {
            this.State = SnailState.Falling;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTurn(BrainEngine.BrainGameTime gameTime)
        {
            if (this._snailAnim.CurrentFrame == this._snailAnim.Sprite.FrameCount - 1)
            {
                if (this._direction == SnailDirection.Clowckwise)
                {
                    int edge = (int)this.WalkEdge;
                    edge++;
                    if (edge > (int)SnailWalkEdge.Left)
                    {
                        edge = (int)SnailWalkEdge.Bottom;
                    }
                    this.WalkEdge = (SnailWalkEdge)edge;
                    this.State = SnailState.Walking;
                    switch (this.WalkEdge)
                    {
                        case SnailWalkEdge.Right:
                            this.Position = new Microsoft.Xna.Framework.Vector2(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width,
                                                                                UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height);
                            break;

                        case SnailWalkEdge.Top:
                            this.Position = new Microsoft.Xna.Framework.Vector2(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width,
                                                                                0.0f);
                            break;

                        case SnailWalkEdge.Left:
                            this.Position = new Microsoft.Xna.Framework.Vector2(0.0f, 0.0f);
                            break;

                        case SnailWalkEdge.Bottom:
                            this.Position = new Microsoft.Xna.Framework.Vector2(0.0f, UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height);
                            break;
                    }
                }
                else
                {
                    int edge = (int)this.WalkEdge;
                    edge--;
                    if (edge < 0)
                    {
                        edge = (int)SnailWalkEdge.Left;
                    }
                    this.WalkEdge = (SnailWalkEdge)edge;
                    this.State = SnailState.Walking;
                    switch (this.WalkEdge)
                    {
                        case SnailWalkEdge.Right:
                            this.Position = new Microsoft.Xna.Framework.Vector2(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width, 0.0f);
                            break;

                        case SnailWalkEdge.Top:
                            this.Position = new Microsoft.Xna.Framework.Vector2(0.0f, 0.0f);
                            break;

                        case SnailWalkEdge.Left:
                            this.Position = new Microsoft.Xna.Framework.Vector2(0.0f, UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height);
                            break;

                        case SnailWalkEdge.Bottom:
                            this.Position = new Microsoft.Xna.Framework.Vector2(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width, 
                                                                                UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateWalk(BrainEngine.BrainGameTime gameTime)
        {
            float speed = (this.Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) * (int)this.Direction;
           
            switch (this.WalkEdge)
            {
                case SnailWalkEdge.Bottom:
                    this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X + speed, this.Position.Y);
                    break;
                case SnailWalkEdge.Top:
                    this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X - speed, this.Position.Y);
                    break;
                case SnailWalkEdge.Left:
                    this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y + speed);
                    break;
                case SnailWalkEdge.Right:
                    this.Position = new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y - speed);
                    break;
            }

            if (this.Position.Y > (float)(UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height))
            {
                this.Position = new Vector2(this.Position.X, UIScreen.MAX_SCREEN_HEIGHT_IN_POINTS - this.Size.Height);
                this.State = SnailState.Turning;
            } 
            
            if (this.Position.Y < 0.0f)
            {
                this.Position = new Vector2(this.Position.X, 0.0f);
                this.State = SnailState.Turning;
            }

            if (this.Position.X < 0.0f)
            {
                this.Position = new Vector2(0.0f, this.Position.Y);
                this.State = SnailState.Turning;
            }

            if (this.Position.X > (float)(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width))
            {
                this.Position = new Vector2(UIScreen.MAX_SCREEN_WITDH_IN_POINTS - this.Size.Width, this.Position.Y);
                this.State = SnailState.Turning;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            if (this.DropShadow)
            {
                this._snailAnim.Draw(this.AbsolutePositionInPixels + this.GetDrawOffset() + this.ShadowDistance, this.Rotation, this.ShadowColor, this.ScreenOwner.SpriteBatch);
            }

            this._snailAnim.Draw(this.AbsolutePositionInPixels + this.GetDrawOffset(), this.Rotation, this.ScreenOwner.SpriteBatch);
        }

        private Vector2 GetDrawOffset()
        {
            //Vector2 offset = this._snailAnim.Sprite.Offset;
            if (this.State == SnailState.Walking)
            {
                switch (this.WalkEdge)
                {
                    case SnailWalkEdge.Bottom:
                        return new Vector2(this._snailAnim.Sprite.Offset.X, this._snailAnim.Sprite.Offset.Y);
                    case SnailWalkEdge.Right:
                        return new Vector2(this._snailAnim.Sprite.Offset.Y, this._snailAnim.Sprite.Offset.X);
                    case SnailWalkEdge.Top:
                        return new Vector2(this._snailAnim.Sprite.Offset.X, 0.0f);
                    case SnailWalkEdge.Left:
                        return new Vector2(0.0f, this._snailAnim.Sprite.Offset.X);
                }
            }
            else
            {
                if (this.Direction == SnailDirection.Clowckwise)
                {
                    switch (this.WalkEdge)
                    {
                        case SnailWalkEdge.Bottom:
                            return new Vector2(this._snailAnim.Sprite.Offset.Y, this._snailAnim.Sprite.Offset.X);
                        case SnailWalkEdge.Right:
                            return new Vector2(this._snailAnim.Sprite.Offset.X, 0.0f);
                        case SnailWalkEdge.Top:
                            return new Vector2(0.0f, 0.0f);
                        case SnailWalkEdge.Left:
                            return new Vector2(0.0f, this._snailAnim.Sprite.Offset.X);
                    }
                }
                else
                {
                    switch (this.WalkEdge)
                    {
                        case SnailWalkEdge.Bottom:
                            return new Vector2(0.0f, this._snailAnim.Sprite.Offset.X);
                        case SnailWalkEdge.Right:
                            return new Vector2(this._snailAnim.Sprite.Offset.X, this._snailAnim.Sprite.Offset.Y);
                        case SnailWalkEdge.Top:
                            return new Vector2(this._snailAnim.Sprite.Offset.X, 0.0f);
                        case SnailWalkEdge.Left:
                            return new Vector2(0.0f, 0.0f);
                    }
                }
            }
            
            return Vector2.Zero;
        }
    }
}
