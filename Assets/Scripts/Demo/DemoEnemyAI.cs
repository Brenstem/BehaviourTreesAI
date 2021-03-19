﻿using System.Collections;
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
    }


    private void Update()
    {

        animator.SetBool("Moving", agent.desiredVelocity.magnitude > agent.stoppingDistance);

        Debug.DrawRay(transform.position, transform.forward * 200, Color.green);
    }

    private void FixedUpdate()
    {
        behaviourTree.topNodeInstance.Evaluate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, fakeVisionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fakeShootRange);
    }
}
