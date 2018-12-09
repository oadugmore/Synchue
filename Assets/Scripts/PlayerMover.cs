using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMover : MonoBehaviour, Pushable
{

	public float speed = 10f;
	public float horizontalDrag = 1f;
	//public float maxSpeed = 5f;
	//public float minSpeed = 0.5f;
	//public PhysicMaterial movingMaterial;
	//public PhysicMaterial stoppedMaterial;

    private InteractColor playerColor;
    new private Rigidbody rigidbody;
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
		rigidbody.maxAngularVelocity = 50f;
		sphereCollider = GetComponentInChildren<SphereCollider>();
		//boxCollider = GetComponent<BoxCollider>();
		playerColor = InteractColor.Blue;
		//Debug.Log(Physics.);
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
		Move();
    }

	private void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Move()
	{
		float control = Controller.GetAxis(playerColor);
		float dragX = -horizontalDrag * rigidbody.velocity.x; // only care about drag in x
		float angularDragX = -horizontalDrag * rigidbody.angularVelocity.z;
		//if (rigidbody.velocity.x < 0) dragX = 0f;
		//float forceX = dragX + speed * control;
		//rigidbody.AddForce(forceX, 0, 0);
		float torqueX = -speed * control;
		Debug.Log(rigidbody.angularVelocity.z);
		rigidbody.AddTorque(0, 0, torqueX);
	}

	bool controlFinishedStopping = false;
	[System.Obsolete]
	void UpdateVelocity()
	{
        float scale = Controller.GetAxis(playerColor);
		float desiredSpeed = 0f;
		//float desiredSpeed = maxSpeed * scale;
        
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
	
	int iterations = 0;
	bool hasPrinted = true;
	[System.Obsolete]
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