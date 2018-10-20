using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBackForthMover : MonoBehaviour
{

    public float controlDistance = 15f;
    public Direction movementDirection = Direction.UpDown;
    public float maxSpeed = 5f;
    public float movementDistance = 4f;
	//public float maxSpeed = 5f;
	public float speedIncrement = 10f;
	public float minSpeed = 0.5f;
    //public static Material blueMaterial;
    //public static Material orangeMaterial;
    public InteractColor color = InteractColor.Blue;

    private bool movingForward;
    private Rigidbody m_rigidbody;
    private Transform cameraTransform;
    private Vector3 forwardDirection;
    private Vector3 backwardDirection;
    private Vector3 initialPosition;
	private float speedZero = 0.01f;

    // Use this for initialization
    void Start()
    {
        movingForward = true;
        m_rigidbody = GetComponent<Rigidbody>();
        //m_rigidbody.isKinematic = true;
        cameraTransform = Camera.main.transform;
        initialPosition = transform.position;
        SetMovementDirectionVectors();
    }

    public void SetMovementDirectionVectors()
    {
        switch (movementDirection)
        {
            case (Direction.DownUp):
                forwardDirection = Vector3.down;
                backwardDirection = Vector3.up;
                break;
            case (Direction.UpDown):
                forwardDirection = Vector3.up;
                backwardDirection = Vector3.down;
                break;
            case (Direction.LeftRight):
                forwardDirection = Vector3.left;
                backwardDirection = Vector3.right;
                break;
            case (Direction.RightLeft):
                forwardDirection = Vector3.right;
                backwardDirection = Vector3.left;
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (color == InteractColor.Blue && Controller.GetBlueButtonDown()
        || color == InteractColor.Orange && Controller.GetOrangeButtonDown())
        {
            if (Vector3.Distance(cameraTransform.position, transform.position) < controlDistance)
            {
                Move();
            }
        }
		else
			Stop();
    }

    void Move()
    {
        if (ShouldSwitchDirection())
        {
            Debug.Log("Switched direction!");
            movingForward = !movingForward;
            m_rigidbody.velocity = Vector3.zero;
        }
        else if (m_rigidbody.velocity.magnitude < maxSpeed)
        {
            Debug.Log("Added force");
            if (movingForward)
                m_rigidbody.AddForce(forwardDirection * maxSpeed, ForceMode.Acceleration);
            else
                m_rigidbody.AddForce(backwardDirection * maxSpeed, ForceMode.Acceleration);
        }
    }

    private void Stop()
    {

        //moving = false;
        //sphereCollider.material = stoppedMaterial;
        //sphereCollider.isTrigger = true;
        //boxCollider.isTrigger = false;
        //

		// switch (movementDirection)
		// {

		// 	case Direction.DownUp:
                
        //     case Direction.UpDown:
        //         return (movingForward && difference.y < -movementDistance) || (!movingForward && difference.y > 0);
        //     case Direction.LeftRight:
        //         return (movingForward && difference.x > movementDistance) || (!movingForward && difference.x < 0);
        //     case Direction.RightLeft:
        //         return (movingForward && difference.x < -movementDistance) || (!movingForward && difference.x > 0);
        //     default:
        //         throw new System.NotImplementedException();
		// }
        if (m_rigidbody.velocity.magnitude > minSpeed)
        {
			Debug.Log("Stopping");
            m_rigidbody.AddForce((movingForward ? backwardDirection : forwardDirection) * speedIncrement, ForceMode.Acceleration);
            //m_rigidbody.AddTorque(m_rigidbody.ResetInertiaTensor)
			//Debug.Log("Stopping...");
        }
        else if (m_rigidbody.velocity.magnitude > speedZero)
        {
			Debug.Log("Stopped");
            m_rigidbody.velocity = Vector3.zero;

            //Debug.Log("Stopped.");
        }

    }


    private bool ShouldSwitchDirection()
    {
        Vector3 difference = initialPosition - transform.position;
        switch (movementDirection)
        {
            case Direction.DownUp:
                return (movingForward && difference.y > movementDistance) || (!movingForward && difference.y < 0);
            case Direction.UpDown:
                return (movingForward && difference.y < -movementDistance) || (!movingForward && difference.y > 0);
            case Direction.LeftRight:
                return (movingForward && difference.x > movementDistance) || (!movingForward && difference.x < 0);
            case Direction.RightLeft:
                return (movingForward && difference.x < -movementDistance) || (!movingForward && difference.x > 0);
            default:
                throw new System.NotImplementedException();
        }
    }

}

public enum Direction
{
    UpDown, DownUp, LeftRight, RightLeft
}