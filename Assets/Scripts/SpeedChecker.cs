using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChecker : MonoBehaviour {

	public List<Rigidbody> trackingRbs;

	// Use this for initialization
	void Start () 
	{
		if (trackingRbs == null) Debug.Log(this + " is not tracking any rigidbodies!");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private void FixedUpdate() 
	{
		Vector3 v1 = trackingRbs[0].velocity;
		for (int i = 1; i < trackingRbs.Count; i++)
		{
			if (trackingRbs[i].velocity.magnitude != v1.magnitude)
			{
				Debug.Log("Speeds out of sync.");
				for (int j = 0; j < trackingRbs.Count; j++)
				{
					Debug.Log(trackingRbs[j].velocity);
				}
				break;
			}
		}
	}
}
