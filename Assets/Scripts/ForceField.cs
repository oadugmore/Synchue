using UnityEngine;

public class ForceField : MonoBehaviour, ICToggleObject
{
    [SerializeField]
    private Vector3 force;
    [SerializeField]
    private ForceMode forceMode;
    [SerializeField]
    private bool on;

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
            r.AddForce(force, forceMode);
        }
    }
}
