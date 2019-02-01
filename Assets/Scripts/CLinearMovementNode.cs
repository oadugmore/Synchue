using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLinearMovementNode : CCycleNode 
{	
	
	public Vector3 Position()
	{
		return this.transform.position;
	}
	
}
