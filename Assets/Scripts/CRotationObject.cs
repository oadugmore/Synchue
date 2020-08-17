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
        cyclePos = Mathf.Repeat(cyclePos + offset, 1);
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

        Debug.Assert(cyclePos <= nextCyclePos);
        Debug.Assert(cyclePos >= previousCyclePos);

        var fraction = (cyclePos - previousCyclePos) / Mathf.Abs(previousCyclePos - nextCyclePos);
        var newRotation = Quaternion.Lerp(previous.transform.rotation, next.transform.rotation, fraction);
        rotationObject.MoveRotation(newRotation);
    }
}
