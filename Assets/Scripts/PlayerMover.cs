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
	//RigidbodyConstraints 

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
		sphereCollider = GetComponentInChildren<SphereCollider>();
		//boxCollider = GetComponent<BoxCollider>();
		playerColor = InteractColor.Blue;
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
        // if (Controller.GetAxis(playerColor) > 0f)
        // {
			//if (!moving)
			Move();
        //}
        if (moving)
        {
            Stop();
        }
		
		//UpdateVelocity();
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

	float lastControl = 0f;
	public void Move()
	{
		if (!moving)
		{
			rigidbody.constraints = RigidbodyConstraints.None;
			moving = true;
		}
		float control = Controller.GetAxis(playerColor);
		//playerCollider.material = movingMaterial;
		//sphereCollider.isTrigger = false;
		//boxCollider.isTrigger = true;
		if (rigidbody.angularVelocity.z < maxSpeed && lastControl <= control)
		{
			//hasPrinted = false;
			//iterations++;
			//rigidbody.AddForce(new Vector3(speedIncrement * control, 0, 0));
			rigidbody.AddTorque(new Vector3(0, 0, -speedIncrement * control));

			//Debug.Log("Added force.");
		}
		else if (rigidbody.angularVelocity.z > speedZero && lastControl > control)
		{
			rigidbody.AddTorque(new Vector3(0, 0, (1f - control) * speedIncrement));


			// if (!hasPrinted)
			// {
			// 	Debug.Log("Player is at max speed. Took " + iterations + " iterations.");
			// 	hasPrinted = true;
			// 	iterations = 0;
			// }

		}
	}

	int iterations = 0;
	bool hasPrinted = true;
	public void Stop()
	{
		moving = false;

		if (rigidbody.velocity.x < 0f)
		{
			Debug.Log("Player started to move backwards in Stop()!");
			rigidbody.AddForce(-rigidbody.velocity.x, 0, 0);
		}

		rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;

		//sphereCollider.material = stoppedMaterial;
		//sphereCollider.isTrigger = true;
		//boxCollider.isTrigger = false;
		//


		// if (rigidbody.velocity.x > minSpeed)
		// {
		// 	rigidbody.AddForce(Vector3.left * speedIncrement, ForceMode.Acceleration);
		// 	//iterations++;
		// 	//Debug.Log("Stopping...");
		// }
		// else if (rigidbody.velocity.x > speedZero)
		// {
		// 	rigidbody.velocity = Vector3.zero;
			
		// 	//Debug.Log("Player stopped. Took " + iterations + " iterations.");
		// 	//iterations = 0;
		// }
		
	}

	public void ChangeColor(InteractColor newColor)
	{
		playerColor = newColor;
	}

}