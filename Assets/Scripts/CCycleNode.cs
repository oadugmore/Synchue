using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    protected float m_targetCyclePosition;
    public float targetCyclePosition { get => m_targetCyclePosition; set => m_targetCyclePosition = Mathf.Clamp(value, 0f, 1f); }

    public CCycleNode previous { get; set; }

}
