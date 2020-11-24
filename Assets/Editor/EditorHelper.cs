using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorHelper
{
    private static CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
    private static NewSceneSetup setup = NewSceneSetup.EmptyScene;
    private static NewSceneMode mode = NewSceneMode.Additive;
    static Scene scene;
    static PhysicsScene physicsScene;

    public static void ManualPhysicsStepGlobal()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }

    public static void ManualPhysicsStepFor(GameObject obj)
    {
        Physics.autoSimulation = false;
        if (!scene.IsValid())
        {
            scene = EditorSceneManager.NewScene(setup, mode);
        }
        if (!physicsScene.IsValid())
        {
            physicsScene = scene.GetPhysicsScene();
        }

        GameObject rootObj = obj.transform.root.gameObject;
        Scene currentScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MoveGameObjectToScene(rootObj, scene);
        physicsScene.Simulate(Time.fixedDeltaTime);
        EditorSceneManager.MoveGameObjectToScene(rootObj, currentScene);
        Physics.autoSimulation = true;
    }
}
