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

    protected InteractColor playerColor;
    protected new Rigidbody rigidbody;
    protected Vector3 movementForce;

    //private BoxCollider boxCollider;
    //private float speedZero = 0.01f;
    //private bool moving;
    //private float lastScale = 0f;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
        var originalDrag = horizontalDragFactor;
        horizontalDragFactor = 0f;
        yield return new WaitForSeconds(seconds);
        horizontalDragFactor = originalDrag;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Calculates movement force from controller, subtracts drag, and applies it to the rigidbody.
    /// </summary>
    public virtual void Move()
    {
        rigidbody.AddForce(movementForce);
        velocityDisplay = rigidbody.velocity;
    }

    /// <summary>
    /// Changes the player's InteractColor.
    /// </summary>
    public void ChangeColor(InteractColor newColor)
    {
        playerColor = newColor;
    }

}