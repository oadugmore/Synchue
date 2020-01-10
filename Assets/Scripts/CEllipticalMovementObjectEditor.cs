using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CEllipticalMovementObject)), CanEditMultipleObjects]
public class CEllipticalMovementObjectEditor : Editor
{
    SerializedProperty horizontalAxis;
    SerializedProperty verticalAxis;
    SerializedProperty offsetAngle;

    void OnEnable()
    {
        horizontalAxis = serializedObject.FindProperty("horizontalAxis");
        verticalAxis = serializedObject.FindProperty("verticalAxis");
        offsetAngle = serializedObject.FindProperty("offsetAngleDegrees");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(horizontalAxis);
        EditorGUILayout.PropertyField(verticalAxis);
        //EditorGUILayout.PropertyField(offsetAngle, new GUIContent("Offset Angle", "Enter an angle in degrees. It will be converted to radians internally."));
        offsetAngle.floatValue = EditorGUILayout.FloatField(new GUIContent("Offset Angle", "Enter an angle in degrees. It will be converted to radians internally."),
            offsetAngle.floatValue);

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