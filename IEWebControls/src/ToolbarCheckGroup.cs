//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.ComponentModel;

    /// <summary>
    /// Represents a toolbar check group.
    /// </summary>
    [
    ParseChildren(true, "Items"),
    PersistChildren(true),
    ]
    public class ToolbarCheckGroup : ToolbarItem
    {
        /// <summary>
        /// Fired when a ToolbarCheckButton within the group is clicked.
        /// </summary>
        [ResDescription("ToolbarButtonClickGroup")]
        public event ToolbarItemEventHandler ButtonClick;

        /// <summary>
        /// Fired when a ToolbarCheckButton's state changes.
        /// </summary>
        [ResDescription("ToolbarCheckChangeGroup")]
        public event ToolbarItemEventHandler CheckChange;

        private ToolbarCheckButtonCollection _Items;
        private CssCollection _HoverStyle;
        private CssCollection _SelectedStyle;

        /// <summary>
        /// Initializes a new ToolbarCheckGroup instance.
        /// </summary>
        public ToolbarCheckGroup() : base()
        {
            _Items = new ToolbarCheckButtonCollection(this);
            _HoverStyle = new CssCollection();
            _SelectedStyle = new CssCollection();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            ToolbarCheckGroup copy = (ToolbarCheckGroup)base.Clone();

            copy.ButtonClick = this.ButtonClick;
            copy.CheckChange = this.CheckChange;

            copy._Items = (ToolbarCheckButtonCollection)this._Items.Clone();
            copy._Items.SetParent(copy);
            copy._HoverStyle = (CssCollection)this._HoverStyle.Clone();
            copy._SelectedStyle = (CssCollection)this._SelectedStyle.Clone();


            return copy;
        }

        /// <summary>
        /// Gets the collection of items in the group.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ResDescription("CheckGroupItems"),
        ]
        public virtual ToolbarCheckButtonCollection Items
        {
            get { return _Items; }
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            base.SetViewStateDirty();

            HoverStyle.Dirty = true;
            SelectedStyle.Dirty = true;

            Items.SetViewStateDirty();
        }

        /// <summary>
        /// Sets all items within the StateBag to be clean
        /// </summary>
        protected internal override void SetViewStateClean()
        {
            base.SetViewStateClean();

            HoverStyle.Dirty = false;
            SelectedStyle.Dirty = false;
        }

        /// <summary>
        /// When the button is clicked, calls the ButtonClick event handlers.
        /// </summary>
        /// <param name="sender">The source item.</param>
        /// <param name="e">Event arguments</param>
        /// <returns>true if the event should bubble. false to cancel bubble.</returns>
        protected internal virtual bool OnButtonClick(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                return ButtonClick(sender, e);   // call the delegate
            }

            return true;
        }

        /// <summary>
        /// When the Selected property changes, calls the CheckChange event handlers.
        /// </summary>
        /// <param name="sender">The source item.</param>
        /// <param name="e">Event arguments</param>
        /// <returns>true if the event should bubble. false to cancel bubble.</returns>
        protected internal virtual bool OnCheckChange(object sender, EventArgs e)
        {
            if (CheckChange != null)
            {
                return CheckChange(sender, e);   // call the delegate
            }

            return true;
        }

        /// <summary>
        /// The style to use when in the hover state.
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ChildHoverStyle"),
        ]
        public CssCollection HoverStyle
        {
            get { return _HoverStyle; }
            set { _HoverStyle = value; }
        }

        /// <summary>
        /// The style to use when in the selected state.
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ChildSelectedStyle"),
        ]
        public CssCollection SelectedStyle
        {
            get { return _SelectedStyle; }
            set { _SelectedStyle = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the state automatically posts back to the server when clicked.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("AutoPostBack"),
        ]
        public bool AutoPostBack
        {
            get
            {
                object obj = ViewState["AutoPostBack"];
                if (obj != null)
                {
                    return (bool)obj;
                }

                Toolbar parent = ParentToolbar;
                return (parent == null) ? false : parent.AutoPostBack;
            }

            set { ViewState["AutoPostBack"] = value; }
        }

        /// <summary>
        /// When true, requires that at least one checkbutton be selected.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("CheckGroupForceSelection"),
        ]
        public bool ForceSelection
        {
            get
            {
                Object obj = ViewState["ForceSelection"];
                return (obj == null) ? false : (bool)obj;
            }

            set
            {
                ViewState["ForceSelection"] = value;
                if (value && (SelectedIndex < 0) && (Items.Count > 0))
                {
                    Items[0].Selected = true;
                }
            }
        }

        /// <summary>
        /// Returns the currently selected checkbutton.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolbarCheckButton SelectedCheckButton
        {
            get
            {
                int index = SelectedIndex;
                if (index >= 0)
                {
                    return Items[index];
                }

                return null;
            }
        }

        /// <summary>
        /// The index of the currently selected checkbutton.
        /// </summary>
        internal int SelectedIndex
        {
            get
            {
                int index = 0;

                foreach (ToolbarCheckButton btn in Items)
                {
                    if (btn.Selected)
                    {
                        return index;
                    }

                    index++;
                }

                return -1;
            }
        }

        /// <summary>
        /// When multiple items are selected, resolve who should be selected.
        /// </summary>
        internal void ResolveSelectedItems()
        {
            bool foundSelected = false;

            foreach (ToolbarCheckButton btn in Items)
            {
                if (btn.Selected)
                {
                    if (foundSelected)
                    {
                        btn.SetSelected(false);
                    }
                    else
                    {
                        foundSelected = true;
                    }
                }
            }

            if (!foundSelected && ForceSelection && (Items.Count > 0))
            {
                Items[0].SetSelected(true);
            }
        }

        /// <summary>
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected override string UpLevelTag
        {
            get { return Toolbar.CheckGroupTagName; }
        }

        /// <summary>
        /// Returns true if the child has uplevel content.
        /// </summary>
        protected override bool HasUpLevelContent
        {
            get { return true; }
        }

        /// <summary>
        /// Renders ToolbarItem attributes.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to receive markup.</param>
        protected override void WriteItemAttributes(HtmlTextWriter writer)
        {
            base.WriteItemAttributes(writer);

            string style = HoverStyle.CssText;
            if (style != String.Empty)
            {
                writer.WriteAttribute("hoverStyle", style);
            }
            style = SelectedStyle.CssText;
            if (style != String.Empty)
            {
                writer.WriteAttribute("selectedStyle", style);
            }

            writer.WriteAttribute("forceSelection", ForceSelection.ToString());
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void UpLevelContent(HtmlTextWriter writer)
        {
            writer.WriteLine();
            writer.Indent++;
            RenderContents(writer, RenderPathID.UpLevelPath);
            writer.Indent--;
        }

        /// <summary>
        /// Renders the item for downlevel browsers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter writer)
        {
            DownLevelContent(writer);
        }

        /// <summary>
        /// Renders the item for visual designers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDesignerPath(HtmlTextWriter writer)
        {
            DesignerContent(writer);
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DownLevelContent(HtmlTextWriter writer)
        {
            RenderContents(writer, RenderPathID.DownLevelPath);
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DesignerContent(HtmlTextWriter writer)
        {
            RenderContents(writer, RenderPathID.DesignerPath);
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        /// <param name="renderPath">The RenderPathID.</param>
        private void RenderContents(HtmlTextWriter writer, RenderPathID renderPath)
        {
            foreach (ToolbarCheckButton btn in Items)
            {
                btn.Render(writer, renderPath);
            }
        }

        /// <summary>
        /// Loads the item's previously saved view state.
        /// </summary>
        /// <param name="savedState">An Object that contains the saved view state values for the item.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] state = (object[])savedState;

                base.LoadViewState(state[0]);
                ((IStateManager)HoverStyle).LoadViewState(state[1]);
                ((IStateManager)SelectedStyle).LoadViewState(state[2]);
                ((IStateManager)Items).LoadViewState(state[3]);
            }
        }

        /// <summary>
        /// Saves the changes to the item's view state to an object.
        /// </summary>
        /// <returns>The object that contains the view state changes.</returns>
        protected override object SaveViewState()
        {
            object state = base.SaveViewState();
            object hoverStyle = ((IStateManager)HoverStyle).SaveViewState();
            object selectedStyle = ((IStateManager)SelectedStyle).SaveViewState();
            object items = ((IStateManager)Items).SaveViewState();

            if ((state != null) || (hoverStyle != null) || (selectedStyle != null) || (items != null))
            {
                return new object[] { state, hoverStyle, selectedStyle, items };
            }

            return null;
        }

        /// <summary>
        /// Instructs the control to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)HoverStyle).TrackViewState();
            ((IStateManager)SelectedStyle).TrackViewState();
            ((IStateManager)Items).TrackViewState();
        }
    }
}
