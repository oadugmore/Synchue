using System;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// Initializes the win screen with the level completion time.
    /// </summary>
    /// <param name="seconds">How long it took to complete the level, in seconds.</param>
    public void SetCompletionTime(float seconds)
    {
        var completionTime = TimeSpan.FromSeconds(seconds);
        timeText.text = "Time: " + completionTime.ToString("mm':'ss'.'fff");
    }

    /// <summary>
    /// Initializes the win screen with the current level name and next level, if it exists.
    /// </summary>
    /// <param name="name">The friendly name of the current level.</param>
    /// <param name="nextSceneName">The name of the next scene. Leave this null if there is no proceeding level.</param>
    public void SetLevelNames(string name, string nextSceneName)
    {
        levelText.text = name;
        nextLevelName = nextSceneName;
        if (nextSceneName == null)
        {
            nextLevelButton.interactable = false;
        }
    }

    public void SetDeathCount(int deaths)
    {
        deathCountText.text = "Deaths: " + deaths;
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
