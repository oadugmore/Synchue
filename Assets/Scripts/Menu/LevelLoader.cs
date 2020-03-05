using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public InputField sceneText;

    // Load any scene by its name.
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneText.text);
    }

    // Load a level by its number.
    public void LoadLevel(int levelNumber)
    {
        var scene = "Level" + levelNumber;
        sceneText.text = scene;
    }
}
