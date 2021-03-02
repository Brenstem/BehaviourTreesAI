using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTreeEditor
{
    /// <summary>
    /// BT editor node search window 
    /// </summary>
    public class AddNodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private BTGraphView _graphView;
        private BTEditorWindow _editorWindow;
        private Texture2D _indentationIcon;

        /// <summary>
        /// Initializes search window for a given graph and editor window
        /// </summary>
        /// <param name="editorWindow"></param>
        /// <param name="graphView"></param>
        public void Init(BTEditorWindow editorWindow, BTGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;

            // This is a hack to avoid wacky indentation because of wack unity stuff
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        /// <summary>
        /// Generates search tree
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            // SearchTreeGroupEntry functions as an submenu accessor 
            // SearchTreeEntry functions as a selectable item that should spawn a node corresponding to its type
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            List<string> existingFolders = new List<string>();
            
            Stack<Tree<NodeTypeData.NodePathData>> nodeStack = new Stack<Tree<NodeTypeData.NodePathData>>();

            // Push all the top nodes children to nodestack 
            for (int i = 0; i < _graphView.typeData.pathData.ChildCount; i++)
            {
                nodeStack.Push(_graphView.typeData.pathData.GetChild(i));
            }

            // Add the top folder
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Nodes"), 0));

            Tree<NodeTypeData.NodePathData> currentNode;

            // Loop until stack is empty aka no more folders or entries to be created 
            while (nodeStack.Count > 0)
            {
                currentNode = nodeStack.Pop();

                for (int i = 0; i < currentNode.ChildCount; i++) // Push all child nodes of currentNode to stack
                {
                    nodeStack.Push(currentNode.GetChild(i));
                }
                
                if (currentNode.GetValue().nodeName == null) // If thsi is true node is a folder
                {
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(currentNode.GetValue().pathName), currentNode.layer));
                }
                else // else this node is a BT node
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(currentNode.GetValue().nodeName, _indentationIcon))
                    {
                        userData = new BTEditorNode() { nodeName = currentNode.GetValue().nodeName, nodeType = currentNode.GetValue().nodeType },
                        level = currentNode.layer
                    });
                }
            }

            return tree;
        }

        /// <summary>
        /// Called when a search tree entry is selected
        /// </summary>
        /// <param name="SearchTreeEntry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            // Get mouse world mosue position in editor window
            Vector2 worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent,
                context.screenMousePosition - _editorWindow.position.position);

            // Convert it to graphview coords to spawn nodes at mouse position
            Vector2 localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

            BTEditorNode tempNode = (BTEditorNode)SearchTreeEntry.userData;

            _graphView.CreateNode(tempNode.nodeName, tempNode.nodeName, tempNode.nodeType, localMousePosition);

            return true;
        }
    }
}
