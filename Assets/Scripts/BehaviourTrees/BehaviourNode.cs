using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BehaviourNode
{
    protected NodeStates _nodeState;

    public NodeStates NodeState { get { return _nodeState; } }

    public abstract NodeStates Evaluate();
}

public enum NodeStates
{
    RUNNING, SUCCESS, FAILURE, 
}