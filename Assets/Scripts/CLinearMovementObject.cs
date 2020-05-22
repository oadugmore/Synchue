using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CLinearMovementObject : CCyclePathingObject
{
    public float offset;
    public bool showNodesInInspector;
    private Rigidbody movementObject;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();

        if (nodes.Count < 2)
        {
            Debug.LogError(this + " has less than 2 nodes.");
        }
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        var next = (CLinearMovementNode)NextNode(cyclePos);
        var previous = (CLinearMovementNode)next.previous;
        float nextCyclePos = next.targetCyclePosition;
        float previousCyclePos = previous.targetCyclePosition;

        if (cyclePos > nextCyclePos)
        {
            nextCyclePos += 1f;
        }

        if (cyclePos < previousCyclePos)
        {
            previousCyclePos -= 1f;
        }

        float fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
        Vector3 newPosition = Vector3.Lerp(previous.position, next.position, fraction);
        movementObject.MovePosition(newPosition);
    }

    protected override void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        foreach (CLinearMovementNode node in nodes)
        {
            var previous = node.previous as CLinearMovementNode;
            var distance = Vector3.Distance(node.position, previous.position);
            totalDistance += distance;
            distances.Add(totalDistance);
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            (nodes[i] as CLinearMovementNode).targetCyclePosition = (distances[i] - distances[0]) / totalDistance;
        }
    }
}
