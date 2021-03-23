using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddNodeMenu("Actions/Movement", "ChaseNode")]
public class ChaseNode : Action
{
    [Header("Node variables")]
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float visionRange;



    NavMeshAgent agent;
    Transform playerTransform;
    Transform ownerTransform;

    public override void Construct()
    {
        ownerTransform = context.owner.transform;
        playerTransform = context.globalData.player.transform;
        agent = context.owner.agent;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            float distance = Vector3.Distance(context.localData.Get<Vector3>("LastKnownPlayerPosition"), agent.transform.position);

            if (distance >= agent.stoppingDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(ownerTransform.position, playerTransform.position - ownerTransform.position, out hit, visionRange, targetLayers))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        //såg spelaren
                        agent.isStopped = true;
                        agent.SetDestination(ownerTransform.position);
                        NodeState = NodeStates.SUCCESS;
                    }
                    else
                    {
                        agent.isStopped = false;
                        agent.SetDestination(context.localData.Get<Vector3>("LastKnownPlayerPosition"));
                        NodeState = NodeStates.RUNNING;
                    }
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(context.localData.Get<Vector3>("LastKnownPlayerPosition"));
                    NodeState = NodeStates.RUNNING;
                }
            }
            else
            {
                //kom fram utan att se spelaren
                agent.isStopped = true;
                context.localData.Set<bool>("Aggroed", false);
                NodeState = NodeStates.FAILURE;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.SUCCESS;
        }

        return NodeState;
    }
}