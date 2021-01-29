using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace BehaviourTreeEditor
{
    /// <summary>
    ///  Behaviour tree editor window
    /// </summary>
    public class BTEditorWindow : EditorWindow
    {
        private BTGraphView _graphView;
        private string _fileName = "New Behaviour Tree";
        private string _newBehaviourName = "New Behaviour Name";
        public static EditorWindow window;

        private Stack<AbstractNode> nodeStack;

        private void Update()
        {
            if (Application.isPlaying)
            {
                RequestDataOperation(false);

                // Update node state labels in editor
                foreach (BTEditorNode node in _graphView.nodes.ToList())
                {
                    Label nodeStateLabel = node.titleContainer.Q<Label>("node-state-label");

                    switch (node.nodeType)
                    {
                        case NodeTypes.Composite:
                            nodeStateLabel.text = node.compositeInstance.NodeState.ToString();
                            break;
                        case NodeTypes.Decorator:
                            nodeStateLabel.text = node.decoratorInstance.NodeState.ToString();
                            break;
                        case NodeTypes.Action:
                            nodeStateLabel.text = node.actionInstance.NodeState.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region Editor window generation

        [MenuItem("BTUtils/BTEditor")]
        public static void ShowWindow()
        {
            window = GetWindow<BTEditorWindow>("Behaviour Tree Editor");
            window.minSize = new Vector2(600, 400);
        }

        private void OnEnable()
        {
            GenerateGraph();
            GenerateSavetoolbar();
            GenerateNodeToolbar();
            _graphView.LoadTypeData();
        }

        // TODO Doesnt work atm :)) 
        private void GenerateMiniMap()
        {
            MiniMap minimap = new MiniMap { anchored = true };
            Vector2 coords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            minimap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
            _graphView.Add(minimap);
        }

        private void OnDisable()
        {
            _graphView.SaveTypeData();
            rootVisualElement.Remove(_graphView);
        }

        // Generate graph view to be displayed on top of window
        private void GenerateGraph()
        {
            _graphView = new BTGraphView(this) { name = "Behaviour Tree Editor" };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        // Generate save file toolbar
        private void GenerateSavetoolbar()
        {
            Toolbar toolbar = new Toolbar();

            TextField fileNameField = new TextField();
            fileNameField.label = "Filename: ";
            fileNameField.labelElement.style.color = Color.black;
            fileNameField.SetValueWithoutNotify(_fileName);
            fileNameField.MarkDirtyRepaint();
            fileNameField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
            toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
            toolbar.Add(new Button(() => GenerateBehaviourTree()) { text = "Generate Behaviour Tree" });
            toolbar.Add(_graphView.contextField);

            rootVisualElement.Add(toolbar);
        }

        // Generate node creation toolbar
        private void GenerateNodeToolbar()
        {
            Toolbar toolbar = new Toolbar();

            // Behaviour name field
            TextField behaviourNameField = new TextField();
            behaviourNameField.label = "Behaviour Name: ";
            behaviourNameField.labelElement.style.color = Color.black;
            behaviourNameField.SetValueWithoutNotify(_newBehaviourName);
            behaviourNameField.MarkDirtyRepaint();
            behaviourNameField.RegisterValueChangedCallback(evt => _newBehaviourName = evt.newValue);
            toolbar.Add(behaviourNameField);

            // Create new node menu
            ToolbarMenu menu = new ToolbarMenu();
            menu.text = "Create New Behaviour";

            menu.menu.AppendAction("New Behaviour", x => { _graphView.CreateNewNode(_newBehaviourName, NodeTypes.Action); });
            menu.menu.AppendAction("New Composite", x => { _graphView.CreateNewNode(_newBehaviourName, NodeTypes.Composite); });
            menu.menu.AppendAction("New Decorator", x => { _graphView.CreateNewNode(_newBehaviourName, NodeTypes.Decorator); });

            toolbar.Add(menu);

            Button button = new Button(() => { SearchWindow.Open(new SearchWindowContext(mouseOverWindow.position.position), _graphView._addNodeSearchWindow); });
            button.text = "Add existing node";
            toolbar.Add(button);

            button = new Button(() => { _graphView.TestPrintBehaviourLists(); });
            button.text = "test print node lists";
            toolbar.Add(button);

            rootVisualElement.Add(toolbar);
        }

        #endregion

        #region BehaviourTree Generation

        // Generates initialized topnode
        private void GenerateBehaviourTree()
        {
            Debug.Log("Generating...");

            nodeStack = new Stack<AbstractNode>();

            BehaviourTree finishedTree = ScriptableObject.CreateInstance<BehaviourTree>();

            // Create rest of nodes here
            if (ConvertEditorNode(GetTopNode()) != null)
            {
                finishedTree.topNode = InitializeNodes(GetTopNode()) as Composite;
                finishedTree.context = _graphView.contextField.value as Context;
            }

            GraphSaveUtility.GetInstance(_graphView).SaveNode(_fileName + "BehaviourTree", finishedTree, _fileName);

            Debug.Log("I have been generated");
        }

        // Get current topnode
        private BTEditorNode GetTopNode()
        {
            foreach (var node in _graphView.nodes.ToList())
            {
                BTEditorNode temp = (BTEditorNode)node;

                if (temp.topNode)
                {
                    return temp;
                }
            }
            return null;
        }

        // Initialize all nodes recursively
        private AbstractNode InitializeNodes(BTEditorNode node)
        {
            // Get save utility to use for getting child nodes
            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (ConvertEditorNode(node) != null)
            {
                // Recursively initialize children and add them to stack
                foreach (var child in saveUtility.GetChildNodes(node.GUID, _fileName))
                {
                    nodeStack.Push(InitializeNodes(child));
                }

                // Construct nodes based on node type
                switch (node.nodeType)
                {
                    case NodeTypes.Composite:
                        List<BTEditorNode> childEditorNodes = saveUtility.GetChildNodes(node.GUID, _fileName);
                        List<AbstractNode> childNodes = new List<AbstractNode>();

                        // loop for the amount of children and pop them from the stack into list of nodes to be used for constructing
                        for (int i = 0; i < childEditorNodes.Count; i++)
                        {
                            childNodes.Add(nodeStack.Pop());
                        }

                        // Reverse list since nodes are popped in reverse order
                        childNodes.Reverse();

                        node.compositeInstance.Construct(childNodes);
                        node.compositeInstance.context = (Context)_graphView.contextField.value;

                        if (node.topNode) // If topnode then save with the name of the behaviourtree
                        {
                            string fileName = node.nodeName + "TopNode";

                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(node.compositeInstance), fileName);
                        }
                        return node.compositeInstance;

                    case NodeTypes.Decorator:
                        // Construct node
                        node.decoratorInstance.Construct(nodeStack.Pop());
                        node.decoratorInstance.context = (Context)_graphView.contextField.value;

                        return node.decoratorInstance;

                    case NodeTypes.Action:
                        // Construct node
                        node.actionInstance.context = (Context)_graphView.contextField.value;

                        return node.actionInstance;
                    default:
                        break;
                }
            }
            return null;
        }

        // Converts editor node to behaviournode
        private AbstractNode ConvertEditorNode(BTEditorNode node)
        {
            AbstractNode tempNode = null;

            switch (node.nodeType)
            {
                case NodeTypes.Composite:
                    tempNode = CreateInstance(node.nodeName) as Composite;
                    break;

                case NodeTypes.Decorator:
                    tempNode = CreateInstance(node.nodeName) as Decorator;
                    break;

                case NodeTypes.Action:
                    tempNode = CreateInstance(node.nodeName) as Action;
                    break;
                default:
                    break;
            }

            return tempNode;
        }
        #endregion

        // Save/Load function
        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName) || _fileName.Contains("/"))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid filename, " +
                    "name should not contain special characters such as /", "Ok");
            }

            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
            {
                saveUtility.SaveGraph(_fileName);
                Debug.Log("Saving graph as: " + _fileName + "...");
            }
            else
            {
                saveUtility.LoadGraph(_fileName);
                Debug.Log("Loading graph " + _fileName + "...");
            }
        }
    }
}
