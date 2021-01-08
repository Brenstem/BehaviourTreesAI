using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBoard
{
    public GameObject player;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
