using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGlobalData", menuName = "ScriptableObjects/EnemyGlobalData", order = 5)]
public class BlackboardGlobalData : ScriptableObject
{
    public GameObject player;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
