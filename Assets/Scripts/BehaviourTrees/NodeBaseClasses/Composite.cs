using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : AbstractNode
{
    [SerializeField] public List<AbstractNode> nodes = new List<AbstractNode>();

    public virtual void Construct(List<AbstractNode> nodes)
    {
        this.nodes = nodes;
        _constructed = true;
    }
}
