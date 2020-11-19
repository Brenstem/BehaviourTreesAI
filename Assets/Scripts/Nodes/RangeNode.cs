using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : BehaviourNode
{
    private float range;
    private Transform target;
    private Transform origin;

    public RangeNode(float range, Transform target, Transform origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeStates Evaluate()
    {
        float distance = Vector3.Distance(origin.position, target.position);

        return distance <= range ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }
}
