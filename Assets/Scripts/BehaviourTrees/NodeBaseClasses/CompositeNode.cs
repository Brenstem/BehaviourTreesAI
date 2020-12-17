using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : AbstractNode
{
    protected List<AbstractNode> nodes = new List<AbstractNode>();

    public virtual void Construct(List<AbstractNode> nodes)
    {
        this.nodes = nodes;
        _constructed = true;
    }
}
