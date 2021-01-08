using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourNode : AbstractNode
{
    protected Context context;

    public abstract void Construct(Context blackboard);
}
