//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;
    using System.ComponentModel;

    /// <summary>
    /// Represents a toolbar checkbutton.
    /// </summary>
    public class ToolbarCheckButton : ToolbarButton
    {
        /// <summary>
        /// Event fired whenever the Selected property changes.
        /// </summary>
        [ResDescription("ToolbarCheckChangeChild")]
        public event ToolbarItemEventHandler CheckChange;

        private ToolbarCheckGroup _Group;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            ToolbarCheckButton copy = (ToolbarCheckButton)base.Clone();

            copy.CheckChange = this.CheckChange;

            return copy;
        }

        /// <summary>
        /// When the Selected property changes, calls the CheckChange event handlers.
        /// </summary>
        /// <param name="e">Event arguments</param>
        /// <returns>true if the event should bubble. false to cancel bubble.</returns>
        protected internal virtual bool OnCheckChange(EventArgs e)
        {
            if (CheckChange != null)
            {
                return CheckChange(this, e);   // call the delegate
            }

            return true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                bool enabled = base.Enabled;
                if (Group != null)
                {
                    return Group.Enabled && enabled;
                }

                return enabled;
            }

            set { base.Enabled = value; }
        }

        /// <summary>
        /// The index of the the toolbar item within the parent.
        /// </summary>
        public override int Index
        {
            get
            {
                if (Group != null)
                {
                    return Group.Items.IndexOf(this);
                }
                else
                {
                    return base.Index;
                }
            }
        }

        /// <summary>
        /// The Toolbar control that contains this item.
        /// </summary>
        public override Toolbar ParentToolbar
        {
            get { return (Group == null) ? base.ParentToolbar : Group.ParentToolbar; }
        }

        /// <summary>
        /// The ToolbarCheckGroup that this checkbutton belongs to.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolbarCheckGroup Group
        {
            get { return _Group; }
        }

        /// <summary>
        /// Sets the checkgroup that this checkbutton belongs to.
        /// </summary>
        /// <param name="group">The checkgroup that this checkbutton belongs to.</param>
        internal void SetParentCheckGroup(ToolbarCheckGroup group)
        {
            _Group = group;
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
                bool bPostBack;
                Object obj = ViewState["AutoPostBack"];

                if (obj == null)
                {
                    ToolbarCheckGroup group = Group;
                    if (group == null)
                    {
                        Toolbar tbParent = ParentToolbar;
                        bPostBack = (tbParent == null) ? false : tbParent.AutoPostBack;
                    }
                    else
                    {
                        bPostBack = group.AutoPostBack;
                    }
                }
                else
                {
                    bPostBack = (bool)obj;
                }

                return bPostBack;
            }

            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        /// <summary>
        /// Indicates if this checkbutton is selected.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("CheckButtonSelected"),
        ]
        public bool Selected
        {
            get
            {
                Object obj = ViewState["Selected"];
                return (obj == null) ? false : (bool)obj;
            }

            set
            {
                ToolbarCheckGroup group = Group;

                if (value)
                {
                    // If part of a group, deselect the selected checkbutton
                    // before selecting this checkbutton.
                    if (group != null)
                    {
                        ToolbarCheckButton prevBtn = group.SelectedCheckButton;
                        if (prevBtn != null)
                        {
                            prevBtn.SetSelected(false);
                        }
                    }

                    SetSelected(true);
                }
                else
                {
                    // If not part of a group or not enforcing a selection,
                    // deselect the checkbutton.
                    if ((group == null) || (!group.ForceSelection))
                    {
                        SetSelected(false);
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the selected state of the checkbutton.
        /// </summary>
        public void Toggle()
        {
            Selected = !Selected;
        }

        /// <summary>
        /// Sets the selected state.
        /// </summary>
        /// <param name="selected">The new selected state.</param>
        internal void SetSelected(bool selected)
        {
            ViewState["Selected"] = selected;
        }

        /// <summary>
        /// The product of merging local and global styles.
        /// </summary>
        protected override CssCollection CurrentStyle
        {
            get
            {
                CssCollection style = base.CurrentStyle;

                // Slide in the group's style
                ToolbarCheckGroup group = Group;
                if (group != null)
                {
                    style.Merge(group.DefaultStyle, true);
                    style.Merge(DefaultStyle, true);
                }

                if (Selected)
                {
                    // Built-in selected style
                    style.Remove("border-color");
                    style["border-top-color"] = style["border-left-color"] = "#999999";
                    style["border-bottom-color"] = style["border-right-color"] = "#FFFFFF";

                    Toolbar parent = ParentToolbar;
                    if (parent != null)
                    {
                        style.Merge(parent.SelectedStyle, true);
                    }
                    if (group != null)
                    {
                        style.Merge(group.SelectedStyle, true);
                    }
                    style.Merge(SelectedStyle, true);
                }

                return style;
            }
        }

        /// <summary>
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected override string UpLevelTag
        {
            get { return Toolbar.CheckButtonTagName; }
        }

        /// <summary>
        /// Renders ToolbarItem attributes.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to receive markup.</param>
        protected override void WriteItemAttributes(HtmlTextWriter writer)
        {
            base.WriteItemAttributes(writer);

            if (AutoPostBack)
            {
                writer.WriteAttribute("_autopostback", "true");
            }

            writer.WriteAttribute("selected", Selected.ToString());
        }

        /// <summary>
        /// The postback anchor script string.
        /// </summary>
        protected override string AnchorHref
        {
            get
            {
                string href = base.AnchorHref;

                Toolbar parent = ParentToolbar;
                if (parent == null)
                {
                    return href;
                }

                string script = parent.ClientHelperID + ".value+='";
                script += Selected ? "-" : "+";
                script += ClientIndex.ToString() + ";';";

                return script + href;
            }
        }

        /// <summary>
        /// Renders the text property.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        /// <param name="text">The text to render.</param>
        protected override void RenderText(HtmlTextWriter writer, string text)
        {
            if (Selected)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
            }

            base.RenderText(writer, text);
        }
    }
}
