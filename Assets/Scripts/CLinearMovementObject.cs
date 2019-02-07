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
		int previousIndex = PreviousNode(nextIndex);

		CLinearMovementNode next = (CLinearMovementNode)nodes[nextIndex];
		CLinearMovementNode previous = (CLinearMovementNode)nodes[previousIndex];
		
		float nextCyclePos = next.TargetCyclePosition();
		float previousCyclePos = previous.TargetCyclePosition();
		//if (nextCyclePos == 0f) nextCyclePos = 1f;
		if (nextIndex == 0)
		{
			if (cyclePos < nextCyclePos) // only happens when the first node has a nonzero Target Cycle Position
				cyclePos += 1f;
			nextCyclePos += 1f;
		}

		float fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
		Vector3 newPosition = Vector3.Lerp(previous.Position(), next.Position(), fraction);
		movementObject.MovePosition(newPosition);
	}

}
