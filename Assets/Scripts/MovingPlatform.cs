using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool useRigidbodyVelocity = true;

    // giving velocity its own field allows us to
    // implement it for different types of platform movement
    public Vector3 velocity { get; private set; }
    
    private Vector3 previousPosition;
    private Rigidbody r;

    private void Start()
    {
        r = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        previousPosition = r.position;
    }

    private void FixedUpdate()
    {
        if (useRigidbodyVelocity)
        {
            velocity = r.velocity;
        }
        else
        {
            velocity = (r.position - previousPosition) / Time.fixedDeltaTime;
            previousPosition = r.position;
        }
    }

}
