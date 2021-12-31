using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Player;

namespace TwoBrainsGames.Snails.Player
{
    public enum AchievementsType
    { 
        None,
        /*1*/
        Unused1,
        Unused2, //- Safely escort 50 snails  
        SafelyEscort100Snails, //- Safely escort 100 snails
        SafelyEscort300Snails, //- Safely escort 500 snails
        SafelyEscort600Snails, //- Safely escort 1000 snails
        SafelyEscortAllSnailsInOneSpecificStage,//- Safely escort all snails in one specific stage 
        Unused7, 
        Save5SnailsKing, //- Save 5 Snails King
        Save10SnailsKing, //- Save 10 Snails King
        /*10*/
        Burn20Snails, //- Burn 20 snails
        Unused11,
        KillEverySnailInOneSpecificStage, //- Kill every snail in one specific stage
        Kill20SnailsWithASingleBox, //- kill 20 snails with a single box
        Kill50SnailsWithDynamite, //- Kill 50 snails with dynamite
        Kill50WithASingleDynamite, //- Kill 50 snails with a single dynamite
        Unused16,
        Kill20SnailsInSpikes, //- Kill 20 snails in spikes
        Kill20SnailsWithLazer, // - Kill 20 snails with lazer
        EmpaleOneSnailWhileFalling, //- Empale 1 snail while falling
        /*20*/
        HibernateASnail, //- Hibernate a snail 
        Unused21, 
        Boost50Snails, //- Boost 20 snails
        Unused23, 
        GetYourFirstBronzeMedal, //- Get your first bronze medal
        Get50BronzeCoins, //- Get 50 bronze medals
        Get100BronzeCoins, //- Get 100 bronze medals
        Unused27,
        Unused28,
        Get50SilverCoins, //- Get 50 silver medals
        /*30*/
        Get100SilverCoins, //- Get 100 silver medals
        Unused31, //- Get your first gold medal  
        Get50GoldCoins, //- Get 50 gold medals
        Get100GoldCoins, //- Get 100 gold medals
        Get200BronzeCoins, //- Get all bronze medals
        Get200SilverCoins,//- Get all silver medals
        Get200GoldCoins, //- Get all gold medals
        ClearAllWildNatureStages, //- Clear all Wild Nature Stages
        UnlockEgyptTheme, //- Unlock Egypt
        ClearAllEgyptStages, //- Clear all Egypt Stages
        /*40*/UnlockGraveyardTheme, //- Unlock Graveyard
        ClearAllGraveyardStages, //- Clear all Graveyard Stages
        UnlockGoldminesTheme, //- Unlock Goldmines
        ClearAllGoldminesStages, //- Clear all Goldmines Stages
        GetAllBronzeMedals, //- Get all bronze medals
        GetAllSilverMedals, //- Get all silver medals
        GetAllGoldMedals, //- Get all gold medals,
        PurchaseTheGame, // - Purchase the game
        KillSnailsIn11DifferentWays, // -Kill snails in 10 different ways
    }

    public enum EarnedAchievementDifficulty
    {
        Bronze,
        Silver,
        Gold
    }

    public static class Achievements
    {
        public const int Kill50WithASingleDynamite_Quantity = 70;
        public const int Kill50WithASingleBox_Quantity = 20;

        /// <summary>
        /// Generic register
        /// </summary>
        private static void RegisterAchievement(int eventType, Callback handler)
        {
            if (!SnailsGame.ProfilesManager.CurrentProfile.IsAchievementEarned(eventType))
            {
                SnailsGame.AchievementsManager.Register(eventType, handler);
            }
        }

        private static void AchievementEarned(int eventType)
        {
            // mark achievement as earned
            SnailsGame.ProfilesManager.CurrentProfile.MarkAchievementEarned(eventType);
            SnailsGame.ProfilesManager.Save();

            if (!SnailsGame.GameSettings.WithAppStore &&
                SnailsGame.AchievementsManager.GetAchievement(eventType).ShowOnAppStore)
            {
                return;
            }

            SnailsGame.AchievementsManager.QueueAchievement(eventType);
        }

        /// <summary>
        /// Static register called on Players Profile to initialize the correct achievements types
        /// </summary>
        public static void Register()
        {
            RegisterAchievement((int)AchievementsType.SafelyEscortAllSnailsInOneSpecificStage, OnSafelyEscortAllSnailsInOneSpecificStage);
            RegisterAchievement((int)AchievementsType.SafelyEscort100Snails, OnSafelyEscort100Snails);
            RegisterAchievement((int)AchievementsType.SafelyEscort300Snails, OnSafelyEscort300Snails);
            RegisterAchievement((int)AchievementsType.SafelyEscort600Snails, OnSafelyEscort600Snails);
            RegisterAchievement((int)AchievementsType.Save5SnailsKing, OnSave5SnailsKing);
            RegisterAchievement((int)AchievementsType.Save10SnailsKing, OnSave10SnailsKing);
            RegisterAchievement((int)AchievementsType.Burn20Snails, OnBurn20Snails);
            RegisterAchievement((int)AchievementsType.Kill20SnailsWithASingleBox, OnKill20SnailsWithASingleBox);
            RegisterAchievement((int)AchievementsType.Kill50SnailsWithDynamite, OnKill50SnailsWithDynamite);
            RegisterAchievement((int)AchievementsType.Kill50WithASingleDynamite, OnKill50WithASingleDynamite);
            RegisterAchievement((int)AchievementsType.Kill20SnailsInSpikes, OnKill20SnailsInSpikes);
            RegisterAchievement((int)AchievementsType.Kill20SnailsWithLazer, OnKill20SnailsWithLazer);
            RegisterAchievement((int)AchievementsType.EmpaleOneSnailWhileFalling, OnEmpaleOneSnailWhileFalling);
            RegisterAchievement((int)AchievementsType.HibernateASnail, OnHibernateASnail);
            RegisterAchievement((int)AchievementsType.Boost50Snails, OnBoost50Snails);
            RegisterAchievement((int)AchievementsType.Get50BronzeCoins, OnGet50BronzeMedals);
            RegisterAchievement((int)AchievementsType.Get100BronzeCoins, OnGet100BronzeMedals);
            RegisterAchievement((int)AchievementsType.Get50SilverCoins, OnGet50SilverMedals);
            RegisterAchievement((int)AchievementsType.Get100SilverCoins, OnGet100SilverMedals);
            RegisterAchievement((int)AchievementsType.Get50GoldCoins, OnGet50GoldMedals);
            RegisterAchievement((int)AchievementsType.Get100GoldCoins, OnGet100GoldMedals);
            RegisterAchievement((int)AchievementsType.Get200BronzeCoins, OnGet200BronzeMedals);
            RegisterAchievement((int)AchievementsType.Get200SilverCoins, OnGet200SilverMedals);
            RegisterAchievement((int)AchievementsType.Get200GoldCoins, OnGet200GoldMedals);
            RegisterAchievement((int)AchievementsType.ClearAllWildNatureStages, OnClearAllWildNatureStages);
            RegisterAchievement((int)AchievementsType.UnlockEgyptTheme, OnUnlockEgyptTheme);
            RegisterAchievement((int)AchievementsType.ClearAllEgyptStages, OnClearAllEgyptStages);
            RegisterAchievement((int)AchievementsType.UnlockGraveyardTheme, OnUnlockGraveyardTheme);
            RegisterAchievement((int)AchievementsType.ClearAllGraveyardStages, OnClearAllGraveyardStages);
            RegisterAchievement((int)AchievementsType.UnlockGoldminesTheme, OnUnlockGoldminesTheme);
            RegisterAchievement((int)AchievementsType.ClearAllGoldminesStages, OnClearAllGoldminesStages);
            RegisterAchievement((int)AchievementsType.GetAllBronzeMedals, OnGetAllBronzeMedals);
            RegisterAchievement((int)AchievementsType.GetAllSilverMedals, OnGetAllSilverMedals);
            RegisterAchievement((int)AchievementsType.GetAllGoldMedals, OnGetAllGoldMedals);
            RegisterAchievement((int)AchievementsType.PurchaseTheGame, OnPurchaseTheGame);
            RegisterAchievement((int)AchievementsType.KillSnailsIn11DifferentWays, OnKillSnailsIn11DifferentWays);
        }

        //SafelyEscort100Snails, //- Safely escort 100 snails  
        private static int OnSafelyEscort100Snails()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.SafelyEscort100Snails).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.SafelyEscort100Snails);
            }

            return value;
        }

        //SafelyEscort300Snails, //- Safely escort 300 snails
        private static int OnSafelyEscort300Snails()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.SafelyEscort300Snails).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.SafelyEscort300Snails);
            }

            return value;
        }

        //SafelyEscort600Snails, //- Safely escort 600 snails
        private static int OnSafelyEscort600Snails()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.SafelyEscort600Snails).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.SafelyEscort600Snails);
            }

            return value;
        }

        //SafelyEscortAllSnailsInOneSpecificStage,//- Safely escort all snails in one specific stage 
        private static int OnSafelyEscortAllSnailsInOneSpecificStage()
        {
            AchievementEarned((int)AchievementsType.SafelyEscortAllSnailsInOneSpecificStage);
            return -1;
        }

        //Save5SnailsKing, //- Save 5 Snails King
        private static int OnSave5SnailsKing()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Save5SnailsKing).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Save5SnailsKing);
            }

            return value;
        }

        //Save10SnailsKing, //- Save 10 Snails King
        private static int OnSave10SnailsKing()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Save10SnailsKing).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Save10SnailsKing);
            }

            return value;
        }

        /*10*/

        //Burn20Snails, //- Burn 20 snails
        private static int OnBurn20Snails()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Burn20Snails).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Burn20Snails);
            }

            return value;
        }

        //Kill20SnailsWithASingleBox, //- kill 20 snails with a single box
        private static int OnKill20SnailsWithASingleBox()
        {
            AchievementEarned((int)AchievementsType.Kill20SnailsWithASingleBox);
            return -1;
        }
        
        //Kill50SnailsWithDynamite, //- Kill 50 snails with dynamite
        private static int OnKill50SnailsWithDynamite()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Kill50SnailsWithDynamite).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Kill50SnailsWithDynamite);
            }

            return value;
        }
        
        //Kill50WithASingleDynamite, //- Kill 50 snails with a single dynamite
        private static int OnKill50WithASingleDynamite()
        {
            AchievementEarned((int)AchievementsType.Kill50WithASingleDynamite);
            
            return -1;
        }

        //Kill20SnailsInSpikes, //- Kill 20 snails in spikes
        private static int OnKill20SnailsInSpikes()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Kill20SnailsInSpikes).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Kill20SnailsInSpikes);
            }

            return value;
        }

        //Kill20SnailsWithLazer, // - Kill 20 snails with lazer
        private static int OnKill20SnailsWithLazer()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Kill20SnailsWithLazer).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Kill20SnailsWithLazer);
            }

            return value;
        }

        //EmpaleOneSnailWhileFalling, //- Empale 1 snail while falling
        private static int OnEmpaleOneSnailWhileFalling()
        {
            AchievementEarned((int)AchievementsType.EmpaleOneSnailWhileFalling);
            return -1;
        }

        //HibernateASnail, //- Hibernate a snail 
        private static int OnHibernateASnail()
        {
            AchievementEarned((int)AchievementsType.HibernateASnail);
            return -1;
        }

        //Boost50Snails, //- Boost 50 snails
        private static int OnBoost50Snails()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Boost50Snails).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Boost50Snails);
            }

            return value;
        }

        //Get50BronzeMedals, //- Get 50 bronze medals
        private static int OnGet50BronzeMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get50BronzeCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get50BronzeCoins);
            }

            return value;
        }

        //Get100BronzeMedals, //- Get 100 bronze medals
        private static int OnGet100BronzeMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get100BronzeCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get100BronzeCoins);
            }

            return value;
        }

        //Get50SilverMedals, //- Get 50 silver medals
        private static int OnGet50SilverMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get50SilverCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get50SilverCoins);
            }

            return value;
        }

        //Get100SilverMedals, //- Get 100 silver medals
        private static int OnGet100SilverMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get100SilverCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get100SilverCoins);
            }

            return value;
        }

        //Get50GoldMedals, //- Get 50 gold medals
        private static int OnGet50GoldMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get50GoldCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get50GoldCoins);
            }

            return value;
        }

        //Get100GoldMedals, //- Get 100 gold medals
        private static int OnGet100GoldMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get100GoldCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get100GoldCoins);
            }

            return value;
        }

        //Get200BronzeMedals, //- Get 200 bronze medals
        private static int OnGet200BronzeMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get200BronzeCoins).Quantity;

            if (value >= quantity) 
            {
                AchievementEarned((int)AchievementsType.Get200BronzeCoins);
            }

            return value;
        }

        //Get200SilverMedals,//- Get 200 silver medals
        private static int OnGet200SilverMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get200SilverCoins).Quantity;

            if (value >= quantity) 
            {
                AchievementEarned((int)AchievementsType.Get200SilverCoins);
            }

            return value;
        }

        //Get200GoldMedals, //- Get 200 gold medals
        private static int OnGet200GoldMedals()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.Get200GoldCoins).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.Get200GoldCoins);
            }

            return value;
        }

        //ClearAllWildNatureStages, //- Clear all Wild Nature Stages
        private static int OnClearAllWildNatureStages()
        {
            AchievementEarned((int)AchievementsType.ClearAllWildNatureStages);
            return -1;
        }

        //UnlockEgyptTheme, //- Unlock Egypt
        private static int OnUnlockEgyptTheme()
        {
            AchievementEarned((int)AchievementsType.UnlockEgyptTheme);
            return -1;
        }

        //ClearAllEgyptStages, //- Clear all Egypt Stages
        private static int OnClearAllEgyptStages()
        {
            AchievementEarned((int)AchievementsType.ClearAllEgyptStages);
            return -1;
        }

        //UnlockGraveyardTheme, //- Unlock Graveyard
        private static int OnUnlockGraveyardTheme()
        {
            AchievementEarned((int)AchievementsType.UnlockGraveyardTheme);
            return -1;
        }

        //ClearAllGraveyardStages, //- Clear all Graveyard Stages
        private static int OnClearAllGraveyardStages()
        {
            AchievementEarned((int)AchievementsType.ClearAllGraveyardStages);
            return -1;
        }

        //UnlockGoldminesTheme, //- Unlock Goldmines
        private static int OnUnlockGoldminesTheme()
        {
            AchievementEarned((int)AchievementsType.UnlockGoldminesTheme);
            return -1;
        }

        //ClearAllGoldminesStages, //- Clear all Goldmines Stages
        private static int OnClearAllGoldminesStages()
        {
            AchievementEarned((int)AchievementsType.ClearAllGoldminesStages);
            return -1;
        }

        //GetAllBronzeMedals, //- Get all bronze medals
        private static int OnGetAllBronzeMedals()
        {
            AchievementEarned((int)AchievementsType.GetAllBronzeMedals);
            return -1;
        }

        //GetAllSilverMedals, //- Get all silver medals
        private static int OnGetAllSilverMedals()
        {
            AchievementEarned((int)AchievementsType.GetAllSilverMedals);
            return -1;
        }

        //GetAllGoldMedals, //- Get all gold medals
        private static int OnGetAllGoldMedals()
        {
            AchievementEarned((int)AchievementsType.GetAllGoldMedals);
            return -1;
        }

        //PurchaseTheGame, // -Purchase the game
        private static int OnPurchaseTheGame()
        {
            AchievementEarned((int)AchievementsType.PurchaseTheGame);
            return -1;
        }

        //KillSnailsIn11DifferentWays, // -Kill snails in 11 different ways
        private static int OnKillSnailsIn11DifferentWays()
        {
            int value = SnailsGame.ProfilesManager.CurrentProfile.PlayerStats.SnailsDeadInDifferentWays;
            int quantity = SnailsGame.AchievementsManager.GetAchievement((int)AchievementsType.KillSnailsIn11DifferentWays).Quantity;

            if (value >= quantity)
            {
                AchievementEarned((int)AchievementsType.KillSnailsIn11DifferentWays);
            }

            return value;
        }
    }
}
