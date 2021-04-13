using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Composite", "Concurrent")]
public class Concurrent : Composite
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

                case NodeStates.SUCCESS:
                    break;

                //if a node returns failure reset currentRunningNodeIndex
                case NodeStates.FAILURE:
                    currentRunningNodeIndex = -1;
                    NodeState = NodeStates.FAILURE;
                    return NodeState;
            }
        }
        //if all nodes returns success reset currentRunningNodeIndex
        currentRunningNodeIndex = -1;
        NodeState = NodeStates.SUCCESS;
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
        foreach (AbstractNode node in nodes)
        {
            planValue += node.GetPlanValue();
        }
    }
    protected override void CalculateRiskValue()
    {
        float risk = 1;
        foreach (AbstractNode node in nodes)
        {
            risk *= (1 - node.GetRiskValue());
        }
        riskValue = 1 - risk;
    }
    protected override void CalculateTimeInterval()
    {
        foreach (AbstractNode node in nodes)
        {
            minTimeValue += node.GetMinTimeValue();
            maxTimeValue += node.GetMaxTimeValue();
        }
    }
}