using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class BTEditorNode : Node
{
    public string GUID;
    public bool topNode = false;
    public System.Type type;
}
