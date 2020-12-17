using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invertor : DecoratorNode
{
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            switch (node.Evaluate())
            {
                case NodeStates.RUNNING:
                    _nodeState = NodeStates.RUNNING;
                    break;
                case NodeStates.SUCCESS:
                    _nodeState = NodeStates.FAILURE;
                    break;
                case NodeStates.FAILURE:
                    _nodeState = NodeStates.RUNNING;
                    break;
                default:
                    break;
            }

            return _nodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
