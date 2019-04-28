using System.Collections;
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
    private new Rigidbody rigidbody;
    private SphereCollider sphereCollider;
    private bool onPlatform = false;
    private Vector3 resultForce;

    //private BoxCollider boxCollider;
    //private float speedZero = 0.01f;
    //private bool moving;
    //private float lastScale = 0f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponentInChildren<SphereCollider>();
        playerColor = InteractColor.Blue;
        rigidbody.maxAngularVelocity = Mathf.Infinity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crusher"))
        {
            Die();
        }

        DragKiller drag;
        if (drag = other.gameObject.GetComponent<DragKiller>())
        {
            StartCoroutine(IgnoreDrag(drag.dragIgnoreTime));
        }
    }

    /// <summary>
    /// Causes the player to ignore the forces of drag for a specified duration.
    /// </summary>
    /// <param name="seconds">The duration in seconds to ignore drag.</param>
    private IEnumerator IgnoreDrag(float seconds)
    {
        float originalDrag = horizontalDragFactor;
        horizontalDragFactor = 0f;
        yield return new WaitForSeconds(seconds);
        horizontalDragFactor = originalDrag;
    }

    private void FixedUpdate()
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

    /// <summary>
    /// Calculates movement force from controller, subtracts drag, and applies it to the rigidbody.
    /// </summary>
    public void Move()
    {
        float control = Controller.GetAxis(playerColor);
        float dragX = -horizontalDragFactor * rigidbody.velocity.x; // only care about drag in x
        float forwardForceX = speed * control;
        float forceX = forwardForceX + dragX;
        resultForce.x += forceX;
        rigidbody.AddForce(resultForce);
    }

    /// <summary>
    /// Adds any relevant moving platform forces to resultForce.
    /// </summary>
    private void CheckPlatform()
    {
        if (Physics.Raycast(sphereCollider.bounds.center, Vector3.down, out RaycastHit hit, sphereCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("CarryPlayer")))
        {
            if (!onPlatform)
            {
                onPlatform = true;
            }

            resultForce.x += hit.rigidbody.GetComponent<MovingPlatform>().getVelocity().x * horizontalDragFactor;
        }
        else
        {
            if (onPlatform)
            {
                onPlatform = false;
            }
        }
    }

    /// <summary>
    /// Changes the player's InteractColor.
    /// </summary>
    public void ChangeColor(InteractColor newColor)
    {
        playerColor = newColor;
    }

}