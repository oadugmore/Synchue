using UnityEngine;

[System.Obsolete("Use CEllipticalMovementObject instead.", true)]
public class CSinusoidalMovementObject : CCyclePathingObject
{

    private Rigidbody movementObject;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        var next = (CSinusoidalMovementNode)NextNode(cyclePos);
        var previous = (CSinusoidalMovementNode)next.previous;

        var nextCyclePos = next.targetCyclePosition;
        var previousCyclePos = previous.targetCyclePosition;
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
        var newRadius = Mathf.Lerp(previous.Radius(), next.Radius(), fraction);

        var angleRadians = Mathf.Deg2Rad * newAngle;
        var h = Mathf.Cos(angleRadians) * newRadius;
        var v = Mathf.Sin(angleRadians) * newRadius;
        var destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

    [System.Obsolete]
    protected override void CalculateCyclePositions()
    {
        throw new System.NotImplementedException();
    }
}
