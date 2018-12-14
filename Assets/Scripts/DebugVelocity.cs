using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVelocity : MonoBehaviour {

	Rigidbody r;

	// Use this for initialization
	void Start () {
		r = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		Debug.Log(r.velocity);
	}
}
