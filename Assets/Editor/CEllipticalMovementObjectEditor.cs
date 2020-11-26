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
    CEllipticalMovementObject t;
    Transform movementObject;

    void OnEnable()
    {
        CalculateEllipsePoints();
        t = target as CEllipticalMovementObject;
        movementObject = t.GetComponentInChildren<Rigidbody>().transform;
        previousPosition = t.transform.position;
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
        EditorGUILayout.PropertyField(horizontalAxis);
        EditorGUILayout.PropertyField(verticalAxis);
        EditorGUILayout.PropertyField(offsetAngle, new GUIContent("Offset Angle", "Enter an angle in degrees. It will be converted to radians internally."));
        EditorGUILayout.PropertyField(rotateClockwise);

        previewCyclePos = EditorGUILayout.Slider("Preview Cycle Pos", previewCyclePos, 0f, 1f);
        if (serializedObject.hasModifiedProperties || previousPosition != t.transform.position)
        {
            ellipseDirty = true;
            previousPosition = t.transform.position;
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        if (ellipseDirty)
        {
            CalculateEllipsePoints();
        }
        Handles.DrawPolyLine(ellipsePoints);
        if (!Application.isPlaying)
        {
            movementObject.position = t.CalculatePosition(previewCyclePos);
        }
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
