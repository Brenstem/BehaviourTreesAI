using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeStates Evaluate()
    {
        bool isAnyNodeRunning = false;

        foreach (Node node in nodes)
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
}
