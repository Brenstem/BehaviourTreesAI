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

        private static ObjectField fileLoadField;

        private StyleSheet successStyleSheet;
        private StyleSheet failureStyleSheet;
        private StyleSheet runningStyleSheet;

        private void Update()
        {
            if (Application.isPlaying)
            {
                // Update node state labels in editor
                foreach (BTEditorNode node in _graphView.nodes.ToList())
                {
                    Label nodeStateLabel = node.titleContainer.Q<Label>("node-state-label");


                    switch (node.nodeType)
                    {
                        case NodeTypes.Composite:
                            nodeStateLabel.text = node.compositeInstance.NodeState.ToString();
                            node.styleSheets.Remove(successStyleSheet);
                            node.styleSheets.Remove(failureStyleSheet);
                            node.styleSheets.Remove(runningStyleSheet);
                            node.styleSheets.Add(UpdateStyleSheet(node.compositeInstance.NodeState));
                            break;
                        case NodeTypes.Decorator:
                            nodeStateLabel.text = node.decoratorInstance.NodeState.ToString();
                            node.styleSheets.Remove(successStyleSheet);
                            node.styleSheets.Remove(failureStyleSheet);
                            node.styleSheets.Remove(runningStyleSheet); 
                            node.styleSheets.Add(UpdateStyleSheet(node.decoratorInstance.NodeState));
                            break;
                        case NodeTypes.Action:
                            nodeStateLabel.text = node.actionInstance.NodeState.ToString();
                            node.styleSheets.Remove(successStyleSheet);
                            node.styleSheets.Remove(failureStyleSheet);
                            node.styleSheets.Remove(runningStyleSheet); 
                            node.styleSheets.Add(UpdateStyleSheet(node.actionInstance.NodeState));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private StyleSheet UpdateStyleSheet(NodeStates nodestate)
        {
            switch (nodestate)
            {
                case NodeStates.FAILURE:
                    return failureStyleSheet;
                case NodeStates.RUNNING:
                    return runningStyleSheet;
                case NodeStates.SUCCESS:
                    return successStyleSheet;
            }
            return null;
        }

        public static void SetFileLoadFieldValue(BTDataContainer btContainer)
        {
            fileLoadField.value = btContainer;
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
            GenerateReferenceToolbar();
            GenerateNodeToolbar();
            _graphView.LoadTypeData();

            successStyleSheet = Resources.Load<StyleSheet>("NodeSuccess");
            failureStyleSheet = Resources.Load<StyleSheet>("NodeFailure");
            runningStyleSheet = Resources.Load<StyleSheet>("NodeRunning");
        }

        private void OnDisable()
        {
            _graphView.SaveTypeData();
            rootVisualElement.Remove(_graphView);
        }

        // TODO Doesnt work atm :)) 
        private void GenerateMiniMap()
        {
            MiniMap minimap = new MiniMap { anchored = true };
            Vector2 coords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            minimap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
            _graphView.Add(minimap);
        }

        // Generate graph view to be displayed on top of window
        private void GenerateGraph()
        {
            _graphView = new BTGraphView(this) { name = "Behaviour Tree Editor" };
            fileLoadField = new ObjectField { objectType = typeof(BTDataContainer) };
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
            toolbar.Add(fileLoadField);

            rootVisualElement.Add(toolbar);
        }

        private void GenerateReferenceToolbar()
        {
            Toolbar toolbar = new Toolbar();

            ObjectField btDebugField = new ObjectField { objectType = typeof(GameObject) };
            btDebugField.label = "Debug Gameobject: ";
            btDebugField.labelElement.style.color = Color.black;
            btDebugField.MarkDirtyRepaint();
            btDebugField.RegisterValueChangedCallback(evt => { if (Application.isPlaying) UpdateCurrentBT((GameObject)evt.newValue); });
            toolbar.Add(btDebugField);

            _graphView.contextField.label = "Context: ";
            _graphView.contextField.labelElement.style.color = Color.black;
            toolbar.Add(_graphView.contextField);

            rootVisualElement.Add(toolbar);
        }

        private void UpdateCurrentBT(GameObject gameObject)
        {
            Debug.Log("Getting bt instance " + gameObject.GetComponent<BaseAI>().GetBehaviourTreeDataInstance());

            if (gameObject.GetComponent<BaseAI>().GetBehaviourTreeDataInstance() != null)
            {
                GraphSaveUtility.GetInstance(_graphView).LoadGraph(gameObject.GetComponent<BaseAI>().GetBehaviourTreeDataInstance());
            }
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

            rootVisualElement.Add(toolbar);
        }

        #endregion

        // Save/Load function
        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName) || _fileName.Contains("/"))
            {
                if (fileLoadField.value == null)
                {
                    EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid filename, " +
                    "name should not contain special characters such as /", "Ok");
                }
            }

            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
            {
                if (fileLoadField.value == null)
                {
                    saveUtility.SaveGraph(_fileName);
                    Debug.Log("Saving graph " + _fileName + "...");
                }
                else
                {
                    saveUtility.SaveGraph((BTDataContainer)fileLoadField.value);
                    Debug.Log("Saving graph to: " + fileLoadField.value.name + "...");
                    fileLoadField.value = null;
                }
            }
            else
            {
                if (fileLoadField.value == null)
                {
                    saveUtility.LoadGraph(_fileName);
                    Debug.Log("Loading graph: " + _fileName + "...");
                }
                else
                {
                    saveUtility.LoadGraph((BTDataContainer)fileLoadField.value);
                    Debug.Log("Loading graph from: " + fileLoadField.value.name + "...");
                }
            }
        }
    }
}
