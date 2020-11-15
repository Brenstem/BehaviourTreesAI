using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invertor : Node
{
    protected Node node;

    public Invertor(Node node)
    {
        this.node = node;
    }

    public override NodeStates Evaluate()
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
}
