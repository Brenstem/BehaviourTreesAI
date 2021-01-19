using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : Action
{
    private Timer timer;
    private float runTime = 0.5f;

    public override void Construct(Context blackboard)
    {
        this.context = blackboard;
        _constructed = true;
        //skulle vara bättre för garbage colection om man skapar en timer som man resetar, men runtime är inte satt när construct körs, man kanske vill ha en typ start funktion man kan kalla?
        timer = new Timer(-1f);
    }

    public override void AddProperties(string[] names)
    {
        foreach (var name in names)
        {
            runTime = context.nodeData.Get<float>(name);
        }
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            //ingen timer körs, skapa en ny timer som börjar köras
            if (timer.Done)
            {
                timer.Reset(runTime);
                //context.owner.agent.SetDestination(context.owner.transform.position);
                return NodeStates.RUNNING;
            }

            timer.DecrementTimer(Time.deltaTime);

            //timern som kördes är nu klar
            if(timer.Done)
            {
                return NodeStates.SUCCESS;
            }
            //timern som körs är inte än klar
            else
            {
                return NodeStates.RUNNING;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
