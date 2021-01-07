using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "TestEnemyBlackboard", menuName = "ScriptableObjects/Blackboards", order = 1)]
public class TestEnemyBlackboard : BlackboardScript
{
    public float aggroRange;
    public GameObject player;
    public Transform myTransform;
    public NavMeshAgent agent;
}
