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
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
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
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
                new SearchTreeGroupEntry(new GUIContent("Composite Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Selector", _indentationIcon))
                {
                    userData = new BTEditorNode(), level = 2
                }, 
                new SearchTreeGroupEntry(new GUIContent("Decorator Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Invertor", _indentationIcon))
                {
                    userData = new BTEditorNode(), level = 2
                },
                new SearchTreeGroupEntry(new GUIContent("Behaviour Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Behaviour", _indentationIcon))
                {
                    userData = new BTEditorNode(), level = 2
                },
            };

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

            // TODO figure out how to spawn correct node type based on userdata
            switch (SearchTreeEntry.userData)
            {
                case BTEditorNode selector:
                    _graphView.CreateNode("Selector", NodeTypes.Composite, localMousePosition);
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}
