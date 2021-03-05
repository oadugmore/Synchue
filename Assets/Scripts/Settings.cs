using UnityEngine;

public class Settings
{
    private const string MUSIC_ENABLED_SETTING_NAME = "MusicEnabled";
    private const string DEATH_HAPTICS_ENABLED_SETTING_NAME = "DeathHapticsEnabled";
    private const string GOAL_HAPTICS_ENABLED_SETTING_NAME = "GoalHapticsEnabled";
    private const string DEATH_SOUND_ENABLED_SETTING_NAME = "DeathSoundEnabled";
    private const string GOAL_SOUND_ENABLED_SETTING_NAME = "GoalSoundEnabled";
    private const string HUD_TIMER_ENABLED_SETTING_NAME = "HudTimerEnabled";
    private const string RESTART_BUTTON_ENABLED_SETTING_NAME = "RestartButtonEnabled";
    private const string GOOGLE_PLAY_SIGNED_IN_SETTING_NAME = "GooglePlaySignedIn";
    private const string CURRENT_WORLD_INDEX_NAME = "CurrentWorldIndex";
    private const string LEVEL_CLEARED_PREFIX = "LevelCleared:";

    public static bool LevelIsCleared(string sceneName)
    {
        return Get(LEVEL_CLEARED_PREFIX + sceneName, false);
    }

    public static void ClearLevel(string sceneName)
    {
        Set(LEVEL_CLEARED_PREFIX + sceneName, true);
    }

    public static int currentWorldIndex
    {
        get
        {
            return Get(CURRENT_WORLD_INDEX_NAME, 0);
        }
        set
        {
            Set(CURRENT_WORLD_INDEX_NAME, value);
        }
    }

    public static bool musicEnabled
    {
        get
        {
            return Get(MUSIC_ENABLED_SETTING_NAME, true);
        }
        set
        {
            Set(MUSIC_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool deathHapticsEnabled
    {
        get
        {
            return Get(DEATH_HAPTICS_ENABLED_SETTING_NAME, true);
        }
        set
        {
            Set(DEATH_HAPTICS_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool goalHapticsEnabled
    {
        get
        {
            return Get(GOAL_HAPTICS_ENABLED_SETTING_NAME, true);
        }
        set
        {
            Set(GOAL_HAPTICS_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool deathSoundEnabled
    {
        get
        {
            return Get(DEATH_SOUND_ENABLED_SETTING_NAME, true);
        }
        set
        {
            Set(DEATH_SOUND_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool goalSoundEnabled
    {
        get
        {
            return Get(GOAL_SOUND_ENABLED_SETTING_NAME, true);
        }
        set
        {
            Set(GOAL_SOUND_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool hudTimerEnabled
    {
        get
        {
            return Get(HUD_TIMER_ENABLED_SETTING_NAME, false);
        }
        set
        {
            Set(HUD_TIMER_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool restartButtonEnabled
    {
        get
        {
            return Get(RESTART_BUTTON_ENABLED_SETTING_NAME, false);
        }
        set
        {
            Set(RESTART_BUTTON_ENABLED_SETTING_NAME, value);
        }
    }

    public static bool googlePlaySignedIn
    {
        get
        {
            return Get(GOOGLE_PLAY_SIGNED_IN_SETTING_NAME, true);
        }
        set
        {
            Set(GOOGLE_PLAY_SIGNED_IN_SETTING_NAME, value);
        }
    }

    private static void Set(string settingName, bool value)
    {
        PlayerPrefs.SetInt(settingName, value ? 1 : 0);
    }

    private static void Set(string settingName, int value)
    {
        PlayerPrefs.SetInt(settingName, value);
    }

    private static bool Get(string settingName, bool defaultValue)
    {
        return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) == 1;
    }

    private static int Get(string settingName, int defaultValue)
    {
        return PlayerPrefs.GetInt(settingName, defaultValue);
    }
}
