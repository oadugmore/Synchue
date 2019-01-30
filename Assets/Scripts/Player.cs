using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public Vector3 velocityDisplay;
    public float speed = 10f;
    //public float max = 10f;
    //public float restingAngularDrag = 30f;
    public float horizontalDragFactor = 15f;
    
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
    private RigidbodyConstraints normalConstraints = RigidbodyConstraints.None;
    //private bool ignoreDrag = false;
    //private RigidbodyConstraints 

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //rigidbody.maxAngularVelocity = 50f;
        sphereCollider = GetComponentInChildren<SphereCollider>();
        playerColor = InteractColor.Blue;
        rigidbody.maxAngularVelocity = Mathf.Infinity;
		//Debug.Log(sphereCollider.bounds.extents);
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
        DragKiller drag;
        if (drag = other.gameObject.GetComponent<DragKiller>())
        {
            StartCoroutine(ignoreDrag(drag.dragIgnoreTime));
        }
    }

    IEnumerator ignoreDrag(float seconds)
    {
        float originalDrag = horizontalDragFactor;
        horizontalDragFactor = 0f;
        yield return new WaitForSeconds(seconds);
        horizontalDragFactor = originalDrag;
    }

    Vector3 resultForce;
    float nextVelUpdate = 0f;
    void FixedUpdate()
    {
        resultForce = Vector3.zero;
        velocityDisplay = rigidbody.velocity;
        CheckPlatform();
        Move();
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //public float maxSpeed = 5f;
    //public float maxDrag = 5f;
    //bool dbg_MaxPrinted = false;
    public void Move()
    {
        float control = Controller.GetAxis(playerColor);
        float dragX = -horizontalDragFactor * rigidbody.velocity.x; // only care about drag in x
        //float angularDragX = -horizontalDragFactor * rigidbody.angularVelocity.z;
        //rigidbody.angularDrag = (1 - control) * restingAngularDrag;
        //if (rigidbody.velocity.x < 0) dragX = 0f;
        // float forceX = dragX + speed * control;
		//float beforeVelocitySign = Mathf.Sign(rigidbody.velocity.x);
        //dragX = Mathf.Clamp(dragX, -maxDrag, maxDrag);
        float forwardForceX = speed * control;
        // if (rigidbody.velocity.x > maxSpeed) 
        // {
        //     forwardForceX = 0f;
        //     if (!dbg_MaxPrinted) Debug.Log("Max speed: " + rigidbody.velocity.x);
        //     dbg_MaxPrinted = true;
        // }
        //else dbg_MaxPrinted = false;
        float forceX = forwardForceX + dragX; //-beforeVelocitySign * horizontalDrag;

        resultForce.x += forceX;
        //forceX = Mathf.Clamp(forceX, -maxForce, maxForce);
        //if (rigidbody.velocity.x < maxVelocityX)
        rigidbody.AddForce(resultForce);
		// if (Mathf.Sign(rigidbody.velocity.x) != beforeVelocitySign)
		// 	rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.z);
    }

    bool onPlatform = false;
    //public float platformForceScale = 1f;
    private void CheckPlatform()
    {
		RaycastHit hit;
        if (Physics.Raycast(sphereCollider.bounds.center, Vector3.down, out hit, sphereCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("CarryPlayer")))
        {
            if (!onPlatform)
            {
                Debug.Log("Player moved onto platform.");
                onPlatform = true;
            }
            
            resultForce.x += hit.rigidbody.GetComponent<MovingPlatform>().getVelocity().x * horizontalDragFactor;
            //rigidbody.AddForce(hit.rigidbody.GetComponent<MovingPlatform>().velocity.x * horizontalDragFactor, 0, 0);
        }
        else
        {
            if (onPlatform)
            {
                Debug.Log("Player moved off platform.");
                onPlatform = false;
            }
        }

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