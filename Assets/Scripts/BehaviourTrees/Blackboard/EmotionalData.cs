using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmotionalData", menuName = "BehaviourTrees/EmotionalData", order = 1)]
public class EmotionalData : ScriptableObject
{
    [SerializeField][Range(0, 1)] private float happiness;
    [SerializeField][Range(0, 1)] private float anxiety;
    [SerializeField][Range(0, 1)] private float anger;
    [SerializeField][Range(0, 1)] private float sadness;
    [SerializeField][Range(0, 1)] private float exhaustion;
    [SerializeField][Range(0, 1)] private float eRiskWeight;
    [SerializeField][Range(0, 1)] private float riskWeight;
    [SerializeField][Range(0, 1)] private float ePlanWeight;
    [SerializeField][Range(0, 1)] private float planWeight;
    [SerializeField][Range(0, 1)] private float planningAmount;
    [SerializeField][Range(0, 1)] private float eTimeWeight;
    [SerializeField][Range(0, 1)] private float timeWeight;
    [SerializeField][Range(0, 1)] private float timeSpan;
    [SerializeField][Range(0, 1)] private float eOptWeight;
    [SerializeField][Range(0, 1)] private float distribution;

    public float Happiness { get { return Mathf.Clamp(happiness, 0, 1); } set { happiness = value; } }
    public float Anxiety { get { return Mathf.Clamp(anxiety, 0, 1); } set { anxiety = value; } }
    public float Anger { get { return Mathf.Clamp(anger, 0, 1); } set { anger = value; } }
    public float Sadness { get { return Mathf.Clamp(sadness, 0, 1); } set { sadness = value; } }
    public float Exhaustion { get { return Mathf.Clamp(exhaustion, 0, 1); } set { exhaustion = value; } }
    public float ERiskWeight { get { return Mathf.Clamp(eRiskWeight, 0, 1); } set { eRiskWeight = value; } }
    public float RiskWeight { get { return Mathf.Clamp(riskWeight, 0, 1); } set { riskWeight = value; } }
    public float EPlanWeight { get { return Mathf.Clamp(ePlanWeight, 0, 1); } set { ePlanWeight = value; } }
    public float PlanWeight { get { return Mathf.Clamp(planWeight, 0, 1); } set { planWeight = value; } }
    //?!?!?!? vafan är ens denna?
    public float PlanningAmount { get { return Mathf.Clamp(planningAmount, 0, 1); } set { planningAmount = value; } }
    public float ETimeWeight { get { return Mathf.Clamp(eTimeWeight, 0, 1); } set { eTimeWeight = value; } }
    public float TimeWeight { get { return Mathf.Clamp(timeWeight, 0, 1); } set { timeWeight = value; } }
    //?!?!?!? vafan är ens denna?
    public float TimeSpan { get { return Mathf.Clamp(timeSpan, 0, 1); } set { timeSpan = value; } }
    public float EOptWeight { get { return Mathf.Clamp(eOptWeight, 0, 1); } set { eOptWeight = value; } }
    public float Distribution { get { return Mathf.Clamp(distribution, 0, 1); } set { distribution = value; } }
}
