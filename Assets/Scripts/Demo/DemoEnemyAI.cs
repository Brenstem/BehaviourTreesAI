using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemyAI : BaseAI
{
    private void Update()
    {
        behaviourTreeInstance.topNodeInstance.Evaluate();
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, 7);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
