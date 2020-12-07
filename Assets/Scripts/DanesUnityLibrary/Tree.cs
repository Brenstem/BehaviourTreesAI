using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<BehaviourNode>
{
    //om _parent är null är det topnoden
    private TreeNode<BehaviourNode> _parent;
    private List<TreeNode<BehaviourNode>> _children;
    private BehaviourNode _nodeType;

    public TreeNode(BehaviourNode nodeType, TreeNode<BehaviourNode> parent = null, List<TreeNode<BehaviourNode>> children = null)
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
        TreeNode<BehaviourNode> newChild = new TreeNode<BehaviourNode>(value, this);
        _children.Add(newChild);
    }
    public void AddParent(TreeNode<BehaviourNode> parent)
    {
        _parent = parent;
    }

    public void AddChildren(BehaviourNode[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            TreeNode<BehaviourNode> newChild = new TreeNode<BehaviourNode>(values[i], this);
            _children.Add(newChild);
        }
    }

    public void Traverse()
    {


    }
}
