using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player), true)]
public class PlayerEditor : Editor
{
    private Player t;
    private bool editingTestPosition = false;
    private SerializedProperty testPosition;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(testPosition);
        EditorGUILayout.BeginHorizontal();
        editingTestPosition = GUILayout.Toggle(editingTestPosition, "Edit Test Position", EditorStyles.miniButton);
        if (GUILayout.Button("Reset"))
        {
            editingTestPosition = false;
            Undo.RecordObject(target, "Reset Player test position");
            testPosition.vector3Value = t.transform.position;
        }
        EditorGUILayout.EndHorizontal();
        DrawPropertiesExcluding(serializedObject, testPosition.name, "m_Script");
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        t = target as Player;
        testPosition = serializedObject.FindProperty(nameof(testPosition));
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
