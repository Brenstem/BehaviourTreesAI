using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Decorators", "Invertor")]
public class Invertor : Decorator
{
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            switch (node.Evaluate())
            {
                case NodeStates.RUNNING:
                    NodeState = NodeStates.RUNNING;
                    break;
                case NodeStates.SUCCESS:
                    NodeState = NodeStates.FAILURE;
                    break;
                case NodeStates.FAILURE:
                    NodeState = NodeStates.SUCCESS;
                    break;
                default:
                    break;
            }

            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
