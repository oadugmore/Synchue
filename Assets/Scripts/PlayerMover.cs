using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

	bool lastMsg = false;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Player starting");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Controller.screenIsPressed() != lastMsg)
		{
			string msg = lastMsg ? "PointerUp" : "PointerDown";
			Debug.Log(msg);
			lastMsg = !lastMsg;
		}
	}
}
