using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BehaviourTreeEditor
{
    public class NodeTypeData : ScriptableObject
    {
        public Tree<NodePathData> pathData;

        public struct NodePathData 
        {
            public string pathName;
            public string nodeName;
            public NodeTypes nodeType;
        }
    }
}

