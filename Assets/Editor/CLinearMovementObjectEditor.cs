using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CLinearMovementObject))]
public class CLinearMovementObjectEditor : Editor
{
    // SerializedProperty nodes;
    SerializedProperty offset;
    CLinearMovementObject t;
    CLinearMovementNode[] nodes;
    List<SerializedObject> nodesSerialized = new List<SerializedObject>();
    List<SerializedObject> nodeTransforms = new List<SerializedObject>();
    List<SerializedProperty> nodePositions = new List<SerializedProperty>();
    List<SerializedProperty> nodeWeights = new List<SerializedProperty>();
    private float previewCyclePos;

    void OnEnable()
    {
        t = target as CLinearMovementObject;
        offset = serializedObject.FindProperty("m_offset");
        FindNodes();
        Undo.undoRedoPerformed += FindNodes;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= FindNodes;
    }

    void FindNodes()
    {
        t.UpdateNodes();
        nodes = t.GetComponentsInChildren<CLinearMovementNode>();
        nodesSerialized.Clear();
        nodeTransforms.Clear();
        nodePositions.Clear();
        nodeWeights.Clear();
        foreach (var node in nodes)
        {
            var so = new SerializedObject(node);
            var transform = new SerializedObject(node.transform);
            nodesSerialized.Add(so);
            nodeTransforms.Add(transform);
            nodeWeights.Add(so.FindProperty("m_weight"));
            nodePositions.Add(transform.FindProperty("m_LocalPosition"));
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUIUtility.wideMode = true;
        EditorGUILayout.PropertyField(offset);
        EditorGUI.BeginChangeCheck();
        var showNodes = EditorGUILayout.BeginFoldoutHeaderGroup(t.showNodesInInspector, "Nodes");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Toggle node visibility");
            t.showNodesInInspector = showNodes;
        }
        if (t.showNodesInInspector)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                var newNode = new GameObject("Node");
                Undo.RegisterCreatedObjectUndo(newNode, "Add node");
                newNode.AddComponent<CLinearMovementNode>();
                newNode.transform.SetParent(t.transform, false);
                FindNodes();
            }
            if (GUILayout.Button("-"))
            {
                if (nodes.Length > 0)
                {
                    Undo.DestroyObjectImmediate(nodes[nodes.Length - 1].gameObject);
                }
                FindNodes();
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < nodesSerialized.Count; i++)
            {
                nodesSerialized[i].Update();
                nodeTransforms[i].Update();
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 30;
                EditorGUILayout.PropertyField(nodePositions[i], new GUIContent("Pos"));
                EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.PropertyField(nodeWeights[i], GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                if (nodesSerialized[i].hasModifiedProperties)
                {
                    t.UpdateNodes();
                }
                nodesSerialized[i].ApplyModifiedProperties();
                nodeTransforms[i].ApplyModifiedProperties();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUIUtility.labelWidth = 0;
        previewCyclePos = EditorGUILayout.Slider("Preview cycle position", previewCyclePos, 0, 1);
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (t.showNodesInInspector)
        {
            foreach (var node in nodes)
            {
                EditorGUI.BeginChangeCheck();
                var newPos = Handles.PositionHandle(node.transform.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(node.transform, "Move node");
                    node.transform.position = newPos;
                }
            }
        }
        if (!Application.isPlaying)
        {
            t.UpdateCyclePosition(previewCyclePos);
            EditorHelper.ManualPhysicsStep();
        }
    }
}