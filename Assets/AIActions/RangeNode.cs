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

            return distance <= range ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}