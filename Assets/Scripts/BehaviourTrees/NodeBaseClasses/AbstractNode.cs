using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNode : ScriptableObject
{
    public NodeStates NodeState { get; protected set; }

    protected bool _constructed = false;
    protected float probability;

    public Context context;



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