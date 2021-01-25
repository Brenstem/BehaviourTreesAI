using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI1 : BaseAI
{
    private Context blackboard;

    [SerializeField] private GameObject player;

    [Header("Behaviour parameters")]
    [SerializeField] private float chasingRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        // ConstructBlackBoard();
    }
    
    private void Update()
    {
        behaviorTree.context.owner = this;
        behaviorTree.topNode.Evaluate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chasingRange);
    }
}
