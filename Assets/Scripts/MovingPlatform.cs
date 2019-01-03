using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 velocity;

    Vector3 previousPosition;
    Rigidbody r;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        previousPosition = r.position;
    }

    void FixedUpdate()
    {
        velocity = (r.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = r.position;
    }
}
