//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Abstract base class for TreeNodes and TreeNodeTypes.
    /// </summary>
    public abstract class TreeBase : BaseChildNode
    {
        private CssCollection _HoverStyle;
        private CssCollection _DefaultStyle;
        private CssCollection _SelectedStyle;
        internal Object _Parent;

        /// <summary>
        /// Initializes a new instance of a TreeBase.
        /// </summary>
        public TreeBase() : base() 
        {
            _HoverStyle = new CssCollection();
            _DefaultStyle = new CssCollection();
            _SelectedStyle = new CssCollection();
        }

        /// <summary>
        /// Style to use when hovered
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ChildHoverStyle"),
        ]
        public CssCollection HoverStyle
        {
            get 
            {
                return _HoverStyle;
            }
            set 
            {
                _HoverStyle = value;
                if (((IStateManager)this).IsTrackingViewState)
                {
                    ((IStateManager)_HoverStyle).TrackViewState();
                    _HoverStyle.Dirty = true;
                }
            }
        }

        /// <summary>
        /// Default style
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ChildDefaultStyle"),
        ]
        public CssCollection DefaultStyle
        {
            get 
            {
                return _DefaultStyle;
            }
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
        /// Style to use when selected
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ChildSelectedStyle"),
        ]
        public CssCollection SelectedStyle
        {
            get
            {
                return _SelectedStyle;
            }
            set 
            {
                _SelectedStyle = value;
                if (((IStateManager)this).IsTrackingViewState)
                {
                    ((IStateManager)_SelectedStyle).TrackViewState();
                    _SelectedStyle.Dirty = true;
                }
            }
        }

        /// <summary>
        /// Returns a reference to the parent object.
        /// </summary>
        public override object Parent
        {
            get
            {
                return _Parent;
            }
        }

        /// <summary>
        /// Url of the image to display when not selected, expanded, or hovered
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        Editor(typeof(Microsoft.Web.UI.WebControls.Design.ObjectImageUrlEditor), typeof(UITypeEditor)),
        ResDescription("TreeImageUrl"),
        ]
        public String ImageUrl
        {
            get
            {
                object str = ViewState["ImageUrl"];
                return ((str == null) ? String.Empty : (String) str); 
            }
            set
            {
                ViewState["ImageUrl"] = value;
            }
        }

        /// <summary>
        /// Url of the image to display when selected
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        Editor(typeof(Microsoft.Web.UI.WebControls.Design.ObjectImageUrlEditor), typeof(UITypeEditor)),
        ResDescription("TreeSelectedImageUrl"),
        ]
        public String SelectedImageUrl
        {
            get
            {
                object str = ViewState["SelectedImageUrl"];
                return ((str == null) ? String.Empty : (String) str); 
            }
            set
            {
                ViewState["SelectedImageUrl"] = value;
            }
        }

        /// <summary>
        /// Url of the image to display when expanded
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        Editor(typeof(Microsoft.Web.UI.WebControls.Design.ObjectImageUrlEditor), typeof(UITypeEditor)),
        ResDescription("TreeExpandedImageUrl"),
        ]
        public String ExpandedImageUrl
        {
            get
            {
                object str = ViewState["ExpandedImageUrl"];
                return ((str == null) ? String.Empty : (String) str); 
            }
            set
            {
                ViewState["ExpandedImageUrl"] = value;
            }
        }

        /// <summary>
        /// id of the window to target with a navigation upon selecting this node
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeTarget"),
        ]
        public String Target
        {
            get
            {
                object str = ViewState["Target"];
                return ((str == null) ? String.Empty : (String)str);
            }
            set
            {
                ViewState["Target"] = value;
            }
        }

        /// <summary>
        /// Name of the TreeNodeType to apply by default to all of the node's children
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        Editor(typeof(Microsoft.Web.UI.WebControls.Design.NodeTypeEditor), typeof(UITypeEditor)),
        ResDescription("TreeChildType"),
        ]
        public String ChildType 
        {
            get 
            {
                object str = ViewState["ChildType"];
                return ((str == null) ? String.Empty : (String) str);
            }
            set 
            {
                ViewState["ChildType"] = value;
            }
        }

        /// <summary>
        /// Whether or not to display a checkbox
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeCheckBox"),
        ]
        public bool CheckBox
        {
            get
            {
                object b = ViewState["CheckBox"];
                return ((b == null) ? false : (bool) b); 
            }
            set
            {
                ViewState["CheckBox"] = value;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            TreeBase copy = (TreeBase)base.Clone();

            copy._DefaultStyle = (CssCollection)this._DefaultStyle.Clone();
            copy._HoverStyle = (CssCollection)this._HoverStyle.Clone();
            copy._SelectedStyle = (CssCollection)this._SelectedStyle.Clone();

            return copy;
        }

        /// <summary>
        /// Derived classes must still override this function to render.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderUpLevelPath(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Derived classes must still override this function to render.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Derived classes must still override this funtion to render.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDesignerPath(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Loads a saved state from ViewState into the object.
        /// </summary>
        /// <param name="savedState">The saved state.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] state = (object[])savedState;

                base.LoadViewState(state[0]);
                ((IStateManager)DefaultStyle).LoadViewState(state[1]);
                ((IStateManager)HoverStyle).LoadViewState(state[2]);
                ((IStateManager)SelectedStyle).LoadViewState(state[3]);
            }
        }

        /// <summary>
        /// Saves the changes to the object's view state to an Object.
        /// </summary>
        /// <returns>The object that contains the view state changes.</returns>
        protected override object SaveViewState()
        {
            object[] state = new object[]
            {
                base.SaveViewState(),
                ((IStateManager)DefaultStyle).SaveViewState(),
                ((IStateManager)HoverStyle).SaveViewState(),
                ((IStateManager)SelectedStyle).SaveViewState(),
            };

            // Check to see if we're really saving anything
            foreach (object obj in state)
            {
                if (obj != null)
                {
                    return state;
                }
            }

            return null;
        }

        /// <summary>
        /// Instructs the item to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)DefaultStyle).TrackViewState();
            ((IStateManager)HoverStyle).TrackViewState();
            ((IStateManager)SelectedStyle).TrackViewState();
        }

        internal object GetStateVar(String att)
        {
            if (att.EndsWith("Style"))
                return this.GetType().InvokeMember (att, BindingFlags.Default | BindingFlags.GetProperty, null, this, new object [] {});
            else
                return ViewState[att];
        }

        /// <summary>
        /// Adds attributes to be rendered on the main tag for this object.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive the output.</param>
        protected override void AddAttributesToRender(HtmlTextWriter output)
        {
            String cssText;

            cssText = DefaultStyle.CssText;
            if (cssText != String.Empty)
                output.AddAttribute("DefaultStyle", cssText);

            cssText = HoverStyle.CssText;
            if (cssText != String.Empty)
                output.AddAttribute("HoverStyle", cssText);

            cssText = SelectedStyle.CssText;
            if (cssText != String.Empty)
                output.AddAttribute("SelectedStyle", cssText);

            if (ImageUrl != String.Empty)
                output.AddAttribute("ImageUrl", ImageUrl);

            if (SelectedImageUrl != String.Empty)
                output.AddAttribute("SelectedImageUrl", SelectedImageUrl);

            if (ExpandedImageUrl != String.Empty)
                output.AddAttribute("ExpandedImageUrl", ExpandedImageUrl);

            if (ChildType != String.Empty)
                output.AddAttribute("ChildType", ChildType);

            if (Target != String.Empty)
                output.AddAttribute("Target", Target);

            if (ViewState["CheckBox"] != null)
                output.AddAttribute("CheckBox", CheckBox.ToString());

            base.AddAttributesToRender(output);
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            DefaultStyle.Dirty = true;
            HoverStyle.Dirty = true;
            SelectedStyle.Dirty = true;

            base.SetViewStateDirty();
        }

        /// <summary>
        /// Sets all items within the StateBag to be clean
        /// </summary>
        protected internal override void SetViewStateClean()
        {
            DefaultStyle.Dirty = true;
            HoverStyle.Dirty = true;
            SelectedStyle.Dirty = true;

            base.SetViewStateClean();
        }
    }
}
