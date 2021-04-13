using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddNodeMenu("Actions/Movement", "GoToPositionNode")]
public class GoToPositionNode : Action
{
    [SerializeField] bool setPositionManually;
    [SerializeField] Vector3 relativeTargetPosition;
    [SerializeField] string blackboardPositionName;

    Vector3 targetPosition;
    NavMeshAgent agent;

    public override void Construct()
    {
        if (setPositionManually)
            targetPosition = relativeTargetPosition + context.owner.transform.position;
        agent = context.owner.agent;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (!setPositionManually)
                targetPosition = context.localData.Get<Vector3>(blackboardPositionName) + relativeTargetPosition;

            targetPosition.y = context.owner.transform.position.y;

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
