using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

namespace BehaviourTreeEditor
{
    /// <summary>
    /// Behaviour tree graph
    /// </summary>
    public class BTGraphView : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(200, 200);

        private NodeSearchWindow _searchWindow;
        public Blackboard Blackboard;
        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();

        #region GraphViewInitialization
        // Initialize graphview with manipulater presets
        public BTGraphView(BTEditorWindow editorWindow)
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            AddElement(GenerateEntryPointNode("Top Node"));
            AddSearchWindow(editorWindow);
        }

        // Generates and adds a search window to a given editorwindow
        private void AddSearchWindow(BTEditorWindow editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Init(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        public void ClearBlackBoardAndExposedProperties()
        {
            exposedProperties.Clear();
            Blackboard.Clear();
        }

        /// <summary>
        /// Adds property to graph blackboard
        /// </summary>
        /// <param name="exposedProperty"></param>
        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
        {
            string localPropertyName = exposedProperty.PropertyName;
            string localPropertyValue = exposedProperty.PropertyValue;

            // Check for duplicate names and add a (1) tag to the end of the name if it's a duplicate
            while (exposedProperties.Any(x => x.PropertyName == localPropertyName))
                localPropertyName = $"{localPropertyName}(1)";

            ExposedProperty property = new ExposedProperty();

            property.PropertyName = localPropertyName;
            property.PropertyValue = localPropertyValue;

            exposedProperties.Add(property);

            // Create a visual element containing all necessary stuff for blackboard field
            VisualElement container = new VisualElement();
            BlackboardField blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string property" };
            container.Add(blackboardField);

            // Property value text input
            TextField propertyValueTextField = new TextField("Value: ")
            {
                value = localPropertyValue
            };

            // Register change callback for property value
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                // Get index for property in list which matches the relevant property name
                int changingPropertyIndex = exposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
                // Change property value to new value
                exposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
            });

            BlackboardRow blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
            container.Add(blackBoardValueRow);

            Blackboard.Add(container);
        }
        #endregion

        #region Node Generation
        // Generates entry point node at editor startup
        private BTEditorNode GenerateEntryPointNode(string nodeName)
        {
            // Instantiate new node
            BTEditorNode node = new BTEditorNode
            {
                title = nodeName,
                GUID = System.Guid.NewGuid().ToString(),
                topNode = true,
                nodeType = NodeTypes.TopNode,
            };

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = "Top Node"
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Instantiate add port button
            Button button = new Button(() => { AddPort(node); });
            button.text = "New Child Behaviour";
            node.titleContainer.Add(button);

            AddPort(node);
            AddPort(node);

            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;

            // Refresh node
            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(new Vector2(100, 200), defaultNodeSize));

            return node;
        }


        /// <summary>
        /// Creates node and adds it to the editor window
        /// </summary>
        /// <param name="nodename"></param>
        /// <param name="type"></param>
        public void CreateNode(string nodename, NodeTypes type, Vector2 position)
        {
            AddElement(GenerateNode(nodename, type, position));
        }

        /// <summary>
        /// Generates and returns an node instance without adding it to the editor window
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public BTEditorNode GenerateNode(string nodeName, NodeTypes type, Vector2 position)
        {
            BTEditorNode node = null;

            switch (type)
            {
                case NodeTypes.TopNode:
                    node = GenerateEntryPointNode(nodeName);
                    break;
                case NodeTypes.Composite:
                    node = GenerateCompositeNode(nodeName, position);
                    break;
                case NodeTypes.Decorator:
                    node = GenerateDecoratorNode(nodeName, position);
                    break;
                case NodeTypes.Behaviour:
                    node = GenerateBehaviourNode(nodeName, position);
                    break;
                default:
                    break;
            }
            return node;
        }

        // Generate composite type node
        private BTEditorNode GenerateCompositeNode(string nodeName, Vector2 position)
        {
            BTEditorNode node = new BTEditorNode
            {
                title = nodeName,
                GUID = System.Guid.NewGuid().ToString(),
            };

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = nodeName
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Input port
            node.inputContainer.Add(GeneratePort(node, Direction.Input));

            // Add port button
            Button button = new Button(() => { AddPort(node); });
            button.text = "New Child Behaviour";
            node.titleContainer.Add(button);

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(position, defaultNodeSize));

            return node;
        }

        // Generate behaviour node
        private BTEditorNode GenerateBehaviourNode(string nodeName, Vector2 position)
        {
            BTEditorNode node = new BTEditorNode
            {
                title = nodeName,
                GUID = System.Guid.NewGuid().ToString(),
            };

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = nodeName
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Input port
            node.inputContainer.Add(GeneratePort(node, Direction.Input));

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(position, defaultNodeSize));

            return node;
        }

        // Generate decorator node
        private BTEditorNode GenerateDecoratorNode(string nodeName, Vector2 position)
        {
            BTEditorNode node = new BTEditorNode
            {
                title = nodeName,
                GUID = System.Guid.NewGuid().ToString(),
            };

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = nodeName
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Input/Output port
            node.inputContainer.Add(GeneratePort(node, Direction.Input, Port.Capacity.Multi));
            node.outputContainer.Add(GeneratePort(node, Direction.Output, Port.Capacity.Multi));

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(position, defaultNodeSize));

            return node;
        }
        #endregion

        #region Port Generation
        
        /// <summary>
        /// Adds a port to a target node
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="overriddenPortName"></param>
        public void AddPort(BTEditorNode targetNode, string overriddenPortName = "")
        {
            // Generate port
            Port generatedPort = GeneratePort(targetNode, Direction.Output);

            // Removes default label from port in favour of editable labels
            Label oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            // Adds default port name
            int outputPortCount = targetNode.outputContainer.Query("connector").ToList().Count;
            generatedPort.portName = $"Child Behaviour {outputPortCount}";

            // Check if default portname is overridden
            string portName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Child Behaviour {outputPortCount}"
                : overriddenPortName;

            // Adds name field
            TextField textField = new TextField
            {
                name = string.Empty,
                value = portName
            };

            textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("     "));
            generatedPort.contentContainer.Add(textField);

            // Adds delete port button
            Button deleteButton = new Button(() => RemovePort(targetNode, generatedPort))
            {
                text = "X"
            };

            generatedPort.contentContainer.Add(deleteButton);
            generatedPort.portName = portName;

            targetNode.outputContainer.Add(generatedPort);

            targetNode.RefreshPorts();
            targetNode.RefreshExpandedState();
        }

        // Removes port from target node
        private void RemovePort(BTEditorNode targetNode, Port portToRemove)
        {
            // Find port in node that matches port to remove
            IEnumerable<Edge> targetEdge = edges.ToList().Where(x => x.output.portName == portToRemove.portName && x.output.node == portToRemove.node);

            // If no edges got added to the list return
            if (!targetEdge.Any()) return; 

            // Get edge matching the above requirements and remove connections and then the port
            Edge edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());

            targetNode.outputContainer.Remove(portToRemove);
            targetNode.RefreshPorts();
            targetNode.RefreshExpandedState();
        }

        // Generate a port
        private Port GeneratePort(BTEditorNode target, Direction portDir, Port.Capacity capacity = Port.Capacity.Single)
        {
            return target.InstantiatePort(Orientation.Horizontal, portDir, capacity, typeof(float));
        }

        /// <summary>
        /// Get compatible ports for a node connection
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }
        #endregion
    }
}