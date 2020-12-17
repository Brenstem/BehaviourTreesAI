using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTypeData : ScriptableObject
{
    public List<string> behaviourNodes = new List<string>();
    public List<string> compositeNodes = new List<string>();
    public List<string> decoratorNodes = new List<string>();
}
