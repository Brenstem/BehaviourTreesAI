using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Action
{
    [SerializeField] float range;

    private Transform targetTransform;

    public override void Construct()
    {
        targetTransform = context.player.transform;

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(context.owner.transform.position, targetTransform.position);

            NodeState = distance <= range ? NodeStates.SUCCESS : NodeStates.FAILURE;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}