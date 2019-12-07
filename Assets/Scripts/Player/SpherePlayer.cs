using UnityEngine;

public class SpherePlayer : Player
{
    private SphereCollider sphereCollider;
    private bool onPlatform = false;
    private float lastControl = 0f;
    //private bool stopping = false;
    private MovementMode movementMode = MovementMode.Stopped;

    public override void Start()
    {
        base.Start();
        sphereCollider = GetComponentInChildren<SphereCollider>();
    }

    private void FixedUpdate()
    {
        movementForce = Vector3.zero;
        CheckPlatform();
        Move();
    }

    public override void Move()
    {
        //var slowDown = 1f;
        var control = Controller.GetAxis(playerColor);
        if (control < lastControl)
        {
            if (control == 0f)
            {
                movementMode = MovementMode.Stopping;
                //Debug.Log("Stopping");
            }
            else
            {
                movementMode = MovementMode.Slowing;
            }
            //slowDown *= -1 * rigidbody.velocity.x;
        }
        else if (control > lastControl)
        {
            movementMode = MovementMode.Accelerating;
            //Debug.Log("Accelerating");
        }

        float forceX;
        switch (movementMode)
        {
            case MovementMode.Stopped:
            case MovementMode.Accelerating:
                forceX = speed * control;
                break;
            case MovementMode.Slowing:
                forceX = -speed * control * rigidbody.velocity.x;
                break;
            case MovementMode.Stopping:
                if (rigidbody.velocity.x < 0.1f)
                {
                    movementMode = MovementMode.Stopped;
                    //Debug.Log("Stopped.");
                }
                forceX = -rigidbody.velocity.x;
                break;
            default:
                throw new System.NotImplementedException("Movement mode " + movementMode + " not supported.");
        }

        movementForce.x += forceX;
        rigidbody.AddForce(movementForce);
        lastControl = control;
    }

    /// <summary>
    /// Adds any relevant moving platform forces to resultForce.
    /// </summary>
    private void CheckPlatform()
    {
        if (Physics.Raycast(sphereCollider.bounds.center, Vector3.down, out var hit, sphereCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("CarryPlayer")))
        {
            if (!onPlatform)
            {
                onPlatform = true;
            }

            movementForce.x += hit.rigidbody.GetComponent<MovingPlatform>().getVelocity().x;
        }
        else
        {
            if (onPlatform)
            {
                onPlatform = false;
            }
        }
    }
}

enum MovementMode
{
    Accelerating, Slowing, Stopping, Stopped
}
