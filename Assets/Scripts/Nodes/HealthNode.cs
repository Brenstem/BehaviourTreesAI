using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : BehaviourNode
{
    private EnemyAI ai;
    private float threshold;

    public HealthNode(EnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeStates Evaluate()
    {
        return ai.CurrentHealth <= threshold ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }
}
