using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{

    public Vector3 velocityDisplay;
    public float speed = 10f;
    //public float max = 10f;
    //public float restingAngularDrag = 30f;

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
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            Die();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crusher"))
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public abstract void Move();

    /// <summary>
    /// Changes the player's InteractColor.
    /// </summary>
    public void ChangeColor(InteractColor newColor)
    {
        playerColor = newColor;
    }

}