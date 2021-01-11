using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Action
{
    Transform targetTransform;
    float range;

    public override void Construct(Context blackboard)
    {
        this.context = blackboard;
        //targetTransform = blackboard.globalData.player.transform;
        targetTransform = blackboard.player.transform;
        range = blackboard.range;

        _constructed = true;
    }

    public override void AddProperties(string[] names)
    {
        foreach (var name in names)
        {
            //range = context.nodeData.Get<float>(name);
        }
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