using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CRotationObject))]
public class CRotationObjectEditor : Editor
{
    public Texture2D editButtonIcon;

    private SerializedProperty offset;
    private CRotationObject t;
    private CRotationNode[] nodes;
    private Transform rotationObject;
    private List<SerializedObject> nodesSerialized = new List<SerializedObject>();
    private List<SerializedObject> nodeTransforms = new List<SerializedObject>();
    private List<SerializedProperty> nodeRotations = new List<SerializedProperty>();
    private List<SerializedProperty> nodeWeights = new List<SerializedProperty>();
    private float previewCyclePos;
    private Quaternion currentRotationOfNodeHandle;

    void OnEnable()
    {
        t = target as CRotationObject;
        rotationObject = t.GetComponentInChildren<Rigidbody>().transform;
        offset = serializedObject.FindProperty("m_offset");
        FindNodes();
        Undo.undoRedoPerformed += FindNodes;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= FindNodes;
        Tools.hidden = false;
    }

    void FindNodes()
    {
        t.UpdateNodes();
        nodes = t.GetComponentsInChildren<CRotationNode>();
        nodesSerialized.Clear();
        nodeTransforms.Clear();
        nodeRotations.Clear();
        nodeWeights.Clear();
        foreach (var node in nodes)
        {
            var nodeSO = new SerializedObject(node);
            var transformSO = new SerializedObject(node.transform);
            nodesSerialized.Add(nodeSO);
            nodeTransforms.Add(transformSO);
            nodeRotations.Add(nodeSO.FindProperty("localRotationHint"));
            nodeWeights.Add(nodeSO.FindProperty("m_weight"));
        }
        if (t.nodeSelectedForEditing >= nodes.Length)
        {
            t.nodeSelectedForEditing = -1;
        }
    }

    private bool ApproximatelyEqualToClosestInt(float f)
    {
        return Mathf.Approximately(f, Mathf.Round(f));
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
            for (int i = 0; i < nodes.Length; i++)
            {
                nodesSerialized[i].Update();
                nodeTransforms[i].Update();
                var transform = nodes[i].transform;

                EditorGUILayout.BeginHorizontal();
                var editing = (t.nodeSelectedForEditing == i);
                if (GUILayout.Toggle(editing, editButtonIcon, EditorStyles.miniButton, GUILayout.MaxWidth(30)))
                {
                    t.nodeSelectedForEditing = i;
                    currentRotationOfNodeHandle = nodes[i].transform.localRotation;
                }
                else if (editing)
                {
                    t.nodeSelectedForEditing = -1;
                }
                EditorGUIUtility.labelWidth = 30;
                EditorGUILayout.PropertyField(nodeRotations[i], new GUIContent("Rot"));
                // EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = 50;
                // EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(nodeWeights[i], GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                // EditorGUILayout.Space();
                if (nodesSerialized[i].hasModifiedProperties)
                {
                    nodesSerialized[i].ApplyModifiedProperties();
                    t.UpdateNodes();
                }
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
            Tools.hidden = true;
            var node = nodes[t.nodeSelectedForEditing];
            EditorGUI.BeginChangeCheck();
            currentRotationOfNodeHandle = Handles.RotationHandle(currentRotationOfNodeHandle, node.transform.position);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(node, "Rotating node");
                node.localRotationHint = currentRotationOfNodeHandle.eulerAngles;
                node.transform.localRotation = currentRotationOfNodeHandle;
            }
        }
        else
        {
            Tools.hidden = false;
        }
        if (!Application.isPlaying)
        {
            rotationObject.rotation = t.CalculateRotation(previewCyclePos);
        }
    }
}
