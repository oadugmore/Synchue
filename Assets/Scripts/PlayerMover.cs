using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private float lastScale = 0f;

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

	private void OnCollisionEnter(Collision other) 
	{
		//Debug.Log("Collided with " + other.gameObject);
		if (other.gameObject.CompareTag("Spike"))
		{
			Die();
		}
	}

	private void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag("Crusher"))
			Die();
	}

	float nextVelUpdate = 0f;
    void FixedUpdate()
    {
		// if (Time.fixedTime > nextVelUpdate)
		// {
        //     //Debug.Log("Player velocity: " + rigidbody.velocity);
		// 	nextVelUpdate += 0.5f;
		// }
        // if (playerColor == InteractColor.Blue && Controller.GetBlueButtonDown()
		// || playerColor == InteractColor.Orange && Controller.GetOrangeButtonDown())
        // {
		// 	//if (!moving)
		// 	Move();
        // }
        // else //if (moving)
        // {
        //     Stop();
        // }
		
		UpdateVelocity();
    }

	private void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	bool controlFinishedStopping = false;
	void UpdateVelocity()
	{
        float scale = Controller.GetAxis(playerColor);
		float desiredSpeed = maxSpeed * scale;
        
        if (rigidbody.velocity.magnitude != desiredSpeed)
        {
            Vector3 newVelocity;
            newVelocity = new Vector3(desiredSpeed, rigidbody.velocity.y, 0);
            rigidbody.velocity = newVelocity;
        }

		// if (scale == 0f && !controlFinishedStopping)
		// {
		// 	controlFinishedStopping = true;
		// }

		// else if (scale != 0f)
		// 	controlFinishedStopping = false;

        //lastScale = scale;
	}

	[System.Obsolete]
	public void Move()
	{
		moving = true;
		//playerCollider.material = movingMaterial;
		//sphereCollider.isTrigger = false;
		//boxCollider.isTrigger = true;
		if (rigidbody.velocity.x < maxSpeed)
		{
			//hasPrinted = false;
			//iterations++;
			rigidbody.AddForce(new Vector3(speedIncrement, rigidbody.velocity.y, 0), ForceMode.Acceleration);
			//Debug.Log("Added force.");
		}
		else //max speed
		{
			if (!hasPrinted)
			{
				Debug.Log("Player is at max speed. Took " + iterations + " iterations.");
				hasPrinted = true;
				iterations = 0;
			}

		}
	}

	int iterations = 0;
	bool hasPrinted = false;
	[System.Obsolete]
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