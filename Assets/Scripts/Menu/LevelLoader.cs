using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public InputField sceneText;
    int currentWorld = 1;
    Button[] levelButtons;

    private void Start()
    {
        levelButtons = GetComponentsInChildren<Button>();
        CheckLevels();
    }

    private void CheckLevels()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (GetSceneNameFromLevel(currentWorld, i + 1) == null)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    /// <summary>
    /// Load the currently selected level.
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneText.text);
    }

    /// <summary>
    /// Select a level by its number.
    /// </summary>
    public void LoadLevel(int levelNumber)
    {
        var scene = "World_" + currentWorld + "_Level_" + levelNumber;
        sceneText.text = scene;
    }

    /// <summary>
    /// Switch to the next world.
    /// </summary>
    public void NextWorld()
    {
        currentWorld++;
        CheckLevels();
        // move camera
    }

    /// <summary>
    /// Switch to the previous world.
    /// </summary>
    public void PreviousWorld()
    {
        currentWorld--;
        CheckLevels();
        // move camera
    }

    /// <summary>
    /// Given a world and level, returns the name of the corresponding scene, or null if the level doesn't exist.
    /// </summary>
    public static string GetSceneNameFromLevel(int world, int level)
    {
        var sceneName = string.Format("World_{0}_Level_{1}", world, level);
        if (!IsValidLevel(sceneName))
        {
            sceneName = null;
        }
        return sceneName;
    }

    /// <summary>
    /// Returns true if the given scene exists.
    /// </summary>
    public static bool IsValidLevel(string sceneName)
    {
        return Application.CanStreamedLevelBeLoaded(sceneName);
    }
}
