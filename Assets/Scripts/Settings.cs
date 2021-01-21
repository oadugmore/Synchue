using UnityEngine;

public class Settings
{
    private const string MUSIC_ENABLED_SETTING_NAME = "MusicEnabled";
    private const string DEATH_HAPTICS_ENABLED_SETTING_NAME = "DeathHapticsEnabled";
    private const string GOAL_HAPTICS_ENABLED_SETTING_NAME = "GoalHapticsEnabled";
    private const string DEATH_SOUND_ENABLED_SETTING_NAME = "DeathSoundEnabled";
    private const string GOAL_SOUND_ENABLED_SETTING_NAME = "GoalSoundEnabled";

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

    private static void Set(string settingName, bool value)
    {
        PlayerPrefs.SetInt(settingName, value ? 1 : 0);
    }

    private static bool Get(string settingName, bool defaultValue)
    {
        return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) == 1;
    }
}
