using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekCoverNode : Action
{
    [SerializeField] float coverVsisionRadius;
    [SerializeField] LayerMask coverLayerMask;
    [SerializeField] LayerMask visibilityLayerMask;

    Transform playerTransform;
    Transform ownerTransform;
    NavMeshAgent agent;

    public override void Construct()
    {
        playerTransform = context.globalData.player.transform;
        ownerTransform = context.owner.transform;
        agent = context.owner.agent;
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            Collider[] coverPositions = Physics.OverlapSphere(ownerTransform.position, coverVsisionRadius, coverLayerMask);
            if (coverPositions.Length > 0)
            {
                Vector3 currentBestCoverPosition = Vector3.positiveInfinity;

                foreach (Collider cover in coverPositions)
                {
                    RaycastHit hit;
                    Physics.Raycast(cover.transform.position, playerTransform.position - cover.transform.position, out hit, Mathf.Infinity, visibilityLayerMask);

                    if (hit.collider.transform != playerTransform)
                    {
                        if (Vector3.Distance(ownerTransform.position, currentBestCoverPosition) > Vector3.Distance(ownerTransform.position, cover.transform.position))
                        {
                            currentBestCoverPosition = cover.transform.position;
                        }
                    }
                }
                if (currentBestCoverPosition.magnitude < Vector3.positiveInfinity.magnitude)
                {
                    agent.SetDestination(currentBestCoverPosition);

                    //TODO fixa så detta är 2 noder, där en är go to position eller ngt idk 

                    //context.localData.Set<>

                    NodeState = NodeStates.SUCCESS;
                    return NodeState;
                }
                else
                {
                    NodeState = NodeStates.FAILURE;
                    Debug.Log("no good cover available");
                    return NodeState;
                }
            }
            else
            {
                NodeState = NodeStates.FAILURE;
                Debug.Log("no cover in sight");
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
