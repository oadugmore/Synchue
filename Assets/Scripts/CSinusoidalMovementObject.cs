﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSinusoidalMovementObject : CCyclePathingObject
{
    //public bool rotateClockwise = false;
    // [SerializeField][Range(0f, 1f)]
    // private float cycleOffset = 0f;

    Rigidbody movementObject;
    //private float radius;

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
        var previous = (CSinusoidalMovementNode)nodes[previousIndex];
        var next = (CSinusoidalMovementNode)nodes[nextIndex];

        float nextCyclePos = next.TargetCyclePosition();
        float previousCyclePos = previous.TargetCyclePosition();
        float nextAngle = next.Angle();
        float previousAngle = previous.Angle();
        if (nextAngle < 0f && previousAngle > 0f && !next.RotateClockwise()) nextAngle += 360f;
        if (nextAngle > 0f && previousAngle < 0f && next.RotateClockwise()) previousAngle += 360f;
        while (nextCyclePos < previousCyclePos) nextCyclePos++;

        float fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
        float newAngle = Mathf.Lerp(previousAngle, nextAngle, fraction);
        float newRadius = Mathf.Lerp(previous.Radius(), next.Radius(), fraction);

        float angleRadians = Mathf.Deg2Rad * newAngle;
        float h = Mathf.Cos(angleRadians) * newRadius;
        float v = Mathf.Sin(angleRadians) * newRadius;
        Vector3 destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

}