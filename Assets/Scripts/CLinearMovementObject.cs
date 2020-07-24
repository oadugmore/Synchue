using System.Collections.Generic;
using UnityEngine;

[SelectionBase, ExecuteInEditMode]
public class CLinearMovementObject : CCyclePathingObject
{
    public bool showNodesInInspector;

    private Rigidbody movementObject;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();

        if (!movementObject)
        {
            Debug.LogError("No Rigidbody found in children.");
        }

        if (nodes.Count < 2)
        {
            Debug.LogError(this + " has less than 2 nodes.");
        }
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        cyclePos = Mathf.Repeat(cyclePos + offset, 1);
        var next = NextNode(cyclePos);
        var previous = next.previous;
        var nextCyclePos = next.targetCyclePosition;
        var previousCyclePos = previous.targetCyclePosition;

        if (cyclePos > nextCyclePos)
        {
            nextCyclePos += 1f;
        }

        if (cyclePos < previousCyclePos)
        {
            previousCyclePos -= 1f;
        }

        var fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
        var newPosition = Vector3.Lerp(previous.transform.position, next.transform.position, fraction);
        movementObject.MovePosition(newPosition);
    }
}
