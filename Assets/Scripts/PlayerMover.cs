using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour, Pushable
{

	public float maxSpeed = 5f;
	public float speedIncrement = 10f;
	public float minSpeed = 0.5f;
	//public PhysicMaterial movingMaterial;
	//public PhysicMaterial stoppedMaterial;

    private InteractColor playerColor;
    private Rigidbody rigidbody;
	private SphereCollider sphereCollider;
	private BoxCollider boxCollider;
	private float speedZero = 0.01f;
	private bool moving = false;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
		sphereCollider = GetComponentInChildren<SphereCollider>();
		//boxCollider = GetComponent<BoxCollider>();
		playerColor = InteractColor.Blue;
    }

    // bool lastBlueMsg = false;
    // bool lastOrangeMsg = false;

    // Update is called once per frame
    void Update()
    {

    }

	float nextVelUpdate = 0f;
    void FixedUpdate()
    {
		if (Time.fixedTime > nextVelUpdate)
		{
            //Debug.Log("Player velocity: " + rigidbody.velocity);
			nextVelUpdate += 0.5f;
		}
        if (playerColor == InteractColor.Blue && Controller.GetBlueButtonDown()
		|| playerColor == InteractColor.Orange && Controller.GetOrangeButtonDown())
        {
			//if (!moving)
			Move();
        }
        else //if (moving)
        {
            Stop();
        }
    }

	public void Move()
	{
		moving = true;
		//playerCollider.material = movingMaterial;
		//sphereCollider.isTrigger = false;
		//boxCollider.isTrigger = true;
		if (rigidbody.velocity.x < maxSpeed)
		{
			iterations++;
			rigidbody.AddForce(Vector3.right * speedIncrement, ForceMode.Acceleration);
			//Debug.Log("Added force.");
		}
		else //max speed
		{
			Debug.Log("Player is at max speed. Took " + iterations + " iterations.");
		}
	}

	int iterations = 0;
	public void Stop()
	{
		moving = false;
		//sphereCollider.material = stoppedMaterial;
		//sphereCollider.isTrigger = true;
		//boxCollider.isTrigger = false;
		//
		if (rigidbody.velocity.x > minSpeed)
		{
			rigidbody.AddForce(Vector3.left * speedIncrement, ForceMode.Acceleration);
			//iterations++;
			//Debug.Log("Stopping...");
		}
		else if (rigidbody.velocity.x > speedZero)
		{
			rigidbody.velocity = Vector3.zero;
			
			//Debug.Log("Player stopped. Took " + iterations + " iterations.");
			//iterations = 0;
		}
		
	}

	public void ChangeColor(InteractColor newColor)
	{
		playerColor = newColor;
	}

}