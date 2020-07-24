using UnityEngine;

[System.Obsolete]
public class RotationLocker : MonoBehaviour
{
    private Quaternion originalRotation;
    private Rigidbody r;

    // Start is called before the first frame update
    private void Start()
    {
        r = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        transform.rotation = originalRotation;
    }

}
