using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

public class GraphSaveUtility
{
    private BTGraphView _targetGraphView;
    private BehaviourTreeContainer _containerCache;

    private List<Edge> edges => _targetGraphView.edges.ToList();
    private List<BTEditorNode> nodes => _targetGraphView.nodes.ToList().Cast<BTEditorNode>().ToList();

    public static GraphSaveUtility GetInstance(BTGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!edges.Any()) return; // If there are no connections than return

        BehaviourTreeContainer btContainer = ScriptableObject.CreateInstance<BehaviourTreeContainer>();

        // Save node connection 
        Edge[] connectedPorts = edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            BTEditorNode outputNode = connectedPorts[i].output.node as BTEditorNode;
            BTEditorNode inputNode = connectedPorts[i].input.node as BTEditorNode;

            btContainer.nodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        // Save individual node data
        foreach (BTEditorNode node in nodes.Where(node =>! node.topNode))
        {
            btContainer.nodeData.Add(new BehaviourNodeData
            {
                nodeName = node.nodeName,
                Guid = node.GUID,
                Position = node.GetPosition().position,
                nodeType = (int)node.nodeType
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/BTResources"))
        {
            AssetDatabase.CreateFolder("Assets", "BTResources");
        }

        AssetDatabase.CreateAsset(btContainer, $"Assets/BTResources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<BehaviourTreeContainer>(fileName);

        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "", "Okay daddy :(");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        throw new NotImplementedException();
    }

    private void CreateNodes()
    {
        foreach (BehaviourNodeData nodeData in _containerCache.nodeData)
        {
            // TODO https://youtu.be/OMDfr1dzBco?t=790

            BTEditorNode tempNode = _targetGraphView.GenerateBehaviournode(nodeData.nodeName, (BTGraphView.NodeTypes)nodeData.nodeType);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            List<NodeLinkData> nodePorts = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddPort(tempNode,x.PortName));
        }
    }

    private void ClearGraph()
    {
        // Set entry points guid based on save file
        nodes.Find(x => x.topNode).GUID = _containerCache.nodeLinks[0].BaseNodeGuid; 
        foreach (BTEditorNode node in nodes)
        {
            if (node.topNode) return;

            // Remove all connections to this node
            edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            // Then remove node
            _targetGraphView.RemoveElement(node);
        }
    }
}

