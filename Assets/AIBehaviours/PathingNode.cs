using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingNode : Action
{
    [SerializeField] Vector3[] relativeTargetPosition;

    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //Write your behaviour code here!

            throw new System.NotImplementedException();
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
