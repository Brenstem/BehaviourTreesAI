using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Attacks", "ThrowGrenade")]
public class ThrowGrenade : Action
{
    public override void Construct()
    {
        _constructed = true;
    }

    //TODO den skapas i AIn s� de kolliderar med varandra, m�ste skapa den p� en annan position
    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            context.owner.animator.SetTrigger("ThrowGrenade");

            NodeState = NodeStates.SUCCESS;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
    }
}
