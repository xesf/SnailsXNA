using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace LevelsUpdater
{
    class Program
    {
        class StageInfo
        {
            public const int BONUS_SNAILS_DELIVERED = 5; // Bonus points for each snail delivered
            public const int BONUS_TIME_TAKEN = 1; // Bonus points for second less then the targe time
            public const int BONUS_UNUSED_TOOL = 5; // Bonus points for each unused tool
            public const int BONUS_BRONZE_COINS = 20;
            public const int BONUS_SILVER_COINS = 50;
            public const int BONUS_GOLD_COINS = 100;

            public string key;
            public int snailsToSave;
            public int snailsToRelease;
            public TimeSpan targetTime;
            public int goal;
            public bool availableInDemo;
            public TimeSpan goldMedalTime;
            public int goldMedalScore;

            public static StageInfo FromXmlElement(XmlElement elem)
            {
                StageInfo si = new StageInfo();
                si.key = elem.Attributes["key"].Value;
                si.snailsToSave = Convert.ToInt32(elem.Attributes["snailsToSave"].Value);
                si.snailsToRelease = Convert.ToInt32(elem.Attributes["snailsToRelease"].Value);
                si.targetTime = TimeSpan.Parse(elem.Attributes["targetTime"].Value);
                si.goal = Convert.ToInt32(elem.Attributes["goal"].Value);
                if (elem.Attributes["availableInDemo"] != null)
                {
                    si.availableInDemo = Convert.ToBoolean(elem.Attributes["availableInDemo"].Value);
                }
                XmlElement elemGold = (XmlElement)elem.SelectSingleNode("goldMedalCriteria");
                si.goldMedalTime = TimeSpan.Parse(elemGold.Attributes["timeNeeded"].Value);
                int goldCoinsNeeded = Convert.ToInt32(elemGold.Attributes["goldCoinsNeeded"].Value);
                int silverCoinsNeeded = Convert.ToInt32(elemGold.Attributes["silverCoinsNeeded"].Value);
                int bronzeCoinsNeeded = Convert.ToInt32(elemGold.Attributes["bronzeCoinsNeeded"].Value);
                int goldSnailsNeeded = Convert.ToInt32(elemGold.Attributes["snailsNeeded"].Value);
                int snailsBonus = goldSnailsNeeded * BONUS_SNAILS_DELIVERED;
                int medalBonus = goldCoinsNeeded * BONUS_GOLD_COINS +
                                 silverCoinsNeeded * BONUS_SILVER_COINS +
                                 bronzeCoinsNeeded * BONUS_BRONZE_COINS;
                int timeBonus = (int)((si.targetTime - si.goldMedalTime).TotalSeconds * BONUS_TIME_TAKEN);
                int totalScore = (snailsBonus + medalBonus + timeBonus);

                si.goldMedalScore = totalScore;
                return si;
            }


        }

        const string levelsFile = ".\\SnailsResourcesCustom\\stages\\levels.xdf";
        static void Main(string[] args)
        {
            XmlDocument xmlLevelsDoc = new XmlDocument();
            xmlLevelsDoc.Load(levelsFile);
            XmlNodeList themeNodes = xmlLevelsDoc.SelectNodes("Levels/Level");
            foreach(XmlElement elemTheme in themeNodes)
            {
                string themeId = elemTheme.Attributes["theme"].Value;
                XmlNodeList stagesNodes = elemTheme.SelectNodes("Stages/Stage");
                foreach (XmlElement elemStage in stagesNodes)
                {
                    string key = elemStage.Attributes["key"].Value;
                    StageInfo si = GetStageInfo(themeId, key);
                    elemStage.SetAttribute("snailsToSave", si.snailsToSave.ToString());
                    elemStage.SetAttribute("snailsToRelease", si.snailsToRelease.ToString());
                    elemStage.SetAttribute("targetTime", string.Format("{0:00}:{1:00}:{2:00}", si.targetTime.Hours, si.targetTime.Minutes, si.targetTime.Seconds));
                    elemStage.SetAttribute("goal", si.goal.ToString());
                    elemStage.SetAttribute("availableInDemo", si.availableInDemo.ToString());
                    elemStage.SetAttribute("goldMedalTime", string.Format("{0:00}:{1:00}:{2:00}", si.goldMedalTime.Hours, si.goldMedalTime.Minutes, si.goldMedalTime.Seconds));
                    elemStage.SetAttribute("goldMedalScore", si.goldMedalScore.ToString());
                }
            }
            xmlLevelsDoc.Save(levelsFile);
        }

        /// <summary>
        /// 
        /// </summary>
        static StageInfo GetStageInfo(string themeId, string key)
        {
            string[] files = Directory.GetFiles(string.Format(".\\SnailsResourcesCustom\\stages\\{0}", themeId), "*.xdf");
            foreach (string file in files)
            {
                XmlDocument xmlStage = new XmlDocument();
                xmlStage.Load(file);
                StageInfo si = StageInfo.FromXmlElement((XmlElement)xmlStage.FirstChild);
                if (si.key == key)
                {
                    return si;
                }
            }
            throw new ApplicationException("Stage with key [" + key + "] not found.");
        }
    }
}
