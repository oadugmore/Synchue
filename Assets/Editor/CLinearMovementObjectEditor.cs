using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CLinearMovementObject))]
public class CLinearMovementObjectEditor : Editor
{
    // ReorderableList nodeList;
    // SerializedProperty nodes;
    // bool showNodes;
    CLinearMovementObject t;
    CLinearMovementNode[] nodes;

    void OnEnable()
    {
        t = target as CLinearMovementObject;
        FindNodes();
        // // nodes = serializedObject.FindProperty("nodes");
        // nodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("nodes"), true, true, true, true);
        // nodeList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        // {
        //     var element = nodeList.serializedProperty.GetArrayElementAtIndex(index);
        //     // var node = () element.objectReferenceValue
        //     var nodeObject = new SerializedObject(element.objectReferenceValue);
        //     var pos = nodeObject.FindProperty("m_position");
        //     var weight = nodeObject.FindProperty("m_weight");
        //     Debug.Log(pos.vector3Value);
        //     Debug.Log(weight.floatValue);
        //     //EditorGUI.Vector3Field(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Pos", pos.vector3Value);
        // };
        // nodeList.onAddCallback = 
    }

    void FindNodes()
    {
        nodes = t.GetComponentsInChildren<CLinearMovementNode>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUIUtility.wideMode = true;
        t.offset = EditorGUILayout.FloatField("Offset", t.offset);
        t.showNodesInInspector = EditorGUILayout.BeginFoldoutHeaderGroup(t.showNodesInInspector, "Nodes");
        if (t.showNodesInInspector)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                var newNode = new GameObject("Node");
                newNode.AddComponent<CLinearMovementNode>();
                newNode.transform.SetParent(t.transform, false);
                FindNodes();
            }
            if (GUILayout.Button("-"))
            {
                if (nodes.Length > 0)
                {
                    DestroyImmediate(nodes[nodes.Length - 1].gameObject);
                }
                FindNodes();
            }
            EditorGUILayout.EndHorizontal();
            foreach (var node in nodes)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 30;
                node.transform.localPosition = EditorGUILayout.Vector3Field("Pos", node.transform.localPosition);
                EditorGUIUtility.labelWidth = 50;
                node.weight = EditorGUILayout.FloatField("Weight", node.weight, GUILayout.MaxWidth(80));
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        // nodeList.DoLayoutList();
        // serializedObject.ApplyModifiedProperties();
        // EditorUtility.SetDirty(target);
        // DrawDefaultInspector();
        // showNodes = EditorGUILayout.BeginFoldoutHeaderGroup(showNodes, "CustomNodes");
        // if (showNodes)
        // {
        //     //SerializedProperty sp = nodes.Copy();

        //     if (nodes.isArray)
        //     {
        //         int size = nodes.arraySize;
        //         var nodeValues = new List<CLinearMovementNode>();
        //         for (int i = 0; i < size; ++i)
        //         {
        //             var node = (CLinearMovementNode)nodes.GetArrayElementAtIndex(i).objectReferenceValue;
        //             nodeValues.Add(node);
        //             var newPos = EditorGUILayout.Vector3Field("Pos", node.position);
        //             var newWeight = EditorGUILayout.FloatField("Weight", node.weight);
        //             node.position = newPos;
        //             node.weight = newWeight;
        //         }

        //     if (GUILayout.Button("+"))
        //     {
        //         nodeValues.Add(new CLinearMovementNode());
        //         nodes.objectReferenceValue = nodeValues;
        //         // nodes.arraySize++;
        //     }
        //     }
        // var nodes = (target as CLinearMovementObject)
        // }
        // this.DrawDefaultInspector();
    }
}