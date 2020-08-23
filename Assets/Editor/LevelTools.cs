using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelTools : EditorWindow
{
    string levelName = "NewLevel";

    [MenuItem("Levels/Initialize")]
    private static void Initialize()
    {
        // var window = GetWindow<LevelTools>();
        // window.titleContent = new GUIContent("Create New Level");
        // window.Show();
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Initialize Level");
        var undoGroupIndex = Undo.GetCurrentGroup();

        var existingCamera = FindObjectOfType<Camera>();
        if (existingCamera)
        {
            Undo.DestroyObjectImmediate(existingCamera.gameObject);
        }
        var existingLight = FindObjectOfType<Light>();
        if (existingLight)
        {
            Undo.DestroyObjectImmediate(existingLight.gameObject);
        }
        var existingCanvas = FindObjectOfType<Canvas>();
        if (existingCanvas)
        {
            Undo.DestroyObjectImmediate(existingCanvas.gameObject);
        }

        var cameraPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Main Camera.prefab", typeof(GameObject));
        var lightPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Directional Light.prefab", typeof(GameObject));
        var canvasPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI Canvas.prefab", typeof(GameObject));
        var instantiatedCamera = PrefabUtility.InstantiatePrefab(cameraPrefab);
        var instantiatedLight = PrefabUtility.InstantiatePrefab(lightPrefab);
        var instantiatedCanvas = PrefabUtility.InstantiatePrefab(canvasPrefab);
        Undo.RegisterCreatedObjectUndo(instantiatedCamera, "camera");
        Undo.RegisterCreatedObjectUndo(instantiatedLight, "light");
        Undo.RegisterCreatedObjectUndo(instantiatedCanvas, "UICanvas");

        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(eventSystem, "evtSystem");
        }

        Undo.CollapseUndoOperations(undoGroupIndex);

        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;

        var thisScene = EditorSceneManager.GetActiveScene();
        if (thisScene.path == "")
        {
            Debug.LogWarning("Could not add scene to build settings because it has not yet been saved.");
            return;
        }
        var buildSettingsScene = new EditorBuildSettingsScene(thisScene.path, true);
        var scenes = EditorBuildSettings.scenes;
        foreach (var scene in scenes)
        {
            if (scene.path == thisScene.path)
            {
                Debug.Log("Not adding scene to build settings because it already exists.");
                return;
            }
        }
        ArrayUtility.Add(ref scenes, buildSettingsScene);
        EditorBuildSettings.scenes = scenes;
    }
}
