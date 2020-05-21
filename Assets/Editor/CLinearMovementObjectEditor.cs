using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CLinearMovementObject))]
public class CLinearMovementObjectEditor : Editor
{
    SerializedProperty nodes;
    bool showNodes;

    void OnEnable() {
        nodes = serializedObject.FindProperty("nodes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        showNodes = EditorGUILayout.BeginFoldoutHeaderGroup(showNodes, "Nodes");
        if (showNodes)
        {
            // var nodes = (target as CLinearMovementObject)
            // nodes.
        }
    }
}