using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedCirclingObject : MonoBehaviour, CentralizedTransformationObject
{
    public Axis horizontal = Axis.X;
    public Axis vertical = Axis.Y;
    public bool rotateClockwise = false;
    [SerializeField][Range(0f, 1f)]
    private float cycleOffset = 0f;

    Rigidbody movementObject;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        movementObject = GetComponentInChildren<Rigidbody>();
        radius = Vector3.Distance(transform.position, movementObject.position);
    }

    public void UpdateCyclePosition(float cyclePos)
    {
        cyclePos += cycleOffset;
        if (cyclePos >= 1f) cyclePos -= 1f;
        float input = Mathf.PI * 2 * cyclePos;
        if (rotateClockwise) input *= -1f;
        float h = Mathf.Cos(input) * radius;
        float v = Mathf.Sin(input) * radius;
        Vector3 moveVector = Vector3.zero;
        
        switch (horizontal)
        {
            case Axis.X:
                moveVector.x = h;
                break;
            case Axis.Y:
                moveVector.y = h;
                break;
            case Axis.Z:
                moveVector.z = h;
                break;
            default:
                break;
        }
        switch (vertical)
        {
            case Axis.X:
                moveVector.x = v;
                break;
            case Axis.Y:
                moveVector.y = v;
                break;
            case Axis.Z:
                moveVector.z = v;
                break;
            default:
                break;
        }

        movementObject.MovePosition(transform.position + moveVector);
    }

}

[System.Serializable]
public enum Axis
{
    X, Y, Z
}