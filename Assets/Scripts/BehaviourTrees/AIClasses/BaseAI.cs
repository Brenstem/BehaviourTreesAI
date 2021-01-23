using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAI : MonoBehaviour
{
    public NavMeshAgent agent { get; protected set; }
    public Health health { get; protected set; }

    [SerializeField] protected BehaviourTree behaviorTree;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

}
