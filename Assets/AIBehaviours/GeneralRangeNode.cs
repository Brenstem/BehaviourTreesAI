using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Conditions", "GeneralRangeNode")]
public class GeneralRangeNode : Action
{
    [Header("Node variables")]
    [SerializeField] float range;
    
    Transform generalTransform;
    Transform ownerTransform;

    public override void Construct()
    {

        //generalTransform = context.localData.Get<GameObject>("General").transform;
        ownerTransform = context.owner.transform;

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {

            if (Vector3.Distance(ownerTransform.position, context.localData.Get<GameObject>("General").transform.position) > range)
                NodeState = NodeStates.SUCCESS;
            else
                NodeState = NodeStates.FAILURE;
            return NodeState;

        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
