using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootNode : Action
{
    DemoWeaponScript weapon;
    public override void Construct()
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            weapon = context.owner.GetComponentInChildren<DemoWeaponScript>();

            weapon.FireWeapon();

            NodeState = NodeStates.SUCCESS;
            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
