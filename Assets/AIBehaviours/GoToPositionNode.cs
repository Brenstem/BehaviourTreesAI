using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToPositionNode : Action
{
    [SerializeField] Vector2 relativeTargetPosition;

    Vector3 targetPosition;
    NavMeshAgent agent;

    public override void Construct()
    {
        targetPosition = (Vector3)relativeTargetPosition + context.owner.transform.position;
        _constructed = true;
        agent = context.owner.agent;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(targetPosition, agent.transform.position);
            if (distance >= agent.stoppingDistance)
            {
                Debug.Log(distance);
                agent.isStopped = false;
                agent.SetDestination(targetPosition);
                NodeState = NodeStates.RUNNING;
                return NodeState;
            }
            else
            {
                agent.isStopped = true;
                NodeState = NodeStates.SUCCESS;
                return NodeState;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
