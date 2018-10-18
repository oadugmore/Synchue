using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		Debug.Log("Player starting");
	}
	
	bool lastBlueMsg = false;
	bool lastOrangeMsg = false;
	// Update is called once per frame
	void Update () 
	{
		if (Controller.GetBlueButtonDown() != lastBlueMsg)
		{
			string msg = lastBlueMsg ? "blue up" : "blue down";
			Debug.Log(msg);
			lastBlueMsg = !lastBlueMsg;
		}
		if (Controller.GetOrangeButtonDown() != lastOrangeMsg)
		{
			string msg = lastOrangeMsg ? "orange up" : "orange down";
			Debug.Log(msg);
			lastOrangeMsg = !lastOrangeMsg;
		}
	}
}
