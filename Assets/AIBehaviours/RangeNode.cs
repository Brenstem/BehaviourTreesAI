using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : BehaviourNode
{
    Transform playerTransform;
    Transform myTransform;
    float aggroRange;

    public override void Construct(Context blackboard)
    {
        this.context = blackboard;
        playerTransform = blackboard.globalData.player.transform;
        myTransform = blackboard.localData.thisAI.transform;
        aggroRange = blackboard.nodeData.Get<float>("meme");

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(myTransform.position, playerTransform.position);

            return distance <= aggroRange ? NodeStates.SUCCESS : NodeStates.FAILURE;
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