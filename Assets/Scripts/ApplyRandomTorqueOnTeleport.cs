using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRandomTorqueOnTeleport : MonoBehaviour
{
    public void OnTeleport()
    {
        GetComponent<RandomSpinner>().ApplyTorque(false);
    }
}
