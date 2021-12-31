using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Stages;
using Microsoft.Xna.Framework;
using System;

namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
    public class SnailSpriteAccessory
    {
        protected Sprite _spriteWalk;
        protected Sprite _spriteTurnDown;
        protected Sprite _spriteTurnUp;
        Sprite _spriteDeathWithStageExit;
        Sprite _spriteByte;
        Sprite _spriteChew;
        Sprite _spriteEmpaled;
        Sprite _spriteDeadBrokenShell;
        Sprite _spriteHiddingInShell;
        Sprite _spriteAccessory;
        protected Snail _snail;
        bool _visible;
        bool _shouldUpdateSprite;
        public virtual bool Visible
        {
            get { return this._visible; }
            set
            {
                this._visible = value;
                this._shouldUpdateSprite = true;
            }
        }

        public SnailSpriteAccessory(Snail snail)
        {
            this._snail = snail;
            this._visible = false;
        }

        public virtual void LoadContent(string spriteSet)
        {
            // The accessories are mandatory in the sprites used for the walk
            this._spriteWalk = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_WALK);
            this._spriteTurnDown = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_TURN_DOWN);
            this._spriteTurnUp = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_TURN_UP);
            
            // Special sprites are optional
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet,Snail.SPRITE_DEATH_STAGE_EXIT))
            {
                this._spriteDeathWithStageExit = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_DEATH_STAGE_EXIT);
            }
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet, Snail.SPRITE_SNAIL_BYTING))
            {
                this._spriteByte = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_BYTING);
            }
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet, Snail.SPRITE_SNAIL_CHEWING))
            {
                this._spriteChew = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_CHEWING);
            }
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet, Snail.SPRITE_SNAIL_EMPALED))
            {
                this._spriteEmpaled = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_EMPALED);
            }
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet, Snail.SPRITE_SNAIL_DEAD_BROKEN_SHELL))
            {
                this._spriteDeadBrokenShell = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_DEAD_BROKEN_SHELL);
            }
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet, Snail.SPRITE_SNAIL_DEAD_BROKEN_SHELL))
            {
                this._spriteDeadBrokenShell = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_DEAD_BROKEN_SHELL);
            }
            if (BrainGame.ResourceManager.ContainsSprite(spriteSet, Snail.SPRITE_SNAIL_HIDDING_IN_SHELL))
            {
                this._spriteHiddingInShell = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, Snail.SPRITE_SNAIL_HIDDING_IN_SHELL);
            }
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Initialize()
        {
            this.Visible = false;
        }

        public virtual void Update(BrainGameTime gameTime)
        {
        }

        public virtual void Draw(bool shadow, SpriteBatch spriteBatch)
        {
            if (!this.Visible)
            {
                return;
            }
            if (this._shouldUpdateSprite)
            {
                this.UpdateActiveSprite();
            }

            if (this._spriteAccessory != null)
            {
                this._snail.DrawParentChild(shadow, this._snail.Sprite, this._spriteAccessory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnEnterLiquid()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnExitLiquid()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateActiveSprite()
        {
            this._shouldUpdateSprite = false;
            if (this._snail.Sprite == this._snail.WalkSprite)
            {
                this._spriteAccessory = this._spriteWalk;
                return;
            }

            if (this._snail.Sprite == this._snail.OuterTurnSprite)
            {
                this._spriteAccessory = this._spriteTurnDown;
                return;
            }

            if (this._snail.Sprite == this._snail.InnerTurnSprite)
            {
                this._spriteAccessory = this._spriteTurnUp;
                return;
            }

            if (this._snail.Sprite == this._snail.DeathWithStageExitSprite)
            {
                this._spriteAccessory = this._spriteDeathWithStageExit;
                return;
            }

            if (this._snail.Sprite == this._snail.SpriteByting)
            {
                this._spriteAccessory = this._spriteByte;
                return;
            }

            if (this._snail.Sprite == this._snail.SpriteChewing)
            {
                this._spriteAccessory = this._spriteChew;
                return;
            }

            if (this._snail.Sprite == this._snail.SpriteEmpaled)
            {
                this._spriteAccessory = this._spriteEmpaled;
                return;
            }

            if (this._snail.Sprite == this._snail.SpriteDeadBrokenShell)
            {
                this._spriteAccessory = this._spriteDeadBrokenShell;
                return;
            }

            if (this._snail.Sprite == this._snail.SpriteHiddingInShell)
            {
                this._spriteAccessory = this._spriteHiddingInShell;
                return;
            }
            
            this._spriteAccessory = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Project()
        {
            Prop prop = ((ISnailSpriteAccessory)this).CreateProp();
            this.Project(prop);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Project(Prop prop)
        {
            prop.Position = this._snail.Position;
            Stage.CurrentStage.AddObjectInRuntime(prop);
            prop.DrawInForeground = true;

            // - 45 - 135
            float angle = BrainGame.Rand.Next(90) - 135;
            float angleSin = (float)Math.Sin(MathHelper.ToRadians(angle));
            float angleCos = (float)Math.Cos(MathHelper.ToRadians(angle));
            Vector2 dir = new Vector2(angleCos, angleSin);

            prop.ProjectWithRotation(dir, BrainGame.Rand.Next(30, 50), 30f);
        }
    }
}
