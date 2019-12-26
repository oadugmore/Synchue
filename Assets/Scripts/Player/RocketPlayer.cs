using UnityEngine;

public class RocketPlayer : Player
{
    public float reverseGravityMultiplier = 2f;

    public override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        var control = Controller.GetAxis(playerColor);
        var upwardForceY = speed * control;
        rigidbody.AddForce(0, upwardForceY, 0);
        if (rigidbody.velocity.y > 0 && control == 0f)
        {
            rigidbody.velocity += Physics.gravity * reverseGravityMultiplier * Time.fixedDeltaTime;
        }
        else if (rigidbody.velocity.y < 0 && control > 0f)
        {
            rigidbody.velocity += Physics.gravity * -reverseGravityMultiplier * Time.fixedDeltaTime;
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
