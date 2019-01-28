using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 velocity;
    public bool useRigidbodyVelocity = true;

    Vector3 previousPosition;
    Rigidbody r;

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
}
