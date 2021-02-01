using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "BlackBoards/BlackBoard", order = 0)]
public class GlobalData : ScriptableObject
{
    [HideInInspector] public GameObject player;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
