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
    public Vector3 testPosition;

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

    protected virtual void Die()
    {
        if (!goal.finished)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public abstract void Move();

    public void ChangeInteractColor(InteractColor newColor)
    {
        playerColor = newColor;
    }
}