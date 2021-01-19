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
        //skulle vara b�ttre f�r garbage colection om man skapar en timer som man resetar, men runtime �r inte satt n�r construct k�rs, man kanske vill ha en typ start funktion man kan kalla?
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
            //ingen timer k�rs, skapa en ny timer som b�rjar k�ras
            if (timer.Done)
            {
                timer.Reset(runTime);
                //context.owner.agent.SetDestination(context.owner.transform.position);
                return NodeStates.RUNNING;
            }

            timer.DecrementTimer(Time.deltaTime);

            //timern som k�rdes �r nu klar
            if(timer.Done)
            {
                return NodeStates.SUCCESS;
            }
            //timern som k�rs �r inte �n klar
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
