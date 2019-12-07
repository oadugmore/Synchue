using UnityEngine;

public class RocketPlayer : Player
{
    public override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        movementForce = Vector3.zero;
        Move();
    }

    public override void Move()
    {
        var control = Controller.GetAxis(playerColor);
        var upwardForceY = speed * control;
        movementForce.y += upwardForceY;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
