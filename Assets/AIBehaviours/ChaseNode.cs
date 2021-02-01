using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Action
{
    Transform playerTransform;
    NavMeshAgent agent;

    public override void Construct()
    {
        playerTransform = context.globalData.player.transform;
        agent = context.owner.agent;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(playerTransform.position, agent.transform.position);

            if (distance >= agent.stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTransform.position);
                return NodeStates.SUCCESS;
            }
            else
            {
                agent.isStopped = true;
                return NodeStates.SUCCESS;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}