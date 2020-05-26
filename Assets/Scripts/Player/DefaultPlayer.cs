﻿using System.Collections;
using UnityEngine;

public class DefaultPlayer : Player
{
    [SerializeField]
    private float horizontalDragFactor = 15f;
    
    new private Collider collider;
    private int carryPlayerMask;

    public override void Start()
    {
        base.Start();
        collider = GetComponentInChildren<Collider>();
        carryPlayerMask = LayerMask.GetMask("CarryPlayer");
    }

    private void FixedUpdate()
    {
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
        rigidbody.AddForce(forceX, 0f, 0f);
    }

    /// <summary>
    /// Adds any relevant moving platform forces to resultForce.
    /// </summary>
    private void CheckPlatform()
    {
        if (Physics.Raycast(collider.bounds.center, Vector3.down, out var hit, collider.bounds.extents.y + 0.1f, carryPlayerMask))
        {
            var platformForce = hit.rigidbody.GetComponent<MovingPlatform>().velocity * horizontalDragFactor;
            rigidbody.AddForce(platformForce.x, 0f, platformForce.z);
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
}
