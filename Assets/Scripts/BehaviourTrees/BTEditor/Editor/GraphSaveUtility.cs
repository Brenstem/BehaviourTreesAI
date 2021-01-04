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
            // Generate data container
            BTDataContainer btContainer = ScriptableObject.CreateInstance<BTDataContainer>();

            // Save nodes to container
            if (!SaveNodes(btContainer)) return;

            SaveExposedProperties(btContainer);

            // Put savefile in Assets/BTResources
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(btContainer, $"Assets/Resources/{fileName}.asset");
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
                btContainer.nodeData.Add(new NodeData
                {
                    nodeName = node.title,
                    Guid = node.GUID,
                    Position = node.GetPosition().position,
                    nodeType = (int)node.nodeType
                });
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
            CreateExposedProperties();
        }

        // Generate exposed blackboardproperties from savedata
        private void CreateExposedProperties()
        {
            // Clear current blackboard
            _targetGraphView.ClearBlackBoardAndExposedProperties();

            // Add properties from data
            foreach (ExposedProperty exposedProperty in _containerCache.exposedProperties)
            {
                _targetGraphView.exposedProperties.Add(exposedProperty);
            }
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

        // TODO fix this shitty ass function
        // Problem is that we need an instance of container cache to get our connections from and theres only an instance of container cache when we load a behaviour tree into the graph view :((

        /// <summary>
        /// Returns list of child nodes based on node GUID
        /// </summary>
        /// <param name="nodeGUID"></param>
        /// <returns></returns>
        public List<Node> GetChildNodes(string nodeGUID)
        {
            // Update cache to contain current nodes
            SaveNodes(_containerCache);

            List<NodeLinkData> connections = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodeGUID).ToList(); // Get connections from active container cache
            List<Node> childNodes = new List<Node>();

            // Loop through connections for a given node and find its child nodes via GUID matching
            for (int i = 0; i < connections.Count; i++)
            {
                string targetNodeGUID = connections[i].TargetNodeGuid;

                Node targetNode = nodes.First(x => x.GUID == targetNodeGUID);

                childNodes.Add(targetNode);
            }

            return childNodes;
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
            foreach (NodeData nodeData in _containerCache.nodeData)
            {
                // Generate node based on node data. We pass node position later so we can use zerovector while loading
                BTEditorNode tempNode = _targetGraphView.GenerateNode(nodeData.nodeName, (NodeTypes)nodeData.nodeType, Vector2.zero);
                tempNode.GUID = nodeData.Guid;

                // Add ports to node based on node data
                List<NodeLinkData> nodePorts = _containerCache.nodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddPort(tempNode, x.PortName));

                _targetGraphView.AddElement(tempNode);
            }
        }

        // Clear the editor before loading a new graph
        private void ClearGraph()
        {
            // Set entry points guid based on save file
            nodes.Find(x => x.topNode).GUID = _containerCache.nodeLinks[0].BaseNodeGuid;
            foreach (BTEditorNode node in nodes)
            {
                // if (node.topNode) continue;

                // Remove all connections to this node
                edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                // Then remove node
                _targetGraphView.RemoveElement(node);
            }
        }
    }
}
