using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Composite", "Selector")]
public class Selector : Composite
{
    [SerializeField] private bool interruptable = true;

    int currentRunningNodeIndex = -1;

    //reset currentRunningNodeIndex every time we exit play mode
    private void OnDisable()
    {
        currentRunningNodeIndex = -1;
    }

    public override void Construct(List<AbstractNode> nodes)
    {
        base.Construct(nodes);

        if (interruptable)
            Action.InterruptEvent += Reset;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            CalculatePlanValue();
            CalculateRiskValue();
            CalculateTimeInterval();

            //if a node returned running last evaluate, we start evaluating from that node
            if (currentRunningNodeIndex > 0)
            {
                return EvaluateFromIndex(currentRunningNodeIndex);
            }
            else
            {
                return EvaluateFromIndex(0);
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
    private NodeStates EvaluateFromIndex(int startingIndex)
    {
        for (int i = startingIndex; i < nodes.Length; i++)
        {
            AbstractNode node = nodes[i];

            switch (node.Evaluate())
            {
                //if a node returns running set currentRunningNodeIndex to that nodes index in the child list
                case NodeStates.RUNNING:
                    currentRunningNodeIndex = i;
                    NodeState = NodeStates.RUNNING;
                    return NodeState;

                //if a node returns success reset currentRunningNodeIndex
                case NodeStates.SUCCESS:
                    currentRunningNodeIndex = -1;
                    NodeState = NodeStates.SUCCESS;
                    return NodeState;

                case NodeStates.FAILURE:
                    break;
            }
        }
        //if all nodes returns failure reset currentRunningNodeIndex
        currentRunningNodeIndex = -1;
        NodeState = NodeStates.FAILURE;
        return NodeState;
    }
    private void Reset(InterruptEventArgs args)
    {
        if (args.id == context.id)
        {
            currentRunningNodeIndex = -1;
        }
    }
    protected override void CalculatePlanValue()
    {
        planValue = 0;

        foreach (AbstractNode node in nodes)
        {
            planValue += node.GetPlanValue();
        }
        planValue = planValue / nodes.Length;
    }

    protected override void CalculateRiskValue()
    {
        riskValue = 0;

        foreach (AbstractNode node in nodes)
        {
            riskValue += node.GetRiskValue();
        }
        riskValue = riskValue / nodes.Length;
    }

    protected override void CalculateTimeInterval()
    {
        minTimeValue = nodes[0].GetMinTimeValue(); 
        maxTimeValue = 0;

        foreach (AbstractNode node in nodes)
        {
            if (minTimeValue > node.GetMinTimeValue())
                minTimeValue = node.GetMinTimeValue();
            
            if (maxTimeValue < node.GetMaxTimeValue())
                maxTimeValue = node.GetMaxTimeValue();
        }
    }
}
