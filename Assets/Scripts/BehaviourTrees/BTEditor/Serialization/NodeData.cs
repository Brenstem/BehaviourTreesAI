using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public string nodeTitle;
    public string nodeName;
    public bool topNode;
    public int nodeType;

    public Composite compositeInstance;
    public Decorator decoratorInstance;
    public Action actionInstance;

    public string GUID;
    public Vector2 Position;
}