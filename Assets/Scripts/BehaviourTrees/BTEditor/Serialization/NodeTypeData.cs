using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BehaviourTreeEditor
{
    public class NodeTypeData : ScriptableObject
    {
        public Dictionary<string[], NodePathData> paths2 = new Dictionary<string[], NodePathData>();

        public List<NodePathData> paths = new List<NodePathData>();

        public List<string> behaviourNodes = new List<string>();
        public List<string> compositeNodes = new List<string>();
        public List<string> decoratorNodes = new List<string>();

        public struct NodePathData
        {
            public string[] path;
            public string name;
            public NodeTypes nodeType;
        }
    }

}

