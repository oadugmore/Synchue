using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private Button nextLevelButton;

    private string nextLevelName;

    void Start()
    {
        int levelNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        nextLevelName = "Level_" + (levelNumber + 1);
        if (!Application.CanStreamedLevelBeLoaded(nextLevelName))
        {
            nextLevelButton.interactable = false;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    // public void DisableNextLevel()
    // {
    //     nextLevelButton.interactable = false;
    // }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        
    }
}
