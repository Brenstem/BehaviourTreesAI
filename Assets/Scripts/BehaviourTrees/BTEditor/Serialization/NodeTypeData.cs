using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BehaviourTreeEditor
{
    public class NodeTypeData : ScriptableObject
    {
        public List<NodePathData> paths = new List<NodePathData>();

        public Tree<NodePathData> pathData;

        public List<string> behaviourNodes = new List<string>();
        public List<string> compositeNodes = new List<string>();
        public List<string> decoratorNodes = new List<string>();

        public void SortPaths()
        {
            List<string> existingPaths = new List<string>();

            for (int i = 0; i < paths.Count; i++)
            {
                if (i != 0)
                {
                    if (!existingPaths.Contains(paths[i].path[0]))
                    {
                        existingPaths.Add(paths[i].path[0]);
                    }
                    else
                    {
                        int j = i;
                        while (paths[j].path[0] != paths[j - 1].path[0])
                        {
                            Swap<NodePathData>(paths, j, j - 1);
                            j--;

                            if (paths[j].path[0] == paths[j - 1].path[0])
                            {
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < paths.Count; i++)
            {
                Debug.Log(paths[i].path[0]);
            }
        }

        private void Swap<T>(List<T> list, int firstIndex, int secondIndex)
        {
            T firstPath = list[firstIndex];
            T secondPath = list[secondIndex];

            list.RemoveAt(firstIndex);
            list.Insert(firstIndex, secondPath);

            list.RemoveAt(secondIndex);
            list.Insert(secondIndex, firstPath);
        }

        public struct NodePathData 
        {
            public string[] path;
            public string name;
            public NodeTypes nodeType;

            public void Print()
            {
                for (int i = 0; i < path.Length; i++)
                {
                    Debug.Log("Path " + i + ": " + path[i]);
                }

                Debug.Log("Name: " + name);
                Debug.Log("");
            }
        }
    }
}

