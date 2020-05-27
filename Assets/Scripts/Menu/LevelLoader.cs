using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public InputField sceneText;
    int currentWorld = 1;
    Button[] levelButtons;

    private void Start() {
        levelButtons = GetComponentsInChildren<Button>();

    }

    // Load the currently selected level.
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneText.text);
    }

    // Select a level by its number.
    public void LoadLevel(int levelNumber)
    {
        var scene = "World_" + currentWorld + "_Level_" + levelNumber;
        sceneText.text = scene;
    }

    public void NextWorld()
    {
        currentWorld++;
        // move camera and update levels
    }

    public void PreviousWorld()
    {
        currentWorld--;
        // move camera and update levels
    }
}
