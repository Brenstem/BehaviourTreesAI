using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Action
{
    HitBoxController hitBox;

    public override void Construct()
    {
        //m�ste lista ut hur vi ska g�ra f�r att h�mta saker som hitboxes p� ett smidigt s�tt
        //alla hitboxes har en ID som man letar efter 
        //hitBox = context.owner.GetComponentInChildren<HitBoxController>();
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            hitBox = context.owner.GetComponentInChildren<HitBoxController>();
            if (hitBox.wasActive && !hitBox.isActive)
            {
                NodeState = NodeStates.SUCCESS;
                return NodeState;
            }
            else if (hitBox.isActive)
            {
                NodeState = NodeStates.RUNNING;
                return NodeState;
            }
            else
            {
                hitBox.ExposeHitBox();
                NodeState = NodeStates.RUNNING;
                return NodeState;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            NodeState = NodeStates.FAILURE;
            return NodeState;
        }
    }
}
