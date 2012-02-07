//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;
    using System.ComponentModel;

    /// <summary>
    /// Event handler for ToolbarItem events.
    /// </summary>
    public delegate bool ToolbarItemEventHandler(Object sender, EventArgs e);

    /// <summary>
    /// Represents a toolbar button.
    /// </summary>
    public class ToolbarButton : ToolbarLabel
    {
        /// <summary>
        /// Event fires when the button is clicked.
        /// </summary>
        [ResDescription("ToolbarButtonClickChild")]
        public event ToolbarItemEventHandler ButtonClick;

        private CssCollection _HoverStyle;
        private CssCollection _SelectedStyle;

        /// <summary>
        /// Initializes a new ToolbarButton instance.
        /// </summary>
        public ToolbarButton() : base()
        {
            _HoverStyle = new CssCollection();
            _SelectedStyle = new CssCollection();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            ToolbarButton copy = (ToolbarButton)base.Clone();

            copy.ButtonClick = this.ButtonClick;

            copy._HoverStyle = (CssCollection)this._HoverStyle.Clone();
            copy._SelectedStyle = (CssCollection)this._SelectedStyle.Clone();

            return copy;
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            base.SetViewStateDirty();

            HoverStyle.Dirty = true;
            SelectedStyle.Dirty = true;
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
        /// <param name="e">Event arguments</param>
        /// <returns>true if the event should bubble. false to cancel bubble.</returns>
        protected internal virtual bool OnButtonClick(EventArgs e)
        {
            if (ButtonClick != null)
            {
                return ButtonClick(this, e);   // call the delegate
            }

            return true;
        }

        /// <summary>
        /// The style of the toolbar button when in the hover state.
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
        /// The style of the toolbar button when in the selected state.
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
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected override string UpLevelTag
        {
            get { return Toolbar.ButtonTagName; }
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
                writer.WriteAttribute("hoverstyle", style);
            }
            style = SelectedStyle.CssText;
            if (style != String.Empty)
            {
                writer.WriteAttribute("selectedstyle", style);
            }

            writer.WriteAttribute("onkeydown", "if (event.keyCode==13){event.returnValue=false}");
        }

        /// <summary>
        /// Renders the image tag.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        /// <param name="imageUrl">The url of the image.</param>
        protected override void RenderImage(HtmlTextWriter writer, string imageUrl)
        {
            if (Enabled)
            {
                AddAnchorAttributes(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }

            base.RenderImage(writer, imageUrl);

            if (Enabled)
            {
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Renders the text property.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        /// <param name="text">The text to render.</param>
        protected override void RenderText(HtmlTextWriter writer, string text)
        {
            if (Enabled)
            {
                AddAnchorAttributes(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
            }

            base.RenderText(writer, text);

            if (Enabled)
            {
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// The postback anchor script string.
        /// </summary>
        protected virtual string AnchorHref
        {
            get { return ParentToolbar.Page.GetPostBackEventReference(ParentToolbar, ClientIndex.ToString()); }
        }

        /// <summary>
        /// Adds attributes for the anchor tag.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        private void AddAnchorAttributes(HtmlTextWriter writer)
        {
            CssCollection currentStyle = CurrentStyle;

            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + AnchorHref);

            if (AccessKey != String.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);
            }

            if (TabIndex != 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString());
            }

		writer.AddAttribute("onclick", "return confirm('Are you sure?');");

            string style = String.Empty;

            // Custom underline
            string textDec = currentStyle["text-decoration"];
            if (textDec != null)
            {
                style += "text-decoration:" + textDec + ";";
            }

            // Custom cursor
            string cursor = currentStyle["cursor"];
            if (cursor != null)
            {
                style += "cursor:" + cursor + ";";
            }

            // Re-apply custom styles
            if (style != String.Empty)
            {
                writer.AddAttribute("style", style);
            }

            // Re-apply the current color;
            string currentColor = currentStyle["color"];
            if ((currentColor != null) && (currentColor != String.Empty))
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, currentColor);
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
            }
        }

        /// <summary>
        /// Saves the changes to the item's view state to an object.
        /// </summary>
        /// <returns>The object that contains the view state changes.</returns>
        protected override object SaveViewState()
        {
            object[] state = new object[]
            {
                base.SaveViewState(),
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
        /// Instructs the control to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)HoverStyle).TrackViewState();
            ((IStateManager)SelectedStyle).TrackViewState();
        }
    }
}
