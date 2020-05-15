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

    private string nextLevelName;

    void Start()
    {
        int levelNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        nextLevelName = "Level_" + (levelNumber + 1);
        if (!Application.CanStreamedLevelBeLoaded(nextLevelName))
        {
            nextLevelButton.interactable = false;
        }
        var completionTime = TimeSpan.FromSeconds(FindObjectOfType<Goal>().GetCompletionTime());
        timeText.text = "Time: " + completionTime.ToString("mm':'ss'.'fff");
    }

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
