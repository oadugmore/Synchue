using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected float speed = 10f;
    protected new Rigidbody rigidbody;
    protected Goal goal;

    public InteractColor playerColor { get; set; }
    [SerializeField]
    private Vector3 testPosition;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        goal = FindObjectOfType<Goal>();
        playerColor = InteractColor.Blue;
        if (Application.isEditor)
        {
            transform.position = testPosition;
        }
    }

    public void SetTestPosition(Vector3 testPosition)
    {
        this.testPosition = testPosition;
        Debug.Log("Updated test position to " + testPosition);
    }

    public Vector3 GetTestPosition()
    {
        Debug.Log("Accessed test position");
        return testPosition;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Spike"))
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
        if (!goal.finished)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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