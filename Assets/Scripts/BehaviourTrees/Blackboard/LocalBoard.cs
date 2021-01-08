using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocalBoard
{
    public GameObject thisAI { get; private set; }

    public LocalBoard(GameObject thisAI)
    {
        this.thisAI = thisAI;
    }
}
