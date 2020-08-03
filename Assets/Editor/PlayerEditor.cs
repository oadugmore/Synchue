using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player), true)]
public class PlayerEditor : Editor
{
    // SerializedProperty testPosition;
    private bool editingTestPosition = false;
    private GUIStyle toggleButtonStyle;
    private SerializedProperty testPosition;
    

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (toggleButtonStyle == null)
        {
            toggleButtonStyle = "IN EditColliderButton";
        }
        EditorGUILayout.PropertyField(testPosition);
        editingTestPosition = GUILayout.Toggle(editingTestPosition, "Edit Test Position", toggleButtonStyle);
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        // testPosition = serializedObject.FindProperty("testPosition");
        testPosition = serializedObject.FindProperty("testPosition");
    }

    private void OnDisable()
    {
    }

    private void OnSceneGUI()
    {
        var t = target as Player;
        if (editingTestPosition && !Application.isPlaying)
        {
            Tools.hidden = true;
            var newPos = Handles.PositionHandle(t.GetTestPosition(), Quaternion.identity);
            if (Vector3.Distance(t.transform.position, newPos) < 1)
            {
                newPos += Vector3.right;
            }
            t.SetTestPosition(newPos);
        }
        else
        {
            Tools.hidden = false;
        }
    }
}