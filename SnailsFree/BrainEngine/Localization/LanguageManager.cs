using System.Reflection;
using System;
using TwoBrainsGames.BrainEngine;
using System.Globalization;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.GameKit;
using MonoTouch.UIKit;

namespace TwoBrainsGames.BrainEngine.Localization
{
    public class LanguageManager
    {
        static Dictionary<string, string> localization;

        internal static void LanguageChanged()
        {
            localization = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetString(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return id;
            }

            // reload localization file
            if (localization == null)
            {
                DataFileRecord localRoot = BrainGame.ResourceManager.Load<DataFileRecord>("localization/language_" + BrainGame.CurrentLanguage.ToString(), ResourceManager.ResourceManagerCacheType.Static);

                localization = new Dictionary<string, string>();

                DataFileRecordList textRecords = localRoot.SelectRecords("text");
                foreach (DataFileRecord textRecord in textRecords)
                {
                    string key = textRecord.GetFieldValue<string>("id");
                    string value = textRecord.GetFieldValue<string>("value");

                    localization.Add(key, value);
                }
            }

            string res = null;
            if (!localization.TryGetValue(id, out res))
            {
                return id;
            }
            res = LanguageManager.ParseId(res);
            return (string.IsNullOrEmpty(res) ? id : res);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string[] GetMultiString(string id)
        {
            return LanguageManager.GetString(id).Split('|');
        }

        /// <summary>
        /// 
        /// </summary>
        public static LanguageCode GetDefaultSystemLanguage()
        {
			string lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower ();
			if (NSLocale.PreferredLanguages.Length > 0) 
			{
				lang = NSLocale.PreferredLanguages[0];
			}
		
			if (lang == "pt")
            {
                return LanguageCode.pt;
            }
			else if (lang == "es")
            {
                return LanguageCode.es;
            }
			else if (lang == "it")
            {
                return LanguageCode.it;
            }
			else if (lang == "fr")
            {
                return LanguageCode.fr;
            }
			else if (lang == "de")
            {
                return LanguageCode.de;
            }
            return LanguageCode.en;
        }

        /// <summary>
        /// This method parses a special text string
        /// Special strings have the following format:
        ///   INPUT_XXXX#text#
        /// Ex:
        /// INPUT_MOUSE#Press mouse button to start#INPUT_GAMEPAD#Press start#INPUT_TOUCH#Tap the screen to start#
        /// 
        /// All this may be improved...
        /// </summary>
        private static string ParseId(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string inputMode = null;
            if (BrainGame.Settings.UseMouse)
            {
                inputMode = "INPUT_MOUSE";
            }
            
            if (BrainGame.Settings.UseTouch)
            {
                inputMode = "INPUT_TOUCH";
            }

            if (BrainGame.Settings.UseGamepad)
            {
                inputMode = "INPUT_GAMEPAD";
            }
			if (string.IsNullOrEmpty (inputMode)) {
				return text;
			}

            int idx = text.IndexOf(inputMode + "#");
            if (idx == -1)
            {
                return text;
            }
            text = text.Substring(idx + inputMode.Length + 1);
            idx = text.IndexOf("#");
            if (idx == -1)
            {
                return text;
            }
            return text.Substring(0, idx);
        }
    }
}
