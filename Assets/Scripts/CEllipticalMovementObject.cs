using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CEllipticalMovementObject : CCyclePathingObject
{
    public bool automaticCycleTime = false;
    [ConditionalField("automaticCycleTime")]
    public int numTrapezoids = 20;

    private Rigidbody movementObject;

    protected override void Start()
    {
        base.Start();
        if (automaticCycleTime) CalculateCyclePositions();
        movementObject = GetComponentInChildren<Rigidbody>();
    }

    private void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count);
        //var i = 0;
        for (int i = 0; i < nodes.Count; ++i)
        {
            var node = nodes[i] as CEllipticalMovementNode;
            var previous = node.Previous() as CEllipticalMovementNode;
            //var previous = node.Previous() as CEllipticalMovementNode;
            //var angle = Mathf.Deg2Rad * Mathf.DeltaAngle(node.Angle(), previous.Angle());
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

            //var fi = Mathf.Atan(a / b * Mathf.Tan(angle));
            var k = 1 - Mathf.Pow(b / a, 2);
            // partial integrals
            //var distance = a * Mathf.Abs(TrapezoidEstimation_Ellipse(theta2, k) - TrapezoidEstimation_Ellipse(theta1, k));
            var distance = a * Mathf.Abs(TrapezoidEstimation_Ellipse(theta1, theta2, k));
            totalDistance += distance;
            distances.Add(totalDistance);
            //distances[i] = (totalDistance - distances[0]);
            //distances[i] -= distances[0];
        }
        // var node = nodes[0] as CEllipticalMovementNode;
        // var previous = node.Previous() as CEllipticalMovementNode;
        // totalDistance += 
        // foreach (CEllipticalMovementNode node in nodes)
        // {
        //     //var node = nodeBase as CEllipticalMovementNode;
        //     Debug.Log("Node count: " + nodes.Count);

        //     //Debug.Log($"Distance: {distances[i]}");
        //     //totalDistance += distances[i];
        // }

        for (int i = 0; i < nodes.Count; ++i)
        {
            (nodes[i] as CEllipticalMovementNode).SetTargetCyclePosition((distances[i] - distances[0]) / totalDistance);
        }

    }

    // Computes the elliptic integral using numeric integration
    private float TrapezoidEstimation_Ellipse(float theta1, float theta2, float k)
    {
        //float deltaX = fi / numTrapezoids;
        float deltaX = (theta2 - theta1) / numTrapezoids;
        float xi = theta1;
        // first one will be 0 because xi = 0f
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

    private float EllipticIntegral(float theta, float k)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(k, 2) * Mathf.Pow(Mathf.Sin(theta), 2));
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
