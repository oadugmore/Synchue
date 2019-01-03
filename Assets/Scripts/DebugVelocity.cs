using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVelocity : MonoBehaviour {

	public bool printInfo = false;
	Rigidbody r;
	public Vector3 worldPos;
	public Vector3 velocity;
	public Vector3 platformVelocity;

	// Use this for initialization
	void Start () {
		r = GetComponent<Rigidbody>();
		velocity = Vector3.zero;
		worldPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		if (printInfo)
		{
			Debug.Log("Velocity: " + r.velocity);
			Debug.Log("World position: " + r.position);
		}
		MovingPlatform p;
		if (p = GetComponent<MovingPlatform>())
			platformVelocity = p.velocity;
		velocity = r.velocity;
		worldPos = r.position;
	}
}
