//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Represents a text box within a toolbar.
    /// </summary>
    public class ToolbarTextBox : ToolbarItem, IPostBackDataHandler
    {
        /// <summary>
        /// Occurs when the Text property value has changed.
        /// </summary>
        public event EventHandler TextChanged;

        private InternalTextBox _TextBox;
        private CssWrapper _CssWrapper;
        private bool _IsTrackingVS;
        private string _HelperID;

        /// <summary>
        /// Initializes a new ToolbarTextBox instance.
        /// </summary>
        public ToolbarTextBox() : base()
        {
            _TextBox = new InternalTextBox();
            _TextBox.TextChanged += new EventHandler(this.OnTextChanged);
            _IsTrackingVS = false;
            _HelperID = null;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            ToolbarTextBox copy = (ToolbarTextBox)base.Clone();

            copy.TextChanged = this.TextChanged;
            copy._CssWrapper = this._CssWrapper;

            copy._TextBox = new InternalTextBox();
            if (this._IsTrackingVS)
            {
                ((IStateManager)copy).TrackViewState();
            }

            foreach (string key in this._TextBox.Attributes.Keys)
            {
                copy._TextBox.Attributes.Add(key, this._TextBox.Attributes[key]);
            }

            foreach (string key in this._TextBox.VS.Keys)
            {
                PropertyInfo pinfo = copy._TextBox.GetType().GetProperty(key);
                if ((pinfo != null) && pinfo.CanRead && pinfo.CanWrite)
                {
                    // Try to be more gentle
                    object obj = pinfo.GetValue(this._TextBox, null);
                    if (obj != null)
                    {
                        pinfo.SetValue(copy._TextBox, obj, null);
                    }
                }
                else
                {
                    // Brute force copy the ViewState
                    object obj = this._TextBox.VS[key];
                    if (obj is ICloneable)
                    {
                        obj = ((ICloneable)obj).Clone();
                    }
                    copy._TextBox.VS[key] = obj;
                }
            }

            copy._TextBox.Font.CopyFrom(this._TextBox.Font);

            return copy;
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            base.SetViewStateDirty();

            Hashtable table = new Hashtable();
            foreach (string key in _TextBox.Attributes.Keys)
            {
                table[key] = _TextBox.Attributes[key];
            }
            _TextBox.Attributes.Clear();
            foreach (string key in table.Keys)
            {
                _TextBox.Attributes[key] = (string)table[key];
            }

            foreach (string key in _TextBox.VS.Keys)
            {
                PropertyInfo pinfo = _TextBox.GetType().GetProperty(key);
                if ((pinfo != null) && pinfo.CanRead && pinfo.CanWrite)
                {
                    object obj = pinfo.GetValue(_TextBox, null);
                    pinfo.SetValue(_TextBox, obj, null);
                }
            }

            _TextBox.Font.CopyFrom(_TextBox.Font);

            foreach (StateItem item in _TextBox.VS.Values)
            {
                item.IsDirty = true;
            }
        }

        /// <summary>
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="sender">Event argument.</param>
        /// <param name="e">The source control.</param>
        protected virtual void OnTextChanged(Object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);   // call the delegate
            }
        }

        //DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),

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
                    _CssWrapper = new CssWrapper(_TextBox);
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
            get { return _TextBox.ControlStyle; }
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
            get { return _TextBox.BackColor; }
            set { _TextBox.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the border color of the Web control.
        /// </summary>summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Color), ""),
        TypeConverterAttribute(typeof(WebColorConverter)),
        ResDescription("InnerControlBorderColor"),
        ]
        public virtual Color BorderColor 
        {
            get { return _TextBox.BorderColor; }
            set { _TextBox.BorderColor = value; }
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
            get { return _TextBox.BorderWidth; }
            set { _TextBox.BorderWidth = value; }
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
            get { return _TextBox.BorderStyle; }
            set { _TextBox.BorderStyle = value; }
        }

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
            get { return _TextBox.CssClass; }
            set { _TextBox.CssClass = value; }
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
            get { return _TextBox.Font; }
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
            get { return _TextBox.ForeColor; }
            set { _TextBox.ForeColor = value; }
        }

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
            get { return _TextBox.Height; }
            set { _TextBox.Height = value; }
        }

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
            get { return _TextBox.Width; }
            set { _TextBox.Width = value; }
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
                    return _TextBox.AutoPostBack;
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
                _TextBox.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Gets or sets the display width of the text box in characters.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(0),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TextBoxColumns"),
        ]
        public virtual int Columns
        {
            get { return _TextBox.Columns; }
            set { _TextBox.Columns = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the text box.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(0),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TextBoxMaxLength"),
        ]
        public virtual int MaxLength
        {
            get { return _TextBox.MaxLength; }
            set { _TextBox.MaxLength = value; }
        }

        /// <summary>
        /// Gets or sets the behavior mode of the text box.
        /// Valid values: TextBoxMode.SingleLine and TextBoxMode.Password
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(ToolbarTextBoxMode.SingleLine),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TextBoxTextMode"),
        ]
        public virtual ToolbarTextBoxMode TextMode
        {
            get { return (_TextBox.TextMode == TextBoxMode.Password) ? ToolbarTextBoxMode.Password : ToolbarTextBoxMode.SingleLine; }
            set { _TextBox.TextMode = (value == ToolbarTextBoxMode.Password) ? TextBoxMode.Password : TextBoxMode.SingleLine; }
        }

        /// <summary>
        /// Gets or sets the text content of the text box.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TextBoxText"),
        ]
        public virtual string Text
        {
            get { return _TextBox.Text; }
            set { _TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the read-only status of the text box.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TextBoxReadOnly"),
        ]
        public virtual bool ReadOnly
        {
            get { return _TextBox.ReadOnly; }
            set { _TextBox.ReadOnly = value; }
        }

        /// <summary>
        /// Gets or sets the keyboard shortcut key (AccessKey) for setting focus to the item.
        /// </summary>
        public override string AccessKey
        {
            get { return _TextBox.AccessKey; }
            set { base.AccessKey = _TextBox.AccessKey = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the textbox is enabled.
        /// </summary>
        public override bool Enabled
        {
            get { return _TextBox.Enabled; }
            set { base.Enabled = _TextBox.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets the tab index of the item.
        /// </summary>
        public override short TabIndex
        {
            get { return _TextBox.TabIndex; }
            set { base.TabIndex = _TextBox.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets the tool tip for the item to be displayed when the mouse cursor is over the control.
        /// </summary>
        public override string ToolTip
        {
            get { return _TextBox.ToolTip; }
            set { base.ToolTip = _TextBox.ToolTip = value; }
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
        /// Processes the new text data.
        /// </summary>
        /// <param name="newText">The new text for the Text property.</param>
        /// <returns>Returns true if there was a change.</returns>
        private bool ProcessData(string newText)
        {
            if (Text != newText)
            {
                Text = newText;
                return true;
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
            OnTextChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes postback data fields. Should be called from OnPreRender.
        /// </summary>
        /// <param name="isUpLevel"></param>
        internal void SetupHiddenHelper(bool isUpLevel)
        {
            _TextBox.ID = HelperID;

            Toolbar parent = ParentToolbar;
            if (isUpLevel && (parent != null) && (parent.Page != null))
            {
                parent.Page.RegisterHiddenField(HelperID, (TextMode != ToolbarTextBoxMode.Password) ? _TextBox.Text : String.Empty);
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
            get { return Toolbar.TextBoxTagName; }
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

            unit = Height;
            if (!unit.IsEmpty)
            {
                style += "height:" + unit.ToString(CultureInfo.InvariantCulture) + ";";
            }

            unit = Width;
            if (!unit.IsEmpty)
            {
                style += "width:" + unit.ToString(CultureInfo.InvariantCulture) + ";";
            }

            style += Style.CssText;

            writer.WriteAttribute("style", style);

            writer.WriteAttribute("type", (TextMode == ToolbarTextBoxMode.Password) ? "password" : "text");

            if (Columns > 0)
            {
                writer.WriteAttribute("size", Columns.ToString());
            }
            if (MaxLength > 0)
            {
                writer.WriteAttribute("maxlength", MaxLength.ToString());
            }
            if (ReadOnly)
            {
                writer.WriteAttribute("readonly", ReadOnly.ToString());
            }
            if ((Text != String.Empty) && (TextMode != ToolbarTextBoxMode.Password))
            {
                writer.WriteAttribute("value", Text, true);
            }

            if (Enabled)
            {
                writer.WriteAttribute("onpropertychange", "window.document.all." + HelperID + ".value=value");
                writer.WriteAttribute("onchange", "window.document.all." + HelperID + ".value=" + ParentToolbar.ClientID + ".getItem(" + Index + ").getAttribute('value')");
                writer.WriteAttribute("onkeyup", "window.document.all." + HelperID + ".value=" + ParentToolbar.ClientID + ".getItem(" + Index + ").getAttribute('value')");
            }
            else
            {
                writer.WriteAttribute("disabled", "true");
            }

            Toolbar parent = ParentToolbar;
            string script = "if (event.keyCode==13){event.returnValue=false;";
            if (Enabled && (parent != null) && (parent.Page != null))
            {
                string postBackRef = "if (" + parent.ClientID + ".getAttribute('_submitting') == null){" + parent.ClientID + ".setAttribute('_submitting', 'true');window.setTimeout('" + parent.Page.GetPostBackEventReference(_TextBox).Replace("'", "\\'") + "', 0, 'JScript');}";
                if (AutoPostBack)
                {
                    // Blur will cause a postback when AutoPostBack is true
                    script += "blur();";

                    // Add the blur postback handler
                    writer.WriteAttribute("_origVal", (TextMode != ToolbarTextBoxMode.Password) ? Text : String.Empty, true);
                    writer.WriteAttribute("onblur", "JScript:if (value != _origVal)" + postBackRef);
                }
                else
                {
                    // Do the postback
                    script += postBackRef + ";";
                }
            }
            script += "}";
            writer.WriteAttribute("onkeydown", script);
        }

        /// <summary>
        /// Renders the item's contents.
        /// </summary>
        /// <param name="inlineWriter">The HtmlTextWriter object that receives the content.</param>
        protected override void DownLevelContent(HtmlTextWriter inlineWriter)
        {
            HtmlTextWriter writer = (HtmlTextWriter)inlineWriter.InnerWriter;

            Toolbar parent = ParentToolbar;
            if (Enabled && AutoPostBack && (parent != null) && (parent.Page != null))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onchange, parent.Page.GetPostBackEventReference(_TextBox));
            }

            // Inherit the current font
            if (Font.Names.Length == 0)
            {
                string font = CurrentStyle["font"];
                string fontFamily = CurrentStyle["font-family"];
                string fontSize = CurrentStyle["font-size"];
                string fontWeight = CurrentStyle["font-weight"];
                string fontStyle = CurrentStyle["font-style"];

                if (font != null)
                {
                    _TextBox.Style["font"] = font;
                }
                if (fontFamily != null)
                {
                    _TextBox.Style["font-family"] = fontFamily;
                }
                if (fontSize != null)
                {
                    _TextBox.Style["font-size"] = fontSize;
                }
                if (fontWeight != null)
                {
                    _TextBox.Style["font-weight"] = fontWeight;
                }
                if (fontStyle != null)
                {
                    _TextBox.Style["font-style"] = fontStyle;
                }
            }

            _TextBox.RenderControl(writer);
        }

        /// <summary>
        /// Renders the item's contents.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DesignerContent(HtmlTextWriter writer)
        {
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

                if ((font != null) && (_TextBox.Style["font"] == null))
                {
                    _TextBox.Style["font"] = font;
                    UndoFont = true;
                }
                if ((fontFamily != null) && (_TextBox.Style["font-family"] == null))
                {
                    _TextBox.Style["font-family"] = fontFamily;
                    UndoFontFamily = true;
                }
                if ((fontSize != null) && (_TextBox.Style["font-size"] == null))
                {
                    _TextBox.Style["font-size"] = fontSize;
                    UndoFontSize = true;
                }
                if ((fontWeight != null) && (_TextBox.Style["font-weight"] == null))
                {
                    _TextBox.Style["font-weight"] = fontWeight;
                    UndoFontWeight = true;
                }
                if ((fontStyle != null) && (_TextBox.Style["font-style"] == null))
                {
                    _TextBox.Style["font-style"] = fontStyle;
                    UndoFontStyle = true;
                }
            }

            _TextBox.RenderControl(writer);

            if (UndoFont)
            {
                _TextBox.Style.Remove("font");
            }
            if (UndoFontFamily)
            {
                _TextBox.Style.Remove("font-family");
            }
            if (UndoFontSize)
            {
                _TextBox.Style.Remove("font-size");
            }
            if (UndoFontWeight)
            {
                _TextBox.Style.Remove("font-weight");
            }
            if (UndoFontStyle)
            {
                _TextBox.Style.Remove("font-style");
            }
        }

        /// <summary>
        /// Saves the item's state.
        /// </summary>
        /// <returns>The saved state.</returns>
        protected override object SaveViewState()
        {
            object state = base.SaveViewState();
            object textBox = _TextBox.SaveVS();

            if ((state != null) || (textBox != null))
            {
                return new object[] {state, textBox};
            }

            return null;
        }

        /// <summary>
        /// Loads the item's previously saved view state.
        /// </summary>
        /// <param name="savedState">An Object that contains the saved view state values for the item.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                return;
            }

            object[] state = (object[])savedState;

            if (state[0] != null)
            {
                base.LoadViewState(state[0]);
            }
            if (state[1] != null)
            {
                _TextBox.LoadVS(state[1]);
            }
        }

        /// <summary>
        /// Tracks changes to the ViewState
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            _TextBox.TrackVS();
            _IsTrackingVS = true;
        }
    }

    /// <summary>
    /// The possible values for the TextMode property of the ToolbarTextBox.
    /// </summary>
    public enum ToolbarTextBoxMode
    {
        /// <summary>
        /// Single-line entry mode.
        /// </summary>
        SingleLine,

        /// <summary>
        /// Password entry mode.
        /// </summary>
        Password,
    };

    /// <summary>
    /// Internal TextBox class to give us access to certain methods
    /// </summary>
    internal class InternalTextBox : TextBox
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
