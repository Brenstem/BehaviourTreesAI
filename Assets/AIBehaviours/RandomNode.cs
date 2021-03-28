using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Conditions", "RandomNode")]
public class RandomNode : Action
{
    [Header("Node variables")]
    [SerializeField] float chanceOfSuccess;

    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (Random.Range(0f, 1f) < chanceOfSuccess)
                NodeState = NodeStates.SUCCESS;
            else
                NodeState = NodeStates.FAILURE;

            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
