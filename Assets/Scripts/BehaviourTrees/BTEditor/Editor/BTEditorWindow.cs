using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class BTEditorWindow : EditorWindow
{
    private BTGraphView _graphView;
    private string _fileName = "New Behaviour Tree";

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
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView); 
    }

    private void GenerateGraph()
    {
        _graphView = new BTGraphView { name = "Behaviour Tree Editor" };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    // Save file Toolbar
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

        rootVisualElement.Add(toolbar);
    }

    // Node creation toolbar
    private void GenerateNodeToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button button = new Button(() => { _graphView.CreateNode("Selector", BTGraphView.NodeTypes.Selector); });
        button.text = "Create Selector";
        toolbar.Add(button);

        button = new Button(() => { _graphView.CreateNode("Sequence", BTGraphView.NodeTypes.Sequence); });
        button.text = "Create Sequence";
        toolbar.Add(button);

        button = new Button(() => { _graphView.CreateNode("Invertor", BTGraphView.NodeTypes.Invertor); });
        button.text = "Create Invertor";
        toolbar.Add(button);

        rootVisualElement.Add(toolbar);
    }

    // Save/Load function
    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Enter a valid filename or ill kikc ur ass", "Okay daddy :(");
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

    #region old
    private static List<BaseNode> windows = new List<BaseNode>();
    private Vector3 mousePos;
    private bool makeTransition;
    private bool clickedOnWindow;
    private BaseNode selectedNode;
    private static EditorWindow window;

    public enum UserActions
    {
        addNode, addChildNode, deleteNode
    }

    public enum NodeTypes
    {
        Selector, Sequence
    }

    // Draws all node windows
    private void DrawWindows()
    {
        BeginWindows();

        foreach (BaseNode n in windows)
        {
            n.DrawCurve();
        }

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }

        EndWindows();
    }

    private void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    // Takes user input
    private void UserInput(Event e)
    {
        // RMB down
        if (e.button == 1 && !makeTransition)
        {
            if (e.type == EventType.MouseDown)
            {
                RightClick(e);
            }
        }

        // LMB down
        if (e.button == 0 && !makeTransition)
        {
            if (e.type == EventType.MouseDown)
            {
                LeftClick(e);
            }
        }
    }

    private void RightClick(Event e)
    {
        selectedNode = null;
        clickedOnWindow = false;

        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].windowRect.Contains(e.mousePosition))
            {
                clickedOnWindow = true;
                selectedNode = windows[i];
                break;
            }
        }

        if (!clickedOnWindow)
        {
            AddNewNode(e);
        }
        else
        {
            ModifyNode(e);
        }
    }

    private void LeftClick(Event e)
    {
        Debug.Log("Not implemented");
    }

    // Right click actions
    private void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Add New Node"), false, ContextCallback, UserActions.addNode);

        menu.ShowAsContext();
        e.Use();
    }

    private void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Add Child Node"), false, ContextCallback, UserActions.addChildNode);
        menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, UserActions.deleteNode);

        menu.ShowAsContext();
        e.Use();
    }

    const int SUB_WINDOW_HEIGHT = 150;
    const int SUB_WINDOW_WIDTH = 200;

    private void ContextCallback(object o)
    {
        UserActions action = (UserActions)o;

        switch (action)
        {
            case UserActions.addNode:
                AddNewNode(NodeTypes.Selector);
                break;
            case UserActions.addChildNode:
                SelectorNode childNode = new SelectorNode();
                childNode.windowRect = new Rect(selectedNode.windowRect.x, selectedNode.windowRect.y + SUB_WINDOW_HEIGHT + 10, SUB_WINDOW_WIDTH, SUB_WINDOW_HEIGHT);
                childNode.windowTitle = "Child Node";
                windows.Add(childNode);
                break;
            case UserActions.deleteNode:
                if (selectedNode != null)
                {
                    windows.Remove(selectedNode);
                }
                break;
            default:
                break;
        }
    }

    private void AddNewNode(NodeTypes nodeType)
    {
        BaseNode node;

        switch (nodeType)
        {
            case NodeTypes.Selector:
                node = new SelectorNode();
                node.windowRect = new Rect(mousePos.x, mousePos.y, SUB_WINDOW_WIDTH, SUB_WINDOW_HEIGHT);
                node.windowTitle = "Selector";
                windows.Add(node);
                break;
            case NodeTypes.Sequence:
                node = new SequenceNode();
                node.windowRect = new Rect(mousePos.x, mousePos.y, SUB_WINDOW_WIDTH, SUB_WINDOW_HEIGHT);
                node.windowTitle = "Sequence";
                windows.Add(node);
                break;
            default:
                break;
        }
    }

    #endregion
}
