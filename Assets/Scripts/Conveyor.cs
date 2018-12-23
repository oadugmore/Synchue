using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{

    public Vector3 force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("stay");
        Rigidbody r;
        if (r=collider.gameObject.GetComponentInChildren<Rigidbody>())
        {
            Debug.Log("force");
            r.AddForce(force);
        }
    }
}
