using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeEditor;
using System.Linq;

[CreateAssetMenu(fileName = "EnemyAIBT", menuName = "BehaviorTrees/EnemyAIBT", order = 0)]
public class BehaviourTree : ScriptableObject
{
    public Composite topNode;
    public Context context;


    public Composite topNodeInstance;

    public BTDataContainer btData;
    public BTDataContainer btDataInstance;

    public void ConstructBehaviourTree()
    {
        btDataInstance = Instantiate(btData);

        topNodeInstance = InitializeNodes(GetTopNode()).compositeInstance;
    }

    Stack<NodeData> nodeStack = new Stack<NodeData>();

    private NodeData InitializeNodes(NodeData node)
    {
        foreach (var child in GetChildNodes(node.GUID))
        {
            nodeStack.Push(InitializeNodes(child));
        }

        switch ((NodeTypes)node.nodeType)
        {
            case NodeTypes.Composite:
                Composite compositeDuplicate = Instantiate(node.compositeInstance);
                List<NodeData> childDataNodes = GetChildNodes(node.GUID);
                List<AbstractNode> childNodes = new List<AbstractNode>();

                for (int i = 0; i < childDataNodes.Count; i++)
                {
                    childNodes.Add(GetNodeInstance(nodeStack.Pop()));
                }

                childNodes.Reverse();

                compositeDuplicate.context = btData.context;
                compositeDuplicate.Construct(childNodes);

                node.compositeInstance = compositeDuplicate;

                return node;

            case NodeTypes.Decorator:
                Decorator decoratorDuplicate = Instantiate(node.decoratorInstance);

                decoratorDuplicate.context = btData.context;
                decoratorDuplicate.Construct(GetNodeInstance(nodeStack.Pop()));

                node.decoratorInstance = decoratorDuplicate;
                return node;
            case NodeTypes.Action:
                Action actionDuplicate = Instantiate(node.actionInstance);

                actionDuplicate.context = btData.context;
                actionDuplicate.Construct();

                node.actionInstance = actionDuplicate;
                return node;
            default:
                break;
        }

        return null;
    }

    private AbstractNode GetNodeInstance(NodeData node)
    {
        switch ((NodeTypes)node.nodeType)
        {
            case NodeTypes.Composite:
                return node.compositeInstance;
            case NodeTypes.Decorator:
                return node.decoratorInstance;
            case NodeTypes.Action:
                return node.actionInstance;
            default:
                return null;
        }
    }

    private NodeData GetTopNode()
    {
        foreach (var node in btDataInstance.nodeData)
        {
            if (node.topNode)
            {
                return node;
            }
        }

        Debug.LogError("Couldn't find top node");
        return null;
    }

    public List<NodeData> GetChildNodes(string nodeGUID)
    {
        List<NodeLinkData> connections = btDataInstance.nodeLinks.Where(linkData => linkData.BaseNodeGuid == nodeGUID).ToList(); // Get connections from active container cache
        List<NodeData> childNodes = new List<NodeData>();

        // Loop through connections for a given node and find its child nodes via GUID matching
        for (int i = 0; i < connections.Count; i++)
        {
            string targetNodeGUID = connections[i].TargetNodeGuid;

            NodeData targetNode = btDataInstance.nodeData.First(nodeData => nodeData.GUID == targetNodeGUID);

            childNodes.Add(targetNode);
        }

        return childNodes;
    }
}
