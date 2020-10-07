using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 testPosition = Vector3.up;
    public GameObject deathParticleSystem;

    protected new Rigidbody rigidbody;
    protected Goal goal;
    protected bool dead;

    public InteractColor playerColor { get; set; }

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
        if (!goal.finished && !dead)
        {
            dead = true;
            DeathCounter.IncrementDeathCount();
            Debug.Log("Deaths: " + DeathCounter.GetDeathCount());
            GetComponentInChildren<MeshRenderer>().enabled = false;
            Instantiate(deathParticleSystem, this.transform);
        }
    }

    public void ParticleSystemStopped()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public abstract void Move();

    public void ChangeInteractColor(InteractColor newColor)
    {
        playerColor = newColor;
    }
}