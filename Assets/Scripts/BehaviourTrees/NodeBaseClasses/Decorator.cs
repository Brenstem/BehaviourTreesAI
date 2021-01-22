using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : AbstractNode
{
    public AbstractNode node;

    public virtual void Construct(AbstractNode node)
    {
        this.node = node;
        _constructed = true;
    }
}
