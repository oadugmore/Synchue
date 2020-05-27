﻿using System.Collections.Generic;
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

    int currentWorldIndex;
    Button[] levelButtons;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        levelButtons = GetComponentsInChildren<Button>();
        previousWorldButton.interactable = false;
        CheckLevels();
    }

    private void CheckLevels()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (GetSceneNameFromLevel(currentWorldIndex + 1, i + 1) == null)
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
        var scene = "World_" + (currentWorldIndex + 1) + "_Level_" + levelNumber;
        sceneText.text = scene;
    }

    /// <summary>
    /// Switch to the next world.
    /// </summary>
    public void NextWorld()
    {
        currentWorldIndex++;
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
}
