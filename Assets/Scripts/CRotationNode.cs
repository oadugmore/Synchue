using UnityEngine;

public class CRotationNode : CCycleNode
{
    // [SerializeField][Range(0f, 1f)]
    // float targetCyclePosition;
    [SerializeField]
    private bool rotateClockwise;

    public Quaternion Rotation()
    {
        return transform.rotation;
    }

    public bool RotateClockwise()
    {
        return rotateClockwise;
    }

}
