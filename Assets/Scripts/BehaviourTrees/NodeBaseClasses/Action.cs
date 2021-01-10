using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : AbstractNode
{
    protected Context context;

    public abstract void Construct(Context blackboard);

    public virtual void AddProperties(string[] names) { }
}
