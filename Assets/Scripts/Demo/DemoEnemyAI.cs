using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemyAI : BaseAI
{
    [SerializeField] float fakeVisionRange;
    [SerializeField] float fakeShootRange;

    public BlackBoardProperty<Vector3> positionToGoToProperty { get; private set; } = new BlackBoardProperty<Vector3>("positionToGoTo", Vector3.zero);

    private Timer fireCooldown;


    private new void Awake()
    {
        base.Awake();

        behaviourTree.context.localData.Add<Vector3>(positionToGoToProperty);
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("TookDamage", false); });
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("Aggroed", false); });
        behaviourTree.context.localData.Add<Vector3>(() => { return new BlackBoardProperty<Vector3>("LastKnownPlayerPosition", Vector3.zero); });
        behaviourTree.context.localData.Add<bool>(() => { return new BlackBoardProperty<bool>("CanFire", true); });
        behaviourTree.context.localData.Add<Action>(() => { return new BlackBoardProperty<Action>("CurrentRunningNode", null); });

        fireCooldown = new Timer(0f, () => { SetFireTimer(); });
    }

    private void Update()
    {
        animator.SetBool("Moving", agent.desiredVelocity.magnitude > agent.stoppingDistance);
        fireCooldown.DecrementTimer(Time.deltaTime);

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

    private void SetFireTimer()
    {
        behaviourTree.context.localData.Set<bool>("CanFire", true);
        fireCooldown.Reset();
    }
}
