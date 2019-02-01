using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)]
    protected float targetCyclePosition;

    public float TargetCyclePosition()
    {
        return targetCyclePosition;
    }
}
