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
        // float localCyclePos = cyclePos + cycleOffset;
        // localCyclePos += cycleOffset;
        // if (localCyclePos >= 1f) localCyclePos -= 1f;
        var nextIndex = NextNode(cyclePos);
        var previousIndex = PreviousNode(nextIndex);
        var previous = (CEllipticalMovementNode)nodes[previousIndex];
        var next = (CEllipticalMovementNode)nodes[nextIndex];

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
        var newAngle = Mathf.Lerp(previousAngle, nextAngle, fraction);
        
        var horizontalAxis = previous.Radius();
        var verticalAxis = next.Radius();
        
        var previousAngleNormalized = Mathf.Abs(previous.Angle());
        if (previousAngleNormalized < 135 && previousAngleNormalized > 45)
        {
            var temp = horizontalAxis;
            horizontalAxis = verticalAxis;
            verticalAxis = temp;
        }

        var angleRadians = Mathf.Deg2Rad * newAngle;
        var h = horizontalAxis * Mathf.Cos(angleRadians);
        var v = verticalAxis * Mathf.Sin(angleRadians);
        var destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

}
