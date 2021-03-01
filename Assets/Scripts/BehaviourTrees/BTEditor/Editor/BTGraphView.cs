using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;
using UnityEditor.UIElements;
using System.Reflection;

namespace BehaviourTreeEditor
{
    /// <summary>
    /// Behaviour tree graph
    /// </summary>
    public class BTGraphView : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(200, 200);
        public AddNodeSearchWindow _addNodeSearchWindow;
        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();
        public NodeTypeData typeData;
        public ObjectField contextField;

        #region GraphViewInitialization
        // Initialize graphview with manipulater presets
        public BTGraphView(BTEditorWindow editorWindow)
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            contextField = new ObjectField { objectType = typeof(GlobalData) };

            AddElement(GenerateEntryPointNode("Top Node"));
            AddNodeSearchWindow(editorWindow);
        }

        // Generates and adds a search window to a given editorwindow
        private void AddNodeSearchWindow(BTEditorWindow editorWindow)
        {
            _addNodeSearchWindow = ScriptableObject.CreateInstance<AddNodeSearchWindow>();
            _addNodeSearchWindow.Init(editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _addNodeSearchWindow);
        }
        #endregion

        #region TypeData
        /// <summary>
        /// Loads type data object from resources folder
        /// </summary>
        public void LoadTypeData()
        {
            Debug.Log("loading typedata");

            typeData = Resources.Load("TypeData") as NodeTypeData;

            if (typeData == null)
            {
                EditorUtility.DisplayDialog("No typedata object found", "The type data object could not be found in the resources folder", "ok");
                return;
            }

            // TODO need to sort typedata.paths array for proper menu creation
            foreach (Type type in typeof(AbstractNode).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(AbstractNode))))
            {
                object[] attributes = type.GetCustomAttributes(typeof(AddNodeMenu), false);

                if (attributes.Length > 0)
                {
                    AddNodeMenu attribute = attributes[0] as AddNodeMenu;

                    NodeTypeData.NodePathData pathData = new NodeTypeData.NodePathData();

                    pathData.path = attribute.menuPath.Split('/');
                    pathData.name = attribute.nodeName;

                    if (type.IsSubclassOf(typeof(Action)))
                    {
                        pathData.nodeType = NodeTypes.Action;
                    }
                    else if(type.IsSubclassOf(typeof(Composite)))
                    {
                        pathData.nodeType = NodeTypes.Composite;
                    }
                    else if(type.IsSubclassOf(typeof(Decorator)))
                    {
                        pathData.nodeType = NodeTypes.Decorator;
                    }

                    typeData.paths.Add(pathData);
                }

            }
        }

        /// <summary>
        /// Save type data as NodeTypeData scriptable object in resources folder
        /// </summary>
        public void SaveTypeData()
        {
            NodeTypeData typeData = ScriptableObject.CreateInstance<NodeTypeData>();

            if (!AssetDatabase.IsValidFolder("Assets/BehaviourTrees"))
                AssetDatabase.CreateFolder("Assets", "BehaviourTrees");

            if (!AssetDatabase.IsValidFolder("Assets/BehaviourTrees/Resources"))
                AssetDatabase.CreateFolder("Assets/BehaviourTrees", "Resources");

            if (!File.Exists("Assets/BehaviourTrees/Resources/TypeData.asset"))
            {
                AssetDatabase.CreateAsset(typeData, $"Assets/BehaviourTrees/Resources/TypeData.asset");
                AssetDatabase.SaveAssets();
            }
        }
        #endregion

        #region Node Script Generation
        private const string ACTION_TEMPLATE_PATH = "ActionTemplate.txt";
        private const string COMPOSITE_TEMPLATE_PATH = "CompositeTemplate.txt";
        private const string DECORATOR_TEMPLATE_PATH = "DecoratorTemplate.txt";
        private const string TEMPLATE_FOLDER_PATH = "Assets/Scripts/BehaviourTrees/BTEditor/ScriptTemplates/";
        private const string ACTION_CLASS_NAME = "#CLASS_NAME_HERE#";
        
        /// <summary>
        /// Creates new node script in the AIBehaviours folder based on template classes
        /// </summary>
        /// <param name="behaviourName"></param>
        /// <param name="type"></param>
        public void CreateNewNode(string behaviourName, NodeTypes type)
        {
            SaveTypeData();

            TextAsset templateTextFile = null;

            // Loads different template based on node type and adds behaviour name to corresponding node list
            switch (type)
            {
                case NodeTypes.Composite:
                    templateTextFile = (TextAsset)AssetDatabase.LoadAssetAtPath(TEMPLATE_FOLDER_PATH + COMPOSITE_TEMPLATE_PATH, typeof(TextAsset));
                    typeData.compositeNodes.Add(behaviourName);
                    break;
                case NodeTypes.Decorator:
                    templateTextFile = (TextAsset)AssetDatabase.LoadAssetAtPath(TEMPLATE_FOLDER_PATH + DECORATOR_TEMPLATE_PATH, typeof(TextAsset));
                    typeData.decoratorNodes.Add(behaviourName);
                    break;
                case NodeTypes.Action:
                    templateTextFile = (TextAsset)AssetDatabase.LoadAssetAtPath(TEMPLATE_FOLDER_PATH + ACTION_TEMPLATE_PATH, typeof(TextAsset));
                    typeData.behaviourNodes.Add(behaviourName);
                    break;
            }

            string content = "";

            // Avoid potential incorrect naming
            behaviourName = behaviourName.Replace(" ", "");

            // If textfile returns replace keywords in textfile with variables 
            if (templateTextFile != null)
            {
                content = templateTextFile.text;
                content = content.Replace(ACTION_CLASS_NAME, behaviourName);
            }
            else
            {
                Debug.LogError("Can't find behaviour template text");
            }

            // If no folder for behaviours create the folder
            if (!AssetDatabase.IsValidFolder("Assets/AIBehaviours"))
                AssetDatabase.CreateFolder("Assets", "AIBehaviours");
 
            // Use streamwriter to create a new .cs file with the correct name in the behaviours folder
            using (StreamWriter sw = new StreamWriter(string.Format(Application.dataPath + $"/AIBehaviours/{behaviourName}.cs", new object[] { behaviourName.Replace(" ", "") })))
            {
                sw.Write(content);
            }
        }

        public void TestPrintBehaviourLists()
        {
            Debug.Log(typeData.behaviourNodes.Count);

            foreach (string name in typeData.behaviourNodes)
            {
                Debug.Log("Behaviour: " + name);
            }

            foreach (string name in typeData.compositeNodes)
            {
                Debug.Log("Composite: " + name);
            }

            foreach (string name in typeData.decoratorNodes)
            {
                Debug.Log("Decorator: " + name);
            }
        }

        #endregion

        #region Node Creation
        // Generates entry point node at editor startup
        private BTEditorNode GenerateEntryPointNode(string nodeName)
        {
            // Instantiate new node
            BTEditorNode node = new BTEditorNode
            {
                title = nodeName,
                nodeName = "Selector",
                compositeInstance = (Composite)ScriptableObject.CreateInstance("Selector"),
                GUID = System.Guid.NewGuid().ToString(),
                nodeType = NodeTypes.Composite,
                topNode = true,
            };

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = nodeName,
                value = "Top Node"
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minixmize button in title container after adding title input field

            // Node state element
            Label nodeStateLabel = new Label { name = "node-state-label", text = node.compositeInstance.NodeState.ToString() };
            node.titleContainer.Add(nodeStateLabel);

            // Instantiate add port button
            Button button = new Button(() => { AddPort(node); });
            button.text = "New Child Behaviour";
            node.titleContainer.Add(button);

            // Instance field
            ObjectField objectField = new ObjectField();
            objectField.objectType = typeof(ScriptableObject);
            objectField.value = node.compositeInstance;
            node.mainContainer.Add(objectField);

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
        public void CreateNode(string nodeTitle, string nodename, NodeTypes type, Vector2 position, bool isTopNode = false, AbstractNode instance = null)
        {
            AddElement(GenerateNode(nodeTitle, nodename, type, position, isTopNode, instance));
        }

        /// <summary>
        /// Generates and returns an node instance without adding it to the editor window
        /// </summary>
        /// <param name="nodeTitle"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public BTEditorNode GenerateNode(string nodeTitle, string nodeName, NodeTypes type, Vector2 position, bool isTopNode = false, AbstractNode instance = null)
        {
            BTEditorNode node = null;

            switch (type)
            {
                case NodeTypes.Composite:
                    node = GenerateCompositeNode(nodeTitle, nodeName, position, isTopNode, instance);
                    break;
                case NodeTypes.Decorator:
                    node = GenerateDecoratorNode(nodeTitle, nodeName, position, isTopNode, instance);
                    break;
                case NodeTypes.Action:
                    node = GenerateActionNode(nodeTitle, nodeName, position, isTopNode, instance);
                    break;
                default:
                    break;
            }
            return node;
        }

        // Generate composite type node
        private BTEditorNode GenerateCompositeNode(string nodeTitle, string name, Vector2 position, bool isTopNode = false, AbstractNode instance = null)
        {
            BTEditorNode node = new BTEditorNode
            {
                title = nodeTitle,
                nodeName = name,
                GUID = System.Guid.NewGuid().ToString(),
                nodeType = NodeTypes.Composite,
                topNode = isTopNode
            };

            // Create new instance if not loading from already existing behaviour tree
            if (instance != null)
                node.compositeInstance = instance as Composite;
            else
                node.compositeInstance = ScriptableObject.CreateInstance(name) as Composite;

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = node.title
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Input port
            node.inputContainer.Add(GeneratePort(node, Direction.Input));

            // Node state element
            Label nodeStateLabel = new Label { name = "node-state-label", text = node.compositeInstance.NodeState.ToString() };
            node.titleContainer.Add(nodeStateLabel);

            // Add port button
            Button button = new Button(() => { AddPort(node); });
            button.text = "New Child Behaviour";
            node.titleContainer.Add(button);

            // Object field for behaviour instance
            ObjectField objectField = new ObjectField();
            objectField.objectType = typeof(AbstractNode);
            objectField.value = node.compositeInstance;
            node.mainContainer.Add(objectField);

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(position, defaultNodeSize));

            return node;
        }

        // Generate behaviour node
        private BTEditorNode GenerateActionNode(string nodeTitle, string name, Vector2 position, bool isTopNode = false, AbstractNode instance = null)
        {
            BTEditorNode node = new BTEditorNode
            {
                title = nodeTitle,
                nodeName = name,
                GUID = System.Guid.NewGuid().ToString(),
                nodeType = NodeTypes.Action,
                topNode = isTopNode
            };

            // Create new instance if not loading from already existing behaviour tree
            if (instance != null)
                node.actionInstance = instance as Action;
            else
                node.actionInstance = ScriptableObject.CreateInstance(name) as Action;

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = node.title
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Input port
            node.inputContainer.Add(GeneratePort(node, Direction.Input));

            // Object field for behaviour instance
            ObjectField objectField = new ObjectField();
            objectField.objectType = typeof(ScriptableObject);
            objectField.value = node.actionInstance;
            node.mainContainer.Add(objectField);

            // Node state element
            Label nodeStateLabel = new Label { name = "node-state-label", text = node.actionInstance.NodeState.ToString() };
            node.titleContainer.Add(nodeStateLabel);

            node.RefreshExpandedState();
            node.RefreshPorts();

            node.SetPosition(new Rect(position, defaultNodeSize));

            return node;
        }

        // Generate decorator node
        private BTEditorNode GenerateDecoratorNode(string nodeTitle, string name, Vector2 position, bool isTopNode = false, AbstractNode instance = null)
        {
            BTEditorNode node = new BTEditorNode
            {
                title = nodeTitle,
                nodeName = name,
                GUID = System.Guid.NewGuid().ToString(),
                nodeType = NodeTypes.Decorator,
                topNode = isTopNode
            };

            // Create new instance if not loading from already existing behaviour tree
            if (instance != null)
                node.decoratorInstance = instance as Decorator;
            else
                node.decoratorInstance = ScriptableObject.CreateInstance(name) as Decorator;

            // Stash and remove old title and minimize button elements
            Label oldTitleLabel = node.titleContainer.Q<Label>("title-label");
            node.titleContainer.Remove(oldTitleLabel);

            VisualElement oldTitleButton = node.titleContainer.Q<VisualElement>("title-button-container");
            node.titleContainer.Remove(oldTitleButton);

            // Create and add text field for title input
            TextField textField = new TextField
            {
                name = string.Empty,
                value = node.title
            };

            textField.RegisterValueChangedCallback(evt => node.title = evt.newValue);
            node.titleContainer.Add(textField);
            node.titleContainer.Add(oldTitleButton); // Add back minimize button in title container after adding title input field

            // Input/Output port
            node.inputContainer.Add(GeneratePort(node, Direction.Input, Port.Capacity.Multi));
            node.outputContainer.Add(GeneratePort(node, Direction.Output, Port.Capacity.Multi));

            // Object field for behaviour instance
            ObjectField objectField = new ObjectField();
            objectField.objectType = typeof(ScriptableObject);
            objectField.value = node.decoratorInstance;
            node.mainContainer.Add(objectField);

            // Node state element
            Label nodeStateLabel = new Label { name = "node-state-label", text = node.decoratorInstance.NodeState.ToString() };
            node.titleContainer.Add(nodeStateLabel);

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
            generatedPort.portName = outputPortCount.ToString();

            // Check if default portname is overridden
            string portName = string.IsNullOrEmpty(overriddenPortName)
                ? outputPortCount.ToString()
                : overriddenPortName;

            // Adds name field
            TextField textField = new TextField
            {
                name = string.Empty,
                value = portName
            };

            // textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
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

            // If no edges got added to the list only remove port
            if (!targetEdge.Any()) 
            {
                targetNode.outputContainer.Remove(portToRemove);
            }
            else // If edges remove edges then remove port
            {
                // Get edge matching the above requirements and remove connections and then the port
                Edge edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());

                targetNode.outputContainer.Remove(portToRemove);
                targetNode.RefreshPorts();
                targetNode.RefreshExpandedState();
            }
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