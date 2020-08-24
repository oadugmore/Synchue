using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player), true)]
public class PlayerEditor : Editor
{
    private Player t;
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
        DrawPropertiesExcluding(serializedObject, testPosition.name, "m_Script");
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        t = target as Player;
        testPosition = serializedObject.FindProperty("testPosition");
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }

    private void OnSceneGUI()
    {
        if (editingTestPosition && !Application.isPlaying)
        {
            Tools.hidden = true;
            var newPos = Handles.PositionHandle(t.testPosition, Quaternion.identity);
            if (newPos != t.testPosition)
            {
                Undo.RecordObject(target, "Update Player test position");
                t.testPosition = newPos;
            }
        }
        else
        {
            Tools.hidden = false;
        }

    }
}