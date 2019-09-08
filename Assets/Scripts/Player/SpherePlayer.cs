using UnityEngine;

public class SpherePlayer : Player
{
    private SphereCollider sphereCollider;
    private bool onPlatform = false;

    // Start is called before the first frame update
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
        var control = Controller.GetAxis(playerColor);
        var dragX = -horizontalDragFactor * rigidbody.velocity.x; // only care about drag in x
        var forwardForceX = speed * control;
        var forceX = forwardForceX + dragX;
        movementForce.x += forceX;
        base.Move();
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

            movementForce.x += hit.rigidbody.GetComponent<MovingPlatform>().getVelocity().x * horizontalDragFactor;
        }
        else
        {
            if (onPlatform)
            {
                onPlatform = false;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
