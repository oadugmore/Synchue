using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPusher : MonoBehaviour {

	List<Pushable> blueObjects;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Starting ObjectPusher");
		blueObjects = new List<Pushable>();
		var ss = FindObjectsOfType<MonoBehaviour>().OfType<Pushable>();
		foreach (Pushable s in ss)
		{
			blueObjects.Add(s);
			Debug.Log("Added " + s);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		if (Controller.GetBlueButtonDown())
		{
			foreach (Pushable p in blueObjects)
			{
				p.Move();
			}
		}
		else
		{
			foreach (Pushable p in blueObjects)
			{
				p.Stop();
			}
		}
	}
}
