using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public void LoadLevel(int levelNumber)
    {
        string sceneName = "Level" + levelNumber;
        SceneManager.LoadScene(sceneName);
    }

    public void InputTest()
    {
        SceneManager.LoadScene("InputTest");
    }

}
