using UnityEngine;

public class CSinusoidalMovementNode : CCycleNode
{
    [SerializeField]
    float radius = 1f;
    [SerializeField][Range(0, 360)]
    float angle = 0f;
    [SerializeField]
    bool rotateClockwise = false;

    public float Angle()
    {
        return angle;
    }

    public float Radius()
    {
        return radius;
    }

    public bool RotateClockwise()
    {
        return rotateClockwise;
    }

}
