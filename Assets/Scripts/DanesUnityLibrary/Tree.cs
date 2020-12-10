using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    //om _parent är null är det topnoden
    private TreeNode _parent;
    private List<TreeNode> _children;
    private BehaviourNode _nodeType;

    public TreeNode(BehaviourNode nodeType, TreeNode parent = null, List<TreeNode> children = null)
    {
        //for (int i = 0; i < children.Count; i++)
        //{
        //    AddChildren(children[i]);
        //}

        _nodeType = nodeType;

        _children = children;
        _parent = parent;
    }

    //kör på topnoden så körs det på allt
    public void ConstructTree()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            _children[i].ConstructTree();
        }
        Initialize();
    }

    public void Initialize()
    {
        //_nodeType
    }

    public void AddChild(BehaviourNode value)
    {
        TreeNode newChild = new TreeNode(value, this);
        _children.Add(newChild);
    }
    public void AddParent(TreeNode parent)
    {
        _parent = parent;
    }

    public void AddChildren(BehaviourNode[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            TreeNode newChild = new TreeNode(values[i], this);
            _children.Add(newChild);
        }
    }

    public void Traverse()
    {


    }
}
