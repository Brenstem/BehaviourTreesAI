using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddNodeMenu("Actions/Movement", "WaitAndRotateNode")]
public class WaitAndRotateNode : Action
{
    private Timer timer;
    [SerializeField] private float runTime = 0.5f;
    [SerializeField] private float rotationSpeed = 0.5f;

    NavMeshAgent agent;
    Transform ownerTransform;
    Transform playerTransform;

    public override void Construct()
    {
        _constructed = true;
        timer = new Timer(-1f);
        agent = context.owner.agent;
        ownerTransform = context.owner.transform;
        playerTransform = context.globalData.player.transform;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //TODO fixa rotationen
            Quaternion lookAtRotation = Quaternion.LookRotation(playerTransform.position - ownerTransform.position);

            ownerTransform.rotation = Quaternion.Lerp(ownerTransform.rotation, lookAtRotation, Time.deltaTime * rotationSpeed);

            //ownerTransform.rotation = Quaternion.RotateTowards(ownerTransform.rotation, lookAtRotation, Time.deltaTime * rotationSpeed);

            //ownerTransform.rotation = Quaternion.Euler(Vector3.RotateTowards(ownerTransform.forward, playerTransform.position - ownerTransform.position, rotationSpeed, 0));
            //ownerTransform.rotation = Quaternion.LookRotation(playerTransform.position - ownerTransform.position, Vector3.up);

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
                NodeState = NodeStates.SUCCESS;
                return NodeState;
            }
            //timern som körs är inte än klar
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
