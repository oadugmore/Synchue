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
    [HideInInspector]
    public bool dead;

    protected new Rigidbody rigidbody;
    protected FollowTrackingCamera cameraController;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraController = FindObjectOfType<FollowTrackingCamera>();
        if (Application.isEditor)
        {
            transform.position = testPosition;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Spike") && !Goal.instance.wasReached && !dead)
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
        if (other.gameObject.CompareTag("Death Barrier") && !Goal.instance.wasReached && !dead)
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

    /// <summary>
    /// Brings the player to a stop.
    /// </summary>
    /// <param name="time">The time in seconds to slow the player down before reaching a complete stop.</param>
    public void Freeze(float time)
    {
        rigidbody.useGravity = false;
        LeanTween.value(gameObject, rigidbody.velocity, Vector3.zero, time)
        .setEaseInExpo()
        .setOnUpdate((Vector3 velocity) => rigidbody.velocity = velocity)
        .setOnComplete(() =>
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.isKinematic = true;
        }
        );
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