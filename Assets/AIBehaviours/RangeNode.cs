using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Action
{
    [SerializeField] float range;

    private Transform targetTransform;
    private Transform ownerTransform;

    public override void Construct()
    {
        targetTransform = context.player.transform;
        ownerTransform = context.owner.transform;

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(ownerTransform.position, targetTransform.position);

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