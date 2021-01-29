using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concurrent : Composite
{
    private Dictionary<BaseAI, int> runningAIDictionary = new Dictionary<BaseAI, int>();

    private BaseAI currentOwner;

    public override NodeStates Evaluate()
    {
        if (_constructed)
        {
            currentOwner = context.owner;

            //if a node returned running last evaluate, we start evaluating from that node
            if (runningAIDictionary.ContainsKey(currentOwner))
            {
                return EvaluateFromIndex(runningAIDictionary[currentOwner]);
            }
            else
            {
                return EvaluateFromIndex(0);
            }
        }
        else
        {
            Debug.LogError("Node not constructed!");
            return NodeStates.FAILURE;
        }
    }

    private NodeStates EvaluateFromIndex(int startingIndex)
    {
        for (int i = startingIndex; i < nodes.Count; i++)
        {
            AbstractNode node = nodes[i];

            switch (node.Evaluate())
            {
                case NodeStates.RUNNING:
                    NodeState = NodeStates.RUNNING;

                    if (!runningAIDictionary.ContainsKey(currentOwner)) // If behaviour returns running and owner is not already in list of running AIs add current owner to that list
                        runningAIDictionary.Add(currentOwner, i);

                    return NodeState;

                case NodeStates.SUCCESS:
                    if (runningAIDictionary.ContainsKey(currentOwner)) // If behaviour returns success remove owner from running AI list and keep evaluating behaviours
                        runningAIDictionary.Remove(currentOwner);
                    break;

                case NodeStates.FAILURE:
                    if (runningAIDictionary.ContainsKey(currentOwner)) // If behaviour returns fail remove owner from running AI list and stop evaluating behaviours. 
                        runningAIDictionary.Remove(currentOwner);

                    NodeState = NodeStates.FAILURE;
                    return NodeState;
            }
        }

        NodeState = NodeStates.SUCCESS;
        return NodeState;
    }
}