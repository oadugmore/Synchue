using System.Collections;
using UnityEngine;

public class DefaultPlayer : Player
{
    [SerializeField]
    private float horizontalDragFactor = 15f;
    
    private SphereCollider sphereCollider;
    private bool onPlatform = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        sphereCollider = GetComponentInChildren<SphereCollider>();
        rigidbody.maxAngularVelocity = Mathf.Infinity; // remove?
    }

    private void FixedUpdate()
    {
        movementForce = Vector3.zero;
        CheckPlatform();
        Move();
    }

    /// <summary>
    /// Calculates movement force from controller, subtracts drag, and applies it to the rigidbody.
    /// </summary>
    public override void Move()
    {
        var control = Controller.GetAxis(playerColor);
        var dragX = -horizontalDragFactor * rigidbody.velocity.x; // only care about drag in x
        var forwardForceX = speed * control;
        var forceX = forwardForceX + dragX;
        movementForce.x += forceX;
        rigidbody.AddForce(movementForce);
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

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        DragKiller drag;
        if (drag = other.gameObject.GetComponent<DragKiller>())
        {
            StartCoroutine(IgnoreDrag(drag.dragIgnoreTime));
        }
    }

    /// <summary>
    /// Causes the player to ignore the forces of drag for a specified duration.
    /// </summary>
    /// <param name="seconds">The duration in seconds to ignore drag.</param>
    private IEnumerator IgnoreDrag(float seconds)
    {
        var originalDrag = horizontalDragFactor;
        horizontalDragFactor = 0f;
        yield return new WaitForSeconds(seconds);
        horizontalDragFactor = originalDrag;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
