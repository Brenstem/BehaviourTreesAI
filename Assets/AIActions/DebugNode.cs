using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : Action
{
    [SerializeField] private string debugText;

    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            Debug.Log(debugText);

            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }
}
