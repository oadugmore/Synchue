using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVelocity : MonoBehaviour
{

    public bool printInfo = false;
    [SerializeField]
    private Vector3 worldPosDisplay;
    [SerializeField]
    private Vector3 velocityDisplay;
    [SerializeField]
    private Vector3 platformVelocityDisplay;

    Rigidbody r;
    MovingPlatform p;

    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
        p = GetComponent<MovingPlatform>();
        velocityDisplay = Vector3.zero;
        worldPosDisplay = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (printInfo)
        {
            Debug.Log("Rigidbody velocity: " + r.velocity);
            Debug.Log("World position: " + r.position);
        }

        if (p)
        {
            platformVelocityDisplay = p.getVelocity();
            if (printInfo)
                Debug.Log("Platform velocity: " + platformVelocityDisplay);
        }

        velocityDisplay = r.velocity;
        worldPosDisplay = r.position;
    }
}
