using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCounter
{
    const string deathCountPrefsPrefix = "DeathCount-";
    private static string CurrentLevelName()
    {
        // var worldNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        // var levelNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[3]);
        // return string.Format("Phase {0}: Level {1}", worldNumber, levelNumber);
        // hardcode for now since it will just be reset anyway
        return "CurrentLevel";
    }

    public static void IncrementDeathCount()
    {
        var levelName = CurrentLevelName();
        var newDeaths = PlayerPrefs.GetInt(deathCountPrefsPrefix + levelName, 0) + 1;
        PlayerPrefs.SetInt(deathCountPrefsPrefix + levelName, newDeaths);
    }

    public static int GetDeathCount()
    {
        var levelName = CurrentLevelName();
        return PlayerPrefs.GetInt(deathCountPrefsPrefix + levelName, 0);
    }

    public static void ResetDeathCount()
    {
        var levelName = CurrentLevelName();
        PlayerPrefs.DeleteKey(deathCountPrefsPrefix + levelName);
    }
}
