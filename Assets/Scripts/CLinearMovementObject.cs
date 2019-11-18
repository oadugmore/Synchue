using System.Collections.Generic;
using UnityEngine;

public class CLinearMovementObject : CCyclePathingObject
{
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
        var previous = (CLinearMovementNode)next.Previous();
        float nextCyclePos = next.TargetCyclePosition();
        float previousCyclePos = previous.TargetCyclePosition();

        if (cyclePos > nextCyclePos)
        {
            nextCyclePos += 1f;
        }

        float fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
        Vector3 newPosition = Vector3.Lerp(previous.Position(), next.Position(), fraction);
        movementObject.MovePosition(newPosition);
    }

    protected override void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        foreach (CLinearMovementNode node in nodes)
        {
            var previous = node.Previous() as CLinearMovementNode;
            var distance = Vector3.Distance(node.Position(), previous.Position());
            totalDistance += distance;
            //Debug.Log("Distance: " + distance);
            distances.Add(totalDistance);
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            (nodes[i] as CLinearMovementNode).SetTargetCyclePosition((distances[i] - distances[0]) / totalDistance);
        }
    }
}
