using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackBoard", menuName = "BlackBoards/BlackBoard", order = 0)]
public class Context : ScriptableObject
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public BaseAI owner;
    [HideInInspector] public LocalData localData;

    // public LocalBoard localData;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
