using UnityEngine;

public class CEllipticalMovementNode : CCycleNode
{
    [SerializeField]
    private bool rotateClockwise = false;

    private float radius = 1f;
    private float angle = 0f;

    private void Start()
    {
        var parent = GetComponentsInParent<Transform>()[1];
        radius = Vector3.Distance(parent.position, transform.position);
        angle = Mathf.Deg2Rad * Vector3.SignedAngle(Vector3.right, transform.localPosition, Vector3.forward);
    }

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
