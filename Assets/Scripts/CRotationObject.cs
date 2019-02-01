using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRotationObject : CCyclePathingObject
{
    //List<CRotationNode> nodes;
	Rigidbody rotationObject;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
		rotationObject = GetComponentInChildren<Rigidbody>();

		if (nodes.Count < 2)
			Debug.LogError(this + " has less than 2 nodes.");
        else if (nodes[0].TargetCyclePosition() != 0f)
            Debug.LogError(this + " is the first node and must have a targetCyclePosition of 0.");
	}

    public override void UpdateCyclePosition(float cyclePos)
    {
        int nextIndex = NextNode(cyclePos);
		int previousIndex = 0;
		if (nextIndex == 0)
			previousIndex = nodes.Count - 1;
		else
			previousIndex = nextIndex - 1;

        CRotationNode next = (CRotationNode)nodes[nextIndex];
		CRotationNode previous = (CRotationNode)nodes[previousIndex];
		
		float nextCyclePos = next.TargetCyclePosition();
		if (nextCyclePos == 0f) nextCyclePos = 1f;

        // TODO: Cache nextEuler and previousEuler and only update
        // when nextIndex = NextNode() is different from last UpdateCyclePosition()

        // do calculations in euler angles because I spent a few hours watching videos on quaternions
        // and decided it would be easier to use euler angles
        Vector3 previousEuler = previous.Rotation().eulerAngles;
        Vector3 nextEuler = next.Rotation().eulerAngles;
        if (!next.RotateBackwards())
        {
            if (nextEuler.x < previousEuler.x)
                nextEuler.x += 360;
            if (nextEuler.y < previousEuler.y)
                nextEuler.y += 360;
            if (nextEuler.z < previousEuler.z)
                nextEuler.z += 360;
        }
        else
        {
            if (nextEuler.x > previousEuler.x)
                nextEuler.x -= 360;
            if (nextEuler.y > previousEuler.y)
                nextEuler.y -= 360;
            if (nextEuler.z > previousEuler.z)
                nextEuler.z -= 360;
        }

		float fraction = (cyclePos - previous.TargetCyclePosition()) / Mathf.Abs(previous.TargetCyclePosition() - nextCyclePos);
		Quaternion newRotation = Quaternion.Euler(Vector3.Lerp(previousEuler, nextEuler, fraction));
		rotationObject.MoveRotation(newRotation);
    }

}
