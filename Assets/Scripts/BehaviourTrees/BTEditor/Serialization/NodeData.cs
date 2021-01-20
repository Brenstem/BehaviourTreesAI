using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public string nodeTitle;
    public string nodeName;
    public Composite compositeInstance;
    public Decorator decoratorInstance;
    public Action actionInstance;
    public string Guid;
    public Vector2 Position;
    public int nodeType;
    public bool topNode;
}