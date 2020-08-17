using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    public float targetCyclePosition;
    
    [SerializeField, Min(0.001f)]
    protected float m_weight = 1;
    public float weight { get => m_weight; set => m_weight = value; }

    public CCycleNode previous { get; set; }
}
