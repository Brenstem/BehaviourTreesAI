using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : Action
{ 
    public override void Construct(Context blackboard)
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
