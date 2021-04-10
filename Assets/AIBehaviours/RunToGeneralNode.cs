using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddNodeMenu("Actions/Movement", "RunToGeneralNode")]
public class RunToGeneralNode : Action
{
    [Header("Node variables")]
    [SerializeField] float stoppingDistance;


    Transform generalTransform;
    Transform ownerTransform;
    NavMeshAgent agent;

    public override void Construct()
    {
        //generalTransform = context.localData.Get<GameObject>("General").transform;
        ownerTransform = context.owner.transform;
        agent = context.owner.agent;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (Vector3.Distance(ownerTransform.position, context.localData.Get<GameObject>("General").transform.position) > stoppingDistance)
            {
                Debug.Log(context.localData.Get<GameObject>("General").transform.position);
                agent.isStopped = false;
                agent.SetDestination(context.localData.Get<GameObject>("General").transform.position);
                NodeState = NodeStates.RUNNING;
            }
            else
            {
                agent.isStopped = true;
                agent.SetDestination(ownerTransform.position);
                NodeState = NodeStates.SUCCESS;
            }
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
