using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private float stoppingDistance;

    public ChaseNode(Transform target, NavMeshAgent agent)
    {
        this.target = target;
        this.agent = agent;
    }

    public override NodeStates Evaluate()
    {
        float distance = Vector3.Distance(target.position, agent.transform.position);

        if (distance >= agent.stoppingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeStates.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeStates.SUCCESS;
        }
    }
}