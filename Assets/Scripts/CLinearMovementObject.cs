using System.Collections.Generic;
using UnityEngine;

[SelectionBase, ExecuteInEditMode]
public class CLinearMovementObject : CCyclePathingObject
{
    public bool showNodesInInspector;

    private Rigidbody movementObject;

    protected override void Start()
    {
        automaticCycleTime = false; // reminder to remove automaticCycleTime
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

    [System.Obsolete]
    protected override void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        foreach (var node in nodes)
        {
            var previous = node.previous;
            var distance = Vector3.Distance(node.transform.position, previous.transform.position);
            totalDistance += distance;
            distances.Add(totalDistance);
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            nodes[i].targetCyclePosition = (distances[i] - distances[0]) / totalDistance;
        }
    }
}
