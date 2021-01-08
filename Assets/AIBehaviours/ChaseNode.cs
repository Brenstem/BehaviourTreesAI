using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : BehaviourNode
{
    Context blackboard;
    Transform playerTransform;
    NavMeshAgent agent;

    public override void Construct(Context blackboard)
    {
        this.blackboard = blackboard;
        playerTransform = blackboard.globalData.player.transform;
        agent = blackboard.localData.thisAI.GetComponent<NavMeshAgent>();
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
                return NodeStates.RUNNING;
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

public class ChaseNodeParameters
{
    public Transform target;
    public NavMeshAgent agent;

    public ChaseNodeParameters(Transform target, NavMeshAgent agent)
    {
        this.target = target;
        this.agent = agent;
    }
}