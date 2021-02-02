﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Action
{
    [SerializeField] float threshold;
    private Health aiHealth;

    public override void Construct()
    {
        aiHealth = context.owner.GetComponent<Health>();

        _constructed = true;
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            return aiHealth.currentHealth <= threshold ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}