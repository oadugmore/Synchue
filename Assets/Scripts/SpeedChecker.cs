using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChecker : MonoBehaviour {

	public List<Rigidbody> trackingRbs;

	List<ColorBackForthMover> movers = new List<ColorBackForthMover>();

	// Use this for initialization
	void Start () 
	{
		if (trackingRbs == null) Debug.Log(this + " is not tracking any rigidbodies!");
		for (int i = 0; i < trackingRbs.Count; i++)
		{
			movers.Add(trackingRbs[i].GetComponent<ColorBackForthMover>());
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private void FixedUpdate() 
	{
		bool switchErr = false, speederr = false;
		bool switched = movers[0].switched;
		float scale = movers[0].scale;

		for (int i = 1; i < movers.Count; i++)
		{
			if (movers[i].scale != scale)
			{
				//switchErr = true;
				Debug.Log("Different scales!");
				for (int j = 0; j < movers.Count; j++)
				{
					Debug.Log(movers[j].scale);
				}
				break;
			}
		}

		for (int i = 1; i < movers.Count; i++)
		{
			if (movers[i].switched != switched)
			{
				switchErr = true;
				//Debug.Log("Not all switched.");
				for (int j = 0; j < movers.Count; j++)
				{
					//Debug.Log(movers[j].switched);
				}
				break;
			}
		}

		Vector3 v1 = trackingRbs[0].velocity;
		for (int i = 1; i < trackingRbs.Count; i++)
		{
			if (trackingRbs[i].velocity.magnitude != v1.magnitude)
			{
				speederr = true;
				//Debug.Log("Speeds out of sync.");
				for (int j = 0; j < trackingRbs.Count; j++)
				{
					//Debug.Log(trackingRbs[j].velocity);
				}
				break;
			}
		}

		if (switchErr && speederr)
		{
			Debug.Log("Failed to switch and different speed.");
		}
		//else if (switchErr && !speederr)
		//Debug.Log("Failed to switch.");

		else if (speederr && !switchErr)
		Debug.Log("Speed out of sync.");
	}
}