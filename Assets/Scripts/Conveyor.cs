using UnityEngine;

public class Conveyor : MonoBehaviour, ICToggleObject
{
    [SerializeField]
    private Vector3 force;
    private bool on = false;

    public void Toggle(bool on)
    {
        this.on = on;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!on) return;
        Rigidbody r;
        if (r = other.gameObject.GetComponentInParent<Rigidbody>())
        {
            r.AddForce(force);
        }
    }
}
