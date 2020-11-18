using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<T>
{
    private T _datatype;
    private TreeNode<T> parent;
    private List<TreeNode<T>> children;

    public TreeNode(T datatype, TreeNode<T> parent, List<TreeNode<T>> children)
    {
        _datatype = datatype;
        this.parent = parent;
        this.children = children;
    }
}
