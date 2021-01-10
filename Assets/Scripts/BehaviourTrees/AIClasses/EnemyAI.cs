using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private int startHealth;
    [SerializeField] private GameObject player;
    
    [Header("Behaviour parameters")]
    [SerializeField] private float chasingRange;

    private NavMeshAgent agent;
    private Selector topNode;
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = Mathf.Clamp(value, 0, startHealth); } }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHealth = startHealth;
        //ConstructBehaviourTree();
    }

    private void Update()
    {
        topNode.Evaluate();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("ded");
    }

    //private void ConstructBehaviourTree()
    //{
    //    IdleNode idleNode = new IdleNode();
    //    RangeNode chaseRangeNode = new RangeNode();
    //    chaseRangeNode.Construct(new RangeNodeParameters(chasingRange, player.transform, transform));
    //    ChaseNode chaseNode = new ChaseNode();
    //    chaseNode.Construct(new ChaseNodeParameters(player.transform, agent));

    //    Sequence chaseSequence = new Sequence();
    //    chaseSequence.Construct(new List<AbstractNode> { chaseRangeNode, chaseNode });

    //    topNode = new Selector();
    //    topNode.Construct(new List<AbstractNode> { chaseSequence, idleNode });
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chasingRange);
    }
}
