using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;


namespace BehaviourTreeEditor
{
    /// <summary>
    /// Behaviour tree editor node
    /// </summary>
    public class BTEditorNode : Node
    {
        public string nodeName;
        public string GUID;
        public bool topNode = false;
        public NodeTypes nodeType;
    }

    public enum NodeTypes { Composite, Decorator, Behaviour }
}