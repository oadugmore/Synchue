using System;
using UnityEngine;

public class SpherePlayer : Player
{
    [SerializeField]
    private float stoppingSpeed = 1;
    private SphereCollider sphereCollider;
    private bool onPlatform = false;
    private float lastControl = 0f;
    private MovementMode movementMode = MovementMode.Inactive;

    public override void Start()
    {
        base.Start();
        sphereCollider = GetComponentInChildren<SphereCollider>();
    }

    private void FixedUpdate()
    {
        CheckPlatform();
        Move();
    }

    public override void Move()
    {
        var control = Controller.GetAxis(playerColor);
        if (control < lastControl)
        {
            movementMode = MovementMode.PassiveBraking;
        }
        else if (control > lastControl)
        {
            movementMode = MovementMode.Active;
        }

        float forceX = 0;
        switch (movementMode)
        {
            case MovementMode.Inactive:
                if (rigidbody.velocity.x > 0.01f)
                {
                    movementMode = MovementMode.PassiveBraking;
                    break;
                }
                break;
            case MovementMode.Active:
                forceX = speed;
                break;
            case MovementMode.PassiveBraking:
                if (rigidbody.velocity.x < 0.01f)
                {
                    movementMode = MovementMode.Inactive;
                    break;
                }
                forceX = -rigidbody.velocity.x * stoppingSpeed;
                break;
            default:
                throw new NotImplementedException("Movement mode " + movementMode + " not supported.");
        }

        rigidbody.AddForce(forceX, 0f, 0f);
        lastControl = control;
    }

    /// <summary>
    /// Adds any relevant moving platform forces to resultForce.
    /// </summary>
    private void CheckPlatform()
    {
        if (Physics.Raycast(sphereCollider.bounds.center, Vector3.down, out var hit, sphereCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("CarryPlayer")))
        {
            var platformForce = hit.rigidbody.GetComponent<MovingPlatform>().velocity.x;
            rigidbody.AddForce(platformForce, 0f, 0f);
        }

    }
}

enum MovementMode
{
    Active, PassiveBraking, Inactive
}
