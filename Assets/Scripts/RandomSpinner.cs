using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpinner : MonoBehaviour
{
    [Tooltip("When this is set, uses torque as the maximum (positive and negative)."), SerializeField]
    private bool randomTorque;
    [SerializeField]
    private Vector3 torque;
    [SerializeField]
    private ForceMode forceMode;
    [Header("When to apply the torque"), SerializeField]
    private bool inStart;
    [Tooltip("Using this option along with the Random Torque option may be resource-intensive."), SerializeField]
    private bool inFixedUpdate;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (inStart)
        {
            ApplyTorque(false);
        }
    }

    void FixedUpdate()
    {
        if (inFixedUpdate)
        {
            ApplyTorque(true);
        }
    }

    private Vector3 getRandomTorque()
    {
        return new Vector3(
            Random.Range(-torque.x, torque.x),
            Random.Range(-torque.y, torque.y),
            Random.Range(-torque.z, torque.z)
        );
    }

    /// <summary>
    /// Applies the torque.
    /// </summary>
    /// <param name="continuous">If true, scales the torque by Time.fixedDeltaTime</param>
    public void ApplyTorque(bool continuous)
    {
        var continuousValue = continuous ? Time.deltaTime : 1f;
        Vector3 thisTorque = randomTorque ? getRandomTorque() : torque;
        rb.AddTorque(thisTorque * continuousValue, forceMode);
    }
}
