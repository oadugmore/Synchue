//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CEllipticalMovementObject))]
[CanEditMultipleObjects]
public class CEllipticalMovementObjectEditor : Editor
{
    SerializedProperty horizontalAxis;
    SerializedProperty verticalAxis;

    void OnEnable()
    {
        horizontalAxis = serializedObject.FindProperty("horizontalAxis");
        verticalAxis = serializedObject.FindProperty("verticalAxis");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(horizontalAxis);
        EditorGUILayout.PropertyField(verticalAxis);
        // if (lookAtPoint.vector3Value.y > (target as LookAtPoint).transform.position.y)
        // {
        //     EditorGUILayout.LabelField("(Above this object)");
        // }
        // if (lookAtPoint.vector3Value.y < (target as LookAtPoint).transform.position.y)
        // {
        //     EditorGUILayout.LabelField("(Below this object)");
        // }
        
            
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        var t = (target as CEllipticalMovementObject);

        //EditorGUI.BeginChangeCheck();
        //Vector3 pos = Handles.PositionHandle(t.lookAtPoint, Quaternion.identity);
        Handles.DrawSolidDisc(t.transform.position + Vector3.right * t.horizontalAxis, Vector3.back, 0.25f);
        Handles.DrawSolidDisc(t.transform.position + Vector3.left * t.horizontalAxis, Vector3.back, 0.25f);
        Handles.DrawSolidDisc(t.transform.position + Vector3.up * t.verticalAxis, Vector3.back, 0.25f);
        Handles.DrawSolidDisc(t.transform.position + Vector3.down * t.verticalAxis, Vector3.back, 0.25f);
        // if (EditorGUI.EndChangeCheck())
        // {
        //     Undo.RecordObject(target, "Move point");
        //     Debug.Log("Moved point.");
        //     t.lookAtPoint = pos;
        //     t.Update();
        // }
    }
}