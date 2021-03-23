using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddNodeMenu("Actions/Movement", "PatrolNode")]
public class PatrolNode : Action
{
    [Header("Node variables")]
    [SerializeField] private float minPositionVariance;
    [SerializeField][Range(3, 50)] private float maxPositionVariance;

    private Transform target;
    private NavMeshAgent agent;
    private float originalAgentSpeed;

    public override void Construct()
    {
        _constructed = true;
        target = context.globalData.player.transform;
        agent = context.owner.GetComponent<NavMeshAgent>();
        originalAgentSpeed = agent.speed;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            agent.speed = agent.speed - 3;
            
            float xPositionVariance;
            float yPositionVariance;

            if (Random.Range(0, 1) == 1)
            {
                xPositionVariance = Random.Range(minPositionVariance, maxPositionVariance);
            }
            else
            {
                xPositionVariance = Random.Range(-minPositionVariance, - maxPositionVariance);
            }

            if (Random.Range(0, 1) == 1)
            {
                yPositionVariance = Random.Range(minPositionVariance, maxPositionVariance);
            }
            else
            {
                yPositionVariance = Random.Range(-minPositionVariance, -maxPositionVariance);
            }

            Vector3 targetPosition = new Vector3(target.position.x + xPositionVariance, 0, target.position.z + yPositionVariance);

            float distance = Vector3.Distance(targetPosition, agent.transform.position);

            if (distance >= agent.stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(targetPosition);
                NodeState = NodeStates.RUNNING;
                return NodeState;
            }
            else
            {
                agent.isStopped = true;
                agent.speed = originalAgentSpeed;
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
