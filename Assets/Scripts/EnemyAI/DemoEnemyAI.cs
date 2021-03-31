using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemyAI : BaseAI
{
    [SerializeField] float fakeVisionRange;
    [SerializeField] float fakeShootRange;

    public BlackBoardProperty<Vector3> positionToGoToProperty { get; private set; } = new BlackBoardProperty<Vector3>("positionToGoTo", Vector3.zero);


    private new void Awake()
    {
        base.Awake();

        // General vars
        behaviourTree.context.localData.Add<Vector3>(positionToGoToProperty);
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("Aggroed", false); });
        behaviourTree.context.localData.Add<Vector3>(() => { return new BlackBoardProperty<Vector3>("LastKnownPlayerPosition", Vector3.zero); });

        // XCOM vars
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("TookDamage", false); });

        // HALO vars
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("AggressiveCombatStyle", true); });
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("GeneralHurt", false); });
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("GeneralDead", false); });
    }

    private void Update()
    {
        if (!GetComponent<Health>().isDead)
        {
            animator.SetBool("Moving", agent.desiredVelocity.magnitude > agent.stoppingDistance);

            float moveX = agent.velocity.x;
            float moveZ = agent.velocity.z;

            animator.SetFloat("moveZ", moveZ);
            animator.SetFloat("moveX", moveX);
        }

        if (turn && !GetComponent<Health>().isDead)
        {
            turn = TurnTowardsInternal(turnTarget, accuracy, speed);
        }
    }

    private bool turn = false;
    private Transform turnTarget;
    private float accuracy;
    private float speed;


    public void TurnTowards(Transform target, float accuracy, float speed)
    {
        turn = true;
        turnTarget = target;
        this.accuracy = accuracy;
        this.speed = speed;
    }

    public bool TurnTowardsInternal(Transform target, float accuracy, float speed)
    {
        float RotateSmoothTime = speed;
        float AngularVelocity = 0f;

        Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
        float delta = Quaternion.Angle(transform.rotation, targetRot);

        if (delta > accuracy)
        {
            float t = Mathf.SmoothDampAngle(delta, 0.0f, ref AngularVelocity, RotateSmoothTime);
            t = 1.0f - t / delta;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
            return true;
        }

        return false;
    }

    private void FixedUpdate()
{
    if (!GetComponent<Health>().isDead)
    {
        behaviourTree.topNodeInstance.Evaluate();
    }
}

private void OnDrawGizmos()
{
    Gizmos.DrawWireSphere(transform.position, fakeVisionRange);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, fakeShootRange);
}
}
