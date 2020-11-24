using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CEllipticalMovementObject)), CanEditMultipleObjects]
public class CEllipticalMovementObjectEditor : Editor
{
    SerializedProperty horizontalAxis;
    SerializedProperty verticalAxis;
    SerializedProperty offsetAngle;
    SerializedProperty rotateClockwise;

    float previewCyclePos;
    Vector3[] ellipsePoints;
    bool ellipseDirty = false;
    Vector3 previousPosition;

    void OnEnable()
    {
        CalculateEllipsePoints();
        previousPosition = (target as CEllipticalMovementObject).transform.position;
        Undo.undoRedoPerformed += CalculateEllipsePoints;
        horizontalAxis = serializedObject.FindProperty("horizontalAxis");
        verticalAxis = serializedObject.FindProperty("verticalAxis");
        offsetAngle = serializedObject.FindProperty("offsetAngleDegrees");
        rotateClockwise = serializedObject.FindProperty("rotateClockwise");
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= CalculateEllipsePoints;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var t = (target as CEllipticalMovementObject);
        EditorGUILayout.PropertyField(horizontalAxis);
        EditorGUILayout.PropertyField(verticalAxis);
        EditorGUILayout.PropertyField(offsetAngle, new GUIContent("Offset Angle", "Enter an angle in degrees. It will be converted to radians internally."));
        //offsetAngle.floatValue = EditorGUILayout.FloatField(new GUIContent("Offset Angle", "Enter an angle in degrees. It will be converted to radians internally."),
        //    offsetAngle.floatValue);
        EditorGUILayout.PropertyField(rotateClockwise);

        previewCyclePos = EditorGUILayout.Slider("Preview Cycle Pos", previewCyclePos, 0f, 1f);
        if (serializedObject.hasModifiedProperties || previousPosition != t.transform.position)
        {
            ellipseDirty = true;
            previousPosition = t.transform.position;
        }


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
        //Handles.DrawSolidDisc(t.transform.position + Vector3.right * t.horizontalAxis, Vector3.back, 0.25f);
        //Handles.DrawSolidDisc(t.transform.position + Vector3.left * t.horizontalAxis, Vector3.back, 0.25f);
        //Handles.DrawSolidDisc(t.transform.position + Vector3.up * t.verticalAxis, Vector3.back, 0.25f);
        //Handles.DrawSolidDisc(t.transform.position + Vector3.down * t.verticalAxis, Vector3.back, 0.25f);
        if (ellipseDirty)
        {
            CalculateEllipsePoints();
        }
        Handles.DrawPolyLine(ellipsePoints);
        if (!Application.isPlaying)
        {
            t.UpdateCyclePosition(previewCyclePos);
            EditorHelper.ManualPhysicsStepGlobal();
        }
        // if (EditorGUI.EndChangeCheck())
        // {
        //     Undo.RecordObject(target, "Move point");
        //     Debug.Log("Moved point.");
        //     t.lookAtPoint = pos;
        //     t.Update();
        // }

    }

    void CalculateEllipsePoints()
    {
        ellipsePoints = new Vector3[21];
        var t = (target as CEllipticalMovementObject);

        var i = 0;
        for (float pos = 0f; pos <= 1.0f; pos += 0.05f)
        {
            ellipsePoints[i] = t.CalculatePosition(pos);
            ++i;
        }
        ellipsePoints[i] = ellipsePoints[0];
        ellipseDirty = false;
    }
}