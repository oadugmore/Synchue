using UnityEngine;
using UnityEngine.Assertions;

public class CSinusoidalMovementObject : CCyclePathingObject
{
    //public bool rotateClockwise = false;
    // [SerializeField][Range(0f, 1f)]
    // private float cycleOffset = 0f;

    private Rigidbody movementObject;
    //private float radius;

    protected override void Start()
    {
        base.Start();
        movementObject = GetComponentInChildren<Rigidbody>();
        //radius = Vector3.Distance(transform.position, movementObject.position);
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        // float localCyclePos = cyclePos + cycleOffset;
        // localCyclePos += cycleOffset;
        // if (localCyclePos >= 1f) localCyclePos -= 1f;
        var nextIndex = NextNode(cyclePos);
        var previousIndex = PreviousNode(nextIndex);
        var previous = (CSinusoidalMovementNode)nodes[previousIndex];
        var next = (CSinusoidalMovementNode)nodes[nextIndex];

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
        var newRadius = Mathf.Lerp(previous.Radius(), next.Radius(), fraction);

        var angleRadians = Mathf.Deg2Rad * newAngle;
        var h = Mathf.Cos(angleRadians) * newRadius;
        var v = Mathf.Sin(angleRadians) * newRadius;
        var destination = transform.TransformPoint(h, v, 0);
        movementObject.MovePosition(destination);
    }

}
