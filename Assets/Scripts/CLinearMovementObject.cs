using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CLinearMovementObject : CCyclePathingObject
{
	Rigidbody movementObject;

	protected override void Start()
	{
		base.Start();
		movementObject = GetComponentInChildren<Rigidbody>();

		if (nodes.Count < 2)
			Debug.LogError(this + " has less than 2 nodes.");
	}

	public override void UpdateCyclePosition(float cyclePos)
	{
		int nextIndex = NextNode(cyclePos);
		int previousIndex = 0;
		if (nextIndex == 0)
			previousIndex = nodes.Count - 1;
		else
			previousIndex = nextIndex - 1;

		CLinearMovementNode next = (CLinearMovementNode)nodes[nextIndex];
		CLinearMovementNode previous = (CLinearMovementNode)nodes[previousIndex];
		
		float nextCyclePos = next.TargetCyclePosition();
		//if (nextCyclePos == 0f) nextCyclePos = 1f;
		if (nextIndex == 0)
		{
			if (cyclePos < nextCyclePos)
				cyclePos += 1f;
			nextCyclePos += 1f;
		}

		float fraction = Mathf.Abs(cyclePos - previous.TargetCyclePosition()) / (nextCyclePos - previous.TargetCyclePosition());
		Vector3 newPosition = Vector3.Lerp(previous.Position(), next.Position(), fraction);
		movementObject.MovePosition(newPosition);
	}

}
