using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSinusoidalMovementObject : CCyclePathingObject
{
    //public bool rotateClockwise = false;
    // [SerializeField][Range(0f, 1f)]
    // private float cycleOffset = 0f;

    Rigidbody movementObject;
    //private float radius;
    private const float pi2 = 2 * Mathf.PI;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();
        //radius = Vector3.Distance(transform.position, movementObject.position);
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        // float localCyclePos = cyclePos + cycleOffset;
        // localCyclePos += cycleOffset;
        // if (localCyclePos >= 1f) localCyclePos -= 1f;
        int nextIndex = NextNode(cyclePos);
        int previousIndex = PreviousNode(nextIndex);
        CSinusoidalMovementNode previous = (CSinusoidalMovementNode)nodes[previousIndex];
        CSinusoidalMovementNode next = (CSinusoidalMovementNode)nodes[nextIndex];

        float nextCyclePos = next.TargetCyclePosition();
        float previousCyclePos = previous.TargetCyclePosition();
        float fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
        float newAngle = Mathf.Lerp(previous.Angle(), next.Angle(), fraction);
        float newRadius = Mathf.Lerp(previous.Radius(), next.Radius(), fraction);

        //float input = pi2 * localCyclePos;
        float angleRadians = Mathf.PI * newAngle / 180f;
        if (next.RotateClockwise()) angleRadians *= -1f;
        float h = Mathf.Cos(angleRadians) * newRadius;
        float v = Mathf.Sin(angleRadians) * newRadius;
        Vector3 destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

}