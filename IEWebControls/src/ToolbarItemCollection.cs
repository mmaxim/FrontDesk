//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Collections;

    /// <summary>
    /// Collection of ToolbarItems within a Toolbar.
    /// </summary>
    [Editor(typeof(Microsoft.Web.UI.WebControls.Design.ToolbarItemCollectionEditor), typeof(UITypeEditor))]
    public class ToolbarItemCollection : BaseChildNodeCollection
    {
        private Toolbar _Parent;

        /// <summary>
        /// Initializes a new instance of a ToolbarItemCollection.
        /// </summary>
        public ToolbarItemCollection() : base()
        {
            _Parent = null;
        }

        /// <summary>
        /// Initializes a new instance of a ToolbarItemCollection.
        /// </summary>
        /// <param name="parent">The parent Toolbar of this collection.</param>
        public ToolbarItemCollection(Toolbar parent) : base()
        {
            _Parent = parent;
        }

        /// <summary>
        /// The parent Toolbar containing this collection of items.
        /// </summary>
        private Toolbar ParentToolbar
        {
            get { return _Parent; }
        }

        /// <summary>
        /// Sets item properties on being inserted.
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsert(int index, object value)
        {
            ToolbarItem item = (ToolbarItem)value;
            IList list = null;

            if ((item is ToolbarCheckButton) &&
                (((ToolbarCheckButton)item).Group != null))
            {
                list = ((ToolbarCheckButton)item).Group.Items;
            }
            else if (item.ParentToolbar != null)
            {
                list = item.ParentToolbar.Items;
            }

            if (list != null)
            {
                list.Remove(item);
            }

            SetItemProperties((ToolbarItem)value);

            base.OnInsert(index, value);
        }

        /// <summary>
        /// Cleans up properties
        /// </summary>
        /// <param name="index">The index of the object that was removed.</param>
        /// <param name="value">The object that was removed.</param>
        protected override void OnRemoveComplete(int index, object value)
        {
            ((ToolbarItem)value).SetParentToolbar(null);
        }

        /// <summary>
        /// Preps the items to be cleared.
        /// </summary>
        protected override void OnClear()
        {
            foreach (ToolbarItem item in List)
            {
                item.SetParentToolbar(null);
            }

            base.OnClear();
        }

        /// <summary>
        /// Sets item properties on being set.
        /// </summary>
        /// <param name="index">The index of the item being changed.</param>
        /// <param name="oldValue">The old item.</param>
        /// <param name="newValue">The new item.</param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            SetItemProperties((ToolbarItem)newValue);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Sets properties of the ToolbarItem before being added.
        /// </summary>
        /// <param name="item">The ToolbarItem to be set.</param>
        private void SetItemProperties(ToolbarItem item)
        {
            item.SetParentToolbar(ParentToolbar);
        }

        /// <summary>
        /// Adds a ToolbarItem to the collection.
        /// </summary>
        /// <param name="item">The ToolbarItem to add.</param>
        public void Add(ToolbarItem item)
        {
            if (!Contains(item))
            {
                List.Add(item);
            }
        }

        /// <summary>
        /// Adds a ToolbarItem to the collection at a specific index.
        /// </summary>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="item">The ToolbarItem to add.</param>
        public void AddAt(int index, ToolbarItem item)
        {
            if (!Contains(item))
            {
                List.Insert(index, item);
            }
        }

        /// <summary>
        /// Determines if a ToolbarItem is in the collection.
        /// </summary>
        /// <param name="item">The ToolbarItem to search for.</param>
        /// <returns>true if the ToolbarItem exists within the collection. false otherwise.</returns>
        public bool Contains(ToolbarItem item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Determines zero-based index of a ToolbarItem within the collection.
        /// </summary>
        /// <param name="item">The ToolbarItem to locate within the collection.</param>
        /// <returns>The zero-based index.</returns>
        public int IndexOf(ToolbarItem item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// The "flat" index of an item.
        /// Example:
        /// Toolbar: Button1 Group1[CheckBtn1, CheckBtn2] Button2
        ///              Flat   IndexOf
        ///   Button1    0      0
        ///   Group1     -1     1
        ///   CheckBtn1  1      -1
        ///   CheckBtn2  2      -1
        ///   Button2    3      2
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int FlatIndexOf(ToolbarItem item)
        {
            if (item is ToolbarCheckGroup)
            {
                return -1;
            }

            int index = 0;

            foreach (ToolbarItem testItem in List)
            {
                if (testItem.Equals(item))
                {
                    return index;
                }

                if (testItem is ToolbarCheckGroup)
                {
                    ToolbarCheckGroup group = (ToolbarCheckGroup)testItem;
                    int innerIndex = (item is ToolbarCheckButton) ? group.Items.IndexOf((ToolbarCheckButton)item) : -1;
                    if (innerIndex >= 0)
                    {
                        return index + innerIndex;
                    }
                    else
                    {
                        index += group.Items.Count;
                    }
                }
                else
                {
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Retrieves the item at the specified flat index.
        /// </summary>
        /// <param name="index">The flat index.</param>
        /// <returns>The item or null if not found.</returns>
        public ToolbarItem FlatIndexItem(int index)
        {
            if (index >= 0)
            {
                foreach (ToolbarItem item in List)
                {
                    if (item is ToolbarCheckGroup)
                    {
                        ToolbarCheckGroup group = (ToolbarCheckGroup)item;
                        if (index < group.Items.Count)
                        {
                            // The item is within the group
                            return group.Items[index];
                        }
                        else
                        {
                            // The item is outside the group
                            index -= group.Items.Count;
                        }
                    }
                    else
                    {
                        index--;

                        if (index < 0)
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Removes a ToolbarItem from the collection.
        /// </summary>
        /// <param name="item">The ToolbarItem to remove.</param>
        public void Remove(ToolbarItem item)
        {
            List.Remove(item);
        }

        /// <summary>
        /// Indexer into the collection.
        /// </summary>
        public ToolbarItem this[int index]
        {
            get { return (ToolbarItem)List[index]; }
        }
    }
}
