using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Conditions", "CanSeeNode")]
public class CanSeeNode : Action
{
    [Header("Node variables")]
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float range;

    Transform target;
    Transform ownerTransform;

    public override void Construct()
    {
        target = context.globalData.player.transform;
        ownerTransform = context.owner.transform;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            RaycastHit hit;

            if (Physics.Raycast(ownerTransform.position, target.position - ownerTransform.position, out hit, range, targetLayers))
            {
                if (hit.collider.gameObject.CompareTag(target.gameObject.tag))
                {
                    NodeState = NodeStates.SUCCESS;
                    return NodeState;
                }
            }
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
    }
}
