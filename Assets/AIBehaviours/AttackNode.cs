using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Action
{
    HitBoxController hitBox;

    public override void Construct(Context blackboard)
    {
        context = blackboard;
        hitBox = blackboard.localData.thisAI.GetComponentInChildren<HitBoxController>();
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            hitBox.ExposeHitBox();
            Debug.Log("Attacking");
            return NodeStates.SUCCESS;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}

//public class AttackNodeParameters
//{

//}
