using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : Action
{
    [SerializeField] private string debugText;
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
            Debug.Log(debugText + ownerName);

            NodeState = NodeStates.SUCCESS;
            return NodeState;
        }
        else
        {
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
    }
}
