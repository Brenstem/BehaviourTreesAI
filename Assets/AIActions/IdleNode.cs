using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Action
{
    public override void Construct(Context blackboard)
    {
        context = blackboard;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            Debug.Log("NOT MEME!!!" + context.player/*.transform.position*/);
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