using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : AbstractNode
{
    public static event Interrupt InterruptEvent;

    public delegate void Interrupt(InterruptEventArgs args);

    public static void RaiseInterruptEvent(InterruptEventArgs args)
    {
        InterruptEvent?.Invoke(args);
    }

    public abstract void Construct();

    public virtual void AddProperties(string[] names) { }
}

public class InterruptEventArgs
{
    public InterruptEventArgs(string id)
    {
        this.id = id;
    }
    public string id { get; }
}
