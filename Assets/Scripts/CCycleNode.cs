using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    protected float m_targetCyclePosition;
    public float targetCyclePosition { get => m_targetCyclePosition; set => m_targetCyclePosition = Mathf.Clamp(value, 0f, 1f); }
    [SerializeField]
    protected float m_weight;
    public float weight { get => m_weight; set => m_weight = value; }

    public CCycleNode previous { get; set; }
}
