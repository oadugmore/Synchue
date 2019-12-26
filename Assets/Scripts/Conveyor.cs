using UnityEngine;

public class Conveyor : MonoBehaviour, ICToggleObject
{
    public Vector3 force;

    private bool on = false;

    public void Toggle(bool on)
    {
        this.on = on;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!on) return;
        Rigidbody r;
        if (r = other.gameObject.GetComponentInChildren<Rigidbody>())
        {
            r.AddForce(force);
        }
    }
}
