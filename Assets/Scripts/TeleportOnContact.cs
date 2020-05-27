using UnityEngine;

public class TeleportOnContact : MonoBehaviour
{
    public Transform destination;
    public bool resetVelocity;

    void OnTriggerEnter(Collider other)
    {
        if (resetVelocity)
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
        }
        other.transform.position = destination.position;
        other.transform.SendMessage("OnTeleport");
    }
}
