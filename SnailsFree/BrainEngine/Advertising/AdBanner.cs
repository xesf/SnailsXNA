using System;
using System.Drawing;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Advertising
{
	// A despachar
	// O anuncio fica sempre em rodapé
	public class AdBanner
	{
		protected bool _initialized;
		protected SpriteAnimation _offLineAdAnim;
		private SpriteBatch _spriteBatch;
		protected BasicEffect _renderEffect;

		public virtual bool Visible {
			get { return false; }
		}

		// In screen units
		public Microsoft.Xna.Framework.Rectangle BannerRect {
			get;
			protected set;
		}

		// In pixels 
		public Microsoft.Xna.Framework.Rectangle BannerRectInPixels {
			get;
			protected set;
		}

		// In pixels - ad units - 1024x768
		protected Microsoft.Xna.Framework.Rectangle BannerRectInPixelsAdUnits {
			get;
			set;
		}

		public Sprite OffLineAdSprite {
			get {
				return this._offLineAdAnim.Sprite;
			}
			set {
				this._offLineAdAnim.Sprite = value;
			}
		}

		public float HeightInPixels { get { return this.BannerRectInPixels.Height; } }
		public float HeightInPixelsIfVisible 
		{ 
			get 
			{ 
				return (this.Visible? this.BannerRectInPixels.Height : 0); 
			} 
		}

		public AdBanner ()
		{
			this._initialized = false;
			this._offLineAdAnim = new SpriteAnimation (null);
			this._offLineAdAnim.Visible = false;
			this._spriteBatch = new SpriteBatch(BrainGame.Graphics);
		}

		public static AdBanner Create()
		{
			#if IOS
			return new AdBannerIOS();
			#else
			return new AdBanner();
			#endif
		}

		/// <summary>
		/// Initialize this instance
		/// </summary>
		public virtual void Initialize()
		{
			if (this._initialized) {
				throw new BrainException ("AdBanner was already initialized.");
			}
	
			this._initialized = true;

		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void OnUpdate(BrainGameTime gameTime)
		{
		
			if (this._offLineAdAnim.Visible) {
				this._offLineAdAnim.Update (gameTime);
			}

		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Draw()
		{
			if (this._offLineAdAnim.Visible) {
				this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, null, null, this._renderEffect);
				this._offLineAdAnim.Draw(this.BannerRectInPixelsAdUnits, Color.White, this._spriteBatch);
				this._spriteBatch.End();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Show()
		{
		
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Hide()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		protected void SetupProjection()
		{
			this._renderEffect = new BasicEffect(BrainGame.Graphics);
			this._renderEffect.World = Matrix.Identity;
			this._renderEffect.View = Matrix.Identity;

			this._renderEffect.Projection = Matrix.CreateTranslation( - 0.5f, - 0.5f , 0) * 
				Matrix.CreateOrthographicOffCenter(0, BrainGame.NativeScreenWidth, BrainGame.NativeScreenHeight, 0, -1, 1);
			this._renderEffect.TextureEnabled = true;
			this._renderEffect.VertexColorEnabled = true;

		}
	}
}

