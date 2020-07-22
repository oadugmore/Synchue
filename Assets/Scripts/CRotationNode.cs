using UnityEngine;

public class CRotationNode : CCycleNode
{
    public Quaternion rotation => transform.rotation;
}
