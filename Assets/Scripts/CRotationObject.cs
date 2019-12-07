using System.Collections.Generic;
using UnityEngine;

public class CRotationObject : CCyclePathingObject
{
    private Rigidbody rotationObject;

    protected override void Start()
    {
        base.Start();
        rotationObject = GetComponentInChildren<Rigidbody>();

        if (nodes.Count < 2)
        {
            Debug.LogError(this + " has less than 2 nodes.");
        }
        else if (nodes[0].TargetCyclePosition() != 0f)
        {
            Debug.LogError(this + " is the first node and must have a targetCyclePosition of 0.");
        }
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        var next = (CRotationNode)NextNode(cyclePos);
        var previous = (CRotationNode)next.Previous();
        var nextCyclePos = next.TargetCyclePosition();
        var previousCyclePos = previous.TargetCyclePosition();

        if (cyclePos > nextCyclePos)
        {
            nextCyclePos += 1f;
        }

        if (cyclePos < previousCyclePos)
        {
            previousCyclePos -= 1f;
        }

        // TODO: Cache nextEuler and previousEuler and only update
        // when nextIndex = NextNode() is different from last UpdateCyclePosition()

        // do calculations in euler angles because I spent a few hours watching videos on quaternions
        // and decided it would be easier to use euler angles
        var previousEuler = previous.Rotation().eulerAngles;
        var nextEuler = next.Rotation().eulerAngles;
        OffsetNextAngle(previousEuler, ref nextEuler, next.RotateClockwise());
        var fraction = (cyclePos - previousCyclePos) / Mathf.Abs(previousCyclePos - nextCyclePos);
        var newVector = Vector3.Slerp(previousEuler, nextEuler, fraction);
        var newRotation = Quaternion.Euler(newVector);
        rotationObject.MoveRotation(newRotation);
    }

    private void OffsetNextAngle(Vector3 previousEuler, ref Vector3 nextEuler, bool rotateClockwise)
    {
        if (!rotateClockwise)
        {
            if (nextEuler.x < previousEuler.x)
            {
                nextEuler.x += 360;
            }

            if (nextEuler.y < previousEuler.y)
            {
                nextEuler.y += 360;
            }

            if (nextEuler.z < previousEuler.z)
            {
                nextEuler.z += 360;
            }
        }
        else
        {
            if (nextEuler.x > previousEuler.x)
            {
                nextEuler.x -= 360;
            }

            if (nextEuler.y > previousEuler.y)
            {
                nextEuler.y -= 360;
            }

            if (nextEuler.z > previousEuler.z)
            {
                nextEuler.z -= 360;
            }
        }
    }

    protected override void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        foreach (CRotationNode node in nodes)
        {
            var previous = node.Previous() as CRotationNode;
            var previousEuler = previous.Rotation().eulerAngles;
            var nextEuler = node.Rotation().eulerAngles;
            OffsetNextAngle(previousEuler, ref nextEuler, node.RotateClockwise());
            var distance = Vector3.Distance(previousEuler, nextEuler);
            totalDistance += distance;
            distances.Add(totalDistance);
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            (nodes[i] as CRotationNode).SetTargetCyclePosition((distances[i] - distances[0]) / totalDistance);
        }
    }
}
