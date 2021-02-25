using System.Collections.Generic;
using CloudOnce;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public float cameraTransitionTime = 1;
    public LeanTweenType cameraTransitionType;
    public List<Transform> cameraLocations;
    public InputField sceneText;
    public Button nextWorldButton;
    public Button previousWorldButton;
    public Button playButton;

    int currentWorldIndex;
    Button[] levelButtons;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        levelButtons = GetComponentsInChildren<Button>();
        currentWorldIndex = Settings.currentWorldIndex;
        if (currentWorldIndex == 0)
        {
            previousWorldButton.interactable = false;
        }
        else if (currentWorldIndex == cameraLocations.Count - 1)
        {
            nextWorldButton.interactable = false;
        }
        nextWorldButton.onClick.AddListener(NextWorld);
        previousWorldButton.onClick.AddListener(PreviousWorld);
        mainCamera.transform.position = cameraLocations[currentWorldIndex].position;
        CheckLevels();
        MobileUtils.InitializeVibrator();
        SFX.Initialize();
    }

    private void CheckLevels()
    {
        playButton.interactable = false;
        for (int i = 0; i < levelButtons.Length; i++)
        {
            var sceneName = GetSceneNameFromLevel(currentWorldIndex + 1, i + 1);
            if (sceneName == null)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
            var checkMark = levelButtons[i].GetComponentsInChildren<Image>(true)[1].gameObject;
            if (Settings.LevelIsCleared(sceneName))
            {
                checkMark.SetActive(true);
            }
            else
            {
                checkMark.SetActive(false);
            }
        }
    }

    public void ShowLeaderboard()
    {
        var leaderboardId = MobileUtils.GetNativeLeaderboardId(sceneText.text);
        Cloud.Leaderboards.ShowOverlay(leaderboardId);
        playButton.interactable = false;
    }

    /// <summary>
    /// Load the currently selected level.
    /// </summary>
    public void LoadScene()
    {
        DeathCounter.ResetDeathCount();
        SceneManager.LoadScene(sceneText.text);
    }

    /// <summary>
    /// Select a level by its number.
    /// </summary>
    public void LoadLevel(int levelNumber)
    {
        var scene = "World_" + (currentWorldIndex + 1) + "_Level_" + levelNumber;
        sceneText.text = scene;
        playButton.interactable = true;
    }

    /// <summary>
    /// Switch to the next world.
    /// </summary>
    public void NextWorld()
    {
        currentWorldIndex++;
        Settings.currentWorldIndex = currentWorldIndex;
        CheckLevels();
        MoveToNextCameraLocation();
        previousWorldButton.interactable = true;
        if (currentWorldIndex + 1 >= cameraLocations.Count)
        {
            nextWorldButton.interactable = false;
        }
        else
        {
            nextWorldButton.interactable = true;
        }
    }

    /// <summary>
    /// Switch to the previous world.
    /// </summary>
    public void PreviousWorld()
    {
        currentWorldIndex--;
        Settings.currentWorldIndex = currentWorldIndex;
        CheckLevels();
        MoveToNextCameraLocation();
        nextWorldButton.interactable = true;
        if (currentWorldIndex <= 0)
        {
            previousWorldButton.interactable = false;
        }
        else
        {
            previousWorldButton.interactable = true;
        }
    }

    private void MoveToNextCameraLocation()
    {
        var nextPos = cameraLocations[currentWorldIndex].position;
        LeanTween.move(mainCamera.gameObject, nextPos, cameraTransitionTime).setEase(cameraTransitionType);
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

    public static int[] ParseWorldAndLevel(string sceneName)
    {
        var sceneNameParts = sceneName.Split('_');
        var worldNumber = int.Parse(sceneNameParts[1]);
        var levelNumber = int.Parse(sceneNameParts[3]);
        return new int[] { worldNumber, levelNumber };
    }
}
