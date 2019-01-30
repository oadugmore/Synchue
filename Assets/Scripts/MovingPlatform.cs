using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool useRigidbodyVelocity = true;

    private Vector3 velocity;
    private Vector3 previousPosition;
    private Rigidbody r;

    void Start()
    {
        r = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        previousPosition = r.position;
    }

    void FixedUpdate()
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

    public Vector3 getVelocity()
    {
        return velocity;
    }
}
