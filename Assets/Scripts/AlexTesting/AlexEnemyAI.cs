using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI : BaseAI
{
    private Context blackboard;

    [SerializeField] private GameObject player;

    [Header("Behaviour parameters")]
    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackStartupTime;
    [SerializeField] private float attackEndLagTime;

    public BlackBoardProperty<float> aggroRangeProperty { get; private set; }
    public BlackBoardProperty<float> attackRangeProperty { get; private set; }
    public BlackBoardProperty<float> attackStartupTimeProperty { get; private set; }
    public BlackBoardProperty<float> attackEndLagTimeProperty { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        aggroRangeProperty = new BlackBoardProperty<float>("aggroRange", aggroRange);
        attackRangeProperty = new BlackBoardProperty<float>("attackRange", attackRange);
        attackStartupTimeProperty = new BlackBoardProperty<float>("attackStartupTime", attackStartupTime);
        attackEndLagTimeProperty = new BlackBoardProperty<float>("attackEndLagTime", attackEndLagTime);
    }

    private void Start()
    {
        ConstructBlackBoard();
        behaviorTree.ConstructBehaviourTree();
        //behaviorTree.TestConstruct();
    }
    
    private void Update()
    {
        behaviorTree.blackboard.owner = this;
        behaviorTree.topNode.Evaluate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void ConstructBlackBoard()
    {
        if (behaviorTree.blackboard == null)
        {
            blackboard = new Context();

            blackboard.nodeData = new NodeBoard();
            blackboard.nodeData.Add<float>(aggroRangeProperty);
            blackboard.nodeData.Add<float>(attackRangeProperty);
            blackboard.nodeData.Add<float>(attackStartupTimeProperty);
            blackboard.nodeData.Add<float>(attackEndLagTimeProperty);


            blackboard.localData = new LocalBoard(this.gameObject);

            blackboard.globalData = new GlobalBoard();
            blackboard.globalData.player = player;

            behaviorTree.blackboard = blackboard;
            print("black borat generated");
            
            behaviorTree.blackboard.owner = this;
        }
    }
}
