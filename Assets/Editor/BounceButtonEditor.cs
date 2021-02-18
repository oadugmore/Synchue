using UnityEditor;

[CustomEditor(typeof(BounceButton), true)]
public class BounceButtonEditor : Editor
{
    string[] hideProperties = {
        "m_SpriteState",
        "m_AnimationTriggers",
        "m_Script"
    };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, hideProperties);
        serializedObject.ApplyModifiedProperties();
    }
}
