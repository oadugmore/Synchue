using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLocker : MonoBehaviour
{
    Quaternion originalRotation;
    Rigidbody r;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        transform.rotation = originalRotation;
    }

}
