using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralEnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject soldier;

    private DemoEnemyAI soldierAI;
    private Health health;
    private NavMeshAgent agent;

    void Awake()
    {
        soldierAI = soldier.GetComponent<DemoEnemyAI>();
        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }
}
