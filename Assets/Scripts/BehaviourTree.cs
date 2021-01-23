using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIBT", menuName = "BehaviorTrees/EnemyAIBT", order = 0)]
public class BehaviourTree : ScriptableObject
{
    public Composite topNode;
    public Context context;

    public void ConstructBehaviourTree()
    {
        Stack<AbstractNode> constructStack = new Stack<AbstractNode>();

        constructStack.Push(topNode);

        while (constructStack.Count > 0)
        {
            AbstractNode currentNode = constructStack.Pop();

            if (currentNode.GetType().IsSubclassOf(typeof(Composite)))
            {
                Composite compositeNode = (Composite)currentNode;
                for (int i = 0; i < compositeNode.nodes.Count; i++)
                {
                    constructStack.Push(compositeNode.nodes[i]);
                }
            }
            else if (currentNode.GetType().IsSubclassOf(typeof(Decorator)))
            {
                Decorator decoratorNode = (Decorator)currentNode;
                constructStack.Push(decoratorNode.node);

            }
            else
            {
                Action actionNode = (Action)currentNode;
                actionNode.Construct();
            }
        }
    }
}
