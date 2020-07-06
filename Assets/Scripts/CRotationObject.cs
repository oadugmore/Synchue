using System.Collections.Generic;
using UnityEngine;

[SelectionBase, ExecuteInEditMode]
public class CRotationObject : CCyclePathingObject
{
    public bool showNodesInInspector;
    public int nodeSelectedForEditing = -1;

    private Rigidbody rotationObject;

    protected override void Start()
    {
        base.Start();
        rotationObject = GetComponentInChildren<Rigidbody>();

        if (!rotationObject)
        {
            Debug.LogError("No Rigidbody found in children.");
        }

        if (nodes.Count < 2)
        {
            Debug.LogError(this + " has less than 2 nodes.");
        }
        else if (nodes[0].targetCyclePosition != 0f)
        {
            Debug.LogError(this + " is the first node and must have a targetCyclePosition of 0.");
        }
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        var next = (CRotationNode)NextNode(cyclePos);
        var previous = (CRotationNode)next.previous;
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

        // TODO: Cache nextEuler and previousEuler and only update
        // when nextIndex = NextNode() is different from last UpdateCyclePosition()

        // do calculations in euler angles because I spent a few hours watching videos on quaternions
        // and decided it would be easier to use euler angles
        var previousEuler = previous.rotation.eulerAngles;
        var nextEuler = next.rotation.eulerAngles;
        OffsetNextAngle(previousEuler, ref nextEuler, next.rotateClockwise);
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

    [System.Obsolete("Use node weights instead.", true)]
    protected override void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        foreach (CRotationNode node in nodes)
        {
            var previous = node.previous as CRotationNode;
            var previousEuler = previous.rotation.eulerAngles;
            var nextEuler = node.rotation.eulerAngles;
            OffsetNextAngle(previousEuler, ref nextEuler, node.rotateClockwise);
            var distance = Vector3.Distance(previousEuler, nextEuler);
            totalDistance += distance;
            distances.Add(totalDistance);
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            (nodes[i] as CRotationNode).targetCyclePosition = (distances[i] - distances[0]) / totalDistance;
        }
    }
}
