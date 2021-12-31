using System.Reflection;
using System;
using TwoBrainsGames.BrainEngine;
using System.Globalization;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using System.Collections.Generic;

namespace TwoBrainsGames.BrainEngine.Localization
{
    public class LanguageManager
    {
        static Dictionary<string, string> _localizationStrings;

        static Dictionary<string, string> LocalizationStrings
        {
            get
            {
                if (_localizationStrings == null)
                {
                    // reload localization file
                    if (_localizationStrings == null)
                    {
                        DataFileRecord localRoot = BrainGame.ResourceManager.Load<DataFileRecord>("localization/language_" + BrainGame.CurrentLanguage.ToString(), ResourceManager.ResourceManagerCacheType.Static);

                        _localizationStrings = new Dictionary<string, string>();

                        DataFileRecordList textRecords = localRoot.SelectRecords("text");
                        foreach (DataFileRecord textRecord in textRecords)
                        {
                            string key = textRecord.GetFieldValue<string>("id");
                            string value = textRecord.GetFieldValue<string>("value");

                            _localizationStrings.Add(key, value);
                        }
                    }
                }
                return _localizationStrings;
            }
        }

        internal static void LanguageChanged()
        {
            _localizationStrings = null;
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

            

            string res = null;
            if (!LocalizationStrings.TryGetValue(id, out res))
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
            CultureInfo ci = CultureInfo.CurrentUICulture;
            if (ci.TwoLetterISOLanguageName.ToLower() == "pt")
            {
                return LanguageCode.pt;
            }
            else if (ci.TwoLetterISOLanguageName.ToLower() == "es")
            {
                return LanguageCode.es;
            }
            else if (ci.TwoLetterISOLanguageName.ToLower() == "it")
            {
                return LanguageCode.it;
            }
            else if (ci.TwoLetterISOLanguageName.ToLower() == "fr")
            {
                return LanguageCode.fr;
            }
            else if (ci.TwoLetterISOLanguageName.ToLower() == "de")
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

        /// <summary>
        /// 
        /// </summary>
        public static bool Contains(string id)
        {
            string res;
            return LocalizationStrings.TryGetValue(id, out res);
        }
    }
}
