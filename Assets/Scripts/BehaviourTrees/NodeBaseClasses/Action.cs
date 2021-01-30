using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : AbstractNode
{
    public abstract void Construct();

    public virtual void AddProperties(string[] names) { }
}
