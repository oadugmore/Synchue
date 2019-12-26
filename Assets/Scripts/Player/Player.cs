using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    public float speed = 10f;

    protected InteractColor playerColor;
    protected new Rigidbody rigidbody;
    [System.Obsolete]
    protected Vector3 movementForce;

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