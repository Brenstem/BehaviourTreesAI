using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackBoard", menuName = "BlackBoards/BlackBoard", order = 0)]
public class Context : ScriptableObject
{
    /*TODO fixa blackboards som funkar med editorn
     * funkade inte om detta är ScriptableObject, för då har den inte koll på ex instansen av spelaren
     * kanske funkar om det är monobehavior men då blir det lite wack
     * kanske funkar om nodeData är scriptable object men de andra inte är det
    */
    public NodeBoard nodeData;
    public LocalBoard localData;
    public GlobalBoard globalData;

    [HideInInspector] public float range;
    [HideInInspector] public GameObject player;

    [HideInInspector] public BaseAI owner;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
