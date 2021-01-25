using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcurrentNode : Composite
{
    int runningNodeIndex = -1;

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //if a node returned running last evaluate, we start evaluating from that node
            if (runningNodeIndex >= 0)
            {
                return EvaluateFromIndex(runningNodeIndex);
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
        for (int i = startingIndex; i < nodes.Count; i++)
        {
            AbstractNode node = nodes[i];

            switch (node.Evaluate())
            {
                case NodeStates.RUNNING:
                    runningNodeIndex = i;
                    NodeState = NodeStates.RUNNING;
                    return NodeState;

                case NodeStates.SUCCESS:
                    break;

                case NodeStates.FAILURE:
                    runningNodeIndex = -1;
                    NodeState = NodeStates.FAILURE;
                    return NodeState;

                default:
                    break;
            }
        }
        runningNodeIndex = -1;
        NodeState = NodeStates.SUCCESS;
        return NodeState;
    }
}