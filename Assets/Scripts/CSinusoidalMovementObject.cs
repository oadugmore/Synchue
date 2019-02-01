using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSinusoidalMovementObject : CCyclePathingObject
{
    public bool rotateClockwise = false;
    [SerializeField][Range(0f, 1f)]
    private float cycleOffset = 0f;

    Rigidbody movementObject;
    private float radius;
    private const float pi2 = 2 * Mathf.PI;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();
        radius = Vector3.Distance(transform.position, movementObject.position);
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        // TODO: Make this work with nodes
        cyclePos += cycleOffset;
        if (cyclePos >= 1f) cyclePos -= 1f;
        float input = pi2 * cyclePos;
        if (rotateClockwise) input *= -1f;
        float h = Mathf.Cos(input) * radius;
        float v = Mathf.Sin(input) * radius;
        Vector3 destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

}