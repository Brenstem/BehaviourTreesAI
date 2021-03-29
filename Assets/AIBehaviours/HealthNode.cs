using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Conditions", "HealthNode")]
public class HealthNode : Action
{
    [SerializeField] float percentageThreshold;
    private Health aiHealth;

    public override void Construct()
    {
        aiHealth = context.owner.GetComponent<Health>();

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            NodeState = aiHealth.percentageHealth <= percentageThreshold ? NodeStates.SUCCESS : NodeStates.FAILURE;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
