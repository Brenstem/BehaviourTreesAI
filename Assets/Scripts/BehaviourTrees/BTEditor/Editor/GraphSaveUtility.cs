using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BehaviourTreeEditor
{
    /// <summary>
    /// Save data utility for behaviour tree
    /// </summary>
    public class GraphSaveUtility
    {
        private BTGraphView _targetGraphView;
        private BTDataContainer _containerCache; // Data container for connection and node data

        private List<Edge> edges => _targetGraphView.edges.ToList();
        private List<BTEditorNode> nodes => _targetGraphView.nodes.ToList().Cast<BTEditorNode>().ToList();

        /// <summary>
        /// Returns an instance of the save utility for a given target graph view
        /// </summary>
        /// <param name="targetGraphView"></param>
        /// <returns></returns>
        public static GraphSaveUtility GetInstance(BTGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }

        /// <summary>
        /// Saves active graph data
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveGraph(string fileName)
        {
            string tempFileName = fileName.Replace(" ", "");

            // Generate data container
            BTDataContainer btContainer = ScriptableObject.CreateInstance<BTDataContainer>();

            // Save nodes to container
            if (!SaveNodes(btContainer)) return;

            SaveExposedProperties(btContainer);

            // Put savefile in Assets/BTResources
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(btContainer, $"Assets/Resources/{tempFileName}.asset");
            AssetDatabase.SaveAssets();
        }

        // Saves exposed blackboard properties
        private void SaveExposedProperties(BTDataContainer btContainer)
        {
            btContainer.exposedProperties.AddRange(_targetGraphView.exposedProperties);
        }

        // Saves nodes
        private bool SaveNodes(BTDataContainer btContainer)
        {
            // If there are no connections then display error
            if (!edges.Any())
            {
                EditorUtility.DisplayDialog("No connections", "Create some connections in your tree before saving!", "Ok");
                return false;
            }

            // Save node connections
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
            foreach (BTEditorNode node in nodes)
            {
                NodeData temp = new NodeData 
                {
                    nodeTitle = node.title,
                    nodeName = node.nodeName,
                    Guid = node.GUID,
                    Position = node.GetPosition().position,
                    nodeType = (int)node.nodeType,
                    topNode = node.topNode
                };

                if (node.compositeInstance != null)
                    temp.compositeInstance = (Composite)SaveNode(node.title + node.GUID, node.compositeInstance);
                else if (node.decoratorInstance != null)
                    temp.decoratorInstance = (Decorator)SaveNode(node.title + node.GUID, node.decoratorInstance);
                else if (node.actionInstance != null)
                    temp.actionInstance = (Action)SaveNode(node.title + node.GUID, node.actionInstance);

                btContainer.nodeData.Add(temp);
            }

            return true;
        }

        /// <summary>
        /// Loads savedata into graph view (overwrites active graph)
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadGraph(string fileName)
        {
            // Load data based on filename
            _containerCache = Resources.Load<BTDataContainer>(fileName);

            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Given file does not exist :(", "Ok");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }


        // Connect nodes based on save data
        private void ConnectNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                // Get connections from savedata where node guid matches data guid
                List<NodeLinkData> connections = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodes[i].GUID).ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].TargetNodeGuid;
                    Node targetNode = nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(_containerCache.nodeData.First(x => x.Guid == targetNodeGuid).Position,
                        _targetGraphView.defaultNodeSize));
                }
            }
        }

        // Link nodes visually in graph based on save data
        private void LinkNodes(Port output, Port input)
        {
            Edge edge = new Edge
            {
                output = output,
                input = input
            };

            edge?.input.Connect(edge);
            edge?.output.Connect(edge);

            _targetGraphView.Add(edge);
        }

        // Generate nodes based on save data
        private void CreateNodes()
        {
            BTEditorNode tempNode = null;

            foreach (NodeData nodeData in _containerCache.nodeData)
            {
                // Generate node based on node data. We pass node position later so we can use zerovector while loading
                switch ((NodeTypes) nodeData.nodeType)
                {
                    case NodeTypes.Composite:
                        tempNode = _targetGraphView.GenerateNode(nodeData.nodeTitle, nodeData.nodeName, (NodeTypes)nodeData.nodeType, Vector2.zero, nodeData.topNode, nodeData.compositeInstance);
                        tempNode.GUID = nodeData.Guid;
                        break;
                    case NodeTypes.Decorator:
                        tempNode = _targetGraphView.GenerateNode(nodeData.nodeTitle, nodeData.nodeName, (NodeTypes)nodeData.nodeType, Vector2.zero, nodeData.topNode, nodeData.decoratorInstance);
                        tempNode.GUID = nodeData.Guid;
                        break;
                    case NodeTypes.Behaviour:
                        tempNode = _targetGraphView.GenerateNode(nodeData.nodeTitle, nodeData.nodeName, (NodeTypes)nodeData.nodeType, Vector2.zero, nodeData.topNode, nodeData.actionInstance);
                        tempNode.GUID = nodeData.Guid;
                        break;
                    default:
                        break;
                }

                // Add ports to node based on node data. If it's a decorator node the port will be generated automatically so theres no need to add ports
                if (tempNode.nodeType != NodeTypes.Decorator)
                {
                    List<NodeLinkData> nodePorts = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                    nodePorts.ForEach(x => _targetGraphView.AddPort(tempNode, x.PortName));
                }

                _targetGraphView.AddElement(tempNode);
            }
        }

        // Clear the editor before loading a new graph
        private void ClearGraph()
        {
            foreach (BTEditorNode node in nodes)
            {
                // Remove all connections to this node
                edges.Where(edge => edge.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                // Then remove node
                _targetGraphView.RemoveElement(node);
            }
        }

        /// <summary>
        /// Returns list of child nodes based on node GUID
        /// </summary>
        /// <param name="nodeGUID"></param>
        /// <returns></returns>
        public List<BTEditorNode> GetChildNodes(string nodeGUID)
        {
            // Update cache to contain current nodes
            SaveGraph("temp");
            _containerCache = Resources.Load<BTDataContainer>("temp");

            List<NodeLinkData> connections = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodeGUID).ToList(); // Get connections from active container cache
            List<BTEditorNode> childNodes = new List<BTEditorNode>();

            // Loop through connections for a given node and find its child nodes via GUID matching
            for (int i = 0; i < connections.Count; i++)
            {
                string targetNodeGUID = connections[i].TargetNodeGuid;

                BTEditorNode targetNode = nodes.First(x => x.GUID == targetNodeGUID);

                childNodes.Add(targetNode);
            }

            return childNodes;
        }

        //TODO kanske inte behöver göra såhär, kanske räcker med en system.serializeable tag, kan vara värt att tästa 
        // Saves object with filename in path (Default is Assets/Resources)
        public ScriptableObject SaveNode(string fileName, ScriptableObject obj, string path = "Assets/Resources")
        {

            if (!AssetDatabase.IsValidFolder(path)) 
            {
                AssetDatabase.CreateFolder("Assets", "Resources"); // TODO fix createfolder to create folder in the correct place
            }

            if (AssetDatabase.GetAssetPath(obj) == "" || AssetDatabase.GetAssetPath(obj) == null)
            {
                AssetDatabase.CreateAsset(obj, $"{ path }/{ fileName }.asset");
                AssetDatabase.SaveAssets();
            }

            return obj;
        }
    }
}
