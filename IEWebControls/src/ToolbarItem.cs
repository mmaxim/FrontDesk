//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;
    using System.ComponentModel;

    /// <summary>
    /// Base class for Toolbar child nodes.
    /// </summary>
    [ToolboxItem(false)]
    public abstract class ToolbarItem : BaseChildNode
    {
        private Toolbar _Parent;
        private CssCollection _DefaultStyle;

        /// <summary>
        /// Initializes a new instance of a ToolbarItem.
        /// </summary>
        public ToolbarItem() : base()
        {
            _DefaultStyle = new CssCollection();
        }

        /// <summary>
        /// Returns a String that represents the current Object.
        /// </summary>
        /// <returns>A String that represents the current Object.</returns>
        public override string ToString()
        {
            string name = this.GetType().Name;

            if (ID != String.Empty)
            {
                name += " - " + ID;
            }

            return name;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            ToolbarItem copy = (ToolbarItem)base.Clone();

            copy._DefaultStyle = (CssCollection)this._DefaultStyle.Clone();

            return copy;
        }

        /// <summary>
        /// The Toolbar control that contains this item.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Toolbar ParentToolbar
        {
            get { return _Parent; }
        }

        /// <summary>
        /// Returns the parent object.
        /// </summary>
        public override object Parent
        {
            get { return ParentToolbar; }
        }

        /// <summary>
        /// Sets the parent of this item.
        /// </summary>
        /// <param name="parent">The parent Toolbar.</param>
        internal void SetParentToolbar(Toolbar parent)
        {
            _Parent = parent;
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            base.SetViewStateDirty();

            DefaultStyle.Dirty = true;
        }

        /// <summary>
        /// Sets all items within the StateBag to be clean
        /// </summary>
        protected internal override void SetViewStateClean()
        {
            base.SetViewStateClean();

            DefaultStyle.Dirty = false;
        }

        /// <summary>
        /// Gets or sets the keyboard shortcut key (AccessKey) for setting focus to the item.
        /// </summary>
        [DefaultValue("")]
        [Category("Behavior")]
        [ResDescription("BaseAccessKey")]
        public virtual string AccessKey
        {
            get
            {
                object obj = ViewState["AccessKey"];
                return (obj == null) ? String.Empty : (string)obj;
            }

            set { ViewState["AccessKey"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [ResDescription("BaseEnabled")]
        public virtual bool Enabled
        {
            get
            {
                object obj = ViewState["Enabled"];
                return (obj == null) ? true : (bool)obj;
            }

            set { ViewState["Enabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the tab index of the item.
        /// </summary>
        [DefaultValue((short)0)]
        [Category("Behavior")]
        [ResDescription("BaseTabIndex")]
        public virtual short TabIndex
        {
            get
            {
                object obj = ViewState["TabIndex"];
                return (obj == null) ? (short)0 : (short)obj;
            }

            set { ViewState["TabIndex"] = value; }
        }

        /// <summary>
        /// Gets or sets the tool tip for the item to be displayed when the mouse cursor is over the control.
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [ResDescription("BaseToolTip")]
        public virtual string ToolTip
        {
            get
            {
                object obj = ViewState["ToolTip"];
                return (obj == null) ? String.Empty : (string)obj;
            }

            set { ViewState["ToolTip"] = value; }
        }
        
        /// <summary>
        /// Retrieves the orientation from the ParentToolbar.
        /// </summary>
        protected Orientation Orientation
        {
            get { return (ParentToolbar == null) ? Orientation.Horizontal : ParentToolbar.Orientation; }
        }

        /// <summary>
        /// The style of the ToolbarItem when in the default state.
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ChildDefaultStyle"),
        ]
        public CssCollection DefaultStyle
        {
            get { return _DefaultStyle; }
            set
            {
                _DefaultStyle = value;
                if (((IStateManager)this).IsTrackingViewState)
                {
                    ((IStateManager)_DefaultStyle).TrackViewState();
                    _DefaultStyle.Dirty = true;
                }
            }
        }

        /// <summary>
        /// The product of merging local and global styles.
        /// </summary>
        protected virtual CssCollection CurrentStyle
        {
            get
            {
                CssCollection style = new CssCollection();

                // Built-in style
                style.Add("padding", "2px");
                style.Add("border-style", "solid");
                style.Add("border-color", "#D0D0D0");
                style.Add("border-width", "1px");

                // Global style
                if (ParentToolbar != null)
                {
                    style.Merge(ParentToolbar.DefaultStyle, true);
                }

                // Local style
                style.Merge(DefaultStyle, true);

                return style;
            }
        }

        /// <summary>
        /// The index of the the toolbar item within the parent toolbar.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int Index
        {
            get { return (ParentToolbar == null) ? -1 : ParentToolbar.Items.IndexOf(this); }
        }

        /// <summary>
        /// The flat index of the toolbar item.
        /// </summary>
        protected virtual int ClientIndex
        {
            get { return (ParentToolbar == null) ? -1 : ParentToolbar.Items.FlatIndexOf(this); }
        }

        /// <summary>
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected abstract string UpLevelTag
        {
            get;
        }

        /// <summary>
        /// Returns true if the child has uplevel content.
        /// </summary>
        protected virtual bool HasUpLevelContent
        {
            get { return false; }
        }

        /// <summary>
        /// Adds attributes to the HtmlTextWriter.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (AccessKey != String.Empty)
            {
                writer.AddAttribute("accesskey", AccessKey);
            }

            if (!Enabled)
            {
                writer.AddAttribute("disabled", "true");
            }

            if (TabIndex != 0)
            {
                writer.AddAttribute("tabindex", TabIndex.ToString());
            }

            if (ToolTip != String.Empty)
            {
                writer.AddAttribute("title", ToolTip);
            }
        }

        /// <summary>
        /// Writes attributes to the HtmlTextWriter.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void WriteAttributes(HtmlTextWriter writer)
        {
            base.WriteAttributes(writer);

            if (AccessKey != String.Empty)
            {
                writer.WriteAttribute("accesskey", AccessKey);
            }

            if (!Enabled)
            {
                writer.WriteAttribute("disabled", "true");
            }

            if (TabIndex != 0)
            {
                writer.WriteAttribute("tabindex", TabIndex.ToString());
            }

            if (ToolTip != String.Empty)
            {
                writer.WriteAttribute("title", ToolTip);
            }
        }

        /// <summary>
        /// Renders ToolbarItem attributes.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to receive markup.</param>
        protected virtual void WriteItemAttributes(HtmlTextWriter writer)
        {
            string style = DefaultStyle.CssText;
            if (style != String.Empty)
            {
                writer.WriteAttribute("defaultStyle", style);
            }
        }

        /// <summary>
        /// Renders the item for uplevel browsers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderUpLevelPath(HtmlTextWriter writer)
        {
            writer.WriteBeginTag(Toolbar.TagNamespace + ":" + UpLevelTag);

            WriteAttributes(writer);
            WriteItemAttributes(writer);

            if (HasUpLevelContent)
            {
                // Finish the begin tag
                writer.Write(HtmlTextWriter.TagRightChar);

                // Render content
                UpLevelContent(writer);

                // Render the close tag
                writer.WriteEndTag(Toolbar.TagNamespace + ":" + UpLevelTag);
            }
            else
            {
                // Finish and close the tag
                writer.Write(HtmlTextWriter.SelfClosingChars + HtmlTextWriter.TagRightChar);
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Render the item's content.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected virtual void UpLevelContent(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Renders the item for downlevel browsers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter writer)
        {
            if (Orientation == Orientation.Vertical)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }

            HtmlInlineWriter inlineWriter = (writer is HtmlInlineWriter) ? (HtmlInlineWriter)writer : new HtmlInlineWriter(writer);
            CurrentStyle.AddAttributesToRender(inlineWriter);
            inlineWriter.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
            inlineWriter.RenderBeginTag(HtmlTextWriterTag.Td);

            DownLevelContent(inlineWriter);

            inlineWriter.RenderEndTag();

            if (Orientation == Orientation.Vertical)
            {
                writer.RenderEndTag();
            }
            // If the inline writer was passed in, then the WriteLine won't happen
            writer.WriteLine();
        }

        /// <summary>
        /// Renders the item's contents.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected virtual void DownLevelContent(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Renders the item for visual designers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDesignerPath(HtmlTextWriter writer)
        {
            CurrentStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
            if (Orientation == Orientation.Vertical)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            DesignerContent(writer);

            writer.RenderEndTag();  // TD
            if (Orientation == Orientation.Vertical)
            {
                writer.RenderEndTag();
            }
            writer.WriteLine();
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected virtual void DesignerContent(HtmlTextWriter writer)
        {
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

                if (state.Length > 1)
                {
                    ((IStateManager)DefaultStyle).LoadViewState(state[1]);
                }
            }
        }

        /// <summary>
        /// Saves the changes to the item's view state to an object.
        /// </summary>
        /// <returns>The object that contains the view state changes.</returns>
        protected override object SaveViewState()
        {
            object state = base.SaveViewState();
            object defaultStyle = ((IStateManager)DefaultStyle).SaveViewState();

            if (defaultStyle != null)
            {
                return new object[] { state, defaultStyle };
            }
            else if (state != null)
            {
                return new object[] { state };
            }

            return null;
        }

        /// <summary>
        /// Instructs the control to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)DefaultStyle).TrackViewState();
        }
    }
}
