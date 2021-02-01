using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Attacks/meme1/meme2", "AttackNode")]
public class AttackNode : Action
{
    HitBoxController hitBox;

    public override void Construct()
    {
        //TODO måste lista ut hur vi ska göra för att hämta saker som hitboxes på ett smidigt sätt
        //alla hitboxes har en ID som man letar efter 
        hitBox = context.owner.GetComponentInChildren<HitBoxController>();
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
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
