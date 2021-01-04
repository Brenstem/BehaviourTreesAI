using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : BehaviourNode<ChaseNodeParameters>
{
    private Transform target;
    private NavMeshAgent agent;

    public override void Construct(ChaseNodeParameters parameters)
    {
        target = parameters.target;
        agent = parameters.agent;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
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