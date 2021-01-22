using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Action
{
    public override void Construct()
    {
        _constructed = true;
        Debug.Log("idle time baybeeee");
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            Debug.Log("IDLE NODE " /*+ Vector3.Distance(context.player.transform.position, context.owner.transform.position)*/);
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