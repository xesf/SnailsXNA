using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using System.IO;

namespace TwoBrainsGames.BrainEngine
{
	public class BrainSettings
	{
		#region consts
		const float DEFAULT_GRAVITY = 10f;
		#endregion
		[Flags]
		public enum PlaformType
		{
			None = 0x00,
			Windows = 0x01,
			XBox = 0x02,
			WP7 = 0x04,
			MacOSX = 0x08,
			Linux = 0x10,
			iOS = 0x20,
			iPhone = 0x40,
			iPad = 0x80,
			Andriod = 0x100,
			Windows8 = 0x200,
			Windows8RT = 0x400,
			WP7Emulation = 0x800,
			iPhone5 = 0x1000,
			All = Windows | XBox | WP7 | MacOSX | Linux | iOS | iPhone | iPad | Andriod | Windows8 | Windows8RT | WP7Emulation |iPhone5,
			WindowsXbox = Windows | XBox,
			MonoGame = MacOSX | Linux | iOS | iPhone | iPad | Andriod |iPhone5
		}

		public enum GameplayModeType
		{
			Retail,
			Demo,
			Beta
		}

		protected enum ConfigSectionType
		{
			Engine,
			Game,
			Presentation,
			Debug
		}
		// Data reader
		public static string DataFileReaderAssembly
		{ get { return "TwoBrainsGames.BrainEngine"; } }
		public static string DataFileReaderType
		{ get { return "TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile.BinaryDataFileReader"; } }
		// Data writer
		public static string DataFileWriterAssembly
		{ get { return "TwoBrainsGames.BrainEngine"; } }
		public static string DataFileWriterType
		{ get { return "TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile.XmlDataFileWriter"; } }

		private DataFileRecord _dataFileRoot;

		public string NavigatorContentFolder;
		public string NavigatorContentId;
		public string NavigatorControlContentFolder
		{
			get { return Path.Combine(NavigatorContentFolder, "controls"); }
		}


		// Debug
		public bool ShowDebugInfo; 
		public bool ShowBoundingBoxes;
		public bool ShowSpriteFrame;
		public bool ShowSprites;
		public bool ShowResourceManagerData;
		public bool WindowAlwaysActive;

		// Game settings
		public bool IsFullScreen;
		public int ScreenWidth;
		public int ScreenHeight;
		public float RatioNativeResolutionWidth { get { return (float)((float)ScreenWidth / (float)BrainGame.NativeScreenWidth); } }
		public float RatioNativeResolutionHeight { get { return (float)((float)ScreenHeight / (float)BrainGame.NativeScreenHeight); } }
		public float RatioPresentationNativeResolutionWidth { get { return (float)((float)ScreenWidth / (float)BrainGame.PresentationNativeScreenWidth); } }
		public float RatioPresentationNativeResolutionHeight { get { return (float)((float)ScreenHeight / (float)BrainGame.PresentationNativeScreenHeight); } }
		public float Gravity;
		public int ExplosionParticles;
		public float ExplosionMinVelocity;
		public float ExplosionMaxVelocity;
		GameplayModeType _gameplayMode;
		public PlaformType Platform;
		public string PresentationModeString;
		public bool CreateAppFolders;
		public bool UseVSync;
		public bool UseAsyncLoading;
		public string StartupScreenGroup;
		public string StartupScreen;
		public bool FPSLocked;
		public bool SupportsShaderEffects;
		public CursorModes MenuCursorMode;
		public bool UseKeyboard;
		public bool UseMouse;
		public bool UseGamepad;
		public bool UseTouch;
		public bool WithRumbble;
		public bool UseAchievements;
		public bool UseAdRotator;
		public string GameVersion;
		public string SupportContact;
		public string GameName;
		public string GameBundleId;
		public string AppStoreId;
		public bool UseAds;
		public int AdBannerHeight;
		public string [] AdBannerTestDevices;
		public string AdBannerAdId;
		#region Properties
		public bool PreferMultiSampling
		{
			get { return BrainGame.PreferMultiSampling; }
			set
			{
				BrainGame.PreferMultiSampling = value;
			}
		}

		public GameplayModeType GameplayMode
		{
			get
			{
				return _gameplayMode;
			}
			set
			{
				if (value != this._gameplayMode)
				{
					this._gameplayMode = value;
					if (BrainGame.ScreenNavigator != null)
					{
						BrainGame.ScreenNavigator.GameplayModeChanged();
					}
				}
			}
		}
		#endregion

		protected BrainSettings()
		{
			this.GameplayMode = GameplayModeType.Retail; 

			#if WINDOWS
			this.Platform = PlaformType.Windows;
			//  this.Platform = PlaformType.WP7Emulation;
			#elif XBOX
			this.Platform = PlaformType.XBox;
			#elif WP7
			this.Platform = PlaformType.WP7;
			#elif MONOMAC
			this.Platform = PlaformType.MacOSX;
			#elif LINUX
			this.Platform = PlaformType.Linux;
			#elif IOS
			// check for Universal build
			string model = MonoTouch.UIKit.UIDevice.CurrentDevice.Model;
			if (model.Contains("iPhone") || model.Contains("iPod"))
			{
				this.Platform = PlaformType.iPhone;
				if (BrainGame.GraphicsManager.PreferredBackBufferWidth > 960) // hacky hacky
				{
					this.Platform = PlaformType.iPhone5;
				}
			}
			else if (model.Contains("iPad"))
			{
				this.Platform = PlaformType.iPad;	
			}

			#elif Android
			this.Platform = PlaformType.Android;
			#elif WIN8
			this.Platform = PlaformType.Windows8;
			// There should be a better way of doing this...
			if (Windows.Devices.Input
			.PointerDevice.GetPointerDevices()
			.Any(p => p.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch))
			{
			this.Platform = PlaformType.Windows8RT;
			}
			#endif
		}

		/// <summary>
		/// 
		/// </summary>
		public void Load(string resourceName)
		{
			this._dataFileRoot = BrainGame.ResourceManager.Load<DataFileRecord>(resourceName, ResourceManager.ResourceManagerCacheType.Static);
			this.Initialize();
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void Initialize()
		{
			this.ShowBoundingBoxes = false;
			this.ShowSprites = true;
			this.ExplosionParticles = ExplosionEffect.DEFAULT_EXPLOSION_PARTICLES;
			this.ExplosionMinVelocity = ExplosionEffect.DEFAULT_MIN_SPEED;
			this.ExplosionMaxVelocity = ExplosionEffect.DEFAULT_MAX_SPEED;
			this.PresentationModeString = this.GetValue<string>("PresentationMode", null, ConfigSectionType.Engine);
			BrainGame.NativeScreenWidth = this.GetValue<int>("NativeScreenWidth", ConfigSectionType.Engine);
			BrainGame.NativeScreenHeight = this.GetValue<int>("NativeScreenHeight", ConfigSectionType.Engine);
			BrainGame.PresentationNativeScreenWidth = this.GetValue<int>("PresentationNativeScreenWidth", ConfigSectionType.Engine);
			BrainGame.PresentationNativeScreenHeight = this.GetValue<int>("PresentationNativeScreenHeight", ConfigSectionType.Engine);
			this.ScreenWidth = this.GetValue<int>("ScreenWidth", ConfigSectionType.Engine);
			this.ScreenHeight = this.GetValue<int>("ScreenHeight", ConfigSectionType.Engine);
			this.IsFullScreen = this.GetValue<bool>("IsFullscreen", ConfigSectionType.Engine);
			this.Gravity = this.GetValue<float>("Gravity", DEFAULT_GRAVITY, ConfigSectionType.Engine);
			this.PreferMultiSampling = this.GetValue<bool>("PreferMultiSampling", true, ConfigSectionType.Engine);
			this.CreateAppFolders = this.GetValue<bool>("CreateAppFolders", true, ConfigSectionType.Engine);
			this.UseKeyboard = this.GetValue<bool>("UseKeyboard", true, ConfigSectionType.Engine);
			this.UseMouse = this.GetValue<bool>("UseMouse", true, ConfigSectionType.Engine);
			this.UseGamepad = this.GetValue<bool>("UseGamepad", true, ConfigSectionType.Engine);
			this.UseTouch = this.GetValue<bool>("UseTouch", true, ConfigSectionType.Engine);
			this.WithRumbble = this.GetValue<bool>("WithRumbble", true, ConfigSectionType.Engine);
			this.MenuCursorMode = (CursorModes)Enum.Parse(typeof(CursorModes), this.GetValue<string>("MenuCursorMode", ConfigSectionType.Engine), true);
			this.UseVSync = this.GetValue<bool>("UseVSync", ConfigSectionType.Engine);
			this.UseAsyncLoading = this.GetValue<bool>("UseAsyncLoading", ConfigSectionType.Engine);
			this.NavigatorContentFolder = this.GetValue<string>("NavigatorContentFolder", ConfigSectionType.Engine);
			this.NavigatorContentId = this.GetValue<string>("NavigatorContentId", ConfigSectionType.Engine);
			this.StartupScreenGroup = this.GetValue<string>("StartupScreenGroup", ConfigSectionType.Engine);
			this.StartupScreen = this.GetValue<string>("StartupScreen", ConfigSectionType.Engine);
			this.FPSLocked = this.GetValue<bool>("FPSLocked", ConfigSectionType.Engine);
			this.SupportsShaderEffects = this.GetValue<bool>("SupportsShaderEffects", ConfigSectionType.Engine);
			this.UseAchievements = this.GetValue<bool>("UseAchievements", true, ConfigSectionType.Engine);
			this.UseAds = this.GetValue<bool>("UseAds", true, ConfigSectionType.Engine);
			this.UseAdRotator = this.GetValue<bool>("UseAdRotator", true, ConfigSectionType.Engine); 
			this.GameVersion = this.GetValue<string>("GameVersion", ConfigSectionType.Engine);
			this.SupportContact = this.GetValue<string>("SupportContact", ConfigSectionType.Engine);
			this.GameName = this.GetValue<string>("GameName", ConfigSectionType.Engine);
			this.GameBundleId = this.GetValue<string>("GameBundleId", ConfigSectionType.Engine);
			this.AppStoreId = this.GetValue<string>("AppStoreId", ConfigSectionType.Engine);

			this.UseAds = this.GetValue<bool>("UseAds", false, ConfigSectionType.Engine);
			this.AdBannerHeight = this.GetValue<int>("AdBannerHeight", ConfigSectionType.Engine);        
			string devices = this.GetValue<string>("AdBannerTestDevices", ConfigSectionType.Engine);
			if (devices != null) {
				this.AdBannerTestDevices = devices.Split (',');
			}
			this.AdBannerAdId = this.GetValue<string>("AdBannerAdId", ConfigSectionType.Engine);        
			#if DEBUG
			this.WindowAlwaysActive = this.GetValue<bool>("WindowAlwaysActive", false, ConfigSectionType.Debug);
			this.ShowSprites = this.GetValue<bool>("ShowSprites", true, ConfigSectionType.Debug);
			#else
			this.WindowAlwaysActive = false;
			this.ShowSprites = true;
			#endif
		}

		/// <summary>
		/// Reads a setting from the config file, with a default value if not found
		/// </summary>
		protected T GetValue<T>(string paramName, T defaultVal, ConfigSectionType configSection)
		{
			return this.GetValue<T>(paramName, defaultVal, configSection, false);
		}

		/// <summary>
		/// Reads a setting from the config file, throws an exception if the parameter is not found
		/// </summary>
		protected T GetValue<T>(string paramName, ConfigSectionType configSection)
		{
			return this.GetValue<T>(paramName, default(T), configSection, true);
		}

		/// <summary>
		/// Gets a value from the settings with a default value
		/// </summary>
		private T GetValue<T>(string paramName, T defaultVal, ConfigSectionType configSection, bool exceptionIfNotFound)
		{
			this.AssertLoaded();
			DataFileRecord record = this._dataFileRoot.SelectRecord(configSection.ToString() + "\\" + paramName);
			if (record == null)
			{
				if (exceptionIfNotFound)
				{
					throw new BrainException("Settings with name [" + paramName + "] does not exist in the game settings file");
				}
				return defaultVal;
			}

			DataFileField field = record.GetFieldByName("value");
			if (field == null)
			{
				if (exceptionIfNotFound)
				{
					throw new BrainException("Settings with name [value] does not exist in the game settings file");
				}
				return defaultVal;
			}

			// Look for condition nodes
			DataFileRecordList condRecords = record.SelectRecords("Condition");
			foreach (DataFileRecord condRecord in condRecords)
			{
				if (this.EvaluateCondition(condRecord))
				{
					record = condRecord;
					break;
				}
			}

			return record.GetFieldValue<T>("value", defaultVal);
		}

		/// <summary>
		/// 
		/// </summary>
		private bool EvaluateCondition(DataFileRecord condRecord)
		{
			if (condRecord == null)
			{
				return false;
			}

			bool cond = false;
			// Platform
			DataFileField platformField = condRecord.GetFieldByName("platform");
			if (platformField != null) 
			{
				if (platformField.Value != null &&
					(string)platformField.Value == this.Platform.ToString())
				{
					cond = true;
				}
			}

			// Presentation Mode
			DataFileField presentationField = condRecord.GetFieldByName("presentation");
			if (presentationField != null)
			{
				if (presentationField.Value != null &&
					(string)presentationField.Value == this.PresentationModeString)
				{
					cond = true;
				}
			}

			// Input Mode
			DataFileField useTouchField = condRecord.GetFieldByName("useTouch");
			if (useTouchField != null)
			{
				if (useTouchField.Value != null &&
					Convert.ToBoolean((string)useTouchField.Value) == this.UseTouch)
				{
					cond = true;
				}
			}
			return cond;
		}

		/// <summary>
		/// 
		/// </summary>
		private void AssertLoaded()
		{
			if (this._dataFileRoot == null)
			{
				throw new BrainException("Settings not loaded. call BrainSettings.Load()");
			}
		}
	}
}
