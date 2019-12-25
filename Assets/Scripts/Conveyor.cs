using UnityEngine;

public class Conveyor : MonoBehaviour, ICToggleObject
{
    public Vector3 force;

    private bool on = false;

    public void Toggle(bool on)
    {
        this.on = on;
        Debug.Log("Toggled " + (on ? "on" : "off"));
    }

    private void OnTriggerStay(Collider other)
    {
        if (!on) return;
        //Debug.Log("stay");
        Rigidbody r;
        if (r = other.gameObject.GetComponentInChildren<Rigidbody>())
        {
            //Debug.Log("force");
            r.AddForce(force);
        }
    }
}
