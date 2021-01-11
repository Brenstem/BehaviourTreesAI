using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractNode : ScriptableObject
{
    protected NodeStates _nodeState;

    [SerializeField] protected bool _constructed = false;

    public NodeStates NodeState { get { return _nodeState; } }

    /// <summary>
    /// This is the update method of your behaviour
    /// </summary>
    /// <returns></returns>
    public abstract NodeStates Evaluate();
}

public enum NodeStates
{
    RUNNING, SUCCESS, FAILURE, 
}