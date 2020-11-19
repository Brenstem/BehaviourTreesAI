using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNode : ScriptableObject
{
    public Rect windowRect;
    public string windowTitle;
    protected BehaviourNode btNode;

    public virtual void DrawWindow() { }

    public virtual void DrawCurve() { }
}
