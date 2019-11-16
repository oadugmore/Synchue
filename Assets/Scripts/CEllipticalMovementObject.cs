using System.Collections.Generic;
using UnityEngine;

public class CEllipticalMovementObject : CCyclePathingObject
{
    public bool automaticCycleTime = false;

    private Rigidbody movementObject;
    private const int numTrapezoids = 10;

    protected override void Start()
    {
        base.Start();
        if (automaticCycleTime) CalculateCyclePositions();
        movementObject = GetComponentInChildren<Rigidbody>();
    }

    private void CalculateCyclePositions()
    {
        var totalDistance = 0f;
        var distances = new List<float>(nodes.Count - 1);
        //var i = 0;
        for (int i = 1; i < nodes.Count; ++i)
        {
            var node = nodes[i] as CEllipticalMovementNode;
            var previous = node.Previous() as CEllipticalMovementNode;
            //var previous = node.Previous() as CEllipticalMovementNode;
            var angle = Mathf.DeltaAngle(node.Angle(), previous.Angle());
            var a = node.Radius();
            var b = previous.Radius();
            var fi = Mathf.Atan(a / b * Mathf.Tan(angle));
            var k = 1 - Mathf.Pow(a / b, 2);
            var distance = b * TrapezoidEstimation_Ellipse(fi, k);
            totalDistance += distance;
            distances.Add(totalDistance);
        }
        // foreach (CEllipticalMovementNode node in nodes)
        // {
        //     //var node = nodeBase as CEllipticalMovementNode;
        //     Debug.Log("Node count: " + nodes.Count);
            
        //     //Debug.Log($"Distance: {distances[i]}");
        //     //totalDistance += distances[i];
        // }

        for (int i = 1; i < nodes.Count; ++i)
        {
            (nodes[i] as CEllipticalMovementNode).SetTargetCyclePosition(distances[i - 1] / totalDistance);
        }

    }

    // Computes the elliptic integral using numeric integration
    private float TrapezoidEstimation_Ellipse(float fi, float k)
    {
        float deltaX = fi / numTrapezoids;
        float xi = 0f;
        // first one will be 0 because xi = 0f
        float sum = EllipticIntegral(xi, k) / 2;
        xi += deltaX;
        for (int i = 0; i < numTrapezoids; ++i)
        {
            sum += EllipticIntegral(xi, k);
            xi += deltaX;
        }
        sum += EllipticIntegral(xi, k) / 2;
        return sum * deltaX;
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
