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
            animator.SetTrigger("BurstShoot");

            //Quaternion lookAtRotation = Quaternion.LookRotation(playerTransform.position - ownerTransform.position);

            //ownerTransform.rotation = Quaternion.Lerp(ownerTransform.rotation, lookAtRotation, Time.fixedDeltaTime * rotationSpeed);

            //TODO denna är konstig fixa den så den funkar
            timer.DecrementTimer(Time.fixedDeltaTime);

            if (timer.Done)
            {
                weapon.FireWeapon();

                _shotsFired++;

                if (_shotsFired < shots)
                {
                    timer.Reset(timeBetweenShots);

                    NodeState = NodeStates.RUNNING;
                }
                else
                {
                    _shotsFired = 0;
                    timer.Reset(-1);
                    NodeState = NodeStates.SUCCESS;
                }
            }
            else
            {
                NodeState = NodeStates.RUNNING;
            }

            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
