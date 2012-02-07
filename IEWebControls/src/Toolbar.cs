//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Reflection;
    using System.Globalization;

    /// <summary>
    /// Represents a toolbar.
    /// </summary>
    [
    ParseChildren(true, "Items"),
    Designer(typeof(Microsoft.Web.UI.WebControls.Design.ToolbarDesigner)),
    ToolboxBitmap(typeof(Microsoft.Web.UI.WebControls.Toolbar)),
    ]
    public class Toolbar : BasePostBackControl
    {
        /// <summary>
        /// The namespace for the Toolbar and its children.
        /// </summary>
        public const string TagNamespace = "TBNS";

        /// <summary>
        /// The Toolbar tag name.
        /// </summary>
        public const string ToolbarTagName = "Toolbar";

        /// <summary>
        /// The ToolbarButton's tag name.
        /// </summary>
        public const string ButtonTagName = "ToolbarButton";

        /// <summary>
        /// The ToolbarCheckButton's tag name.
        /// </summary>
        public const string CheckButtonTagName = "ToolbarCheckButton";

        /// <summary>
        /// The ToolbarCheckGroup's tag name.
        /// </summary>
        public const string CheckGroupTagName = "ToolbarCheckGroup";

        /// <summary>
        /// The ToolbarSeparator's tag name.
        /// </summary>
        public const string SeparatorTagName = "ToolbarSeparator";

        /// <summary>
        /// The ToolbarLabel's tag name.
        /// </summary>
        public const string LabelTagName = "ToolbarLabel";

        /// <summary>
        /// The ToolbarDropDownList's tag name.
        /// </summary>
        public const string DropDownListTagName = "ToolbarDropDownList";

        /// <summary>
        /// The ToolbarTextBox's tag name.
        /// </summary>
        public const string TextBoxTagName = "ToolbarTextBox";

        /// <summary>
        /// Fired when a button is clicked.
        /// </summary>
        [ResDescription("ToolbarButtonClick")]
        public event EventHandler ButtonClick;

        /// <summary>
        /// Fired when a checkbutton's state changes.
        /// </summary>
        [ResDescription("ToolbarCheckChange")]
        public event EventHandler CheckChange;

        private ToolbarItemCollection _Items;
        private CssCollection _DefaultStyle;
        private CssCollection _HoverStyle;
        private CssCollection _SelectedStyle;

#if false
        // Databinding
        private IEnumerable _DataSource;
        private bool _ClearChildViewState;
#endif

        private ArrayList _Changed;
        private ArrayList _DataChanged;

        /// <summary>
        /// Initializes a new instance of a Toolbar control.
        /// </summary>
        public Toolbar() : base()
        {
            _Items = new ToolbarItemCollection(this);
            _DefaultStyle = new CssCollection();
            _HoverStyle = new CssCollection();
            _SelectedStyle = new CssCollection();

            _Changed = new ArrayList();
            _DataChanged = new ArrayList();
#if false
            // Databinding
            _ClearChildViewState = false;
#endif
        }

        /// <summary>
        /// Gets the collection of items in the control.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ResDescription("ToolbarItems"),
        ]
        public virtual ToolbarItemCollection Items
        {
            get { return _Items; }
        }

        /// <summary>
        /// Overridden. Creates an EmptyControlCollection to prevent controls from
        /// being added to the ControlCollection.
        /// </summary>
        /// <returns>An EmptyControlCollection object.</returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new EmptyControlCollection(this);
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
                Object obj = ViewState["AutoPostBack"];
                return ((obj == null) ? false : (bool)obj);
            }

            set { ViewState["AutoPostBack"] = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal or vertical orientation of the Toolbar.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(Orientation.Horizontal),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ToolbarOrientation"),
        ]
        public Orientation Orientation
        {
            get
            {
                object obj = ViewState["Orientation"];
                return (obj == null) ? Orientation.Horizontal : (Orientation)obj;
            }

            set { ViewState["Orientation"] = value; }
        }

        /// <summary>
        /// Global style for items.
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ParentDefaultStyle"),
        ]
        public CssCollection DefaultStyle
        {
            get { return _DefaultStyle; }
            set
            {
                _DefaultStyle = value;
                if (IsTrackingViewState)
                {
                    ((IStateManager)_DefaultStyle).TrackViewState();
                    _DefaultStyle.Dirty = true;
                }
            }
        }

        /// <summary>
        /// Global style for items in the hover state.
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ParentHoverStyle"),
        ]
        public CssCollection HoverStyle
        {
            get { return _HoverStyle; }
            set
            {
                _HoverStyle = value;
                if (IsTrackingViewState)
                {
                    ((IStateManager)_HoverStyle).TrackViewState();
                    _HoverStyle.Dirty = true;
                }
            }
        }

        /// <summary>
        /// Global style for items in the selected state.
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ParentSelectedStyle"),
        ]
        public CssCollection SelectedStyle
        {
            get { return _SelectedStyle; }
            set
            {
                _SelectedStyle = value;
                if (IsTrackingViewState)
                {
                    ((IStateManager)_SelectedStyle).TrackViewState();
                    _SelectedStyle.Dirty = true;
                }
            }
        }

#if false
        // Databinding
        /// <summary>
        /// Gets or sets the data source that populates the items of the control.
        /// </summary>
        public virtual IEnumerable DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the type content.
        /// </summary>
        public string DataTypeField
        {
            get
            {
                Object obj = ViewState["DataTypeField"];
                return((obj == null) ? String.Empty : (string)obj);
            }

            set { ViewState["DataTypeField"] = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the text content.
        /// </summary>
        public string DataTextField
        {
            get
            {
                Object obj = ViewState["DataTextField"];
                return((obj == null) ? String.Empty : (string)obj);
            }

            set { ViewState["DataTextField"] = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the image url content.
        /// </summary>
        public string DataImageUrlField
        {
            get
            {
                Object obj = ViewState["DataImageUrlField"];
                return((obj == null) ? String.Empty : (string)obj);
            }

            set { ViewState["DataImageUrlField"] = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the selected content.
        /// </summary>
        public string DataSelectedField
        {
            get
            {
                Object obj = ViewState["DataSelectedField"];
                return((obj == null) ? String.Empty : (string)obj);
            }

            set { ViewState["DataSelectedField"] = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the groupname content.
        /// </summary>
        public string DataGroupnameField
        {
            get
            {
                Object obj = ViewState["DataGroupnameField"];
                return((obj == null) ? String.Empty : (string)obj);
            }

            set { ViewState["DataGroupnameField"] = value; }
        }
#endif

        /// <summary>
        /// Fired when a button is clicked.
        /// </summary>
        /// <param name="sender">The source item.</param>
        /// <param name="e">Event parameters.</param>
        protected virtual void OnButtonClick(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);   // call the delegate if non-null
            }
        }

        /// <summary>
        /// Fired when a checkbutton's state changes.
        /// </summary>
        /// <param name="sender">The source item.</param>
        /// <param name="e">The event parameters.</param>
        protected virtual void OnCheckChange(object sender, EventArgs e)
        {
            if (CheckChange != null)
            {
                CheckChange(sender, e);   // call the delegate if non-null
            }
        }

        /// <summary>
        /// Returns a true value to indicate that a hidden helper is needed by this control.
        /// </summary>
        protected override bool NeedHelper
        {
            get { return true; }
        }

        /// <summary>
        /// Perform the bubbling necessary in firing the ButtonClick event.
        /// </summary>
        /// <param name="item">The source ToolbarItem.</param>
        private void PostButtonClickEvent(ToolbarItem item)
        {
            bool bBubble = true;
            EventArgs eventArgs = new EventArgs();

            if (item is ToolbarButton)
            {
                bBubble = ((ToolbarButton)item).OnButtonClick(eventArgs);
            }

            if (bBubble && (item is ToolbarCheckButton))
            {
                ToolbarCheckGroup group = ((ToolbarCheckButton)item).Group;
                if (group != null)
                {
                    bBubble = group.OnButtonClick(item, eventArgs);
                }
            }

            if (bBubble)
            {
                OnButtonClick(item, eventArgs);
            }
        }

        /// <summary>
        /// Perform the bubbling necessary in firing the CheckChange event.
        /// </summary>
        /// <param name="btn">The source ToolbarCheckButton.</param>
        protected void PostCheckChangeEvent(ToolbarCheckButton btn)
        {
            bool bBubble = true;
            EventArgs eventArgs = new EventArgs();

            bBubble = btn.OnCheckChange(eventArgs);

            if (bBubble && (btn.Group != null))
            {
                bBubble = btn.Group.OnCheckChange(btn, eventArgs);
            }

            if (bBubble)
            {
                OnCheckChange(btn, eventArgs);
            }
        }

        /// <summary>
        /// Parses the posted data string and returns an ArrayList of indexes in string format.
        /// </summary>
        /// <param name="szData">The postback string.</param>
        /// <returns>An ArrayList of indexes.</returns>
        private ArrayList ParseDataString(string szData)
        {
            ArrayList list = new ArrayList();
            int start = 0;
            int end;

            while (start < szData.Length)
            {
                end = szData.IndexOf(';', start);
                if (end > start)
                {
                    string szParam = szData.Substring(start, end - start);
                    list.Add(szParam);

                    start = end + 1;
                }
                else
                {
                    break;
                }
            }

            return list;
        }

        /// <summary>
        /// Processes post back data for the server control given the data from the hidden helper.
        /// </summary>
        /// <param name="szData">The data from the hidden helper</param>
        /// <returns>true if the server control's state changes as a result of the post back; otherwise false.</returns>
        protected override bool ProcessData(string szData)
        {
            if ((szData == null) || (szData == String.Empty))
                return false;

            ArrayList eventList = ParseDataString(szData);
            ArrayList changedBtns = new ArrayList();
            ArrayList oldVals = new ArrayList();

            // Apply the changes
            foreach (string szParam in eventList)
            {
                int flatIndex = Convert.ToInt32(szParam.Substring(1, szParam.Length - 1));
                ToolbarItem item = Items.FlatIndexItem(flatIndex);
                if ((item != null) && (item is ToolbarCheckButton))
                {
                    ToolbarCheckButton btn = (ToolbarCheckButton)item;

                    // If the button affects other buttons, then store those values
                    if (btn.Group != null)
                    {
                        ToolbarCheckButton affectedBtn = btn.Group.SelectedCheckButton;
                        if ((affectedBtn != null) && !changedBtns.Contains(affectedBtn))
                        {
                            changedBtns.Add(affectedBtn);
                            oldVals.Add(affectedBtn.Selected);
                        }
                    }

                    // If we haven't stored the original value, then store it
                    if (!changedBtns.Contains(btn))
                    {
                        changedBtns.Add(btn);
                        oldVals.Add(btn.Selected);
                    }

                    // Apply the change (+ means selected, - means not selected)
                    btn.Selected = ((szParam.Length > 0) && (szParam[0] == '+'));
                }
            }

            // Determine which items changed
            for (int i = 0; i < changedBtns.Count; i++)
            {
                ToolbarCheckButton btn = (ToolbarCheckButton)changedBtns[i];
                if (btn.Selected != (bool)oldVals[i])
                {
                    _Changed.Add(btn);
                }
            }

            return (_Changed.Count > 0);
        }

        /// <summary>
        /// Processes post back data for a server control.
        /// </summary>
        /// <param name="postDataKey">The key identifier for the control.</param>
        /// <param name="postCollection">The collection of all incoming name values.</param>
        /// <returns>true if the server control's state changes as a result of the post back; otherwise false.</returns>
        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            bool changed = base.LoadPostData(postDataKey, postCollection);

            // Find IPostBackDataHandlers and invoke the interface
            // NOTE: Does not recurse into CheckGroups because they should only have
            // CheckButtons which don't implement the interface.
            foreach (ToolbarItem item in Items)
            {
                if (item is IPostBackDataHandler)
                {
                    if (((IPostBackDataHandler)item).LoadPostData(postDataKey, postCollection))
                    {
                        _DataChanged.Add(item);
                    }
                }
            }

            return (changed || (_DataChanged.Count > 0));
        }

        /// <summary>
        /// Signals the server control object to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        protected override void RaisePostDataChangedEvent()
        {
            foreach (ToolbarCheckButton btn in _Changed)
            {
                PostCheckChangeEvent(btn);
            }

            _Changed.Clear();

            foreach (IPostBackDataHandler item in _DataChanged)
            {
                item.RaisePostDataChangedEvent();
            }
        }

        /// <summary>
        /// Enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
        protected override void RaisePostBackEvent(string eventArgument)
        {
            if ((eventArgument != null) && (eventArgument != String.Empty))
            {
                ToolbarItem item = Items.FlatIndexItem(Convert.ToInt32(eventArgument));
                if (item != null)
                {
                    PostButtonClickEvent(item);
                }
            }
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            foreach (ToolbarItem item in Items)
            {
                if (item is ToolbarCheckGroup)
                {
                    ((ToolbarCheckGroup)item).ResolveSelectedItems();
                }
            }
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            bool isUpLevel = (RenderPath == RenderPathID.UpLevelPath);

            foreach (ToolbarItem item in Items)
            {
                if (item is ToolbarDropDownList)
                {
                    ((ToolbarDropDownList)item).SetupHiddenHelper(isUpLevel);
                }
                else if (item is ToolbarTextBox)
                {
                    ((ToolbarTextBox)item).SetupHiddenHelper(isUpLevel);
                }
            }

            if (ID == null)
            {
                ID = UniqueID;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Renders the contents of the control into the specified writer.
        /// </summary>
        /// <param name="writer">The output stream that renders HTML content to the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            foreach (ToolbarItem item in Items)
            {
                item.Render(writer, RenderPath);
            }
        }

        /// <summary>
        /// Renders the control for an uplevel browser.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the control content.</param>
        protected override void RenderUpLevelPath(HtmlTextWriter writer)
        {
            writer.Write("<?XML:NAMESPACE PREFIX=\"" + TagNamespace
                + "\" /><?IMPORT NAMESPACE=\"" + TagNamespace + "\" IMPLEMENTATION=\""
                + AddPathToFilename("toolbar.htc") + "\" />");
            writer.WriteLine();

            AddAttributesToRender(writer);

            if (Orientation == Orientation.Vertical)
                writer.AddAttribute("orientation", "vertical");

            if (Page != null)
            {
                // We replace the '<replaceme>' with event.flatIndex so that we get the actual value 
                // of the variable at runtime and not the string 'event.flatIndex'
                string postBack = "{setAttribute('_submitting', 'true');try{" + Page.GetPostBackEventReference(this, "<replaceme>").Replace("'<replaceme>'", "event.flatIndex") + ";}catch(e){setAttribute('_submitting', 'false');}}";
                string checkNode = "if (event.srcNode != null) ";
                string checkPostBack = "if ((event.srcNode.getType() != 'checkbutton') || (event.srcNode.getAttribute('_autopostback') != null)) if (getAttribute('_submitting') != 'true')";
                string setHelper = HelperID + ".value+=((event.srcNode.getAttribute('selected')=='true')?'+':'-')+event.flatIndex+';';";

                writer.AddAttribute("oncheckchange", "JScript:" + checkNode + setHelper);
                writer.AddAttribute("onbuttonclick", "JScript:" + checkNode + checkPostBack + postBack);

                string readyScript = "JScript:try{" + HelperID + ".value = ''}catch(e){}";
                foreach (ToolbarItem item in Items)
                {
                    ToolbarDropDownList ddl = item as ToolbarDropDownList;
                    ToolbarTextBox tbox = item as ToolbarTextBox;
                    if (ddl != null)
                    {
                        ListItem selItem = ddl.SelectedItem;
                        readyScript += "try{" + ddl.HelperID + ".value = getItem(" + ddl.Index + ").getAttribute('value');}catch(e){}";
                    }
                    else if (tbox != null)
                    {
                        readyScript += "try{" + tbox.HelperID + ".value = getItem(" + tbox.Index + ").getAttribute('value');}catch(e){}";
                    }
                }
                writer.AddAttribute("onwcready", readyScript);
            }

            string style = DefaultStyle.CssText;
            if (style != String.Empty)
                writer.AddAttribute("defaultstyle", style);
            style = HoverStyle.CssText;
            if (style != String.Empty)
                writer.AddAttribute("hoverstyle", style);
            style = SelectedStyle.CssText;
            if (style != String.Empty)
                writer.AddAttribute("selectedstyle", style);

            writer.RenderBeginTag(TagNamespace + ":" + ToolbarTagName);
            writer.WriteLine();

            base.RenderUpLevelPath(writer);

            writer.RenderEndTag();
        }

        /// <summary>
        /// Performs a case insensitive check of the Style collection by looping through it.
        /// CssStyleCollection is case sensitive unlike CssCollection.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The value or null.</returns>
        private string GetStyle(string name)
        {
            // First try it in case it works
            string val = Style[name];
            if (val != null)
                return val;

            // Go for a case insensitive search
            foreach (string key in Style.Keys)
            {
                if (String.Compare(key, name, true) == 0)
                {
                    return Style[key];
                }
            }

            return null;
        }

        /// <summary>
        /// Tests for the existence of a border style being set.
        /// </summary>
        /// <param name="type">The type of border setting (color, width, style).</param>
        /// <returns>true if nothing was set.</returns>
        private bool NotExistBorder(string type)
        {
            return ((GetStyle("border") == null) && (GetStyle("border-" + type) == null) &&
                (GetStyle("border-top") == null) && (GetStyle("border-top-" + type) == null) &&
                (GetStyle("border-bottom") == null) && (GetStyle("border-bottom-" + type) == null) &&
                (GetStyle("border-left") == null) && (GetStyle("border-left-" + type) == null) &&
                (GetStyle("border-right") == null) && (GetStyle("border-right-" + type) == null));
        }

        /// <summary>
        /// Opens the downlevel table.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the control content.</param>
        /// <param name="isDesignMode">Indicates whether this is design mode.</param>
        private void RenderBeginTable(HtmlTextWriter writer, bool isDesignMode)
        {
            bool resetBorderWidth = false;
            bool resetBorderStyle = false;
            bool resetBorderColor = false;

            if ((BackColor == Color.Empty) && (GetStyle("background-color") == null))
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#D0D0D0");
            }

            if (!isDesignMode &&
                (Width == Unit.Empty) && (GetStyle("width") == null) && 
                (String.Compare(GetStyle("position"), "absolute", true) != 0))
                
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            }

            if ((BorderWidth == Unit.Empty) && NotExistBorder("width"))
            {
                Style["border-width"] = "1";
                resetBorderWidth = true;
            }

            if ((BorderStyle == BorderStyle.NotSet) && NotExistBorder("style"))
            {
                Style["border-style"] = "solid";
                resetBorderStyle = true;
            }

            if ((BorderColor == Color.Empty) && NotExistBorder("color"))
            {
                Style["border-top-color"] = "#FFFFFF";
                Style["border-left-color"] = "#FFFFFF";
                Style["border-bottom-color"] = "#999999";
                Style["border-right-color"] = "#999999";
                resetBorderColor = true;
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");

            string padding = Util.ExtractNumberString(GetStyle("padding"));
            if (padding == String.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, padding);
            }

            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            if (DefaultStyle["border-width"] == null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Border, DefaultStyle["border-width"]);
            }

            padding = Util.ExtractNumberString(DefaultStyle["padding"]);
            if (padding == String.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, padding);
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "border-width:0");

            FontInfo font = Font;

            string[] names = font.Names;
            if (names.Length > 0)
            {
                string fontNames = String.Empty;
                for (int i = 0; i < names.Length; i++)
                {
                    if (i > 0)
                    {
                        fontNames += ",";
                    }
                    fontNames += names[i];
                }
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, fontNames);
            }

            FontUnit fu = font.Size;
            if (!fu.IsEmpty)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, fu.ToString(CultureInfo.InvariantCulture));
            }

            if (font.Bold)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
            }
            if (font.Italic)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontStyle, "italic");
            }

            bool underline = font.Underline;
            bool overline = font.Overline;
            bool strikeout = font.Strikeout;
            string td = String.Empty;

            if (underline)
                td = "underline";
            if (overline)
                td += " overline";
            if (strikeout)
                td += " line-through";
            if (td.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.TextDecoration, td);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            if (Orientation == Orientation.Horizontal)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }

            if (resetBorderWidth)
            {
                Style.Remove("border-width");
            }

            if (resetBorderStyle)
            {
                Style.Remove("border-style");
            }

            if (resetBorderColor)
            {
                Style.Remove("border-top-color");
                Style.Remove("border-left-color");
                Style.Remove("border-bottom-color");
                Style.Remove("border-right-color");
            }
        }

        /// <summary>
        /// Closes the downlevel table.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the control content.</param>
        private void RenderEndTable(HtmlTextWriter writer)
        {
            if (Orientation == Orientation.Horizontal)
            {
                writer.RenderEndTag();      // TR
            }
            writer.RenderEndTag();      // TABLE

            writer.RenderEndTag();      // TD
            writer.RenderEndTag();      // TR
            writer.RenderEndTag();      // TABLE
        }

        /// <summary>
        /// Renders the control for a downlevel browser.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the control content.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter writer)
        {
            writer.WriteLine("<script language=\"javascript\">" + ClientHelperID + ".value='';</script>");

            RenderBeginTable(writer, false);
            base.RenderDownLevelPath(writer);
            RenderEndTable(writer);
        }

        /// <summary>
        /// Renders the control for a visual designer.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the control content.</param>
        protected override void RenderDesignerPath(HtmlTextWriter writer)
        {
            RenderBeginTable(writer, true);
            base.RenderDesignerPath(writer);
            RenderEndTable(writer);
        }

        /// <summary>
        /// Loads the control's previously saved view state.
        /// </summary>
        /// <param name="savedState">An object that contains the saved view state values for the control.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] state = (object[])savedState;

                base.LoadViewState(state[0]);
                ((IStateManager)Items).LoadViewState(state[1]);
                ((IStateManager)DefaultStyle).LoadViewState(state[2]);
                ((IStateManager)HoverStyle).LoadViewState(state[3]);
                ((IStateManager)SelectedStyle).LoadViewState(state[4]);
#if false
                // Databinding
                if (!_ClearChildViewState)
                {
                    ((IStateManager)Items).LoadViewState(state[4]);
                }
#endif
            }
        }

        /// <summary>
        /// Saves the changes to the control's view state to an Object.
        /// </summary>
        /// <returns>The object that contains the view state changes.</returns>
        protected override object SaveViewState()
        {
            object[] state = new object[]
            {
                base.SaveViewState(),
                ((IStateManager)Items).SaveViewState(),
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
        /// Instructs the control to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)DefaultStyle).TrackViewState();
            ((IStateManager)HoverStyle).TrackViewState();
            ((IStateManager)SelectedStyle).TrackViewState();
            ((IStateManager)Items).TrackViewState();
        }

#if false
        // Databinding
        /// <summary>
        /// Creates a ToolbarItem.
        /// </summary>
        /// <param name="szType">The type to create.</param>
        /// <returns>A ToolbarItem.</returns>
        private ToolbarItem MakeToolbarItem(string szType)
        {
            if (szType == null)
            {
                szType = "button";
            }
            else
            {
                szType = szType.ToLower();
            }

            switch (szType)
            {
            case "button":
                return new ToolbarButton();

            case "checkbutton":
                return new ToolbarCheckButton();

            case "separator":
                return new ToolbarSeparator();

            case "dropdownlist":
                return new ToolbarDropDownList();

            case "textbox":
                return new ToolbarTextBox();

            case "label":
                return new ToolbarLabel();

            default:
                return null;
            }
        }

        /// <summary>
        /// Raises the DataBinding event. This notifies a control to perform any data binding logic that is associated with it.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (_DataSource != null)
            {
                Items.Clear();
                _ClearChildViewState = true;

                string szCurGroup = String.Empty;
                ToolbarCheckGroup group = null;

                foreach (Object dataItem in _DataSource)
                {
                    String type = null;
                    String text = null;
                    String imageUrl = null;
                    String selected = null;
                    String groupname = null;

                    if (DataTypeField != String.Empty)
                        type = DataBinder.GetPropertyValue(dataItem, DataTypeField, null);
                    if (DataTextField != String.Empty)
                        text = DataBinder.GetPropertyValue(dataItem, DataTextField, null);
                    if (DataImageUrlField != String.Empty)
                        imageUrl = DataBinder.GetPropertyValue(dataItem, DataImageUrlField, null);
                    if (DataSelectedField != String.Empty)
                        selected = DataBinder.GetPropertyValue(dataItem, DataSelectedField, null);
                    if (DataGroupnameField != String.Empty)
                        groupname = DataBinder.GetPropertyValue(dataItem, DataGroupnameField, null);

                    ToolbarItem item = MakeToolbarItem(type);

                    if (item == null)
                        continue;

                    bool bEndPrevGroup = (group != null);
                    bool bMakeNewGroup = false;

                    if (item is ToolbarCheckButton)
                    {
                        bEndPrevGroup = 
                            (group != null) && 
                            ((groupname == null) || (groupname != szCurGroup));
                        bMakeNewGroup = ((groupname != null) && (groupname != szCurGroup));
                    }

                    if (bEndPrevGroup)
                    {
                        group = null;
                        szCurGroup = String.Empty;
                    }

                    if (bMakeNewGroup)
                    {
                        group = new ToolbarCheckGroup();
                        Items.Add(group);
                        szCurGroup = groupname;
                    }

                    if (group != null)
                    {
                        group.Items.Add((ToolbarCheckButton)item);
                    }
                    else
                    {
                        Items.Add(item);
                    }

                    if (item is ToolbarButton)
                    {
                        ToolbarButton btn = (ToolbarButton)item;
                        if (text != null)
                            btn.Text = text;
                        if (imageUrl != null)
                            btn.ImageUrl = imageUrl;
                    }

                    if (item is ToolbarLabel)
                    {
                        ToolbarLabel label = (ToolbarLabel)item;
                        if (text != null)
                            label.Text = text;
                        if (imageUrl != null)
                            label.ImageUrl = imageUrl;
                    }

                    if (item is ToolbarCheckButton)
                    {
                        ToolbarCheckButton btn = (ToolbarCheckButton)item;
                        if (selected != null)
                            btn.SetSelected(selected.ToLower() == "true");
                    }
                }
            }
        }
#endif
    }
}
