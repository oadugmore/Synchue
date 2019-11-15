using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CCyclePathingObject : MonoBehaviour, ICCycleObject
{
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

        nodes[0].SetPrevious(nodes[nodes.Count - 1]);
        for (int i = 1; i < nodes.Count; i++)
        {
            nodes[i].SetPrevious(nodes[i - 1]);
        }
    }

    public abstract void UpdateCyclePosition(float cyclePos);

    protected CCycleNode NextNode(float cyclePos)
    {
        int nextNode = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].TargetCyclePosition() > cyclePos)
            {
                nextNode = i;
                break;
            }
        }
        return nodes[nextNode];
    }

}