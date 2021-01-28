using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 testPosition = Vector3.up;
    public GameObject deathParticleSystem;
    public InteractColor playerColor = InteractColor.Blue;
    public AudioSource fallingSound;

    protected new Rigidbody rigidbody;
    protected Goal goal;
    protected bool dead;
    protected FollowTrackingCamera cameraController;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        goal = FindObjectOfType<Goal>();
        cameraController = FindObjectOfType<FollowTrackingCamera>();
        if (Application.isEditor)
        {
            transform.position = testPosition;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Spike") && !goal.finished && !dead)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            var particleSystem = Instantiate(deathParticleSystem, this.transform);
            if (Settings.deathSoundEnabled)
            {
                SFX.Play(particleSystem.GetComponent<AudioSource>(), SFX.deathFileID);
            }
            if (Settings.deathHapticsEnabled)
            {
                MobileUtils.Vibrate(0.5f, 0.5f, 0.3f);
            }
            Die();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death Barrier") && !goal.finished && !dead)
        {
            cameraController.lockVerticalMovement = true;
            if (Settings.deathSoundEnabled)
            {
                SFX.Play(fallingSound, SFX.fallingFileID);
            }
            if (Settings.deathHapticsEnabled)
            {
                MobileUtils.Vibrate(0.5f, 0.3f, 0.2f);
            }
            Die();
            Invoke(nameof(ReloadScene), 0.5f);
        }
        else if (other.gameObject.CompareTag("Crusher"))
        {
            throw new System.NotImplementedException("Interaction with Crusher objects is not implemented");
        }
    }

    protected virtual void Die()
    {
        dead = true;
        DeathCounter.IncrementDeathCount();
    }

    public void ParticleSystemStopped()
    {
        ReloadScene();
    }

    protected void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public abstract void Move();

    public void ChangeInteractColor(InteractColor newColor)
    {
        playerColor = newColor;
    }
}