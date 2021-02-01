using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIBT", menuName = "BehaviorTrees/EnemyAIBT", order = 0)]
public class BehaviourTree : ScriptableObject
{
    public Composite topNode;
    public Context context;

    public Composite topNodeInstance;

    public void ConstructBehaviourTree()
    {
        topNodeInstance = (Composite)InitializeNodes(topNode);

        //context.localData = new LocalData

        //topNodeInstance = Instantiate(topNode);
        //constructStack.Push(topNodeInstance);

        //while (constructStack.Count > 0)
        //{
        //    AbstractNode currentNode = constructStack.Pop();

        //    if (currentNode.GetType().IsSubclassOf(typeof(Composite)))
        //    {
        //        Composite compositeNode = (Composite)currentNode;

        //        if (compositeNode.nodes.Count == 0)
        //        {
        //            Debug.Log("wat");
        //        }

        //        for (int i = 0; i < compositeNode.nodes.Count; i++)
        //        {
        //            compositeNode.nodes[i] = Instantiate(compositeNode.nodes[i]);
        //            constructStack.Push(compositeNode.nodes[i]);
        //        }

        //        compositeNode.Construct(compositeNode.nodes);
        //        compositeNode.context = context;
        //    }
        //    else if (currentNode.GetType().IsSubclassOf(typeof(Decorator)))
        //    {
        //        Decorator decoratorNode = (Decorator)currentNode;
        //        decoratorNode.Construct(Instantiate(decoratorNode.node));
        //        constructStack.Push(decoratorNode.node);
        //        decoratorNode.context = context;
        //    }
        //    else
        //    {
        //        Action actionNode = (Action)currentNode;
        //        actionNode.context = context;
        //        actionNode.Construct();
        //    }
        //}
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
