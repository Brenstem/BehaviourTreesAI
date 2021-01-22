using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : AbstractNode
{
    public Context context;

    //public abstract void Construct(Context blackboard);
    public abstract void Construct();

    public virtual void AddProperties(string[] names) { }
}
