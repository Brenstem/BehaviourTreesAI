using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Conditions", "SetConditionNode")]
public class SetConditionNode : Action
{
    [Header("Node variables")]
    [SerializeField] private string conditionToSet;
    [SerializeField] private bool value;

    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (context.localData.Set<bool>(conditionToSet, value))
            {
                NodeState = NodeStates.SUCCESS;
            }
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
