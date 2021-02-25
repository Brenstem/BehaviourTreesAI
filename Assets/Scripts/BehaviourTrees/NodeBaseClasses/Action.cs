using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : AbstractNode
{
    //[Header("Emotional values")]

    public static event Interrupt InterruptEvent;

    [SerializeField] protected float riskValue;
    [SerializeField] protected float minTimeValue;
    [SerializeField] protected float maxTimeValue;
    [SerializeField] protected float planValue;


    public delegate void Interrupt(InterruptEventArgs args);

    public static void RaiseInterruptEvent(InterruptEventArgs args)
    {
        InterruptEvent?.Invoke(args);
    }

    public abstract void Construct();

    public virtual void AddProperties(string[] names) { }

    public override float GetRiskValue() 
    {
        return riskValue;
    }
    public override float GetMinTimeValue()
    {
        return minTimeValue;
    }
    public override float GetMaxTimeValue()
    {
        return maxTimeValue;
    }
    public override float GetPlanValue()
    {
        return planValue;
    }
}

public class InterruptEventArgs
{
    public InterruptEventArgs(string id)
    {
        this.id = id;
    }

    public string id { get; }
}
