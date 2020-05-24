using UnityEngine;

public class EditorHelper
{
    public static void ManualPhysicsStep()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }
}