using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree<T>
{
    private T value;
    private List<Tree<T>> children;
    private T parent;

    public Tree(T value, List<Tree<T>> children = null)
    {
        this.value = value;

        if (children == null)
        {
            this.children = new List<Tree<T>>();
        }
        else
        {
            this.children = children;
        }
    }

    public Tree(T value, T parent, List<Tree<T>> children = null)
    {
        this.value = value;
        this.parent = parent;

        if (children == null)
        {
            this.children = new List<Tree<T>>();
        }
        else
        {
            this.children = children;
        }
    }

    public T GetValue()
    {
        return value;
    }

    public void AddChild(Tree<T> value)
    {
        children.Add(value);
    }

    public void AddChildren(List<Tree<T>> children)
    {
        foreach (var child in children)
        {
            this.children.Add(child);
        }
    }

    /// <summary>
    /// Returns false if data could not be removed
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool RemoveChild(int i)
    {
        if (i > children.Count || i < 0) 
        { 
            children.RemoveAt(i);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns null if data could not be obtained
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Tree<T> GetChild(int i)
    {
        if (children[i] != null)
        {
            return children[i];
        }
        else
        {
            return null;
        }
    }

    public T GetParent()
    {
        return parent;
    }
}
