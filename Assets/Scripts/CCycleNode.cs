using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    // protected float m_targetCyclePosition;
    [Range(0f, 1f)]
    public float targetCyclePosition;
    [SerializeField]
    protected float m_weight;
    public float weight { get => m_weight; set => m_weight = value; }

    public CCycleNode previous { get; set; }
}
