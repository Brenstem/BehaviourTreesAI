using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree<T>
{
    private T value;
    private List<Tree<T>> children;
    public int ChildCount { get { return children.Count; } }
    private Tree<T> parent;
    public int layer = 0;

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

            for (int i = 0; i < children.Count; i++)
            {
                children[i].layer = this.layer + 1;
            }
        }
    }

    public Tree(T value, Tree<T> parent, List<Tree<T>> children = null)
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

            for (int i = 0; i < children.Count; i++)
            {
                children[i].layer = this.layer + 1;
            }
        }
    }

    public T GetValue()
    {
        return value;
    }

    public Tree<T> AddChild(Tree<T> child)
    {
        child.layer = this.layer + 1;
        children.Add(child);

        return child;
    }

    public void AddChildren(List<Tree<T>> children)
    {
        foreach (var child in children)
        {
            child.layer = this.layer + 1;
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

    public Tree<T> GetParent()
    {
        return parent;
    }
}
