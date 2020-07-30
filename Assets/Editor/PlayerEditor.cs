using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player), true)]
public class PlayerEditor : Editor
{
    // SerializedProperty testPosition;
    private bool editingTestPosition = false;
    private GUIStyle toggleButtonStyle;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (toggleButtonStyle == null)
        {
            toggleButtonStyle = "IN EditColliderButton";
        }
        editingTestPosition = GUILayout.Toggle(editingTestPosition, "Edit Test Position", toggleButtonStyle);
    }

    private void OnEnable()
    {
        // testPosition = serializedObject.FindProperty("testPosition");
    }

    private void OnDisable()
    {
    }

    private void OnSceneGUI()
    {
        var t = target as Player;
        if (editingTestPosition)
        {
            Tools.hidden = true;
            var newPos = Handles.PositionHandle(t.testPosition, Quaternion.identity);
            if (newPos.magnitude < 1)
            {
                newPos = Vector3.right;
            }
            t.testPosition = newPos;
        }
        else
        {
            Tools.hidden = false;
        }
    }
}