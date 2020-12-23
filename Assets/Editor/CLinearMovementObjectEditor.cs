using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(CLinearMovementObject)), CanEditMultipleObjects]
public class CLinearMovementObjectEditor : Editor
{
    public Texture2D editButtonIcon;

    private SerializedProperty offset;
    private List<CLinearMovementObject> ts = new List<CLinearMovementObject>();
    private List<SerializedObject> nodesSerialized = new List<SerializedObject>();
    private List<SerializedObject> nodeTransforms = new List<SerializedObject>();
    private List<SerializedProperty> nodeLocalPositions = new List<SerializedProperty>();
    private List<SerializedProperty> nodeWeights = new List<SerializedProperty>();
    private float previewCyclePos;
    private List<Transform> movementObjects = new List<Transform>();
    private int nodeSelectedForEditing = -1;
    private bool uniformNodeCount = true;
    private Vector3 currentPositionOfNodeHandle;

    void OnEnable()
    {
        foreach (var target in targets)
        {
            var obj = target as CLinearMovementObject;
            ts.Add(obj);
            movementObjects.Add(obj.GetComponentInChildren<Rigidbody>().transform);
        }
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
        // nodes.Clear();
        nodesSerialized.Clear();
        nodeTransforms.Clear();
        nodeLocalPositions.Clear();
        nodeWeights.Clear();
        var minNodes = ts[0].nodes.Count;
        foreach (var obj in ts)
        {
            obj.UpdateNodes();

            if (obj.nodes.Count != minNodes)
            {
                uniformNodeCount = false;
            }
            minNodes = obj.nodes.Count < minNodes ? obj.nodes.Count : minNodes;
        }
        for (var i = 0; i < minNodes; ++i)
        {
            var nodesSpan = new CLinearMovementNode[targets.Length];
            for (var j = 0; j < nodesSpan.Length; ++j)
            {
                nodesSpan[j] = ts[j].nodes[i] as CLinearMovementNode;
            }
            var so = new SerializedObject(nodesSpan);
            var transform = new SerializedObject(nodesSpan.Select(node => node.transform).ToArray());
            nodesSerialized.Add(so);
            nodeTransforms.Add(transform);
            nodeWeights.Add(so.FindProperty("m_weight"));
            nodeLocalPositions.Add(transform.FindProperty("m_LocalPosition"));
        }
    }

    public override void OnInspectorGUI()
    {
        var t = target as CLinearMovementObject;
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
            if (uniformNodeCount && GUILayout.Button("+"))
            {
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("Add node");
                var undoGroupIndex = Undo.GetCurrentGroup();
                foreach (var obj in ts)
                {
                    var newNode = new GameObject("Node", typeof(CLinearMovementNode));
                    Undo.RegisterCreatedObjectUndo(newNode, "node");
                    newNode.transform.SetParent(obj.transform, false);
                }
                Undo.CollapseUndoOperations(undoGroupIndex);
                FindNodes();
            }
            if (uniformNodeCount && GUILayout.Button("-"))
            {
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("Delete node");
                var undoGroupIndex = Undo.GetCurrentGroup();
                if (nodesSerialized.Count > 1)
                {
                    foreach (var obj in ts)
                    {
                        Undo.DestroyObjectImmediate(obj.nodes[obj.nodes.Count - 1].gameObject);
                    }
                }
                Undo.CollapseUndoOperations(undoGroupIndex);
                FindNodes();
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < nodesSerialized.Count; i++)
            {
                nodesSerialized[i].Update();
                nodeTransforms[i].Update();
                EditorGUILayout.BeginHorizontal();
                var editing = (nodeSelectedForEditing == i);
                if (GUILayout.Toggle(editing, editButtonIcon, EditorStyles.miniButton, GUILayout.MaxWidth(30)))
                {
                    nodeSelectedForEditing = i;
                    currentPositionOfNodeHandle = t.nodes[i].transform.position;
                }
                else if (editing)
                {
                    nodeSelectedForEditing = -1;
                }
                EditorGUIUtility.labelWidth = 30;
                EditorGUILayout.PropertyField(nodeLocalPositions[i], new GUIContent("Pos"));
                EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.PropertyField(nodeWeights[i], GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                if (nodesSerialized[i].hasModifiedProperties)
                {
                    foreach (var obj in ts)
                    {
                        obj.UpdateNodes();
                    }
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
        var t = target as CLinearMovementObject;
        if (t.showNodesInInspector && nodeSelectedForEditing != -1)
        {
            Tools.hidden = true;
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Move node");
            var undoGroupIndex = Undo.GetCurrentGroup();
            EditorGUI.BeginChangeCheck();
            currentPositionOfNodeHandle = Handles.PositionHandle(currentPositionOfNodeHandle, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var obj in ts)
                {
                    var node = obj.nodes[nodeSelectedForEditing];
                    Undo.RecordObject(node.transform, "Move node");
                    node.transform.position = currentPositionOfNodeHandle;
                }
            }
            Undo.CollapseUndoOperations(undoGroupIndex);
        }
        else
        {
            Tools.hidden = false;
        }
        if (!Application.isPlaying)
        {
            for (var i = 0; i < ts.Count; ++i)
            {
                movementObjects[i].position = ts[i].CalculatePosition(previewCyclePos);
            }
        }
    }
}
