﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BTDataContainer : ScriptableObject
{
    public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
    public List<NodeData> nodeData = new List<NodeData>();
    public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();
}
