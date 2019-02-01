using UnityEngine;

public class CSinusoidalMovementNode : CCycleNode
{
    [SerializeField]
    float radius = 1f;
    [SerializeField]
    bool rotateClockwise = false;

    public Quaternion Angle()
    {
        return this.transform.rotation;
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
