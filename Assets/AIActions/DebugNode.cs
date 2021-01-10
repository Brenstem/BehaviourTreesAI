using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : Action
{
    public override void Construct(Context blackboard)
    {
        Debug.Log("WTF");
        context = blackboard;

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            Debug.Log("MEME!");

            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }
}
