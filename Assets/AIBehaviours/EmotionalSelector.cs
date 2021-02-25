using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionalSelector : Composite
{
    private float eRisk;
    private float ePlan;
    private float eTime;

    public override void Construct(List<AbstractNode> nodes)
    {
        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }

    //TODO fixa klart emoselectorn
    private void CalculateEmotionalFactors()
    {
        eRisk = context.emotionalData.Happiness;
        ePlan = context.emotionalData.Anger;
        eTime = context.emotionalData.Anxiety;
    }

    protected override void CalculatePlanValue()
    {
        foreach (AbstractNode node in nodes)
        {
            planValue += node.GetPlanValue();
        }
        planValue = planValue / nodes.Count;
    }
    protected override void CalculateRiskValue()
    {
        foreach (AbstractNode node in nodes)
        {
            riskValue += node.GetRiskValue();
        }
        riskValue = riskValue / nodes.Count;
    }
    protected override void CalculateTimeInterval()
    {
        foreach (AbstractNode node in nodes)
        {
            if (minTimeValue > node.GetMinTimeValue())
                minTimeValue = node.GetMinTimeValue();

            if (maxTimeValue < node.GetMaxTimeValue())
                maxTimeValue = node.GetMaxTimeValue();
        }
    }


}