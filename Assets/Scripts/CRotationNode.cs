using UnityEngine;

public class CRotationNode : CCycleNode
{
    // [SerializeField][Range(0f, 1f)]
    // float targetCyclePosition;
    [SerializeField]
    private bool rotateBackwards; // set to true to rotate by subtracting rotation


    public Quaternion Rotation()
    {
        return transform.rotation;
    }

    public bool RotateBackwards()
    {
        return rotateBackwards;
    }

}
