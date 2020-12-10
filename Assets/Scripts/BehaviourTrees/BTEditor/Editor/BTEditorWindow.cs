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
            toolbar.Add(new Button(() => GenerateBehaviourTree()));

            rootVisualElement.Add(toolbar);
        }

        // Generate node creation toolbar
        private void GenerateNodeToolbar()
        {
            Toolbar toolbar = new Toolbar();

            Button button = new Button(() => { _graphView.CreateNode("Composite", NodeTypes.Composite, Vector2.zero); });
            button.text = "Create Composite node";
            toolbar.Add(button);

            button = new Button(() => { _graphView.CreateNode("Decorator", NodeTypes.Decorator, Vector2.zero); });
            button.text = "Create Decorator node";
            toolbar.Add(button);

            button = new Button(() => { _graphView.CreateNode("Behaviour", NodeTypes.Behaviour, Vector2.zero); });
            button.text = "Create Behaviour node";
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

        // Generates AI usable behaviour tree thingy
        private void GenerateBehaviourTree()
        {
            List<Node> tempNodes = _graphView.nodes.ToList();
            List<BTEditorNode> nodes = new List<BTEditorNode>();

            foreach (Node node in tempNodes)
                nodes.Add((BTEditorNode)node);
            
            BTEditorNode[] childNodes = (BTEditorNode[])nodes[1].Children().ToArray();
        }
    }
}
