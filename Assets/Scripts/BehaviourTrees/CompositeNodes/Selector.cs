using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Composite", "Selector")]
public class Selector : Composite
{
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (context.localData.Get<Action>("CurrentRunningNode") != null)
            {
                Action currentAction = context.localData.Get<Action>("CurrentRunningNode");

                switch (currentAction.Evaluate())
                {
                    case NodeStates.FAILURE:
                        context.localData.Set<Action>("CurrentRunningNode", null);
                        break;
                    case NodeStates.RUNNING:
                        NodeState = NodeStates.RUNNING;
                        break;
                    case NodeStates.SUCCESS:
                        context.localData.Set<Action>("CurrentRunningNode", null);
                        break;
                    default:
                        break;
                }
            }

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
