using UnityEngine;

public class CRotationNode : CCycleNode
{
    [SerializeField]
    private bool m_rotateClockwise;
    public bool rotateClockwise => m_rotateClockwise;

    public Quaternion rotation => transform.rotation;

}
