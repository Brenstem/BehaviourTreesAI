using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BTEditorWindow : EditorWindow
{
    private static List<BaseNode> windows = new List<BaseNode>();
    private Vector3 mousePos;
    private bool makeTransition;
    private bool clickedOnWindow;
    private BaseNode selectedNode;

    public enum UserActions
    {
        addNode, deleteNode
    }

    [MenuItem("BTUtils/BTEditor")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<BTEditorWindow>("Behaviour Tree Editor");
        window.minSize = new Vector2(600, 400);
    }

    private void OnEnable()
    {
        windows.Clear();
    }

    private void OnGUI()
    {
        Event e = Event.current;
        mousePos = e.mousePosition;

        UserInput(e);
        DrawWindows();
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

        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].windowRect.Contains(e.mousePosition))
            {
                clickedOnWindow = true;
                selectedNode = windows[i];
                break;
            }
            else
            {
                clickedOnWindow = false;
                selectedNode = null;
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
        menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, UserActions.addNode);


        menu.ShowAsContext();
        e.Use();
    }

    private void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, UserActions.deleteNode);

        menu.ShowAsContext();
        e.Use();
    }

    private void ContextCallback(object o)
    {
        UserActions action = (UserActions)o;

        switch (action)
        {
            case UserActions.addNode:
                BaseNode node = new BaseNode { };
                node.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
                node.windowTitle = "Node";
                windows.Add(node);
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
}
