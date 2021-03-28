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

        //Debug.Log(Vector3.Distance(transform.position, behaviourTree.context.globalData.player.transform.position));

        Debug.DrawRay(transform.position, transform.forward * 200, Color.green);
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
