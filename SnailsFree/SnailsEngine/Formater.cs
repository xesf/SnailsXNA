using TwoBrainsGames.Snails.Stages;
using System;
using TwoBrainsGames.BrainEngine.Localization;

namespace TwoBrainsGames.Snails
{
    class Formater
    {
        /// <summary>
        /// 
        /// </summary>
        public static string FormatModeName(GoalType goal)
        {
           switch (goal)
            {
                case GoalType.SnailDelivery:
                    return LanguageManager.GetString("GAME_MODE_ESCORT");
                case GoalType.SnailKiller:
                    return LanguageManager.GetString("GAME_MODE_KILLER");
                case GoalType.SnailKing:
                    return LanguageManager.GetString("GAME_MODE_KING");
                case GoalType.TimeAttack:
                    return LanguageManager.GetString("GAME_MODE_TIME");
            }
            throw new SnailsException("Invalid goal type [" + goal.ToString() + "]");
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FormatGoalDescription(LevelStage levelStage)
        {
            return Formater.FormatGoalDescription(levelStage._goal, levelStage._snailsToSave, levelStage._targetTime, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FormatGoalDescription(LevelStage levelStage, bool formatForHud)
        {
            return Formater.FormatGoalDescription(levelStage._goal, levelStage._snailsToSave, levelStage._targetTime, formatForHud);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string FormatGoalDescription(GoalType goal, int snailsToDeliver, TimeSpan targetTime, bool formatForHud)
        {
            string goalStr = "";
            switch (goal)
            {
                case GoalType.SnailDelivery:
                    if (snailsToDeliver == 1)
                    {
                        goalStr = string.Format(LanguageManager.GetString("GAME_MODE_ESCORT_GOAL_SINGLE"));
                    }
                    else
                    {
                        goalStr = string.Format(LanguageManager.GetString("GAME_MODE_ESCORT_GOAL"), snailsToDeliver);
                    }
                    break;
                case GoalType.TimeAttack:
                    if (!formatForHud)
                    {
                        if (snailsToDeliver == 1)
                        {
                            goalStr = string.Format(LanguageManager.GetString("GAME_MODE_TIME_GOAL_ONE"), snailsToDeliver, targetTime.Minutes, targetTime.Seconds);
                        }
                        else
                        {
                            goalStr = string.Format(LanguageManager.GetString("GAME_MODE_TIME_GOAL"), snailsToDeliver, targetTime.Minutes, targetTime.Seconds);
                        }
                    }
                    else
                    {
                        if (snailsToDeliver == 1)
                        {
                            goalStr = string.Format(LanguageManager.GetString("GAME_MODE_TIME_GOAL_HUD_ONE"), snailsToDeliver, targetTime.Minutes, targetTime.Seconds);
                        }
                        else
                        {
                            goalStr = string.Format(LanguageManager.GetString("GAME_MODE_TIME_GOAL_HUD"), snailsToDeliver, targetTime.Minutes, targetTime.Seconds);
                        }
                    }
                    break;
                case GoalType.SnailKiller:
                    goalStr = LanguageManager.GetString("GAME_MODE_KILLER_GOAL");
                    break;
                case GoalType.SnailKing:
                    goalStr = LanguageManager.GetString("GAME_MODE_KING_GOAL");
                    break;
            }

            return goalStr;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetThemeName(ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.ThemeA:
                    return LanguageManager.GetString("THEME_A_NAME");
                case ThemeType.ThemeB:
                    return LanguageManager.GetString("THEME_B_NAME");
                case ThemeType.ThemeC:
                    return LanguageManager.GetString("THEME_C_NAME");
                case ThemeType.ThemeD:
                    return LanguageManager.GetString("THEME_D_NAME");
            }
            return "";
        }


        public static string FormatLevelTime(TimeSpan ts)
        {
            return string.Format("{0:00}:{1:00}", (ts.Hours * 60) + ts.Minutes, ts.Seconds);
        }

        public static string FormatLevelScore(int score)
        {
            return string.Format("{0} pts", score);
        }
    }
}
