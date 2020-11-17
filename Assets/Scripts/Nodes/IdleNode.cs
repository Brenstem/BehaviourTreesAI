using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    public override NodeStates Evaluate()
    {
        return NodeStates.SUCCESS;
    }
}