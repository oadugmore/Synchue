using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CentralizedMovementObject : MonoBehaviour 
{
	List<CentralizedMovementNode> nodes;
	Rigidbody movementObject;

	// Use this for initialization
	void Start () 
	{
		movementObject = GetComponentInChildren<Rigidbody>();
		nodes = new List<CentralizedMovementNode>();
		GetComponentsInChildren<CentralizedMovementNode>(nodes);

		if (nodes.Count < 2)
			Debug.LogError(this + " has less than 2 nodes.");
		else if (nodes[0].TargetCyclePosition() != 0f)
			Debug.LogError(this + " is the first node and must have a targetCyclePosition of 0.");
	}

	public void UpdatePosition(float cyclePos)
	{
		int nextIndex = NextNode(cyclePos);
		int previousIndex = 0;
		if (nextIndex == 0)
			previousIndex = nodes.Count - 1;
		else
			previousIndex = nextIndex - 1;

		CentralizedMovementNode next = nodes[nextIndex];
		CentralizedMovementNode previous = nodes[previousIndex];
		
		float nextCyclePos = next.TargetCyclePosition();
		if (nextCyclePos == 0f) nextCyclePos = 1f;

		float fraction = (cyclePos - previous.TargetCyclePosition()) / Mathf.Abs(previous.TargetCyclePosition() - nextCyclePos);

		Vector3 newPosition = Vector3.Lerp(previous.Position(), next.Position(), fraction);
		movementObject.MovePosition(newPosition);

		Debug.Log(fraction);
	}

	int NextNode(float cyclePos)
	{
		//bool found = false;
		//int previous = 0;
		int nextNode = 0;
		// while (index < nodes.Count && !found)
		// {
		// 	if (nodes[index].TargetCyclePosition() > cyclePos)
		// }

		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i].TargetCyclePosition() > cyclePos)
			{
				nextNode = i;
				break;
			}
		}

		return nextNode;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		
	}
}
