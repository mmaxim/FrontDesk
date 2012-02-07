//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    /// <summary>
    /// Collection of TreeNodes within a TreeView.
    /// </summary>
    [Editor(typeof(Microsoft.Web.UI.WebControls.Design.TreeNodeCollectionEditor), typeof(UITypeEditor))]
    public class TreeNodeCollection : BaseChildNodeCollection
    {
        private Object _Parent;
        private TreeNode _tnSelected;

        /// <summary>
        /// Initializes a new instance of a TreeNodeCollection.
        /// </summary>
        /// <param name="parent">The parent TreeNode of this collection.</param>
        public TreeNodeCollection(Object parent) : base()
        {
            _Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of a TreeNodeCollection.
        /// </summary>
        public TreeNodeCollection() : base ()
        {
            _Parent = null;
        }

        /// <summary>
        /// The parent object of this collection.
        /// </summary>
        public Object Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsert(int index, object value)
        {
            if (((TreeNode)value)._Parent != null)
            {
                throw new Exception(Util.GetStringResource("TreeNodeAlreadyInCollection"));
            }

            SetItemProperties((TreeNode)value);

            if (!Reloading)
            {
                TreeView tv;
                if (Parent is TreeNode)
                    tv = ((TreeNode)Parent).ParentTreeView;
                else
                    tv = (TreeView)Parent;

                if ((tv != null) && tv.IsInitialized)
                {
                    _tnSelected = tv.GetNodeFromIndex(tv.SelectedNodeIndex);
                }
            }

            base.OnInsert(index, value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);

            if (!Reloading)
            {
                TreeView tv;
                if (Parent is TreeNode)
                    tv = ((TreeNode)Parent).ParentTreeView;
                else
                    tv = (TreeView)Parent;

                if ((tv != null) && tv.IsInitialized)
                {
                    if (_tnSelected != null)
                        tv.SelectedNodeIndex = _tnSelected.GetNodeIndex();
                    else
                        tv.SelectedNodeIndex = "0";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being changed.</param>
        /// <param name="oldValue">The old item.</param>
        /// <param name="newValue">The new item.</param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            SetItemProperties((TreeNode)newValue);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Sets properties of the TreeNode before being added.
        /// </summary>
        /// <param name="item">The TreeNode to be set.</param>
        private void SetItemProperties(TreeNode item)
        {
            item._Parent = Parent;
        }

        /// <summary>
        /// Adds a TreeNode to the collection.
        /// </summary>
        /// <param name="item">The TreeNode to add.</param>
        public void Add(TreeNode item)
        {
            List.Add(item);
        }

        /// <summary>
        /// Adds a TreeNode to the collection at a specific index.
        /// </summary>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="item">The TreeNode to add.</param>
        public void AddAt(int index, TreeNode item)
        {
            List.Insert(index, item);
        }

        /// <summary>
        /// Determines if a TreeNode is in the collection.
        /// </summary>
        /// <param name="item">The TreeNode to search for.</param>
        /// <returns>true if the TreeNode exists within the collection. false otherwise.</returns>
        public bool Contains(TreeNode item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Determines zero-based index of a TreeNode within the collection.
        /// </summary>
        /// <param name="item">The TreeNode to locate within the collection.</param>
        /// <returns>The zero-based index.</returns>
        public int IndexOf(TreeNode item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// Removes a TreeNode from the collection.
        /// </summary>
        /// <param name="item">The TreeNode to remove.</param>
        public void Remove(TreeNode item)
        {           
            List.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being removed.</param>
        /// <param name="value">The item being removed.</param>
        protected override void OnRemove(int index, object value)
        {
            if (!Reloading)
            {
                TreeNode node = (TreeNode)value;
                TreeView tv = node.ParentTreeView;
                if (tv != null)
                {
                    if (tv.SelectedNodeIndex.IndexOf(node.GetNodeIndex()) == 0)
                    {
                        // The node being removed is the selected node or one of its parents
                        TreeNode newNode = null;
                        if (Count > 1)
                        {
                            // Set the new selected node index to the next node
                            // or the previous one if the node is the last node
                            if (index == (Count - 1))
                            {
                                newNode = this[index - 1];
                            }
                            else
                            {
                                newNode = this[index + 1];
                            }
                        }
                        else if ((Parent != null) && (Parent is TreeNode))
                        {
                            // There are no other nodes in this collection, so
                            // try setting to its parent
                            newNode = (TreeNode)Parent;
                        }

                        _tnSelected = newNode;
                    }
                    else
                    {
                        // The selected node does not need to change, but its
                        // index may be affected by this removal.
                        _tnSelected = tv.GetNodeFromIndex(tv.SelectedNodeIndex);
                    }
                }
            }

            base.OnRemove(index, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being removed.</param>
        /// <param name="value">The item being removed.</param>
        protected override void OnRemoveComplete(int index, object value)
        {          
            base.OnRemoveComplete(index, value);

            TreeNode node = (TreeNode)value;
            node.ParentTreeView = null;
            node._Parent = null;

            TreeView tv = (Parent is TreeNode) ? ((TreeNode)Parent).ParentTreeView : (TreeView)Parent;
            if (!Reloading && (tv != null))
            {
                if (_tnSelected != null)
                {
                    tv.SelectedNodeIndex = _tnSelected.GetNodeIndex();
                }
                else
                {
                    tv.SelectedNodeIndex = null;
                }

                if (tv.HoverNodeIndex != null && tv.GetNodeFromIndex(tv.HoverNodeIndex) == null)
                {
                    tv.HoverNodeIndex = "";
                }
            }
        }

        /// <summary>
        /// Adjusts the SelectedNodeIndex of the TreeView when a clear is performed.
        /// </summary>
        protected override void OnClear()
        {
            if (!Reloading && (Parent != null))
            {
                if (Parent is TreeView)
                {
                    ((TreeView)Parent).SelectedNodeIndex = null;
                }
                else
                {
                    TreeView tv = ((TreeNode)Parent).ParentTreeView;
                    if (tv != null)
                    {
                        string parentIndex = ((TreeNode)Parent).GetNodeIndex();
                        if (tv.SelectedNodeIndex.IndexOf(parentIndex) == 0)
                        {
                            tv.SelectedNodeIndex = parentIndex;
                        }
                    }
                }
            }

            base.OnClear();
        }

        /// <summary>
        /// Indexer into the collection.
        /// </summary>
        public TreeNode this[int index]
        {
            get { return (TreeNode)List[index]; }
        }
    }
}
