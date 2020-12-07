using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BehaviourNode
{
    protected List<BehaviourNode> nodes = new List<BehaviourNode>();

    public Sequence(List<BehaviourNode> nodes)
    {
        this.nodes = nodes;
    }
    //public Sequence() { }

    //public override void Initialize()
    //{
    //    throw new System.NotImplementedException();
    //}

    public override NodeStates Evaluate()
    {
        bool isAnyNodeRunning = false;

        foreach (BehaviourNode node in nodes)
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
