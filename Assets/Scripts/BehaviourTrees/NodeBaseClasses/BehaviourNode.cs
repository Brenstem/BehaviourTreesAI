using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourNode : AbstractNode
{
    public abstract void Construct(BlackboardScript blackboard);
}
