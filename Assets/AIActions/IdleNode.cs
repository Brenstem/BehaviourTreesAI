using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Action
{
    Context blackboard;
    public override void Construct(Context blackboard)
    {
        this.blackboard = blackboard;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            return NodeStates.SUCCESS;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}

public class IdleNodeParameters
{

}