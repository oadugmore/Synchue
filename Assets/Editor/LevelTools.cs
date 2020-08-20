using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelTools : EditorWindow
{
    string levelName = "NewLevel";

    [MenuItem("Levels/New")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelTools>();
        window.titleContent = new GUIContent("Create New Level");
        window.Show();
    }

    private void OnGUI()
    {
        levelName = GUILayout.TextField(levelName);
        if (GUILayout.Button("Create"))
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            if (newScene.IsValid())
            {
                var buildSettingsScene = new EditorBuildSettingsScene(newScene.path, true);
                var scenes = EditorBuildSettings.scenes;
                ArrayUtility.Add(ref scenes, buildSettingsScene);
                EditorBuildSettings.scenes = scenes;
            }
            else
            {
                Debug.LogError("Failed to create scene.");
            }
        }
    }
}
