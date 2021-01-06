using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : BehaviourNode<RangeNodeParameters>
{
    private float range;
    private Transform target;
    private Transform origin;

    public override void Construct(RangeNodeParameters parameters)
    {
        range = parameters.range;
        target = parameters.target;
        origin = parameters.origin;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(origin.position, target.position);

            return distance <= range ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}

public class RangeNodeParameters
{
    public float range;
    public Transform target;
    public Transform origin;

    public RangeNodeParameters(float range, Transform target, Transform origin) 
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }
}