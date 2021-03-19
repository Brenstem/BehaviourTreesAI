using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Animations", "PlayAnimationNode")]
public class PlayAnimationNode : Action
{
    [Header("Node variables")]
    [SerializeField] private string animVarName;
    [SerializeField] private bool animBoolValue;

    private Animator animator;

    public override void Construct()
    {
        _constructed = true;
        animator = context.owner.GetComponentInChildren<Animator>();
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (animator != null)
            {
                animator.SetBool(animVarName, true);
            }
            else
            {
                NodeState = NodeStates.FAILURE;
            }

            return NodeState;
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }
}
