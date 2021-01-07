using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : BehaviourNode
{
    BlackboardScript blackboard;
    public override void Construct(BlackboardScript blackboard)
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