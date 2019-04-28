using UnityEngine;

public class Conveyor : MonoBehaviour
{

    public Vector3 force;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("stay");
        Rigidbody r;
        if (r = other.gameObject.GetComponentInChildren<Rigidbody>())
        {
            //Debug.Log("force");
            r.AddForce(force);
        }
    }
}
