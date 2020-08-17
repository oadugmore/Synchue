using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CentralizedObjectMenuItems : ScriptableObject
{
    private const string centralizeMenuName = "Add Centralized Control";

    [MenuItem(centralizeMenuName + "/Linear Movement")]
    public static void AddCLinearMovement()
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
            Undo.RegisterCreatedObjectUndo(containerObject, "Add CLinearMovement");
        }
    }

    [MenuItem(centralizeMenuName + "/Rotation")]
    public static void AddCRotation()
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
            Undo.RegisterCreatedObjectUndo(containerObject, "Add CRotation");
        }
    }

    [MenuItem(centralizeMenuName + "/Elliptical Movement")]
    public static void AddCEllipticalMovement()
    {
        var selectedObjects = SelectedGameObjectsNotPartOfAnyPrefab();
        foreach (var obj in selectedObjects)
        {
            var containerObject = AddContainerObject(obj);
            containerObject.name = "C Elliptical Movement Object";
            var emo = containerObject.AddComponent<CEllipticalMovementObject>();
            emo.verticalAxis = emo.horizontalAxis = 1f;
            AddCustomRigidbody(obj);
            Undo.RegisterCreatedObjectUndo(containerObject, "Add CEllipticalMovement" + obj.name);
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
    /// Manually sets Rigidbody stats that are normally only changed while the game is playing.
    /// Use this if an ExecuteInEditMode script misbehaves.
    /// </summary>
    [MenuItem(centralizeMenuName + "/Debug/Reset Rigidbody Kinematics")]
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

    /// <summary>
    /// Gets a non-null list of currently selected GameObjects that are
    /// in the scene, including prefab instances.
    /// </summary>
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