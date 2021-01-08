using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI : MonoBehaviour
{
    private Context blackboard;

    [SerializeField] private GameObject player;

    [Header("Behaviour parameters")]
    [SerializeField] private float chasingRange;

    private NavMeshAgent agent;
    private Selector topNode;
    public Health health { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //currentHealth = startHealth;
        ConstructBlackBoard();
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
        HealthNode healthCheck = ScriptableObject.CreateInstance<HealthNode>();
        healthCheck.Construct(blackboard);

        Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
        chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

        topNode = ScriptableObject.CreateInstance<Selector>();
        topNode.blackboard = blackboard;
        topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });
    }

    private void ConstructBlackBoard()
    {
        blackboard = new Context();

        blackboard.nodeData = new Enemy1NodeBoard();
        blackboard.nodeData.aggroRange = chasingRange;

        blackboard.localData = new LocalBoard();
        blackboard.localData.thisAI = this;

        blackboard.globalData = new GlobalBoard();
        blackboard.globalData.player = player;
    }
}
