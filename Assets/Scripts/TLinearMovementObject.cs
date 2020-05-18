using UnityEngine;

public class TLinearMovementObject : MonoBehaviour, ICToggleObject
{
    [SerializeField]
    private Vector3 movementDirection;
    
    private bool isMoving;
    private Rigidbody rb;

    public void Toggle(bool on)
    {
        isMoving = on;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.AddForce(movementDirection);
        }
    }
}
