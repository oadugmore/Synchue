using System;
using CloudOnce;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public Button nextLevelButton;
    public Button wrButton;
    public Text levelText;
    public Text timeText;
    public Text wrText;
    public Text deathCountText;

    private string levelName;
    private string nextLevelName;
    private string leaderboardId;
    private const string menuSceneName = "Menu";

    private void Awake()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var sceneNameParts = sceneName.Split('_');
        var worldNumber = int.Parse(sceneNameParts[1]);
        var levelNumber = int.Parse(sceneNameParts[3]);
        levelName = $"Level {worldNumber}-{levelNumber}";
        nextLevelName = LevelLoader.GetSceneNameFromLevel(worldNumber, levelNumber + 1);
        leaderboardId = MobileUtils.GetNativeLeaderboardId(sceneName);
    }

    private void Start()
    {
        levelText.text = levelName;
        if (nextLevelName == null)
        {
            nextLevelButton.interactable = false;
        }
        deathCountText.text = "Deaths: " + DeathCounter.GetDeathCount();
    }

    /// <summary>
    /// Initializes the win screen with the level completion time,
    /// and attempts to submit the score.
    /// </summary>
    /// <param name="completionTime">How long it took to complete the level.</param>
    public void SetTimeInfo(TimeSpan completionTime, TimeSpan currentPb, TimeSpan currentWr)
    {
        timeText.text = "Time: " + completionTime.ToString("mm':'ss'.'ff");
        var completionTimeCentiSeconds = (long)completionTime.TotalMilliseconds / 10;
        var leaderboardTimeScale = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 10;
        var submittedTime = completionTimeCentiSeconds * leaderboardTimeScale;

        wrButton.GetComponentInChildren<Text>().text = completionTime < currentWr ? "New WR" : "New PB";
        wrButton.gameObject.SetActive(completionTime < currentPb);
        var wrTextPrefix = completionTime < currentWr ? "Previous WR: " : "Current WR: ";
        if (currentWr > TimeSpan.Zero)
        {
            wrText.text = wrTextPrefix + currentWr.ToString("mm':'ss'.'ff");
        }

        // Submit new score
        Cloud.Leaderboards.SubmitScore(leaderboardId, submittedTime, result =>
        {
            if (result.HasError)
            {
                Debug.LogError("Failed to submit leaderboard score: " + result.Error);
            }
        });
    }

    /// <summary>
    /// Shows the native leaderboard UI for the current level.
    /// </summary>
    public void OpenLeaderboard()
    {
        Cloud.Leaderboards.ShowOverlay(leaderboardId);
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
        DeathCounter.ResetDeathCount();
    }

    /// <summary>
    /// Loads the menu.
    /// </summary>
    public void Menu()
    {
        AudioLooper.Shutdown();
        SceneManager.LoadScene(menuSceneName);
    }

    /// <summary>
    /// Reloads the current level.
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
