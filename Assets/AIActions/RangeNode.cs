using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Action
{
    Transform targetTransform;
    [SerializeField] float range;

    public override void Construct()
    {
        //targetTransform = blackboard.globalData.player.transform;
        targetTransform = context.player.transform;
        range = context.range;

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
        //Debug.Log("meme");

        if (_constructed)
        {
            float distance = Vector3.Distance(context.owner.transform.position, targetTransform.position);

            Debug.Log(distance);

            return distance <= range ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}