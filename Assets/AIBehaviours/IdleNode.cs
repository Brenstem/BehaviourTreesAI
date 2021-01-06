using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : BehaviourNode<IdleNodeParameters>
{
    public override void Construct(IdleNodeParameters parameters) 
    { 
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