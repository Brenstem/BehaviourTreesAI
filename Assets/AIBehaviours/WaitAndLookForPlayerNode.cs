using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddNodeMenu("Actions/Movement", "WaitAndLookForPlayerNode")]
public class WaitAndLookForPlayerNode : Action
{
    [Header("Node variables")]
    [SerializeField] private float runTime = 0.5f;
    [SerializeField] LayerMask visionLayers;
    [SerializeField] float visionRange;

    private Timer timer;

    NavMeshAgent agent;
    Transform ownerTransform;
    Transform playerTransform;
    string targetTag;
    RaycastHit hit;

    public override void Construct()
    {
        _constructed = true;
        timer = new Timer(-1f);
        agent = context.owner.agent;
        ownerTransform = context.owner.transform;
        playerTransform = context.globalData.player.transform;
        targetTag = context.globalData.player.tag;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //ingen timer körs, skapa en ny timer som börjar köras
            if (timer.Done)
            {
                timer.Reset(runTime);

                agent.SetDestination(ownerTransform.position);

                NodeState = NodeStates.RUNNING;
                return NodeState;
            }

            timer.DecrementTimer(Time.deltaTime);

            //timern som kördes är nu klar
            if (timer.Done)
            {
                Debug.Log("TIMER DONE");
                NodeState = NodeStates.SUCCESS;
                return NodeState;
            }
            //NPCn kan någonting
            else if (Physics.Raycast(ownerTransform.position, playerTransform.position - ownerTransform.position, out hit, visionRange, visionLayers))
            {
                //NPCn kan se spelaren
                if (hit.collider.gameObject.CompareTag(targetTag))
                {
                    Debug.Log("I SEE THE ENEMY!!");
                    NodeState = NodeStates.SUCCESS;
                    return NodeState;
                }
                //NPCn kan inte se spelaren
                else
                {
                    NodeState = NodeStates.RUNNING;
                    return NodeState;
                }
            }
            //timern som körs är inte än klar och NPCn kan inte se spelaren 
            else
            {
                NodeState = NodeStates.RUNNING;
                return NodeState;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
    }
}
