using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private Button nextLevelButton;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text levelText;

    private string nextLevelName;

    void Start()
    {
        // levelText.text = "Level " + levelNumber;
    }

    public void SetCompletionTime(float seconds)
    {
        var completionTime = TimeSpan.FromSeconds(seconds);
        timeText.text = "Time: " + completionTime.ToString("mm':'ss'.'fff");
    }

    public void SetLevelNames(string name, string nextSceneName)
    {
        levelText.text = name;
        nextLevelName = nextSceneName;
        if (nextSceneName == null)
        {
            nextLevelButton.interactable = false;
        }
    }

    // public void SetNextLevelName(string sceneName)
    // {
        
    // }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
