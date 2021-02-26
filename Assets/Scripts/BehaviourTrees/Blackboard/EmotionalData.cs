using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make scriptableobject

public class EmotionalData
{
    public EmotionalData(float happiness, float anxiety, float anger, float sadness, float exhaustion, float eRiskWeight, float riskWeight, float ePlanWeight, float planWeight, float planingAmount, float eTimeWeight, float timeWeight, float timeSpan, float eOptWeight)
    {
        Happiness = happiness;
        Anxiety = anxiety;
        Anger = anger;
        Sadness = sadness;
        Exhaustion = exhaustion;
        this.eRiskWeight = eRiskWeight;
        this.riskWeight = riskWeight;
        this.ePlanWeight = ePlanWeight;
        this.planWeight = planWeight;
        this.planingAmount = planingAmount;
        this.eTimeWeight = eTimeWeight;
        this.timeWeight = timeWeight;
        this.timeSpan = timeSpan;
        this.eOptWeight = eOptWeight;
    }

    public float Happiness { get { return Mathf.Clamp(Happiness, 0, 1); } set { Happiness = value; } }
    public float Anxiety { get { return Mathf.Clamp(Anxiety, 0, 1); } set { Anxiety = value; } }
    public float Anger { get { return Mathf.Clamp(Anger, 0, 1); } set { Anger = value; } }
    public float Sadness { get { return Mathf.Clamp(Sadness, 0, 1); } set { Sadness = value; } }
    public float Exhaustion { get { return Mathf.Clamp(Exhaustion, 0, 1); } set { Exhaustion = value; } }

    public float eRiskWeight { get { return Mathf.Clamp(eRiskWeight, 0, 1); } set { eRiskWeight = value; } }
    public float riskWeight { get { return Mathf.Clamp(riskWeight, 0, 1); } set { riskWeight = value; } }
    public float ePlanWeight { get { return Mathf.Clamp(ePlanWeight, 0, 1); } set { ePlanWeight = value; } }
    public float planWeight { get { return Mathf.Clamp(planWeight, 0, 1); } set { planWeight = value; } }
    //?!?!?!? vafan är ens denna?
    public float planingAmount { get { return Mathf.Clamp(timeWeight, 0, 1); } set { timeWeight = value; } }
    public float eTimeWeight { get { return Mathf.Clamp(eTimeWeight, 0, 1); } set { eTimeWeight = value; } }
    public float timeWeight { get { return Mathf.Clamp(timeWeight, 0, 1); } set { timeWeight = value; } }
    //?!?!?!? vafan är ens denna?
    public float timeSpan { get { return Mathf.Clamp(timeWeight, 0, 1); } set { timeWeight = value; } }
    public float eOptWeight { get { return Mathf.Clamp(eTimeWeight, 0, 1); } set { eTimeWeight = value; } }

}
