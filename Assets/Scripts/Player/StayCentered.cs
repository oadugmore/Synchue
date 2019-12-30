using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayCentered : MonoBehaviour
{
    public float threshold = 0.1f;
    public float effortMultiplier = 2;
    public float maxEffort = 10;
    public float effort = 2;

    private new Rigidbody rigidbody;
    private float centerZ;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        centerZ = rigidbody.position.z;
    }

    private void FixedUpdate()
    {
        //float offset = rigidbody.position.z - centerZ;
        var zPos = Mathf.MoveTowards(rigidbody.position.z, centerZ, effort * Time.fixedDeltaTime);
        rigidbody.MovePosition(new Vector3(rigidbody.position.x, rigidbody.position.y, zPos));
        // if (Mathf.Abs(offset) > threshold)
        // {
        //     offset = Mathf.Clamp(offset * effortMultiplier, -maxEffort, maxEffort);
        //     rigidbody.AddForce(0, 0, -offset);
        // }
    }
}
