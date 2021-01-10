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
        private static EditorWindow window;

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
            GenerateBlackBoard();
            GenerateMiniMap();
            _graphView.LoadTypeData();
        }

        // Generates blackboard
        private void GenerateBlackBoard()
        {
            Blackboard blackBoard = new Blackboard(_graphView);
            blackBoard.Add(new BlackboardSection { title = "Exposed properties" });

            blackBoard.addItemRequested = _blackboard =>
            {
                _graphView.AddPropertyToBlackBoard(new ExposedProperty());
            };

            blackBoard.editTextRequested = (blackBoard1, element, newValue) =>
            {
                string oldPropertyName = ((BlackboardField)element).text;
                if (_graphView.exposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please choose another one!", "ok :(");
                    return;
                }

                int propertyIndex = _graphView.exposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                _graphView.exposedProperties[propertyIndex].PropertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };

            blackBoard.SetPosition(new Rect(10, 30, 200, 300));

            _graphView.Add(blackBoard);
            _graphView.Blackboard = blackBoard;
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

            menu.menu.AppendAction("New Behaviour", x => { _graphView.CreateNewNode(_newBehaviourName, NodeTypes.Behaviour); });
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
            }
            else
            {
                saveUtility.LoadGraph(_fileName);
            }
        }

        // TODO finish this function
        // Generates AI usable behaviour tree thingy

        private void GenerateBehaviourTree()
        {
            Debug.Log("Generating...");

            EnemyAIBT behaviourTree = CreateInstance<EnemyAIBT>();
            BTEditorNode topNode = GetTopNode();

            CompositeNode tempTopNode = null;

            // Create rest of nodes here
            if (ConvertEditorNode(topNode) != null)
            {
                tempTopNode = InitializeNodes(topNode);
            }

            // Debug.Log((CompositeNode)ConvertEditorNode(topNode));

            // behaviourTree.SetTopNode();

            // Put savefile in Assets/BTResources
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(tempTopNode, $"Assets/Resources/{_fileName}BehaviourTree.asset");
            AssetDatabase.SaveAssets();

            Debug.Log("I have been generated");
        }

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

        Context context = new Context();

        private CompositeNode InitializeNodes(BTEditorNode node)
        {
            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (ConvertEditorNode(node) != null)
            {
                foreach (var child in saveUtility.GetChildNodes(node.GUID))
                {
                    InitializeNodes(child);
                }

                // do construct things here
                switch (node.nodeType)
                {
                    case NodeTypes.Composite:
                        List<BTEditorNode> childEditorNodes = saveUtility.GetChildNodes(node.GUID);
                        List<AbstractNode> childNodes = new List<AbstractNode>();

                        CompositeNode tempCompositeNode = CreateInstance(node.nodeName) as CompositeNode;

                        // TODO we need to get references to the constructed children instead of instatiating new instances of them here
                        // Maybe use an external list or dictionary with node.GUID as key to do this?
                        foreach (var child in childEditorNodes)
                            childNodes.Add(ConvertEditorNode(child));

                        tempCompositeNode.Construct(childNodes);

                        Debug.Log("Constructed: " + node.title);

                        if (node.topNode)
                        {
                            Debug.Log("top node: " + node.title);
                            return tempCompositeNode;
                        }
                        break;

                    case NodeTypes.Decorator:
                        DecoratorNode tempDecoratorNode = CreateInstance(node.nodeName) as DecoratorNode;
                        tempDecoratorNode.Construct(ConvertEditorNode(saveUtility.GetChildNodes(node.GUID)[0]));

                        Debug.Log("Constructed: " + node.title);
                        break;

                    case NodeTypes.Behaviour:
                        Action tempActionNode = CreateInstance(node.nodeName) as Action;
                        tempActionNode.Construct(context); // TODO this is stupid fix ur blackboard bitch

                        Debug.Log("Constructed: " + node.title);
                        break;
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
                    tempNode = CreateInstance(node.nodeName) as CompositeNode;
                    break;

                case NodeTypes.Decorator:
                    tempNode = CreateInstance(node.nodeName) as DecoratorNode;
                    break;

                case NodeTypes.Behaviour:
                    tempNode = CreateInstance(node.nodeName) as Action;
                    break;
                default:
                    break;
            }

            return tempNode;
        }
    }
}
