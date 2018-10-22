using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBackForthMover : MonoBehaviour, Pushable
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
    private Rigidbody rigidbody;
    private Transform cameraTransform;
    private Vector3 forwardDirection;
    private Vector3 backwardDirection;
    private Vector3 initialPosition;
    private float speedZero = 0.01f;

    // Use this for initialization
    void Start()
    {
        movingForward = true;
        rigidbody = GetComponent<Rigidbody>();
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

    //float nextVelUpdate = 0f;
    void FixedUpdate()
    {
        // if (Time.fixedTime > nextVelUpdate)
        // {
        //     Debug.Log("ColorMover velocity: " + rigidbody.velocity);
        // 	nextVelUpdate += 0.5f;
        // }

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

    public void Move()
    {
        if (ShouldSwitchDirection())
        {
            //Debug.Log("Switched direction!");
            movingForward = !movingForward;
            rigidbody.velocity = -rigidbody.velocity;
        }
        else if (rigidbody.velocity.magnitude < maxSpeed)
        {
            iterations++;
            //Debug.Log("Added force");
            if (movingForward)
                rigidbody.AddForce(forwardDirection * speedIncrement, ForceMode.Acceleration);
            else
                rigidbody.AddForce(backwardDirection * speedIncrement, ForceMode.Acceleration);
        }
        else //max speed
        {
            //Debug.Log("ColorMover is at max speed. Took " + iterations + " iterations.");
            //iterations = 0;
        }
    }

    int iterations = 0;
    public void Stop()
    {

        //moving = false;
        //sphereCollider.material = stoppedMaterial;
        //sphereCollider.isTrigger = true;
        //boxCollider.isTrigger = false;

        if (rigidbody.velocity.magnitude > minSpeed)
        {
            //Debug.Log("Stopping");
            //iterations++;
            rigidbody.AddForce((movingForward ? backwardDirection : forwardDirection) * speedIncrement, ForceMode.Acceleration);
            //m_rigidbody.AddTorque(m_rigidbody.ResetInertiaTensor)
            //Debug.Log("Stopping...");
        }
        else if (rigidbody.velocity.magnitude > speedZero)
        {
            //Debug.Log("ColorMover stopped. Took " + iterations + " iterations.");
            //iterations = 0;
            rigidbody.velocity = Vector3.zero;
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