using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNode : ScriptableObject
{
    public NodeStates NodeState { get; protected set; }

    protected bool _constructed = false;

    public Context context;


    /// <summary>
    /// This is the update method of your behaviour
    /// </summary>
    /// <returns></returns>
    public abstract NodeStates Evaluate();

    public abstract float GetRiskValue();
    public abstract float GetPlanValue();
    public abstract float GetMinTimeValue();
    public abstract float GetMaxTimeValue();
}

public enum NodeStates
{
    FAILURE, RUNNING, SUCCESS,
}