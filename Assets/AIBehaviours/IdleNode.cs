using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Action
{
    string ownerName;
    public override void Construct()
    {
        ownerName = context.owner.name;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //Debug.Log("idle" + ownerName);
            NodeState = NodeStates.SUCCESS;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.FAILURE; 
            return NodeState;
        }
    }
}