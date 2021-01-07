using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(fileName = "EnemyLocalData", menuName = "ScriptableObjects/EnemyLocalData", order = 3)]

public class BlackboardLocalData : ScriptableObject
{
    public AlexEnemyAI thisAI;
}
