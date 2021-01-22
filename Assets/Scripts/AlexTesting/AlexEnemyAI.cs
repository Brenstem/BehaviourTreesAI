using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI : BaseAI
{
    //private Context blackboard;

    [SerializeField] private GameObject player;

    [Header("Behaviour parameters")]
    [SerializeField] private float aggroRange;
    [SerializeField] private float attackRange;

    Stack<AbstractNode> constructStack = new Stack<AbstractNode>();

    public BlackBoardProperty<float> aggroRangeProperty { get; private set; }
    public BlackBoardProperty<float> attackRangeProperty { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        aggroRangeProperty = new BlackBoardProperty<float>("aggroRange", aggroRange);
        attackRangeProperty = new BlackBoardProperty<float>("attackRange", attackRange);
        behaviorTree.blackboard.Initialize();
        
        behaviorTree.blackboard.owner = this;

        constructStack.Push(behaviorTree.topNode);

        while(constructStack.Count > 0)
        {
            AbstractNode currentNode = constructStack.Pop();

            if (currentNode.GetType().IsSubclassOf(typeof(Composite)))
            {
                Composite compositetNode = (Composite)currentNode;
                print(compositetNode.nodes.Count);
                for (int i = 0; i < compositetNode.nodes.Count; i++)
                {
                    constructStack.Push(compositetNode.nodes[i]);
                }
                print("Composite");
            }
            else if (currentNode.GetType().IsSubclassOf(typeof(Decorator)))
            {
                Decorator decoratorNode = (Decorator)currentNode;
                constructStack.Push(decoratorNode.node);
                print("Decorator");

            }
            else
            {
                Action actionNode = (Action)currentNode;
                actionNode.Construct();
                print("Action");
            }
            print("did a node :)");
        }
    }

    private void Start()
    {
        //ConstructBlackBoard();
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

    //private void ConstructBlackBoard()
    //{
    //    if (behaviorTree.blackboard == null)
    //    {
    //        blackboard = new Context();

    //        blackboard.nodeData = new NodeBoard();
    //        blackboard.nodeData.Add<float>(aggroRangeProperty);
    //        blackboard.nodeData.Add<float>(attackRangeProperty);

    //        blackboard.localData = new LocalBoard(this.gameObject);

    //        blackboard.globalData = new GlobalBoard();
    //        blackboard.globalData.player = player;

    //        behaviorTree.blackboard = blackboard;
    //        print("black borat generated");
            
    //        //behaviorTree.blackboard.owner = this;
    //    }
    //}
}
