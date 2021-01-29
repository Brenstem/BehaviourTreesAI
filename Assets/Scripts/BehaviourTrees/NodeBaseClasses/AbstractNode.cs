using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractNode : ScriptableObject
{
    public Context context;

    public NodeStates NodeState { get; protected set; }

    [SerializeField] protected bool _constructed = false;

    /// <summary>
    /// This is the update method of your behaviour
    /// </summary>
    /// <returns></returns>
    public abstract NodeStates Evaluate();
}

public enum NodeStates
{
    FAILURE, RUNNING, SUCCESS,  
}