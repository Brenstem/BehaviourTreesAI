using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite
{
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            foreach (AbstractNode node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.RUNNING:
                        NodeState = NodeStates.RUNNING;
                        return NodeState;
                    case NodeStates.SUCCESS:
                        NodeState = NodeStates.SUCCESS;
                        return NodeState;
                    case NodeStates.FAILURE:
                        break;
                    default:
                        break;
                }
            }
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed! " + this.name);
            return NodeStates.FAILURE;
        }
    }

    protected override void CalculatePlanValue()
    {
        foreach (AbstractNode node in nodes)
        {
            planValue += node.GetPlanValue();
        }
        planValue = planValue / nodes.Count;
    }

    protected override void CalculateRiskValue()
    {
        foreach (AbstractNode node in nodes)
        {
            riskValue += node.GetRiskValue();
        }
        riskValue = riskValue / nodes.Count;
    }

    protected override void CalculateTimeInterval()
    {
        foreach (AbstractNode node in nodes)
        {
            if (minTimeValue > node.GetMinTimeValue())
                minTimeValue = node.GetMinTimeValue();
            
            if (maxTimeValue < node.GetMaxTimeValue())
                maxTimeValue = node.GetMaxTimeValue();
        }
    }
}
