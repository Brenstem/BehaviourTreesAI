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

        behaviourTree.context.localData.Add<Vector3>(positionToGoToProperty);
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("TookDamage", false); });
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("Aggroed", false); });
        behaviourTree.context.localData.Add<Vector3>(() => { return new BlackBoardProperty<Vector3>("LastKnownPlayerPosition", Vector3.zero); });
    }

    private void Update()
    {
        if (!GetComponent<Health>().isDead)
        {
            animator.SetBool("Moving", agent.desiredVelocity.magnitude > agent.stoppingDistance);
        }

        if (turn)
        {
            turn = TurnTowardsInternal(turnTarget);
        }
        
    }

    private bool turn = false;
    private Transform turnTarget;

    public void TurnTowards(Transform target)
    {
        turn = true;
        turnTarget = target;
    }

    private bool TurnTowardsInternal(Transform target)
    {
        float RotateSmoothTime = 0.05f;
        float AngularVelocity = 0f;

        Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
        float delta = Quaternion.Angle(transform.rotation, targetRot);

        if (delta > 5f)
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
