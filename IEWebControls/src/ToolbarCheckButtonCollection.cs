//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;
    using System.Collections;

    /// <summary>
    /// Collection of ToolbarCheckButtons within a Toolbar.
    /// </summary>
    public class ToolbarCheckButtonCollection : BaseChildNodeCollection
    {
        private ToolbarCheckGroup _Parent;

        /// <summary>
        /// Initializes a new instance of a ToolbarCheckButtonCollection.
        /// </summary>
        public ToolbarCheckButtonCollection() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of a ToolbarCheckButtonCollection.
        /// </summary>
        /// <param name="parent">The parent Toolbar of this collection.</param>
        public ToolbarCheckButtonCollection(ToolbarCheckGroup parent) : base()
        {
            _Parent = parent;
        }

        /// <summary>
        /// The parent Toolbar containing this collection of items.
        /// </summary>
        private Toolbar ParentToolbar
        {
            get { return (_Parent != null) ? _Parent.ParentToolbar : null; }
        }

        /// <summary>
        /// The parent ToolbarCheckGroup containing this collection.
        /// </summary>
        private ToolbarCheckGroup ParentCheckGroup
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
            ToolbarCheckButton btn = (ToolbarCheckButton)value;
            IList list = null;

            if (btn.Group != null)
            {
                list = ((ToolbarCheckButton)btn).Group.Items;
            }
            else if (btn.ParentToolbar != null)
            {
                list = btn.ParentToolbar.Items;
            }

            if (list != null)
            {
                list.Remove(btn);
            }


            if (!Reloading && ((IStateManager)this).IsTrackingViewState &&
                (ParentCheckGroup.SelectedCheckButton != null) && (btn.Selected))
            {
                ParentCheckGroup.SelectedCheckButton.SetSelected(false);
            }

            SetItemProperties(btn);

            base.OnInsert(index, value);
        }

        /// <summary>
        /// Adjusts the selected button after inserting an item.
        /// </summary>
        /// <param name="index">The index of the item</param>
        /// <param name="value">The item inserted</param>
        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);

            ToolbarCheckGroup group = ParentCheckGroup;
            if (!Reloading && (group != null) && ((IStateManager)this).IsTrackingViewState)
            {
                group.ResolveSelectedItems();
            }
        }

        /// <summary>
        /// Cleans up SelectedCheckButton property
        /// </summary>
        /// <param name="index">The index of the object that was removed.</param>
        /// <param name="value">The object that was removed.</param>
        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);

            ToolbarCheckButton oldBtn = (ToolbarCheckButton)value;
            ToolbarCheckGroup group = ParentCheckGroup;
            if (!Reloading && (group != null) && oldBtn.Selected)
            {
                // The selected button was removed
                group.ResolveSelectedItems();
            }

            oldBtn.SetParentCheckGroup(null);
            oldBtn.SetParentToolbar(null);
        }

        /// <summary>
        /// Preps the items to be cleared.
        /// </summary>
        protected override void OnClear()
        {
            foreach (ToolbarCheckButton btn in List)
            {
                btn.SetParentCheckGroup(null);
                btn.SetParentToolbar(null);
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
            SetItemProperties((ToolbarCheckButton)newValue);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Sets properties of the ToolbarCheckButton before being added.
        /// </summary>
        /// <param name="item">The ToolbarCheckButton to be set.</param>
        private void SetItemProperties(ToolbarCheckButton item)
        {
            item.SetParentToolbar(ParentToolbar);
            item.SetParentCheckGroup(ParentCheckGroup);
        }

        /// <summary>
        /// Sets the parent checkgroup when one was not initially available.
        /// </summary>
        /// <param name="parentCheckGroup"></param>
        internal void SetParent(ToolbarCheckGroup parentCheckGroup)
        {
            _Parent = parentCheckGroup;

            foreach (ToolbarCheckButton item in this)
            {
                SetItemProperties(item);
            }
        }

        /// <summary>
        /// Adds a ToolbarCheckButton to the collection.
        /// </summary>
        /// <param name="item">The ToolbarCheckButton to add.</param>
        public void Add(ToolbarCheckButton item)
        {
            if (!Contains(item))
            {
                List.Add(item);
            }
        }

        /// <summary>
        /// Adds a ToolbarCheckButton to the collection at a specific index.
        /// </summary>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="item">The ToolbarCheckButton to add.</param>
        public void AddAt(int index, ToolbarCheckButton item)
        {
            if (!Contains(item))
            {
                List.Insert(index, item);
            }
        }

        /// <summary>
        /// Determines if a ToolbarCheckButton is in the collection.
        /// </summary>
        /// <param name="item">The ToolbarCheckButton to search for.</param>
        /// <returns>true if the ToolbarCheckButton exists within the collection. false otherwise.</returns>
        public bool Contains(ToolbarCheckButton item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Determines zero-based index of a ToolbarCheckButton within the collection.
        /// </summary>
        /// <param name="item">The ToolbarCheckButton to locate within the collection.</param>
        /// <returns>The zero-based index.</returns>
        public int IndexOf(ToolbarCheckButton item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// Removes a ToolbarCheckButton from the collection.
        /// </summary>
        /// <param name="item">The ToolbarCheckButton to remove.</param>
        public void Remove(ToolbarCheckButton item)
        {
            List.Remove(item);
        }

        /// <summary>
        /// Indexer into the collection.
        /// </summary>
        public ToolbarCheckButton this[int index]
        {
            get { return (ToolbarCheckButton)List[index]; }
        }
    }
}
