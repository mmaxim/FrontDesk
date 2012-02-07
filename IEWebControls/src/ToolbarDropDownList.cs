//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Represents a dropdownlist in a toolbar.
    /// </summary>
    [
    ParseChildren(true, "Items"),
    PersistChildren(true),
    ]
    public class ToolbarDropDownList : ToolbarItem, IPostBackDataHandler
    {
        /// <summary>
        /// Fired when the SelectedIndex property changes.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        private InternalDropDownList _List;
        private CssWrapper _CssWrapper;
        private bool _IsTrackingVS;
        private string _HelperID;

        /// <summary>
        /// Initializes a new ToolbarDropDownList.
        /// </summary>
        public ToolbarDropDownList() : base()
        {
            _List = new InternalDropDownList();
            _List.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChanged);
            _IsTrackingVS = false;
            _HelperID = null;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            ToolbarDropDownList copy = (ToolbarDropDownList)base.Clone();

            copy.SelectedIndexChanged = this.SelectedIndexChanged;

            copy._List = new InternalDropDownList();
            if (this._IsTrackingVS)
            {
                ((IStateManager)copy).TrackViewState();
            }

            foreach (ListItem item in this._List.Items)
            {
                ListItem newItem = new ListItem(item.Text, item.Value);
                if (item.Selected)
                {
                    newItem.Selected = true;
                }

                copy._List.Items.Add(newItem);
            }

            foreach (string key in this._List.Attributes.Keys)
            {
                copy._List.Attributes.Add(key, this._List.Attributes[key]);
            }

            foreach (string key in this._List.VS.Keys)
            {
                PropertyInfo pinfo = copy._List.GetType().GetProperty(key);
                if ((pinfo != null) && pinfo.CanRead && pinfo.CanWrite)
                {
                    // Try to be more gentle
                    object obj = pinfo.GetValue(this._List, null);
                    pinfo.SetValue(copy._List, obj, null);
                }
                else
                {
                    // Brute force copy the ViewState
                    object obj = this._List.VS[key];
                    if (obj is ICloneable)
                    {
                        obj = ((ICloneable)obj).Clone();
                    }
                    copy._List.VS[key] = obj;
                }
            }

            copy._List.Font.CopyFrom(this._List.Font);

            return copy;
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            base.SetViewStateDirty();

            Hashtable table = new Hashtable();
            foreach (string key in _List.Attributes.Keys)
            {
                table[key] = _List.Attributes[key];
            }
            _List.Attributes.Clear();
            foreach (string key in table.Keys)
            {
                _List.Attributes[key] = (string)table[key];
            }

            foreach (string key in _List.VS.Keys)
            {
                PropertyInfo pinfo = _List.GetType().GetProperty(key);
                if ((pinfo != null) && pinfo.CanRead && pinfo.CanWrite)
                {
                    object obj = pinfo.GetValue(_List, null);
                    pinfo.SetValue(_List, obj, null);
                }
            }

            _List.Font.CopyFrom(_List.Font);

            foreach (StateItem item in _List.VS.Values)
            {
                item.IsDirty = true;
            }
        }

        /// <summary>
        /// Fires the SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender">Source object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, e);   // call the delegate
            }
        }

        /// <summary>
        /// Gets a collection of text attributes that will be rendered as a style attribute on the outer tag.
        /// </summary>
        [
        Browsable(false),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("InnerControlStyle"),
        ]
        public CssCollection Style 
        {
            get
            {
                if (_CssWrapper == null)
                {
                    _CssWrapper = new CssWrapper(_List);
                }

                return _CssWrapper;
            }
            set
            {
                CssCollection style = this.Style;
                style.Clear();
                style.Merge(value, true);
            }
        }

        /// <summary>
        /// Gets the style of the control
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public Style ControlStyle
        {
            get { return _List.ControlStyle; }
        }

        /// <summary>
        /// Gets or sets the background color of the Web control.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Color), ""),
        TypeConverterAttribute(typeof(WebColorConverter)),
        ResDescription("InnerControlBackColor"),
        ]
        public virtual Color BackColor 
        {
            get { return _List.BackColor; }
            set { _List.BackColor = value; }
        }

        // We've chosen not to support certain attributes which have been "pound-if'd out"
#if false
        /// <summary>
        /// Gets or sets the border color of the Web control.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Color), ""),
        TypeConverterAttribute(typeof(WebColorConverter)),
        ResDescription("InnerControlBorderColor"),
        ]
        public virtual Color BorderColor 
        {
            get { return _List.BorderColor; }
            set { _List.BorderColor = value; }
        }

        /// <summary>
        /// Gets or sets the border width of the Web control.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Unit), ""),
        ResDescription("InnerControlBorderWidth"),
        ]
        public virtual Unit BorderWidth 
        {
            get { return _List.BorderWidth; }
            set { _List.BorderWidth = value; }
        }

        /// <summary>
        /// Gets or sets the border style of the Web control.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(BorderStyle.NotSet),
        ResDescription("InnerControlBorderStyle"),
        ]
        public virtual BorderStyle BorderStyle 
        {
            get { return _List.BorderStyle; }
            set { _List.BorderStyle = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the CssClass
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        ResDescription("InnerControlCssClass"),
        ]
        public virtual string CssClass 
        {
            get { return _List.CssClass; }
            set { _List.CssClass = value; }
        }

        /// <summary>
        /// Gets font information
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        ResDescription("InnerControlFont"),
        ]
        public virtual FontInfo Font 
        {
            get { return _List.Font; }
        }

        /// <summary>
        /// Gets or sets the foreground color (typically the color of the text) of the control
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Color), ""),
        TypeConverterAttribute(typeof(WebColorConverter)),
        ResDescription("InnerControlForeColor"),
        ]
        public virtual Color ForeColor 
        {
            get { return _List.ForeColor; }
            set { _List.ForeColor = value; }
        }

#if false
        /// <summary>
        /// The height of the control
        /// </summary>
        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(typeof(Unit), ""),
        ResDescription("InnerControlHeight"),
        ]
        public virtual Unit Height 
        {
            get { return _List.Height; }
            set { _List.Height = value; }
        }
#endif

        /// <summary>
        /// The Width of the control
        /// </summary>
        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(typeof(Unit), ""),
        ResDescription("InnerControlWidth"),
        ]
        public virtual Unit Width
        {
            get { return _List.Width; }
            set { _List.Width = value; }
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
        public virtual bool AutoPostBack
        {
            get
            {
                // If the AutoPostBack property has been set,
                // then retrieve it.
                if (ViewState["AutoPostBack"] != null)
                {
                    return _List.AutoPostBack;
                }

                // The AutoPostBack property has not been set,
                // so look to the parent
                Toolbar tbParent = ParentToolbar;
                if (tbParent != null)
                {
                    return tbParent.AutoPostBack;
                }

                return false;
            }

            set
            {
                ViewState["AutoPostBack"] = value;
                _List.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Gets or sets the lowest ordinal index of the selected items in the list.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(0),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Browsable(false),
        ]
        public virtual int SelectedIndex
        {
            get { return _List.SelectedIndex; }
            set { _List.SelectedIndex = value; }
        }

        /// <summary>
        /// Gets the collection of items in the list control.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ResDescription("DropDownItems"),
        ]
        public virtual ListItemCollection Items
        {
            get { return _List.Items; }
        }

        /// <summary>
        /// Gets the selected item with the lowest index in the list control.
        /// </summary>
        [
        Category("Behavior"),
        Browsable(false),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public virtual ListItem SelectedItem
        {
            get { return _List.SelectedItem; }
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public virtual void ClearSelection()
        {
            _List.ClearSelection();
        }

        /// <summary>
        /// Gets or sets the keyboard shortcut key (AccessKey) for setting focus to the item.
        /// </summary>
        public override string AccessKey
        {
            get { return _List.AccessKey; }
            set { base.AccessKey = _List.AccessKey = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the textbox is enabled.
        /// </summary>
        public override bool Enabled
        {
            get { return _List.Enabled; }
            set { base.Enabled = _List.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets the tab index of the item.
        /// </summary>
        public override short TabIndex
        {
            get { return _List.TabIndex; }
            set { base.TabIndex = _List.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets the tool tip for the item to be displayed when the mouse cursor is over the control.
        /// </summary>
        [
        Bindable(false),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsableAttribute(EditorBrowsableState.Never)
        ]
        public override string ToolTip
        {
            get { return _List.ToolTip; }
            set { base.ToolTip = _List.ToolTip = value; }
        }

        /// <summary>
        /// Gets or sets the data source that populates the items of the list control.
        /// </summary>
        [
        Bindable(true),
        Category("Data"),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        ]
        public virtual object DataSource
        {
            get { return _List.DataSource; }
            set { _List.DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the specific table in the DataSource to bind to the control.
        /// </summary>
        [
        DefaultValue(""),
        Category("Data"),
        ResDescription("DropDownDataMember"),
        ]
        public virtual string DataMember
        {
            get { return _List.DataMember; }
            set { _List.DataMember = value; }
        }

        /// <summary>
        /// Gets or sets the formatting string used to control how data bound to the list control is displayed.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        ResDescription("DropDownDataTextFormatString"),
        ]
        public virtual string DataTextFormatString
        {
            get { return _List.DataTextFormatString; }
            set { _List.DataTextFormatString = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the text content of the list items.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        ResDescription("DropDownDataTextField"),
        ]
        public virtual string DataTextField
        {
            get { return _List.DataTextField; }
            set { _List.DataTextField = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the value of each list item.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        ResDescription("DropDownDataValueField"),
        ]
        public virtual string DataValueField
        {
            get { return _List.DataValueField; }
            set { _List.DataValueField = value; }
        }

        /// <summary>
        /// Causes data binding to occur on the invoked control and all of its child controls.
        /// </summary>
        public void DataBind()
        {
            _List.DataBind();
        }

        /// <summary>
        /// The ID of the hidden helper.
        /// </summary>
        internal string HelperID
        {
            get
            {
                if (_HelperID == null)
                {
                    Toolbar parent = ParentToolbar;
                    if (parent == null)
                    {
                        return String.Empty;
                    }

                    _HelperID = "__" + parent.ClientID + "_" + Index.ToString() + "__";
                }

                return _HelperID;
            }
        }

        /// <summary>
        /// Processes the new data.
        /// </summary>
        /// <param name="data">The new selected value.</param>
        /// <returns>Returns true if there was a change.</returns>
        private bool ProcessData(string data)
        {
            try
            {
                ListItem item = Items.FindByValue(data);
                if (item != null)
                {
                    int index = Items.IndexOf(item);
                    if (index != SelectedIndex)
                    {
                        SelectedIndex = index;
                        return true;
                    }
                }
            }
            catch
            {
                // If there is an error, ignore it and don't set the data
            }
            
            return false;
        }

        /// <summary>
        /// Loads postback data into the control.
        /// </summary>
        /// <param name="postDataKey">The key identifier for the control.</param>
        /// <param name="postCollection">The collection of all incoming name values.</param>
        /// <returns>true if the server control's state changes as a result of the post back; otherwise false.</returns>
        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string szData = postCollection[HelperID];
            if (szData != null)
            {
                return ProcessData(szData);
            }

            return false;
        }

        /// <summary>
        /// Signals the server control object to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            OnSelectedIndexChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes postback data fields. Should be called from OnPreRender.
        /// </summary>
        /// <param name="isUpLevel"></param>
        internal void SetupHiddenHelper(bool isUpLevel)
        {
            _List.ID = HelperID;

            Toolbar parent = ParentToolbar;
            if (isUpLevel && (parent != null) && (parent.Page != null) && (_List.SelectedItem != null))
            {
                parent.Page.RegisterHiddenField(HelperID, _List.SelectedItem.Value);
            }
        }

        /// <summary>
        /// Initializes the selected index.
        /// </summary>
        private void ResolveSelectedIndex()
        {
            try
            {
                int index = SelectedIndex;
                if ((index >= 0) && (index < Items.Count))
                {
                    _List.SelectedIndex = index;
                }
            }
            catch
            {
                // Ignore errors
            }
        }

        /// <summary>
        /// The product of merging local and global styles.
        /// </summary>
        protected override CssCollection CurrentStyle
        {
            get
            {
                CssCollection style = base.CurrentStyle;

                // Due to the order in which we want to inherit fonts,
                // we need to make DefaultStyle override Style override Parent.DefaultStyle
                if (style["font-family"] != null)
                {
                    if ((DefaultStyle["font-family"] == null) && (Font.Names.Length > 0))
                    {
                        style.Remove("font-family");
                    }
                }

                if (style["font-size"] != null)
                {
                    if ((DefaultStyle["font-size"] == null) && (!Font.Size.IsEmpty))
                    {
                        style.Remove("font-size");
                    }
                }

                if (style["font-weight"] != null)
                {
                    if ((DefaultStyle["font-weight"] == null) && Font.Bold)
                    {
                        style.Remove("font-weight");
                    }
                }

                if (style["font-style"] != null)
                {
                    if ((DefaultStyle["font-style"] == null) && Font.Italic)
                    {
                        style.Remove("font-style");
                    }
                }

                return style;
            }
        }

        /// <summary>
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected override string UpLevelTag
        {
            get { return Toolbar.DropDownListTagName; }
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

            string css = CssClass;
            if (css.Length > 0)
            {
                writer.WriteAttribute("class", css);
            }

            string style = String.Empty;

            Color color = ForeColor;
            if (!color.IsEmpty)
            {
                style += "color:" + ColorTranslator.ToHtml(color) + ";";
            }

            color = BackColor;
            if (!color.IsEmpty)
            {
                style += "background-color:" + ColorTranslator.ToHtml(color) + ";";
            }

#if false
            color = BorderColor;
            if (!color.IsEmpty)
            {
                style += "border-color:" + ColorTranslator.ToHtml(color) + ";";
            }

            BorderStyle bs = BorderStyle;
            Unit unit = BorderWidth;

            if (bs != BorderStyle.NotSet)
            {
                style += "border-style:" + Enum.Format(typeof(BorderStyle), bs, "G") + ";";
            }

            if (!unit.IsEmpty) 
            {
                style += "border-width:" + unit.ToString(CultureInfo.InvariantCulture) + ";";

                if ((bs == BorderStyle.NotSet) && (unit.Value != 0.0))
                {
                    style += "border-style:solid;";
                }
            }
#endif

            FontInfo font = Font;

            string[] names = font.Names;
            if (names.Length > 0)
            {
                style += "font-family:";
                for (int i = 0; i < names.Length; i++)
                {
                    if (i > 0)
                    {
                        style += ",";
                    }
                    style += names[i];
                }
                style += ";";
            }

            FontUnit fu = font.Size;
            if (!fu.IsEmpty)
            {
                style += "font-size:" + fu.ToString(CultureInfo.InvariantCulture) + ";";
            }

            if (font.Bold)
            {
                style += "font-weight:bold;";
            }
            if (font.Italic)
            {
                style += "font-style:italic;";
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
                style += "text-decoration:" + td + ";";

#if false
            unit = Height;
            if (!unit.IsEmpty)
            {
                style += "height:" + unit.ToString(CultureInfo.InvariantCulture) + ";";
            }
#endif

            Unit unit = Width;
            if (!unit.IsEmpty)
            {
                style += "width:" + unit.ToString(CultureInfo.InvariantCulture) + ";";
            }

            style += Style.CssText;

            writer.WriteAttribute("style", style);

            if (Enabled)
            {
                string script = "if ((event.propertyName!=null)&&(event.propertyName.toLowerCase()=='selectedindex')){window.document.all." + HelperID + ".value=options[selectedIndex].value;";

                Toolbar parent = ParentToolbar;
                if (AutoPostBack && (parent != null))
                {
                    script += "if (" + parent.ClientID + ".getAttribute('_submitting') == null){" + parent.ClientID + ".setAttribute('_submitting', 'true');window.setTimeout('" + parent.Page.GetPostBackEventReference(_List).Replace("'", "\\'") + "', 0, 'JScript');}";
                }

                script += "}";

                writer.WriteAttribute("onpropertychange", script);
            }
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void UpLevelContent(HtmlTextWriter writer)
        {
            ResolveSelectedIndex();

            writer.WriteLine();
            writer.Indent++;

            foreach (ListItem item in _List.Items)
            {
                writer.WriteBeginTag("option");
                if (item.Selected)
                {
                    writer.WriteAttribute("selected", "selected", false);
                }

                writer.WriteAttribute("value", item.Value, true);
                writer.Write(HtmlTextWriter.TagRightChar);
                HttpUtility.HtmlEncode(item.Text, writer);
                writer.WriteEndTag("option");
                writer.WriteLine();
            }

            writer.Indent--;
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="inlineWriter">The HtmlTextWriter object that receives the content.</param>
        protected override void DownLevelContent(HtmlTextWriter inlineWriter)
        {
            HtmlTextWriter writer = (HtmlTextWriter)inlineWriter.InnerWriter;

            ResolveSelectedIndex();
            Toolbar parent = ParentToolbar;
            if (Enabled && AutoPostBack && (parent != null) && (parent.Page != null))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onchange, parent.Page.GetPostBackEventReference(_List));
            }

            if (Font.Names.Length == 0)
            {
                string font = CurrentStyle["font"];
                string fontFamily = CurrentStyle["font-family"];
                string fontSize = CurrentStyle["font-size"];
                string fontWeight = CurrentStyle["font-weight"];
                string fontStyle = CurrentStyle["font-style"];

                if (font != null)
                {
                    _List.Style["font"] = font;
                }
                if (fontFamily != null)
                {
                    _List.Style["font-family"] = fontFamily;
                }
                if (fontSize != null)
                {
                    _List.Style["font-size"] = fontSize;
                }
                if (fontWeight != null)
                {
                    _List.Style["font-weight"] = fontWeight;
                }
                if (fontStyle != null)
                {
                    _List.Style["font-style"] = fontStyle;
                }
            }

            _List.RenderControl(writer);
        }

        /// <summary>
        /// Renders the item's contents
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DesignerContent(HtmlTextWriter writer)
        {
            ResolveSelectedIndex();

            bool empty = (_List.Items.Count == 0);
            if (empty)
            {
                ListItem msg = new ListItem();
                msg.Text = Util.GetStringResource("Unbound");
                _List.Items.Add(msg);
            }

            bool UndoFont = false;
            bool UndoFontFamily = false;
            bool UndoFontSize = false;
            bool UndoFontWeight = false;
            bool UndoFontStyle = false;

            if (Font.Names.Length == 0)
            {
                string font = CurrentStyle["font"];
                string fontFamily = CurrentStyle["font-family"];
                string fontSize = CurrentStyle["font-size"];
                string fontWeight = CurrentStyle["font-weight"];
                string fontStyle = CurrentStyle["font-style"];

                if ((font != null) && (_List.Style["font"] == null))
                {
                    _List.Style["font"] = font;
                    UndoFont = true;
                }
                if ((fontFamily != null) && (_List.Style["font-family"] == null))
                {
                    _List.Style["font-family"] = fontFamily;
                    UndoFontFamily = true;
                }
                if ((fontSize != null) && (_List.Style["font-size"] == null))
                {
                    _List.Style["font-size"] = fontSize;
                    UndoFontSize = true;
                }
                if ((fontWeight != null) && (_List.Style["font-weight"] == null))
                {
                    _List.Style["font-weight"] = fontWeight;
                    UndoFontWeight = true;
                }
                if ((fontStyle != null) && (_List.Style["font-style"] == null))
                {
                    _List.Style["font-style"] = fontStyle;
                    UndoFontStyle = true;
                }
            }

            _List.RenderControl(writer);

            if (UndoFont)
            {
                _List.Style.Remove("font");
            }
            if (UndoFontFamily)
            {
                _List.Style.Remove("font-family");
            }
            if (UndoFontSize)
            {
                _List.Style.Remove("font-size");
            }
            if (UndoFontWeight)
            {
                _List.Style.Remove("font-weight");
            }
            if (UndoFontStyle)
            {
                _List.Style.Remove("font-style");
            }

            if (empty)
            {
                _List.Items.Clear();
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

                // Restore state of items
                if (state.Length > 1)
                {
                    _List.LoadVS(state[1]);
                }
            }
        }

        /// <summary>
        /// Saves the changes to the item's view state to an object.
        /// </summary>
        /// <returns>The object that contains the view state changes.</returns>
        protected override object SaveViewState()
        {
            object baseState = base.SaveViewState();
            object list = _List.SaveVS();

            if (list != null)
            {
                return new object[2] { baseState, list };
            }
            else if (baseState != null)
            {
                return new object[1] { baseState };
            }

            return null;
        }

        /// <summary>
        /// Instructs the control to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            _IsTrackingVS = true;
            _List.TrackVS();
        }
    }

    /// <summary>
    /// Internal DropDownList class to give us access to certain methods
    /// </summary>
    internal class InternalDropDownList : DropDownList
    {
        /// <summary>
        /// Internal access to ViewState
        /// </summary>
        internal StateBag VS
        {
            get { return ViewState; }
        }

        /// <summary>
        /// TrackViewState
        /// </summary>
        internal void TrackVS()
        {
            TrackViewState();
        }

        /// <summary>
        /// SaveViewState
        /// </summary>
        /// <returns></returns>
        internal object SaveVS()
        {
            return SaveViewState();
        }

        /// <summary>
        /// LoadViewState
        /// </summary>
        /// <param name="state">State object</param>
        internal void LoadVS(object state)
        {
            LoadViewState(state);
        }
    }
}
