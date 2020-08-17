using UnityEngine;

public class CRotationNode : CCycleNode
{
    public Vector3 localRotationHint;

    private void OnValidate()
    {
        transform.localRotation = Quaternion.Euler(localRotationHint);
    }
}
