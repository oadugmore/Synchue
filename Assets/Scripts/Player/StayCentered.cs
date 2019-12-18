using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayCentered : MonoBehaviour
{
    public float threshold = 0.1f;
    public float effortMultiplier = 2;
    public float maxEffort = 10;
    public float currentCorrectionalForce = 0f;

    private new Rigidbody rigidbody;
    private float centerZ;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        centerZ = rigidbody.position.z;
    }

    private void FixedUpdate() {
        float offset = rigidbody.position.z - centerZ;
        if (Mathf.Abs(offset) > threshold)
        {
            offset = Mathf.Clamp(offset * effortMultiplier, -maxEffort, maxEffort);
            currentCorrectionalForce = Mathf.Abs(offset);
            rigidbody.AddForce(0, 0, -offset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
