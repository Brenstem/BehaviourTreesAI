using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;


namespace BehaviourTreeEditor
{
    /// <summary>
    /// Behaviour tree editor node
    /// </summary>
    public class BTEditorNode : Node
    {
        public string nodeName;
        public Composite compositeInstance;
        public Decorator decoratorInstance;
        public Action actionInstance;
        public string GUID;
        public bool topNode = false;
        public NodeTypes nodeType;
    }

    public enum NodeTypes { Composite, Decorator, Behaviour }
}