using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionalSelector : Composite
{
    private float eRisk;
    private float ePlan;
    private float eTime;
    private float eOpt;

    private float[] riskFactors;
    private float[] planFactors;
    private float[] timeFactors;

    private float[] totalFactors;


    public new void Construct(List<AbstractNode> nodes)
    {
        this.nodes = nodes;

        riskFactors = new float[nodes.Count];
        planFactors = new float[nodes.Count];
        timeFactors = new float[nodes.Count];

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

    //TODO gör detta fast good
    private void CalculateEmotionalValues()
    {
        eRisk = context.emotionalData.Happiness;
        ePlan = context.emotionalData.Anger;
        eTime = context.emotionalData.Anxiety;
        eOpt = context.emotionalData.Exhaustion;
    }
    private void CalculateEmotionalFactors()
    {
        CalculateRiskFactors();
        CalculateTimeFactors();
        CalculatePlanFactors();



    }

    private void CalculateRiskFactors()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            riskFactors[i] = (1 - eRisk * context.emotionalData.eRiskWeight) * nodes[i].GetRiskValue();
        }
    }
    private void CalculateTimeFactors()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            float time = nodes[i].GetMinTimeValue() + ((nodes[i].GetMaxTimeValue() + nodes[i].GetMinTimeValue()) / 2) * (1 - context.emotionalData.eOptWeight * eOpt);
            timeFactors[i] = (1 - (1 / (1 + context.emotionalData.timeSpan * time))) * Mathf.Max(1 - context.emotionalData.eTimeWeight + context.emotionalData.eTimeWeight * eTime, 0);
        }
    }
    private void CalculatePlanFactors()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            planFactors[i] = (1 - 1 / (1 + context.emotionalData.planingAmount * nodes[i].GetPlanValue())) * Mathf.Max(1 - context.emotionalData.ePlanWeight + context.emotionalData.ePlanWeight * ePlan, 0);
        }
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