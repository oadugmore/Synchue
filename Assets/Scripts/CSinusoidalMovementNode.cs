using UnityEngine;

public class CSinusoidalMovementNode : CCycleNode
{
    [SerializeField]
    bool rotateClockwise = false;
    
    float radius = 1f;
    float angle = 0f;

    private void Start()
    {
        Transform parent = GetComponentsInParent<Transform>()[1];
        //Debug.Log("Parent: " + parent.transform.position);
        //Debug.Log("This: " + this.transform.position);
        radius = Vector3.Distance(parent.position, transform.position);
        angle = Vector3.SignedAngle(Vector3.right, transform.localPosition, Vector3.back);
        //if (angle < 0) angle += 360f;
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
