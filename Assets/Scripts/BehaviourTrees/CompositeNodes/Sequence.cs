using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite
{
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            bool isAnyNodeRunning = false;

            foreach (AbstractNode node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.RUNNING:
                        isAnyNodeRunning = true;
                        break;
                    case NodeStates.SUCCESS:
                        break;
                    case NodeStates.FAILURE:
                        NodeState = NodeStates.FAILURE;
                        return NodeState;
                    default:
                        break;
                }
            }
            NodeState = isAnyNodeRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }

    protected override void CalculateEmotionalFactors()
    {
        float rVal;

        foreach (var node in nodes)
        {

        }
    }
}
