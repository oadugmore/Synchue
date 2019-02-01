using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CCyclePathingObject : MonoBehaviour, CCycleObject
{
    protected List<CCycleNode> nodes;

    protected virtual void Start()
    {
        nodes = new List<CCycleNode>();
        GetComponentsInChildren<CCycleNode>(nodes);
    }

    public abstract void UpdateCyclePosition(float cyclePos);

    protected int NextNode(float cyclePos)
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
        return nextNode;
    }

}