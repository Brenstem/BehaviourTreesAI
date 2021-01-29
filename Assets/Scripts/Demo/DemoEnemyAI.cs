using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemyAI : BaseAI
{

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        behaviourTree.context.Initialize();
        behaviourTree.context.owner = this;
        behaviourTree.ConstructBehaviourTree();
    }

    private void Update()
    {
        behaviourTree.context.owner = this;
        behaviourTree.topNode.Evaluate();
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, 7);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
