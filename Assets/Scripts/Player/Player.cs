using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected float speed = 10f;
    protected new Rigidbody rigidbody;

    public InteractColor playerColor { get; set; }

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
        if (!FindObjectOfType<Goal>().finished)
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