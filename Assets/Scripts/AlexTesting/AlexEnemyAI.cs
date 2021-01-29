using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI : BaseAI
{
    private new void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        behaviourTreeInstance.topNodeInstance.Evaluate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
