using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Text sceneText;

    // Load any scene by its name.
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneText.text);
    }

    // Load a level by its number.
    public void LoadLevel(int levelNumber)
    {
        string sceneName = "Level" + levelNumber;
        SceneManager.LoadScene(sceneName);
    }

}
