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
    private Node topNode;
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = Mathf.Clamp(value, 0, startHealth); } }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentHealth = startHealth;
        ConstructBehaviourTree();

        // agent.SetDestination(player.transform.position);
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

    private void ConstructBehaviourTree()
    {
        IdleNode idleNode = new IdleNode();
        RangeNode chaseRangeNode = new RangeNode(chasingRange, player.transform.position, transform.position);
        ChaseNode chaseNode = new ChaseNode(player.transform, agent);

        Sequence chaseSequence = new Sequence(new List<Node> { chaseRangeNode, chaseNode });

        topNode = new Selector(new List<Node> { chaseSequence, idleNode });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chasingRange);
    }
}
