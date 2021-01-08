using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : CompositeNode
{
    public Context blackboard { get; set; }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            foreach (AbstractNode node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStates.RUNNING:
                        _nodeState = NodeStates.RUNNING;
                        return _nodeState;
                    case NodeStates.SUCCESS:
                        _nodeState = NodeStates.SUCCESS;
                        return _nodeState;
                    case NodeStates.FAILURE:
                        break;
                    default:
                        break;
                }
            }
            _nodeState = NodeStates.FAILURE;
            return _nodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
