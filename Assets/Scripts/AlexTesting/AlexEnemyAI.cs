using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlexEnemyAI : MonoBehaviour
{
    //[SerializeField] private int startHealth;
    [SerializeField] private GameObject player;

    [Header("Behaviour parameters")]
    [SerializeField] private float chasingRange;

    private NavMeshAgent agent;
    private Selector topNode;
    //private int currentHealth;
    //public int CurrentHealth { get { return currentHealth; } set { currentHealth = Mathf.Clamp(value, 0, startHealth); } }

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
        idleNode.Construct(new IdleNodeParameters());
        RangeNode chaseRangeNode = ScriptableObject.CreateInstance<RangeNode>();
        chaseRangeNode.Construct(new RangeNodeParameters(chasingRange, player.transform, transform));
        ChaseNode chaseNode = ScriptableObject.CreateInstance<ChaseNode>();
        chaseNode.Construct(new ChaseNodeParameters(player.transform, agent));



        Sequence chaseSequence = ScriptableObject.CreateInstance<Sequence>();
        chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

        topNode = ScriptableObject.CreateInstance<Selector>();
        topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });
    }
}
