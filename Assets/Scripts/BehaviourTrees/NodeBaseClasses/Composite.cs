using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : AbstractNode
{
    //[SerializeField] public List<AbstractNode> nodes = new List<AbstractNode>();
    [SerializeField] public AbstractNode[] nodes;

    [SerializeField] protected float riskValue;
    [SerializeField] protected float minTimeValue;
    [SerializeField] protected float maxTimeValue;
    [SerializeField] protected float planValue;

    protected abstract void CalculatePlanValue();
    protected abstract void CalculateRiskValue();
    protected abstract void CalculateTimeInterval();

    public virtual void Construct(List<AbstractNode> nodes)
    {
        this.nodes = nodes.ToArray();

        CalculatePlanValue();
        CalculateRiskValue();
        CalculateTimeInterval();

        _constructed = true;
    }
    public override float GetRiskValue()
    {
        return riskValue;
    }
    public override float GetMinTimeValue()
    {
        return minTimeValue;
    }
    public override float GetMaxTimeValue()
    {
        return maxTimeValue;
    }
    public override float GetPlanValue()
    {
        return planValue;
    }
}
