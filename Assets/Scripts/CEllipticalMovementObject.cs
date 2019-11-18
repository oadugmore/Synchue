using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CEllipticalMovementObject : CCyclePathingObject
{
    [ConditionalField("automaticCycleTime")]
    public int numTrapezoids = 20;

    private Rigidbody movementObject;

    protected override void Start()
    {
        base.Start();
        DrawEllipse();
        movementObject = GetComponentInChildren<Rigidbody>();
    }

    private void DrawEllipse()
    {
        var previousPoint = CalculatePosition(0f);
        for (float pos = 0f; pos <= 1.0f; pos += 0.05f)
        {
            var newPoint = CalculatePosition(pos);
            Debug.DrawLine(previousPoint, newPoint, Color.green, Mathf.Infinity);
            previousPoint = newPoint;
        }
    }

    protected override void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        for (int i = 0; i < nodes.Count; ++i)
        {
            var node = nodes[i] as CEllipticalMovementNode;
            var previous = node.Previous() as CEllipticalMovementNode;
            var a = previous.Radius();
            var b = node.Radius();
            if (a < b)
            {
                var temp = a;
                a = b;
                b = temp;
            }

            var theta1 = Mathf.Deg2Rad * previous.Angle();
            var theta2 = Mathf.Deg2Rad * node.Angle();
            while (theta2 < theta1 && !node.RotateClockwise())
            {
                theta2 += 2 * Mathf.PI;
            }
            while (theta2 > theta1 && node.RotateClockwise())
            {
                theta1 += 2 * Mathf.PI;
            }

            var k = 1 - Mathf.Pow(b / a, 2);
            var distance = a * Mathf.Abs(TrapezoidEstimation_Ellipse(theta1, theta2, k));
            totalDistance += distance;
            distances.Add(totalDistance);
        }

        for (int i = 0; i < nodes.Count; ++i)
        {
            (nodes[i] as CEllipticalMovementNode).SetTargetCyclePosition((distances[i] - distances[0]) / totalDistance);
        }
    }

    // Estimates the elliptic integral using numeric integration
    private float TrapezoidEstimation_Ellipse(float theta1, float theta2, float k)
    {
        float deltaX = (theta2 - theta1) / numTrapezoids;
        float xi = theta1;
        float sum = EllipticIntegral(xi, k) / 2;
        xi += deltaX;
        for (int i = 1; i < numTrapezoids; ++i)
        {
            sum += EllipticIntegral(xi, k);
            xi += deltaX;
        }
        sum += EllipticIntegral(xi, k) / 2;
        return sum * Mathf.Abs(deltaX);
    }

    // the incomplete elliptic integral of the second kind
    private float EllipticIntegral(float theta, float k)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(k, 2) * Mathf.Pow(Mathf.Sin(theta), 2));
    }

    public override void UpdateCyclePosition(float cyclePos)
    {
        var destination = CalculatePosition(cyclePos);
        movementObject.MovePosition(destination);
    }

    private Vector3 CalculatePosition(float cyclePos)
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
        return transform.TransformPoint(h, v, 0);
    }
}
