using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class CCyclePathingObject : MonoBehaviour, ICCycleObject
{
    public List<CCycleNode> nodes = new List<CCycleNode>();

    [SerializeField]
    private float m_offset;
    public float offset { get => m_offset; set => m_offset = Mathf.Clamp01(value); }

    protected virtual void Start()
    {
        UpdateNodes();
    }

    void OnValidate()
    {
        m_offset = Mathf.Clamp01(m_offset);
    }

    public void UpdateNodes()
    {
        GetComponentsInChildren<CCycleNode>(nodes);
        if (nodes.Count < 1)
        {
            Debug.LogError(this + " needs at least one node.");
            return;
        }

        nodes[0].previous = nodes[nodes.Count - 1];
        for (int i = 1; i < nodes.Count; i++)
        {
            nodes[i].previous = nodes[i - 1];
        }

        var totalWeight = 0f;
        foreach (var node in nodes)
        {
            totalWeight += 1f / node.weight;
        }
        var currentWeight = 0f;
        for (int i = 1; i < nodes.Count; i++)
        {
            currentWeight += 1f / nodes[i].weight / totalWeight;
            nodes[i].targetCyclePosition = currentWeight;
        }
        nodes[0].targetCyclePosition = 0;
    }

    public abstract void UpdateCyclePosition(float cyclePos);

    protected CCycleNode NextNode(float cyclePos)
    {
        int nextNode = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].targetCyclePosition > cyclePos)
            {
                nextNode = i;
                break;
            }
        }
        return nodes[nextNode];
    }
}
