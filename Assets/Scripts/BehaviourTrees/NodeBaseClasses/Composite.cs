using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : AbstractNode
{
    [SerializeField] public List<AbstractNode> nodes = new List<AbstractNode>();

    [HideInInspector] public float riskValue;
    [HideInInspector] public Vector2 timeInterval;
    [HideInInspector] public float planValue;

    protected abstract void CalculateEmotionalFactors();

    public virtual void Construct(List<AbstractNode> nodes)
    {
        this.nodes = nodes;
        _constructed = true;
    }
}
