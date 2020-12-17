using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : AbstractNode
{
    protected AbstractNode node;

    public virtual void Construct(AbstractNode node)
    {
        this.node = node;
        _constructed = true;
    }
}
