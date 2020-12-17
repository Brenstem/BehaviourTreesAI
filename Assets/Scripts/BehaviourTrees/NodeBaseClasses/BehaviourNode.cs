using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourNode<TParams> : AbstractNode
{
    public abstract void Construct(TParams parameters);
}
