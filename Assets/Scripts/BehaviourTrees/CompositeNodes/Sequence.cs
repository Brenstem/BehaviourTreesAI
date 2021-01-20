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
                        _nodeState = NodeStates.FAILURE;
                        return _nodeState;
                    default:
                        break;
                }
            }
            _nodeState = isAnyNodeRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
            return _nodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
