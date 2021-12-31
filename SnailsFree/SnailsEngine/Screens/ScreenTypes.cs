using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.Screens
{
    public enum ScreenGroupType
    {
        Intro,
        MainMenu,
        InGame,
        GameCompletion
    }

    public enum ScreenType
    {
        None,
        BrainsLogo,
        Intro,
        MainMenu,
        Gameplay,
        InGameOptions,
        Transition,
        Options,
        Credits,
        Overscan,
        ThemeSelection,
        StageSelection,
        DebugOptions,
        Congrats,
        Recipe,
        StageCompleted,
        StageStart,
        AutoSave,
        Startup,
        Quit,
        NewGame,
        MissionFailed,
        ThemeUnlocked,
        XBoxControllerHelp,
        HowToPlay,
        Purchase,
        Awards,
        PlayerStats,
        HintWarning,
        Rate
    }
}
