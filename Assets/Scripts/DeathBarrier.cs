using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.SendMessageUpwards("Die", SendMessageOptions.DontRequireReceiver);
    }
}
