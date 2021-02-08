using System;
using CloudOnce;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public Button nextLevelButton;
    public Text timeText;
    public Text levelText;
    public Text deathCountText;

    private string nextLevelName;
    private int worldNumber;
    private int levelNumber;
    private string leaderboardId;

    private void Awake()
    {
        var sceneNameParts = SceneManager.GetActiveScene().name.Split('_');
        worldNumber = int.Parse(sceneNameParts[1]);
        levelNumber = int.Parse(sceneNameParts[3]);
        nextLevelName = LevelLoader.GetSceneNameFromLevel(worldNumber, levelNumber + 1);
    }

    private void Start()
    {
        var levelName = $"Level {worldNumber}-{levelNumber}";
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
    /// <param name="seconds">How long it took to complete the level, in seconds.</param>
    public void SetCompletionTime(float seconds)
    {
        var completionTime = TimeSpan.FromSeconds(seconds);
        timeText.text = "Time: " + completionTime.ToString("mm':'ss'.'ff");

        var internalId = $"Level{worldNumber}_{levelNumber}";
        leaderboardId = Leaderboards.GetPlatformID(internalId);
        var leaderboardTimeScale = Application.platform == RuntimePlatform.IPhonePlayer ? 100 : 1000;
        Cloud.Leaderboards.SubmitScore(leaderboardId, (long)(seconds * leaderboardTimeScale), result =>
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
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Reloads the current level.
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
