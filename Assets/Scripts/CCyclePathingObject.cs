using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class CCyclePathingObject : MonoBehaviour, ICCycleObject
{
    [SerializeField]
    protected bool automaticCycleTime = false;
    protected List<CCycleNode> nodes;

    protected virtual void Start()
    {
        nodes = new List<CCycleNode>();
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
        if (automaticCycleTime)
        {
            CalculateCyclePositions();
        }
    }

    public abstract void UpdateCyclePosition(float cyclePos);

    protected abstract void CalculateCyclePositions();

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