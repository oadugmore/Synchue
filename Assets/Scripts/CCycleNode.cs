using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    // protected float m_targetCyclePosition;
    [HideInInspector]
    public float targetCyclePosition;
    
    [SerializeField]
    protected float m_weight = 1;
    public float weight { get => m_weight; set => m_weight = value; }

    public CCycleNode previous { get; set; }
}
