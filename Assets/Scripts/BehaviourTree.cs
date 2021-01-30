using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIBT", menuName = "BehaviorTrees/EnemyAIBT", order = 0)]
public class BehaviourTree : ScriptableObject
{
    public Composite topNode;
    public Context context;
    public Composite topNodeInstance;

    public BTDataContainer btData;

    public void ConstructBehaviourTree()
    {
        topNodeInstance = (Composite)InitializeNodes(topNode);
    }

    Stack<AbstractNode> constructStack = new Stack<AbstractNode>();

    private AbstractNode InitializeNodes(AbstractNode node)
    {
        // Run this for all of the nodes chilldren
        if (node.GetType().IsSubclassOf(typeof(Composite)))
        {
            Composite temp = (Composite)node;

            foreach (var child in temp.nodes)
            {
                constructStack.Push(InitializeNodes(child));
            }
        }
        else if (node.GetType().IsSubclassOf(typeof(Decorator)))
        {
            Decorator temp = (Decorator)node;

            constructStack.Push(InitializeNodes(temp.node));
        }

        // Instantiate the node
        if (node.GetType().IsSubclassOf(typeof(Composite)))
        {
            Composite temp = (Composite)node;

            List<AbstractNode> childNodes = new List<AbstractNode>();

            for (int i = 0; i < temp.nodes.Count; i++)
            {
                childNodes.Add(constructStack.Pop());
            }

            childNodes.Reverse();

            temp = Instantiate(temp);

            temp.context = context;
            temp.Construct(childNodes);

            return temp;
        }
        else if (node.GetType().IsSubclassOf(typeof(Decorator)))
        {
            Decorator temp = (Decorator)node;

            AbstractNode child = constructStack.Pop();

            temp = Instantiate(temp);

            temp.context = context;
            temp.Construct(child);

            return temp;
        }
        else if (node.GetType().IsSubclassOf(typeof(Action)))
        {
            Action temp = Instantiate((Action)node);

            temp.context = context;
            temp.Construct();

            return temp;
        }

        return null;
    }

    //private AbstractNode InitializeNodes(BTEditorNode node)
    //{
    //    // Get save utility to use for getting child nodes
    //    GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(_graphView);

    //    if (ConvertEditorNode(node) != null)
    //    {
    //        // Recursively initialize children and add them to stack
    //        foreach (var child in saveUtility.GetChildNodes(node.GUID, _fileName))
    //        {
    //            nodeStack.Push(InitializeNodes(child));
    //        }

    //        // Construct nodes based on node type
    //        switch (node.nodeType)
    //        {
    //            case NodeTypes.Composite:
    //                List<BTEditorNode> childEditorNodes = saveUtility.GetChildNodes(node.GUID, _fileName);
    //                List<AbstractNode> childNodes = new List<AbstractNode>();

    //                // loop for the amount of children and pop them from the stack into list of nodes to be used for constructing
    //                for (int i = 0; i < childEditorNodes.Count; i++)
    //                {
    //                    childNodes.Add(nodeStack.Pop());
    //                }

    //                // Reverse list since nodes are popped in reverse order
    //                childNodes.Reverse();

    //                node.compositeInstance.Construct(childNodes);
    //                node.compositeInstance.context = (Context)_graphView.contextField.value;

    //                if (node.topNode) // If topnode then save with the name of the behaviourtree
    //                {
    //                    string fileName = node.nodeName + "TopNode";

    //                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(node.compositeInstance), fileName);
    //                }
    //                return node.compositeInstance;

    //            case NodeTypes.Decorator:
    //                // Construct node
    //                node.decoratorInstance.Construct(nodeStack.Pop());
    //                node.decoratorInstance.context = (Context)_graphView.contextField.value;

    //                return node.decoratorInstance;

    //            case NodeTypes.Action:
    //                // Construct node
    //                node.actionInstance.context = (Context)_graphView.contextField.value;

    //                return node.actionInstance;
    //            default:
    //                break;
    //        }
    //    }
    //    return null;
    //}
}
