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
    [SerializeField] private string[] animationNames;



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
                //else if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                else
                {
                    animationDone = false;

                    timer.DecrementTimer(Time.fixedDeltaTime);

                    bool animPlaying = false;

                    //TODO fixa detta med alla animationer och kolla om det funkar, i så fall gör så om ingen animation spelas så returnar den success

                    foreach (string animationName in animationNames)
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                        {
                            animPlaying = true;
                            Debug.Log(animationName);
                            break;
                        }
                    }

                    if (!animPlaying)
                    {
                        Debug.Log("no anim playing");
                    }

                    if (!animationDone)
                    {
                        NodeState = NodeStates.RUNNING;
                    }
                    else
                    {
                        Debug.Log("Animation was done");
                        NodeState = NodeStates.SUCCESS;
                    }
                }
                //else
                //{
                //    NodeState = NodeStates.SUCCESS;
                //}
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
