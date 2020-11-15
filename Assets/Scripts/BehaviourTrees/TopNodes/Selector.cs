using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> nodes = new List<Node>();

    public Selector(List<Node> nodes)
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
                    _nodeState = NodeStates.RUNNING;
                    break;
                case NodeStates.SUCCESS:
                    _nodeState = NodeStates.SUCCESS;
                    break;
                case NodeStates.FAILURE:
                    return _nodeState;
                default:
                    break;
            }
        }
        _nodeState = NodeStates.FAILURE;
        return _nodeState;
    }
}
