using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails
{
    // theme types - must match the name of theme files in lower case
    public enum ThemeType
    {
        ThemeA = 0, // important to start at 0 so we can auto calculate each stage theme
        ThemeB = 1,
        ThemeC = 2,
        ThemeD = 3,
        None = 5,
        All = 6
    }


    public enum MedalType
    {
        None,
        Bronze,
        Silver,
        Gold
    }

    public enum GoalType
    {
        SnailDelivery,
        SnailKiller,
        SnailKing,
        TimeAttack
    }

    public enum ToDataFileRecordContext
    {
        StageDataSave,
        StageSave
    }

    [Flags]
    public enum LanguageType
    {
        English = 0x01,
        Portuguese = 0x02,
        German = 0x04,
        French = 0x08,
        Spanish = 0x10,
        Italian = 0x020

    }
}
