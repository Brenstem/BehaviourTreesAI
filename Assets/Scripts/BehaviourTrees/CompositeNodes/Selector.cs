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
        Debug.Log(NodeState);

        foreach (Node node in nodes)
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
}
