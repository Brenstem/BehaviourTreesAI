using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private Vector3 target;
    private Vector3 origin;

    public RangeNode(float range, Vector3 target, Vector3 origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeStates Evaluate()
    {
        Debug.Log("Range node");

        float distance = Vector3.Distance(origin, target);
        return distance <= range ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }
}
