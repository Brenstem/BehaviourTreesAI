using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddNodeMenu("Actions/Animations", "PlayAnimationNode")]
public class PlayAnimationNode : Action
{
    [Header("Node variables")]
    [SerializeField] private string animVarName;
    [SerializeField] private bool animBoolValue;

    [SerializeField] private bool waitForAnimation;
    [SerializeField] private string animationName;

    private Animator animator;
    private Timer timer;
    private bool animationDone = false;

    public override void Construct()
    {
        _constructed = true;
        animator = context.owner.GetComponentInChildren<Animator>();
        timer = new Timer(1f, () => { animationDone = true; timer.Reset(); });
    }

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            if (animator != null)
            {
                animator.SetBool(animVarName, animBoolValue);

                if (!waitForAnimation)
                {
                    NodeState = NodeStates.SUCCESS;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                {
                    animationDone = false;

                    timer.DecrementTimer(Time.fixedDeltaTime);

                    if (!animationDone)
                    {
                        NodeState = NodeStates.RUNNING;
                        Debug.Log("running");
                    }
                    else
                    {
                        NodeState = NodeStates.SUCCESS;
                    }
                }
                else
                {
                    NodeState = NodeStates.SUCCESS;
                }
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
