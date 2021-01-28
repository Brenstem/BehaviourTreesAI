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

        behaviorTree.context.Initialize();
        behaviorTree.context.owner = this;
        behaviorTree.ConstructBehaviourTree();
    }

    private void Update()
    {
        behaviorTree.context.owner = this;
        behaviorTree.topNode.Evaluate();
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, 7);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
