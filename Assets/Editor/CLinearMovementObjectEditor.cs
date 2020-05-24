using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CLinearMovementObject))]
public class CLinearMovementObjectEditor : Editor
{
    // ReorderableList nodeList;
    // SerializedProperty nodes;
    // bool showNodes;
    SerializedProperty offset;
    CLinearMovementObject t;
    CLinearMovementNode[] nodes;
    List<SerializedObject> nodesSerialized = new List<SerializedObject>();
    List<SerializedObject> nodeTransforms = new List<SerializedObject>();
    List<SerializedProperty> nodePositions = new List<SerializedProperty>();
    List<SerializedProperty> nodeWeights = new List<SerializedProperty>();


    void OnEnable()
    {
        t = target as CLinearMovementObject;
        offset = serializedObject.FindProperty("offset");
        FindNodes();
        Undo.undoRedoPerformed += FindNodes;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= FindNodes;
    }

    void FindNodes()
    {
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
            // Debug.Log("Node weight recorded: " + nodeWeights[nodeWeights.Count - 1].name);
            // Debug.Log("Node position recorded: " + nodePositions[nodePositions.Count - 1].name);
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
            Undo.RecordObject(target, "Toggle node visibility in inspector");
            t.showNodesInInspector = showNodes;
        }
        if (t.showNodesInInspector)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                var newNode = new GameObject("Node");
                Undo.RegisterCreatedObjectUndo(newNode, "Add node");
                // Undo.AddComponent<CLinearMovementNode>(newNode);
                newNode.AddComponent<CLinearMovementNode>();
                // Undo.SetTransformParent(t.transform, )
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
                EditorGUILayout.PropertyField(nodePositions[i]);
                // node.transform.localPosition = EditorGUILayout.Vector3Field("Pos", node.transform.localPosition);
                EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.PropertyField(nodeWeights[i], GUILayout.ExpandWidth(false)); //  GUILayout.MaxWidth(80)
                // node.weight = EditorGUILayout.FloatField("Weight", node.weight, GUILayout.MaxWidth(80));
                EditorGUILayout.EndHorizontal();
                nodesSerialized[i].ApplyModifiedProperties();
                nodeTransforms[i].ApplyModifiedProperties();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        
    }
}