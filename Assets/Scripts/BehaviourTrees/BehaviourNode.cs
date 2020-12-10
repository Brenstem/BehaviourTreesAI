using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditor;

[System.Serializable]
public abstract class BehaviourNode : BTEditorNode
{
    protected NodeStates _nodeState;

    public NodeStates NodeState { get { return _nodeState; } }

    public abstract NodeStates Evaluate();

    // public abstract void Instantiate();
}

public enum NodeStates
{
    RUNNING, SUCCESS, FAILURE, 
}