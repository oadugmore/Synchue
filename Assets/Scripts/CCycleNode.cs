using UnityEngine;

public abstract class CCycleNode : MonoBehaviour
{
    // protected float m_targetCyclePosition;
    // [HideInInspector]
    public float targetCyclePosition;
    
    [SerializeField]
    protected float m_weight = 1;
    public float weight { get => m_weight; set => m_weight = value; }

    [SerializeField]
    private Vector3 m_localRotationHint;

    public CCycleNode previous { get; set; }

    private void OnValidate()
    {
        transform.localRotation = Quaternion.Euler(m_localRotationHint);
    }
}
