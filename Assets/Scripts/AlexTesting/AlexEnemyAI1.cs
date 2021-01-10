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

    //private Selector topNode;

    public static BlackBoardProperty<float> aggroRange { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        aggroRange = new BlackBoardProperty<float>("aggroRange", chasingRange);
    }

    private void Start()
    {
        ConstructBlackBoard();
        behaviorTree.ConstructBehaviourTree();
        //ConstructBehaviourTree();
    }
    
    private void Update()
    {
        behaviorTree.blackboard.owner = this;
        behaviorTree.topNode.Evaluate();
        //topNode.Evaluate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chasingRange);
    }

    //private void ConstructBehaviourTree()
    //{
    //    IdleNode idleNode = ScriptableObject.CreateInstance<IdleNode>();
    //    idleNode.Construct(blackboard);
    //    RangeNode chaseRangeNode = ScriptableObject.CreateInstance<RangeNode>();
    //    chaseRangeNode.Construct(blackboard);
    //    ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
    //    chaseNode.Construct(blackboard);

    //    Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
    //    chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

    //    topNode = ScriptableObject.CreateInstance<Selector>();
    //    topNode.blackboard = blackboard;
    //    topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });
    //}

    private void ConstructBlackBoard()
    {
        if (behaviorTree.blackboard == null)
        {
            blackboard = new Context();

            blackboard.nodeData = new NodeBoard();
            blackboard.nodeData.Add<float>(aggroRange);

            blackboard.localData = new LocalBoard(this.gameObject);

            blackboard.globalData = new GlobalBoard();
            blackboard.globalData.player = player;

            behaviorTree.blackboard = blackboard;
            print("black borad generated");
            
            behaviorTree.blackboard.owner = this;
        }
    }
}
