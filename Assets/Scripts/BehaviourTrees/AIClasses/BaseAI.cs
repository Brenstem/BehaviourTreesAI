using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAI : MonoBehaviour
{
    public NavMeshAgent agent { get; protected set; }
    public Health health { get; protected set; }

    protected BehaviourTree behaviourTree;

    [SerializeField] protected BTDataContainer btGenerationData;

    [HideInInspector] public Animator animator;

    public BTDataContainer GetBehaviourTreeInstance()
    {
        return behaviourTree.btDataInstance;
    }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();

        behaviourTree = new BehaviourTree();

        behaviourTree.btData = btGenerationData;

        behaviourTree.ConstructBehaviourTree(this);

    }

}
