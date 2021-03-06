using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Attacks", "BurstShootNode")]
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

    Animator animator;

    public override void Construct()
    {
        timer = new Timer(-1f);
        _constructed = true;
        ownerTransform = context.owner.transform;
        playerTransform = context.globalData.player.transform;
        weapon = context.owner.GetComponentInChildren<DemoWeaponScript>();
        animator = context.owner.animator;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            animator.SetTrigger("Shooting");

            Quaternion lookAtRotation = Quaternion.LookRotation(playerTransform.position - ownerTransform.position);

            ownerTransform.rotation = Quaternion.Lerp(ownerTransform.rotation, lookAtRotation, Time.deltaTime * rotationSpeed);

            //TODO denna �r konstig fixa den s� den funkar
            timer.DecrementTimer(Time.deltaTime);

            if (timer.Done)
            {
                weapon.FireWeapon(ownerTransform.rotation);

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
