using UnityEngine;

public class CEllipticalMovementObject : CCyclePathingObject
{
    private Rigidbody movementObject;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        var next = (CEllipticalMovementNode)NextNode(cyclePos);
        var previous = (CEllipticalMovementNode)next.Previous();

        var nextCyclePos = next.TargetCyclePosition();
        var previousCyclePos = previous.TargetCyclePosition();
        var nextAngle = next.Angle();
        var previousAngle = previous.Angle();
        while (nextAngle < previousAngle && !next.RotateClockwise())
        {
            nextAngle += 360f;
        }

        while (nextAngle > previousAngle && next.RotateClockwise())
        {
            previousAngle += 360f;
        }

        while (nextCyclePos < previousCyclePos)
        {
            nextCyclePos++;
        }

        var fraction = Mathf.Abs(cyclePos - previousCyclePos) / (nextCyclePos - previousCyclePos);
        var newAngle = Mathf.Deg2Rad * Mathf.Lerp(previousAngle, nextAngle, fraction);
        var horizontalAxis = previous.Radius();
        var verticalAxis = next.Radius();
        var previousAngleNormalized = Mathf.Abs(previous.Angle());
        
        if (previousAngleNormalized < 135 && previousAngleNormalized > 45)
        {
            var temp = horizontalAxis;
            horizontalAxis = verticalAxis;
            verticalAxis = temp;
        }

        var h = horizontalAxis * Mathf.Cos(newAngle);
        var v = verticalAxis * Mathf.Sin(newAngle);
        var destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

}
