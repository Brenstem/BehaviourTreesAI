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
    [SerializeField] private string animationToWaitFor;



    private Animator animator;
    //private Timer timer;
    private bool animationDone = false;
    bool animationPlaying = false;
    public override void Construct()
    {
        _constructed = true;
        animator = context.owner.GetComponentInChildren<Animator>();
        //timer = new Timer(1f, () => { animationDone = true; timer.Reset(); });
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
                else
                {
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName(animationToWaitFor))
                    {
                        NodeState = NodeStates.SUCCESS;
                    }
                    else
                    {
                        NodeState = NodeStates.RUNNING;
                    }
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
