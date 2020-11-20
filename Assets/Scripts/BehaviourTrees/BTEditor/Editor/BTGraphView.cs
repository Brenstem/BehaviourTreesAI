using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class BTGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(200, 200);

    public enum NodeTypes { Composite, Decorator, Behaviour }

    public BTGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateEntryPointNode("Top Node"));
    }

#region Node Generation
    private BTEditorNode GenerateEntryPointNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            topNode = true
        };

        Button button = new Button(() => { AddPort(node); });
        button.text = "New Child Behaviour";
        node.titleContainer.Add(button);


        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(new Vector2(100, 200), defaultNodeSize));

        return node;
    }

    public void CreateNode(string nodename, NodeTypes type)
    {
        AddElement(GenerateBehaviournode(nodename, type));
    }

    // General behaviour node generation
    public BTEditorNode GenerateBehaviournode(string nodeName, NodeTypes type)
    {
        BTEditorNode node = null;

        switch (type)
        {
            case NodeTypes.Composite:
                node = GenerateCompositeNode(nodeName);
                break;
            case NodeTypes.Decorator:
                node = GenerateBehaviourNode(nodeName);
                break;
            case NodeTypes.Behaviour:
                node = GenerateDecoratorNode(nodeName);
                break;
            default:
                break;
        }
        return node;
    }

    private BTEditorNode GenerateCompositeNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
        };

        node.inputContainer.Add(GeneratePort(node, Direction.Input));

        Button button = new Button(() => { AddPort(node); });
        button.text = "New Child Behaviour";
        node.titleContainer.Add(button);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return node;
    }

    private BTEditorNode GenerateBehaviourNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
        };

        node.inputContainer.Add(GeneratePort(node, Direction.Input));

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return node;
    }

    private BTEditorNode GenerateDecoratorNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
        };

        node.inputContainer.Add(GeneratePort(node, Direction.Input, Port.Capacity.Multi));
        node.outputContainer.Add(GeneratePort(node, Direction.Output, Port.Capacity.Multi));

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return node;
    }
#endregion

#region Port Generation

    public void AddPort(BTEditorNode target, string overriddenPortName= "")
    {
        Port generatedPort = GeneratePort(target, Direction.Output);

        int outputPortCount = target.outputContainer.Query("connector").ToList().Count;
        generatedPort.portName = $"Child Behaviour {outputPortCount}";

        string portName = string.IsNullOrEmpty(overriddenPortName) 
            ? $"Child Behaviour {outputPortCount}" 
            : overriddenPortName;

        TextField textField = new TextField
        {
            name = string.Empty,
            value = portName
        };

        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("   "));
        generatedPort.contentContainer.Add(textField);
        // Button deleteButton = new Button(() => RemovePort());

        generatedPort.portName = portName;
        target.outputContainer.Add(generatedPort);
        target.RefreshPorts();
        target.RefreshExpandedState();
    }

    private Port GeneratePort(BTEditorNode target, Direction portDir, Port.Capacity capacity = Port.Capacity.Single)
    {
        return target.InstantiatePort(Orientation.Horizontal, portDir, capacity, typeof(float));
    }


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
