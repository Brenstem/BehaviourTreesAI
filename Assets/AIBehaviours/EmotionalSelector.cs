using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Composite", "EmotionalSelector")]
public class EmotionalSelector : Composite
{
    private float eRisk;
    private float ePlan;
    private float eTime;
    private float eOpt;

    private float[] riskFactors;
    private float[] planFactors;
    private float[] timeFactors;

    private float[] nodeWeights;
    private float[] nodeProbabilities;


    public override void Construct(List<AbstractNode> nodes)
    {
        this.nodes = nodes;

        riskFactors = new float[nodes.Count];
        planFactors = new float[nodes.Count];
        timeFactors = new float[nodes.Count];
        nodeWeights = new float[nodes.Count];
        nodeProbabilities = new float[nodes.Count];

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            CalculateProbabilities();

            float random = UnityEngine.Random.Range(0f, 1f);

            for (int i = 0; i < nodeProbabilities.Length; i++)
            {
                if (random < nodeProbabilities[i])
                {
                    NodeState = nodes[i].Evaluate();
                    break;
                }
                else
                {
                    random -= nodeProbabilities[i];
                }
            }

            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }


    // Set emotional values based on emotions
    // happiness, anger, anxiety, exhaustion, sadness
    private void CalculateEmotionalValues()
    {
        eRisk = (context.emotionalData.Happiness + context.emotionalData.Anger + context.emotionalData.Sadness) / 3 - context.emotionalData.Anxiety;
        ePlan = context.emotionalData.Happiness - (context.emotionalData.Anger + context.emotionalData.Sadness + context.emotionalData.Exhaustion) / 3;
        eTime = context.emotionalData.Happiness - (context.emotionalData.Anger + context.emotionalData.Anxiety + context.emotionalData.Exhaustion + context.emotionalData.Sadness) / 4;
        eOpt = context.emotionalData.Happiness - (context.emotionalData.Anger + context.emotionalData.Anxiety + context.emotionalData.Exhaustion + context.emotionalData.Sadness) / 4;
    }

    // Calculate probability of a given child node running
    private void CalculateProbabilities()
    {
        CalculateEmotionalValues();
        CalculateEmotionalFactors();
        CalculateWeights();

        float remainder = 1; 

        for (int i = 0; i < nodes.Count; i++)
        {
            nodeProbabilities[i] = context.emotionalData.Distribution * Mathf.Pow(1 - context.emotionalData.Distribution, i);
            remainder -= nodeProbabilities[i];
        }

        nodeProbabilities[nodeProbabilities.Length - 1] += remainder; // Add remainder chance to the last node probability so there is a 0 percent chance of no behaviour running 
    }

    private void CalculateWeights()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodeWeights[i] = context.emotionalData.RiskWeight * riskFactors[i] + context.emotionalData.PlanWeight * planFactors[i] + context.emotionalData.TimeWeight * timeFactors[i];
        }

        Array.Sort(nodeWeights);
        Array.Reverse(nodeWeights);
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
            riskFactors[i] = (1 - eRisk * context.emotionalData.ERiskWeight) * nodes[i].GetRiskValue();
        }
    }

    private void CalculateTimeFactors()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            float time = nodes[i].GetMinTimeValue() + ((nodes[i].GetMaxTimeValue() + nodes[i].GetMinTimeValue()) / 2) * (1 - context.emotionalData.EOptWeight * eOpt);
            timeFactors[i] = (1 - (1 / (1 + context.emotionalData.TimeSpan * time))) * Mathf.Max(1 - context.emotionalData.ETimeWeight + context.emotionalData.ETimeWeight * eTime, 0);
        }
    }

    private void CalculatePlanFactors()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            planFactors[i] = (1 - 1 / (1 + context.emotionalData.PlanningAmount * nodes[i].GetPlanValue())) * Mathf.Max(1 - context.emotionalData.EPlanWeight + context.emotionalData.EPlanWeight * ePlan, 0);
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