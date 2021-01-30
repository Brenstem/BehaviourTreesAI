using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAI : MonoBehaviour
{
    public NavMeshAgent agent { get; protected set; }
    public Health health { get; protected set; }

    [SerializeField] protected BehaviourTree behaviourTree;
    [SerializeField] protected BehaviourTree behaviourTreeInstance;

    public BehaviourTree GetBehaviourTreeInstance()
    {
        return behaviourTreeInstance;
    }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        behaviourTreeInstance = Instantiate(behaviourTree);
        behaviourTreeInstance.context.Initialize();
        behaviourTreeInstance.context.owner = this;
        behaviourTreeInstance.ConstructBehaviourTree();
    }

}
