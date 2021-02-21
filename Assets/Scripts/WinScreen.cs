using System;
using CloudOnce;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public Button nextLevelButton;
    public Button leaderboardButton;
    public Button wrButton;
    public Text levelText;
    public Text timeText;
    public Text wrText;
    public Text deathCountText;
    public TimeSpan completionTime;
    public TimeSpan currentPb;
    public TimeSpan currentWr;

    private string levelName;
    private string nextLevelName;
    private string leaderboardId;
    private const string menuSceneName = "Menu";

    private void Awake()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var worldAndLevel = LevelLoader.ParseWorldAndLevel(sceneName);
        levelName = $"Level {worldAndLevel[0]}-{worldAndLevel[1]}";
        nextLevelName = LevelLoader.GetSceneNameFromLevel(worldAndLevel[0], worldAndLevel[1] + 1);
        leaderboardId = MobileUtils.GetNativeLeaderboardId(sceneName);
    }

    private void Start()
    {
        if (completionTime == TimeSpan.Zero)
        {
            Debug.LogError("WinScreen was created without setting a valid completionTime!");
            return;
        }
        levelText.text = levelName;
        if (nextLevelName == null)
        {
            nextLevelButton.interactable = false;
        }
        deathCountText.text = $"Attempts: {DeathCounter.GetDeathCount() + 1}";
        leaderboardButton.onClick.AddListener(OpenLeaderboard);
        wrButton.onClick.AddListener(OpenLeaderboard);

        timeText.text = "Time: " + completionTime.ToString("mm':'ss'.'ff");
        var completionTimeCentiSeconds = (long)completionTime.TotalMilliseconds / 10;
        var leaderboardTimeScale = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 10;
        var submittedTime = completionTimeCentiSeconds * leaderboardTimeScale;

        wrButton.GetComponentInChildren<Text>().text = completionTime < currentWr ? "New WR" : "New PB";
        wrButton.gameObject.SetActive(completionTime < currentPb);
        leaderboardButton.gameObject.SetActive(!wrButton.gameObject.activeInHierarchy);
        if (currentWr > TimeSpan.Zero)
        {
            var wrTextPrefix = completionTime < currentWr ? "Previous WR: " : "Current WR: ";
            wrText.text = wrTextPrefix + currentWr.ToString("mm':'ss'.'ff");
        }

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
