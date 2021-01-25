using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Action
{
    HitBoxController hitBox;

    public override void Construct()
    {
        hitBox = context.owner.GetComponentInChildren<HitBoxController>();
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (hitBox.wasActive && !hitBox.isActive)
            {
                return NodeStates.SUCCESS;
            }
            else if (hitBox.isActive)
            {
                return NodeStates.RUNNING;
            }
            else
            {
                hitBox.ExposeHitBox();
                return NodeStates.RUNNING;
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
