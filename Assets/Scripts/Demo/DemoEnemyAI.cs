using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemyAI : BaseAI
{
    [SerializeField] float fakeVisionRange;
    [SerializeField] float fakeShootRange;

    public BlackBoardProperty<Vector3> positionToGoToProperty { get; private set; } = new BlackBoardProperty<Vector3>("positionToGoTo", Vector3.zero);

    private void Update()
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
