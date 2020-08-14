using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CentralizedObjectMenuItems : ScriptableObject
{
    private const string centralizeMenuName = "Centralize-ize";

    [MenuItem(centralizeMenuName + "/Linear Movement")]
    public static void CreateCLinearMovementObject()
    {
        var selectedObjects = SelectedGameObjectsNotPartOfAnyPrefab();
        foreach (var obj in selectedObjects)
        {
            var containerObject = AddContainerObject(obj);
            containerObject.name = "C Linear Movement Object";
            containerObject.AddComponent<CLinearMovementObject>();
            AddCustomRigidbody(obj);
            for (int i = 0; i < 2; i++)
            {
                var node = new GameObject("Node " + (i + 1));
                node.AddComponent<CLinearMovementNode>();
                node.transform.SetParent(containerObject.transform);
            }
            Undo.RegisterCreatedObjectUndo(containerObject, "Create CLinearMovementObject");
        }
    }

    [MenuItem(centralizeMenuName + "/Rotation Object")]
    public static void CreateCRotationObject()
    {
        var selectedObjects = SelectedGameObjectsNotPartOfAnyPrefab();
        foreach (var obj in selectedObjects)
        {
            var containerObject = AddContainerObject(obj);
            containerObject.name = "C Rotation Object";
            containerObject.AddComponent<CRotationObject>();
            AddCustomRigidbody(obj);
            for (int i = 0; i < 2; i++)
            {
                var node = new GameObject("Node " + (i + 1));
                node.AddComponent<CRotationNode>();
                node.transform.SetParent(containerObject.transform, false);
            }
            Undo.RegisterCreatedObjectUndo(containerObject, "Create CRotationObject");
        }
    }

    [MenuItem(centralizeMenuName + "/Elliptical Movement Object")]
    public static void CreateCEllipticalMovementObject()
    {
        var selectedObjects = SelectedGameObjectsNotPartOfAnyPrefab();
        foreach (var obj in selectedObjects)
        {
            var containerObject = AddContainerObject(obj);
            containerObject.name = "C Elliptical Movement Object";
            containerObject.AddComponent<CEllipticalMovementObject>();
            AddCustomRigidbody(obj);
            Undo.RegisterCreatedObjectUndo(containerObject, "Create CEllipticalMovementObject");
        }
    }

    /// <summary>
    /// Sets the GameObject as a child of a new GameObject which takes the old GameObject's position.
    /// </summary>
    /// <returns>The newly created container GameObject</returns>
    private static GameObject AddContainerObject(GameObject obj)
    {
        var containerObject = new GameObject();
        var objOriginalParent = obj.transform.parent;
        containerObject.transform.position = obj.transform.position;
        obj.transform.SetParent(containerObject.transform);
        containerObject.transform.SetParent(objOriginalParent);
        return containerObject;
    }

    /// <summary>
    /// 
    /// </summary>
    [MenuItem(centralizeMenuName + "/Reset Rigidbody Kinematics")]
    public static void ResetRigidbodyKinematics()
    {
        var selectedObjects = SelectedGameObjectsInScene();
        foreach (var obj in selectedObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = rb.angularVelocity = Vector3.zero;
                rb.inertiaTensorRotation = Quaternion.identity;
                rb.ResetInertiaTensor();
                rb.ResetCenterOfMass();
            }
        }
    }

    /// <summary>
    /// Adds a Rigidbody with standard Centralized Object properties to the GameObject.
    /// If the GameObject already has a Rigidbody, it applies the properties to it.
    /// </summary>
    /// <param name="obj">The GameObject to add the Rigidbody to.</param>
    private static void AddCustomRigidbody(GameObject obj)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (!rb)
        {
            rb = obj.AddComponent<Rigidbody>();
        }
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

    private static List<GameObject> SelectedGameObjectsInScene()
    {
        var objs = new List<GameObject>();
        foreach (var obj in Selection.gameObjects)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                objs.Add(obj);
            }
        }
        return objs;
    }
}