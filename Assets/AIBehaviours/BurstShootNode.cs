using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShootNode : Action
{
    [SerializeField] private float timeBetweenShots = 0.1f;
    [SerializeField] private int shots = 1;
    private int shotsFired;

    DemoWeaponScript weapon;
    private Timer timer;

    public override void Construct()
    {
        timer = new Timer(-1f);
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {

            //TODO denna �r konstig fixa den s� den funkar
            timer.DecrementTimer(Time.deltaTime);

            if (timer.Done)
            {
                weapon = context.owner.GetComponentInChildren<DemoWeaponScript>();
                weapon.FireWeapon();

                shotsFired++;

                if (shotsFired < shots)
                {
                    timer.Reset(timeBetweenShots);

                    NodeState = NodeStates.RUNNING;
                    return NodeState;
                }
                else
                {
                    shotsFired = 0;
                    Debug.Log("done fire");
                    NodeState = NodeStates.SUCCESS;
                    return NodeState;
                }
            }
            else
            {
                NodeState = NodeStates.RUNNING;
                return NodeState;
            }

            ////ingen timer k�rs, skapa en ny timer som b�rjar k�ras
            //if (timer.Done)
            //{
            //    timer.Reset(timeBetweenShots);

            //    weapon = context.owner.GetComponent<DemoWeaponScript>();
            //    weapon.FireWeapon();

            //    shotsFired++;

            //    NodeState = NodeStates.RUNNING;
            //    return NodeState;
            //}

            //timer.DecrementTimer(Time.deltaTime);

            ////timern som k�rdes �r nu klar
            //if (timer.Done)
            //{
            //    NodeState = NodeStates.SUCCESS;
            //    return NodeState;
            //}
            ////timern som k�rs �r inte �n klar
            //else
            //{
            //    NodeState = NodeStates.RUNNING;
            //    return NodeState;
            //}
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
