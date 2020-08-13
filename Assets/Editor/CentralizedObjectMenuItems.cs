using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CentralizedObjectMenuItems : ScriptableObject
{
    private const string createCObjectMenuName = "Create Centralized Object";

    // TODO: Add version that constructs objects with a default (cube) mesh + renderer
    [MenuItem(createCObjectMenuName + "/Linear Movement Object")]
    public static void CreateCLinearMovementObject()
    {
        var selectedObjects = SelectedGameObjectsNotPartOfAnyPrefab();
        foreach (var obj in selectedObjects)
        {
            var containerObject = new GameObject("C Linear Movement Object");
            containerObject.AddComponent<CLinearMovementObject>();
            var movementObject = new GameObject("Movement Object");
            AddCustomRigidbody(movementObject);
            movementObject.transform.SetParent(containerObject.transform);
            for (int i = 0; i < 2; i++)
            {
                var node = new GameObject("Node " + (i + 1));
                node.AddComponent<CLinearMovementNode>();
                node.transform.SetParent(containerObject.transform);
            }
            containerObject.transform.SetParent(obj.transform);
            Undo.RegisterCreatedObjectUndo(containerObject, "Create CLinearMovementObject");
        }
    }

    [MenuItem(createCObjectMenuName + "/Rotation Object")]
    public static void CreateCRotationObject()
    {

    }

    [MenuItem(createCObjectMenuName + "/Elliptical Movement Object")]
    public static void CreateCEllipticalMovementObject()
    {

    }

    /// <summary>
    /// Adds a Rigidbody with standard Centralized Object properties to the GameObject.
    /// </summary>
    /// <param name="obj">The GameObject to add the Rigidbody to.</param>
    private static void AddCustomRigidbody(GameObject obj)
    {
        var rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    /// <summary>
    /// Gets a non-null list of currently selected GameObjects that are not
    /// part of any prefab.
    /// </summary>
    /// <returns>A non-null list of GameObjects that are currently selected and not part of any prefab.</returns>
    private static List<GameObject> SelectedGameObjectsNotPartOfAnyPrefab()
    {
        var objs = new List<GameObject>();
        foreach (var obj in Selection.gameObjects)
        {
            if (!PrefabUtility.IsPartOfAnyPrefab(obj))
            {
                objs.Add(obj);
            }
        }
        return objs;
    }
}