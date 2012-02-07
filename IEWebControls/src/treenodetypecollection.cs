//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;

    /// <summary>
    /// Collection of TreeNodes within a TreeView.
    /// </summary>
    public class TreeNodeTypeCollection : BaseChildNodeCollection
    {
        private Object _Parent;

        /// <summary>
        /// Initializes a new instance of a TreeNodeCollection.
        /// </summary>
        public TreeNodeTypeCollection() : base()
        {
            _Parent = null;
        }

        /// <summary>
        /// Initializes a new instance of a TreeNodeCollection.
        /// </summary>
        /// <param name="parent">The parent TreeNodeType of this collection.</param>
        public TreeNodeTypeCollection(Object parent) : base()
        {
            _Parent = parent;
        }

        /// <summary>
        /// The parent object of this collection.
        /// </summary>
        public Object Parent
        {
            get { return _Parent; }
            set
            {
                _Parent = value;

                if (value != null)
                {
                    foreach (TreeNodeType item in this)
                    {
                        SetItemProperties(item);
                    }
                }
            }
        }

        private void SetItemProperties(TreeNodeType item)
        {
            item._Parent = Parent;
        }

        /// <summary>
        /// Sets item properties on being inserted.
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsert(int index, object value)
        {
            SetItemProperties((TreeNodeType)value);

            base.OnInsert(index, value);
        }

        /// <summary>
        /// Sets item properties on being set.
        /// </summary>
        /// <param name="index">The index of the item being changed.</param>
        /// <param name="oldValue">The old item.</param>
        /// <param name="newValue">The new item.</param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            SetItemProperties((TreeNodeType)newValue);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Adds a TreeNodeType to the collection.
        /// </summary>
        /// <param name="item">The TreeNodeType to add.</param>
        public void Add(TreeNodeType item)
        {
            List.Add(item);
        }

        /// <summary>
        /// Adds a TreeNodeType to the collection at a specific index.
        /// </summary>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="item">The TreeNodeType to add.</param>
        public void AddAt(int index, TreeNodeType item)
        {
            List.Insert(index, item);
        }

        /// <summary>
        /// Determines if a TreeNodeType is in the collection.
        /// </summary>
        /// <param name="item">The TreeNodeType to search for.</param>
        /// <returns>true if the TreeNodeType exists within the collection. false otherwise.</returns>
        public bool Contains(TreeNodeType item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Determines zero-based index of a TreeNodeType within the collection.
        /// </summary>
        /// <param name="item">The TreeNodeType to locate within the collection.</param>
        /// <returns>The zero-based index.</returns>
        public int IndexOf(TreeNodeType item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// Removes a TreeNodeType from the collection.
        /// </summary>
        /// <param name="item">The TreeNodeType to remove.</param>
        public void Remove(TreeNodeType item)
        {
            List.Remove(item);
        }

        /// <summary>
        /// Indexer into the collection.
        /// </summary>
        public TreeNodeType this[int index]
        {
            get { return (TreeNodeType)List[index]; }
        }
    }
}
