using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CRotationObject))]
public class CRotationObjectEditor : Editor
{
    SerializedProperty offset;
    CRotationObject t;
    CRotationNode[] nodes;
    List<SerializedObject> nodesSerialized = new List<SerializedObject>();
    List<SerializedObject> nodeTransforms = new List<SerializedObject>();
    List<SerializedProperty> nodeRotations = new List<SerializedProperty>();
    List<SerializedProperty> nodeWeights = new List<SerializedProperty>();
    List<SerializedProperty> nodeClockwise = new List<SerializedProperty>();
    private float previewCyclePos;
    // private int currentlyEditingNode = -1;
    private GUIStyle editNodesButtonStyle;

    void OnEnable()
    {
        t = target as CRotationObject;
        offset = serializedObject.FindProperty("m_offset");
        FindNodes();
        Undo.undoRedoPerformed += FindNodes;
        // editNodesButtonStyle.onNormal.background.SetPixels(new Color[] {Color.green});
        // editNodesButtonStyle.normal.background.SetPixels(new Color[] {Color.red});
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= FindNodes;
    }

    void FindNodes()
    {
        t.UpdateNodes();
        nodes = t.GetComponentsInChildren<CRotationNode>();
        nodesSerialized.Clear();
        nodeTransforms.Clear();
        nodeRotations.Clear();
        nodeWeights.Clear();
        nodeClockwise.Clear();
        foreach (var node in nodes)
        {
            var so = new SerializedObject(node);
            var transform = new SerializedObject(node.transform);
            nodesSerialized.Add(so);
            nodeTransforms.Add(transform);
            nodeRotations.Add(transform.FindProperty("m_LocalRotation"));
            nodeWeights.Add(so.FindProperty("m_weight"));
            nodeClockwise.Add(so.FindProperty("m_rotateClockwise"));
        }
        if (t.nodeSelectedForEditing >= nodes.Length)
        {
            t.nodeSelectedForEditing = -1;
        }
    }

    void CreateDefaultNodes()
    {
        
    }

    public override void OnInspectorGUI()
    {
        // Styles
        if (editNodesButtonStyle == null)
        {
            editNodesButtonStyle = "IN EditColliderButton";
        }

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
                newNode.AddComponent<CRotationNode>();
                newNode.transform.SetParent(t.transform, false);
                FindNodes();
            }
            if (GUILayout.Button("-"))
            {
                if (nodes.Length > 0)
                {
                    Undo.DestroyObjectImmediate(nodes[nodes.Length - 1].gameObject);
                    FindNodes();
                }
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < nodesSerialized.Count; i++)
            {
                nodesSerialized[i].Update();
                nodeTransforms[i].Update();

                EditorGUILayout.BeginHorizontal();
                var editing = (t.nodeSelectedForEditing == i);
                if (GUILayout.Toggle(editing, "Edit", editNodesButtonStyle))
                {
                    // if (editing)
                    // {

                    // }
                    // else
                    // {
                    t.nodeSelectedForEditing = i;
                    // }
                }
                else if (editing)
                {
                    t.nodeSelectedForEditing = -1;
                }
                EditorGUIUtility.labelWidth = 30;
                var eulerRotation = nodeRotations[i].quaternionValue.eulerAngles;
                // EditorGUILayout.PropertyField(nodeRotations[i], new GUIContent("Rot"), true);
                nodeRotations[i].quaternionValue = Quaternion.Euler(EditorGUILayout.Vector3Field("", eulerRotation));
                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.BeginHorizontal();
                // EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.PropertyField(nodeWeights[i]);
                // EditorGUIUtility.labelWidth = 30;
                EditorGUILayout.PropertyField(nodeClockwise[i], new GUIContent("CW", "Whether to rotate clockwise."));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
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
        if (t.showNodesInInspector && t.nodeSelectedForEditing != -1)
        {
            var node = nodes[t.nodeSelectedForEditing];
            EditorGUI.BeginChangeCheck();
            var newRot = Handles.RotationHandle(node.transform.localRotation, node.transform.position);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(node.transform, "Move node");
                node.transform.localRotation = newRot;
            }
            // var offset = 0f;
            // foreach (var node in nodes)
            // {

            //     offset++;
            // }
        }
        if (!Application.isPlaying)
        {
            t.UpdateCyclePosition(previewCyclePos);
            EditorHelper.ManualPhysicsStep();
        }
    }
}