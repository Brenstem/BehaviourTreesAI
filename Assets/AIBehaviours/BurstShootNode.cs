using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShootNode : Action
{
    [SerializeField] private float timeBetweenShots = 0.1f;
    [SerializeField] private int shots = 1;
    [SerializeField] private float rotationSpeed = 0.5f;

    private int _shotsFired;

    DemoWeaponScript weapon;
    private Timer timer;
    Transform ownerTransform;
    Transform playerTransform;

    public override void Construct()
    {
        timer = new Timer(-1f);
        _constructed = true;
        ownerTransform = context.owner.transform;
        playerTransform = context.player.transform;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(playerTransform.position - ownerTransform.position);

            ownerTransform.rotation = Quaternion.Slerp(ownerTransform.rotation, lookAtRotation, Time.deltaTime * rotationSpeed);

            //TODO denna är konstig fixa den så den funkar
            timer.DecrementTimer(Time.deltaTime);

            if (timer.Done)
            {
                weapon = context.owner.GetComponentInChildren<DemoWeaponScript>();
                weapon.FireWeapon();

                _shotsFired++;

                if (_shotsFired < shots)
                {
                    timer.Reset(timeBetweenShots);

                    NodeState = NodeStates.RUNNING;
                    return NodeState;
                }
                else
                {
                    _shotsFired = 0;
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

            ////ingen timer körs, skapa en ny timer som börjar köras
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

            ////timern som kördes är nu klar
            //if (timer.Done)
            //{
            //    NodeState = NodeStates.SUCCESS;
            //    return NodeState;
            //}
            ////timern som körs är inte än klar
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
