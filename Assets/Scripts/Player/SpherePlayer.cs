using UnityEngine;

public class SpherePlayer : Player
{
    private SphereCollider sphereCollider;
    private bool onPlatform = false;
    private float lastControl = 0f;
    private bool stopping = false;

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
        var slowDown = 1f;
        var control = Controller.GetAxis(playerColor);
        if (control < lastControl)
        {
            if (control == 0f)
            {
                stopping = true;
                Debug.Log("Stopping");
            }
            slowDown *= -1 * rigidbody.velocity.x;
        }
        else if (control > lastControl)
        {
            stopping = false;
            Debug.Log("Done stopping");
        }
        //var dragX = -horizontalDragFactor * rigidbody.velocity.x; // only care about drag in x
        float forceX;
        if (stopping)
        {
            forceX = -rigidbody.velocity.x;
            if (rigidbody.velocity.x < 0.1f)
            {
                stopping = false;
                Debug.Log("Done stopping");
            }
        }
        else
        {
            forceX = speed * control * slowDown;
        }

        //var forceX = forwardForceX + dragX;
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

    // Update is called once per frame
    private void Update()
    {

    }
}
