using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldColorButton : MonoBehaviour
{
    public bool isPressed { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = false;
        }
    }
}
