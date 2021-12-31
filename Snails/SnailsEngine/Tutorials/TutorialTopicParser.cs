using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace TwoBrainsGames.Snails.Tutorials
{
    class TutorialTopicParser 
    {
        const string LINE_BREAK = "[br]";
        static Color _currentColor = Colors.TutorialTextColor;
        
        /// <summary>
        /// Parses a text of st code
        /// Supported tags:
        ///   -[img=XXXXXXXXX]
        /// </summary>
        public static TutorialLine ParseCode(string stCode)
		{
            if (stCode == null)
            {
                throw new SnailsException("Paremeter stCode cannot be null");
            }
            TutorialLine parsedLine = new TutorialLine();
            TutorialTopicParser.ParseCodeLine(stCode, parsedLine);

		    return parsedLine;
		}
         
        /// <summary>
        /// 
        /// </summary>
        public static void ParseCodeLine(string t, TutorialLine tutorialLine)
		{
			int idx = t.IndexOf('[');
			if (idx != -1)
			{
				string text = t.Substring(0, idx);
				if (!string.IsNullOrEmpty(text))
				{
                    tutorialLine.Add(new TutorialText(text, TutorialTopicParser._currentColor));
				}
                TutorialTopicParser.ParseTag(t.Substring(idx), tutorialLine);
			}
			else
			{
				if (!string.IsNullOrEmpty(t))
				{
                    tutorialLine.Add(new TutorialText(t, TutorialTopicParser._currentColor));
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private static void ParseTag(string t, TutorialLine tutorialLine)
        {
            t = TutorialTopicParser.ParseToken("[", t);
            string token;
            t = TutorialTopicParser.GetToken(t, out token);
            if (token == "img")
            {
                t = TutorialTopicParser.ParseImage(t, tutorialLine);
            }
            else
            if (token == "color")
            {
                t = TutorialTopicParser.ParseColor(t);
            }
            if (token == "enph")
            {
                t = TutorialTopicParser.ParseEnphasize(t);
            }
            else
            if (token == "unenph")
            {
                t = TutorialTopicParser.ParseUnenphasize(t);
            }
            TutorialTopicParser.ParseCodeLine(t, tutorialLine);
       }

        /// <summary>
        /// 
        /// </summary>
        private static string ParseImage(string t, TutorialLine tutorialLine)
		{
			t = TutorialTopicParser.ParseToken("=", t);
			string token;
			t = TutorialTopicParser.GetToken(t, out token);
            tutorialLine.Add(new TutorialImage(token));
			t = TutorialTopicParser.ParseToken("]", t);
            return t;
		}


        /// <summary>
        /// 
        /// </summary>
        private static string ParseColor(string t)
        {
            t = TutorialTopicParser.ParseToken("=", t);
            string r, g, b;
            t = TutorialTopicParser.GetToken(t, out r);
            if (r != "DEFAULT")
            {
                t = TutorialTopicParser.ParseToken(",", t);
                t = TutorialTopicParser.GetToken(t, out g);
                t = TutorialTopicParser.ParseToken(",", t);
                t = TutorialTopicParser.GetToken(t, out b);
                TutorialTopicParser._currentColor = new Color(Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b), 1f);
            }
            else
            {
                TutorialTopicParser._currentColor = Colors.TutorialTextColor;
            }
            t = TutorialTopicParser.ParseToken("]", t);
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        private static string ParseEnphasize(string t)
        {
            TutorialTopicParser._currentColor = Colors.TutorialEnphasizeTextColor;
            t = TutorialTopicParser.ParseToken("]", t);
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        private static string ParseUnenphasize(string t)
        {
            TutorialTopicParser._currentColor = Colors.TutorialTextColor;
            t = TutorialTopicParser.ParseToken("]", t);
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
		private static string GetToken(string t, out string token)
		{
			token = null;
			t = t.TrimStart();
            int idx = t.IndexOfAny(new char []  {'=',',',']'});
		//	int idx = t.IndexOf("]");
			if (idx != -1)
			{
				token = t.Substring(0, idx);
				return t.Substring(idx);
			}
			return "";
		}

        /// <summary>
        /// 
        /// </summary>
		private static string ParseToken(string token, string t)
		{
			t = t.TrimStart(' ');
            if (t.Substring(0, token.Length) != token)
            {
				throw new SnailsException("Error parsing tutorial text. Token [" + token+ "] not found.");
			}
			return t.Substring(token.Length);
		}    
   }
}
