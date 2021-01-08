using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : BehaviourNode
{
    private AlexEnemyAI ai;
    private float threshold;

    public override void Construct(Context context)
    {
        this.context = context;
        ai = context.localData.thisAI;

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            return ai.health.currentHealth <= threshold ? NodeStates.SUCCESS : NodeStates.FAILURE;
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
