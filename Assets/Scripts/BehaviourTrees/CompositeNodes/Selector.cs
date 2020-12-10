using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BehaviourNode
{
    protected List<BehaviourNode> nodes = new List<BehaviourNode>();

    public Selector(List<BehaviourNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeStates Evaluate()
    {
        foreach (BehaviourNode node in nodes)
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
