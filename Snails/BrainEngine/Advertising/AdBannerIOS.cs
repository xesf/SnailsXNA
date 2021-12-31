using System;
using GoogleAdMobAds;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.BrainEngine.Advertising
{
	// A despachar
	// O anuncio fica sempre em rodapé
	public class AdBannerIOS: AdBanner
	{
		private GADBannerView _adView;
		private bool _adMobAdVisible;

		public override bool Visible {
			get { return (this._adMobAdVisible || this._offLineAdAnim.Visible); }
		}


		public AdBannerIOS ()
		{
			this._adMobAdVisible = false;
		}

		/// <summary>
		/// Initialize this instance
		/// </summary>
		public override void Initialize()
		{
			if (this._initialized) {
				throw new BrainException ("AdBanner was already initialized.");
			}
			UIWindow _window = (UIWindow)BrainGame.Instance.Services.GetService (typeof(UIWindow));
			UIViewController controller = _window.RootViewController;

			float heightIniosUnits = BrainGame.Settings.AdBannerHeight;

			float screenHeight = _window.Screen.Bounds.Height;
			if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft ||
				UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
				screenHeight = _window.Screen.Bounds.Width;
			}


			PointF posInUnits = new PointF (0, screenHeight - heightIniosUnits);
			MonoTouch.UIKit.UIApplication uiApp = MonoTouch.UIKit.UIApplication.SharedApplication;
			this._adView = new GADBannerView (size: GADAdSizeCons.GADAdSizeFullWidthLandscapeWithHeight(heightIniosUnits), origin: posInUnits) {
				AdUnitID = BrainGame.Settings.AdBannerAdId,
				RootViewController = controller
			};

			// Calculo dos rects em pixeis e em BrainUnits
			float heightInPixels = heightIniosUnits * _window.Screen.Scale;
			Vector2 posInPixels = new Vector2 (posInUnits.X * _window.Screen.Scale, posInUnits.Y * _window.Screen.Scale);

			heightInPixels = BrainGame.PresentationNativeScreenHeight * heightIniosUnits / _window.Screen.Bounds.Width;
			posInPixels = new Vector2 (0, BrainGame.PresentationNativeScreenHeight * posInUnits.Y / _window.Screen.Bounds.Width);

			this.BannerRect = new Microsoft.Xna.Framework.Rectangle (0, (int)BrainEngine.UI.Screens.UIScreen.PixelsToScreenUnitsY_(posInPixels.Y), 
				(int)BrainEngine.UI.Screens.UIScreen.MAX_SCREEN_WITDH_IN_POINTS, 
				(int)BrainEngine.UI.Screens.UIScreen.PixelsToScreenUnitsY_(heightInPixels));
			this.BannerRectInPixels = new Microsoft.Xna.Framework.Rectangle (0, (int)posInPixels.Y, 
				(int)BrainGame.Viewport.Width, 
				(int)heightInPixels);


			// Em pixeis é sempre relativo a presentation native (1024x768)
			heightInPixels = BrainGame.NativeScreenHeight * heightIniosUnits / _window.Screen.Bounds.Width;
			posInPixels = new Vector2 (0, BrainGame.NativeScreenHeight * posInUnits.Y / _window.Screen.Bounds.Width);
			this.BannerRectInPixelsAdUnits = new Microsoft.Xna.Framework.Rectangle (0, (int)posInPixels.Y, (int)BrainGame.NativeScreenWidth, (int)heightInPixels);

			this._adView.DidFailToReceiveAd += OnDidFailToReceiveAd;
			this._adView.DidReceiveAd += OnDidReceiveAd;
			this.SetupProjection ();
			this._initialized = true;

		}

		/// <summary>
		/// 
		/// </summary>
		public override void Show()
		{
			if (!this._initialized) {
				throw new BrainException ("AdBanner was not  initialized. Please check property UseAds in game-settings.");
			}

			if (this.Visible) {
				return;
			}

			GADRequest request = GADRequest.Request;
			request.TestDevices = BrainGame.Settings.AdBannerTestDevices;

			this._adView.LoadRequest (request);

		}

		/// <summary>
		/// 
		/// </summary>
		public override void Hide()
		{
			if (!this._initialized) {
				throw new BrainException ("AdBanner was not  initialized. Please check property UsAds in game-settings.");
			}
			if (!this.Visible) {
				return;
			}
			if (this._adMobAdVisible) {
				UIViewController controller = UIApplication.SharedApplication.Windows [0].RootViewController;
				this._adView.RemoveFromSuperview ();
				this._adMobAdVisible = false;
			}
			this._offLineAdAnim.Visible = false;
		}

		/// <summary>
		/// 
		/// </summary>
		void OnDidReceiveAd (object sender, EventArgs e)
		{
			//UIViewController controller = UIApplication.SharedApplication.Windows [0].RootViewController;
			UIWindow _window = (UIWindow)BrainGame.Instance.Services.GetService (typeof(UIWindow));
			UIViewController controller = _window.RootViewController;
			controller.View.AddSubview (this._adView);
			this._adMobAdVisible = true;
			this._offLineAdAnim.Visible = false;
		}

		/// <summary>
		/// 
		/// </summary>
		void OnDidFailToReceiveAd (object sender, GADBannerViewErrorEventArgs e)
		{
			this._adMobAdVisible = false;
			this._offLineAdAnim.Visible = (this._offLineAdAnim.Sprite != null);
		}


	}
}

