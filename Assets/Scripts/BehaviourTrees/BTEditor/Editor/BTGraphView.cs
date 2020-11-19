using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class BTGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(200, 200);

    public enum NodeTypes { Selector, Sequence, Invertor }

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
    private BTEditorNode GenerateBehaviournode(string nodeName, NodeTypes type)
    {
        BTEditorNode node = null;

        switch (type)
        {
            case NodeTypes.Selector:
                node = GenerateSelectorNode(nodeName);
                break;
            case NodeTypes.Sequence:
                node = GenerateSequenceNode(nodeName);
                break;
            case NodeTypes.Invertor:
                node = GenerateInvertorNode(nodeName);
                break;
            default:
                break;
        }
        return node;
    }

    private BTEditorNode GenerateSelectorNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            type = typeof(Selector)
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

    private BTEditorNode GenerateSequenceNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString()
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

    private BTEditorNode GenerateInvertorNode(string nodeName)
    {
        BTEditorNode node = new BTEditorNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString()
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

    private void AddPort(BTEditorNode target)
    {
        Port generatedPort = GeneratePort(target, Direction.Output);

        int outputPortCount = target.outputContainer.Query("connector").ToList().Count;
        generatedPort.portName = $"Child Behaviour {outputPortCount}";

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
