using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sends a received OnParticleSystemStopped message upwards as
/// "ParticleSystemStopped" to avoid stack overflow
/// </summary>
public class ParticleSystemStoppedMessageForwarder : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        SendMessageUpwards("ParticleSystemStopped");
    }
}
