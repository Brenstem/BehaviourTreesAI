using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI : MonoBehaviour
{
    [SerializeField] BlackboardScript blackboard;

    [SerializeField] private GameObject player;

    [Header("Behaviour parameters")]
    [SerializeField] private float chasingRange;

    private NavMeshAgent agent;
    private Selector topNode;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //currentHealth = startHealth;
        ConstructBehaviourTree();
    }
    
    private void Update()
    {
        topNode.Evaluate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chasingRange);
    }

    private void ConstructBehaviourTree()
    {
        IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
        idleNode.Construct(blackboard);
        RangeNode chaseRangeNode = ScriptableObject.CreateInstance<RangeNode>();
        chaseRangeNode.Construct(blackboard);
        ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
        chaseNode.Construct(blackboard);

        Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
        chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

        topNode = ScriptableObject.CreateInstance<Selector>();
        topNode.blackboard = blackboard;
        topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });
    }

    private void ConstructBlackBoard()
    {

    }
}
