using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

	public float maxSpeed = 5f;
	public float speedIncrement = 10f;
	public float minSpeed = 0.5f;
	//public PhysicMaterial movingMaterial;
	//public PhysicMaterial stoppedMaterial;

    private InteractColor playerColor;
    private Rigidbody playerRigidbody;
	private SphereCollider sphereCollider;
	private BoxCollider boxCollider;
	private bool moving = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Player starting");
        playerRigidbody = GetComponent<Rigidbody>();
		sphereCollider = GetComponentInChildren<SphereCollider>();
		boxCollider = GetComponent<BoxCollider>();
		playerColor = InteractColor.Blue;
    }

    // bool lastBlueMsg = false;
    // bool lastOrangeMsg = false;

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
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

	private void Move()
	{
		moving = true;
		//playerCollider.material = movingMaterial;
		//sphereCollider.isTrigger = false;
		//boxCollider.isTrigger = true;
		if (playerRigidbody.velocity.x < maxSpeed)
		{
		playerRigidbody.AddForce(Vector3.right * speedIncrement);

		Debug.Log("Added force.");
		}
	}

	private void Stop()
	{
		moving = false;
		//sphereCollider.material = stoppedMaterial;
		//sphereCollider.isTrigger = true;
		//boxCollider.isTrigger = false;
		//
		if (playerRigidbody.velocity.x > minSpeed)
		{
			playerRigidbody.AddForce(Vector3.left * speedIncrement);
			Debug.Log("Stopping...");
		}
		else if (playerRigidbody.velocity.x != 0)
		{
			playerRigidbody.velocity = Vector3.zero;
			
			Debug.Log("Stopped.");
		}
		
	}

	public void ChangeColor(InteractColor newColor)
	{
		playerColor = newColor;
	}

}
