using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    protected float targetCyclePosition;

    private CCycleNode previous;

    public float TargetCyclePosition()
    {
        return targetCyclePosition;
    }

    public void SetPrevious(CCycleNode previous)
    {
        this.previous = previous;
    }

    public CCycleNode Previous()
    {
        return previous;
    }
}
