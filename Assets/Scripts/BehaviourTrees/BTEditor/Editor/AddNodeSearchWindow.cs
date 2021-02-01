using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
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

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Nodes"), 0));

            // Loop through all pathdata in the typedata object
            for (int i = 0; i < _graphView.typeData.paths.Count; i++)
            {
                for (int ii = 0; ii < _graphView.typeData.paths[i].path.Length; ii++) // Loop through the path stringarray and create a submenu for each entry
                {
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(_graphView.typeData.paths[i].path[ii]), ii + 1));

                    Debug.Log(tree.Contains(new SearchTreeGroupEntry(new GUIContent(_graphView.typeData.paths[i].path[ii]), ii + 1)));

                    if (ii == _graphView.typeData.paths[i].path.Length-1) // Add the searchtreeentry with corresponding data from typedata on the final iteration
                    {
                        tree.Add(new SearchTreeEntry(new GUIContent(_graphView.typeData.paths[i].name, _indentationIcon))
                        {
                            userData = new BTEditorNode() { nodeName = _graphView.typeData.paths[i].name, nodeType = _graphView.typeData.paths[i].nodeType },
                            level = ii + 2
                        });
                    }
                }
            }


            tree.Add(new SearchTreeGroupEntry(new GUIContent("Behaviour Nodes"), 1));

            foreach (var name in _graphView.typeData.behaviourNodes)
            {
                tree.Add(new SearchTreeEntry(new GUIContent(name, _indentationIcon))
                {
                    userData = new BTEditorNode() { nodeName = name, nodeType = NodeTypes.Action },
                    level = 2
                });
            }

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Composite Nodes"), 1));

            foreach (var name in _graphView.typeData.compositeNodes)
            {
                tree.Add(new SearchTreeEntry(new GUIContent(name, _indentationIcon))
                {
                    userData = new BTEditorNode() { nodeName = name, nodeType = NodeTypes.Composite },
                    level = 2
                });
            }

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Decorator Nodes"), 1));

            foreach (var name in _graphView.typeData.decoratorNodes)
            {
                tree.Add(new SearchTreeEntry(new GUIContent(name, _indentationIcon))
                {
                    userData = new BTEditorNode() { nodeName = name, nodeType = NodeTypes.Decorator },
                    level = 2
                });
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
