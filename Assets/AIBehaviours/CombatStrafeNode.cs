using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatStrafeNode : Action
{
    [Header("Node variables")]
    [SerializeField] float strafeDistance = 1;

    NavMeshAgent ownerAgent;
    Transform ownerTransform;

    float targetPosition;

    public override void Construct()
    {
        ownerAgent = context.owner.GetComponent<NavMeshAgent>();
        ownerTransform = context.owner.transform;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {

            context.localData.Set<Vector3>("positionToGoTo", ownerTransform.position + Vector3.right * strafeDistance);

            NodeState = NodeStates.SUCCESS;
            return NodeState;

        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
