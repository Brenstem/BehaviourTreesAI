using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : AbstractNode
{
    public AbstractNode node;

    protected float riskValue;
    protected float minTimeValue;
    protected float maxTimeValue;
    protected float planValue;

    public virtual void Construct(AbstractNode node)
    {
        this.node = node;
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
