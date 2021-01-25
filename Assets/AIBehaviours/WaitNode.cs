using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : Action
{
    private Timer timer;
    [SerializeField] private float runTime = 0.5f;

    public override void Construct()
    {
        _constructed = true;
        timer = new Timer(-1f);
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //ingen timer körs, skapa en ny timer som börjar köras
            if (timer.Done)
            {
                timer.Reset(runTime);

                context.owner.agent.SetDestination(context.owner.transform.position);

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
