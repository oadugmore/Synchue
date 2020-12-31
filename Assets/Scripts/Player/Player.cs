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
        SFX.Initialize();
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
        // TODO: Use different haptic patterns for spikes/falling
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
            GetComponentInChildren<MeshRenderer>().enabled = false;
            var particleSystem = Instantiate(deathParticleSystem, this.transform);
            if (Settings.deathSoundEnabled)
            {
                SFX.Play(particleSystem.GetComponent<AudioSource>(), SFX.deathFileID);
            }
            if (Settings.hapticsEnabled)
            {
                MobileUtils.Vibrate(0.5f, 0.5f, 0.3f);
            }
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