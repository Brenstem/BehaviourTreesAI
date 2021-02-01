using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class AddNodeMenu : Attribute
{
    public readonly string menuPath;
    public readonly string nodeName;

    public AddNodeMenu(string path, string name)
    {
        menuPath = path;
        nodeName = name;
    }
}
