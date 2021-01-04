using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : BehaviourNode<HealthNodeParameters>
{
    private EnemyAI ai;
    private float threshold;

    public override void Construct(HealthNodeParameters parameters)
    {
        ai = parameters.ai;
        threshold = parameters.threshold;

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            return ai.CurrentHealth <= threshold ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}

public class HealthNodeParameters
{
    public EnemyAI ai;
    public float threshold;

    public HealthNodeParameters(EnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }
}
