using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeeNode : Action
{
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float range;

    Transform target;

    public override void Construct()
    {
        target = context.player.transform;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            RaycastHit hit;

            if (Physics.Raycast(context.owner.transform.position, target.position - context.owner.transform.position, out hit, range, targetLayers))
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
