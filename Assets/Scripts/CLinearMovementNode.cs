using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLinearMovementNode : MonoBehaviour 
{
	[SerializeField][Range(0f, 1f)]
	float targetCyclePosition;
	

	public Vector3 Position()
	{
		return this.transform.position;
	}

	public float TargetCyclePosition()
	{
		return targetCyclePosition;
	}
	
}
