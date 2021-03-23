using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Conditions", "ConditionCheckNode")]
public class ConditionCheckNode : Action
{
    [Header("Node variables")]
    [SerializeField] private string conditionToCheck;
    [SerializeField] private bool desiredValue;
    [SerializeField] private bool reset;

    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (context.localData.Get<bool>(conditionToCheck) == desiredValue)
            {
                if (reset)
                {
                    context.localData.Set<bool>(conditionToCheck, !context.localData.Get<bool>(conditionToCheck));
                }
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
