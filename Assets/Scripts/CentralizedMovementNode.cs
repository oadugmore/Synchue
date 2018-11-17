using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedMovementNode : MonoBehaviour 
{
	[SerializeField][Range(0f, 1f)]
	float targetCyclePosition;
	
	// Use this for initialization
	void Start ()
	{
		// if (targetCyclePosition > 1 || targetCyclePosition < 0)
		// 	Debug.LogError(this + " targetCyclePosition must be between 0 and 1.");
	}

	public Vector3 Position()
	{
		return this.transform.position;
	}

	public float TargetCyclePosition()
	{
		return targetCyclePosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
