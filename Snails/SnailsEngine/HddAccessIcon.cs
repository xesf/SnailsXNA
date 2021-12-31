using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails
{
    // This is the hdd access indicator
    // It has a DVD and a walking snail
    class HddAccessIcon : IHddIndicator
    {
//        Sprite _spriteDvd;
        SpriteAnimation _snailAnim;
        Vector2 _snailPosition;
        //Vector2 _dvdPosition;
        Vector2 _textPosition;
        Vector2 _position;
//        float _rotation;
        Color _shadowColor;
        Vector2 _shadowDistance;
        TextFont _font;
        string _loadingText;
        Color _textColor;

        /// <summary>
        /// 
        /// </summary>
        public HddAccessIcon()
        {
        }

        #region IHddIndicator 
        public bool Visible { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this._snailAnim = new SpriteAnimation(new Sprite(BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "LoadingAnim")));
            this._position = new Vector2(BrainGame.ScreenWidth - 110, BrainGame.ScreenHeight - 30);
            this._snailPosition = new Vector2(-0f, 25f);
            //this._dvdPosition = new Vector2(-20f, -25f);
            this._shadowColor = new Color(0, 0, 0, 100);
            this._shadowDistance = new Vector2(3f, 2f);
            SnailsGame.Instance.OnLanguageChanged += new System.EventHandler(Instance_OnLanguageChanged);
            this._font = BrainGame.ResourceManager.Load<TextFont>(FontResources.MAIN_FONT_MEDIUM, ResourceManager.ResourceManagerCacheType.Static);
            this._textColor = new Color(255, 180, 45);
            this.UpdateLoadingText();
        }

        /// <summary>
        /// 
        /// </summary>
        void Instance_OnLanguageChanged(object sender, System.EventArgs e)
        {
            this.UpdateLoadingText();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateLoadingText()
        {
            this._loadingText = LanguageManager.GetString("LBL_LOADING");
            this._textPosition = new Vector2(-this._font.MeasureString(this._loadingText) / 2, -50f);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            this._snailAnim.Update(gameTime);
       /*     this._rotation += 0.2f * (float)gameTime.ElapsedRealTime.TotalMilliseconds;
            if (this._rotation > 360)
            {
                this._rotation -= 360;
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch, this._position);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
//            this._spriteDvd.Draw(this._dvdPosition + this._shadowDistance + position, 0, 0, SpriteEffects.None, 1.0f, this._shadowColor, 1f, spriteBatch);
//            this._spriteDvd.Draw(this._dvdPosition + position, 0, this._rotation, SpriteEffects.None, spriteBatch);
            this._font.DrawString(spriteBatch, this._loadingText, position + this._textPosition + this._shadowDistance, Vector2.One, this._shadowColor);
            this._font.DrawString(spriteBatch, this._loadingText, position + this._textPosition, Vector2.One, this._textColor);
            this._snailAnim.Draw(this._snailPosition + this._shadowDistance + position, this._shadowColor, spriteBatch);
            this._snailAnim.Draw(this._snailPosition + position, spriteBatch);
        }
        /// <summary>
        /// 
        /// </summary>
        public void UnloadContent()
        {
            
        }
        #endregion
    }
}
