//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Drawing.Design;
    using System.Web;
    using System.Web.UI;
    using System.Xml;

    /// <summary>
    /// Indicates how a TreeNode should handle expanding and the plus sign.
    /// </summary>
    public enum ExpandableValue 
    {
        /// <summary>
        /// Always shows a plus sign and attempts to expand.
        /// </summary>
        Always,

        /// <summary>
        /// Shows a plus sign and allows expanding only when there are children.
        /// </summary>
        Auto,

        /// <summary>
        /// Allows expanding to be attempted once, such as in a databinding case, when
        /// the existence of children is unknown.
        /// </summary>
        CheckOnce
    };

    /// <summary>
    /// Class to eliminate runat="server" requirement and restrict content to approved tags
    /// </summary>
    internal class TreeNodeControlBuilder : FilterControlBuilder
    {
        protected override void FillTagTypeTable()
        {
            Add("treenode", typeof(TreeNode));
        }
    }


    /// <summary>
    /// TreeNode class: represents a tree node.
    /// Renders the necessary tags to display a treenode and handle its events.
    /// </summary>
    [
    ParseChildren(false),
    ControlBuilderAttribute(typeof(TreeNodeControlBuilder)),
    ToolboxItem(false),
    ]
    public class TreeNode : TreeBase, IParserAccessor
    {
        private int _level;
        private TreeView _treeview;     // parent
        private string _strInheritedType;
        private int _NodeTypeIndex;
        internal TreeNodeCollection _Nodes;
        private bool _bBound;

        /// <summary>
        /// Adds a parsed child object to the Nodes collection.
        /// Only accepts TreeNodes.
        /// </summary>
        /// <param name="obj">The object to add (must be a TreeNode).</param>
        void IParserAccessor.AddParsedSubObject(Object obj)
        {
            if (obj is TreeNode)
            {
                _Nodes.Add((TreeNode)obj);
            }
        }

        /// <summary>
        /// A node's inheritedType is the first of:
        /// * parent's childType
        /// * parent's type's childType
        /// * parent's inheritedType's childType
        /// * parent's inheritedType
        /// </summary>
        internal String InheritedType
        {
            get 
            {
                if ((_strInheritedType == null) || ((ParentTreeView != null) && ParentTreeView.AlwaysCalcValues))
                {
                    if (Parent is TreeNode)
                    {
                        TreeNode tnParent = (TreeNode)Parent;
                        // parent's ChildType
                        if (tnParent.ChildType != String.Empty)
                            _strInheritedType = tnParent.ChildType;

                        else 
                        {
                            // parent's type's or inheritedType's childType
                            TreeNodeType _parenttype = tnParent.NodeTypeObject;
                            if (_parenttype != null && _parenttype.ChildType != String.Empty)
                                _strInheritedType = _parenttype.ChildType;
                            else
                                _strInheritedType = tnParent.InheritedType;
                        }
                    }
                    else
                        _strInheritedType = ((TreeView)Parent).ChildType;
                }
                return _strInheritedType;
            }
        }

        /// <summary>
        /// The TreeNodeType that this TreeNode inherits properties from.
        /// </summary>
        private TreeNodeType NodeTypeObject
        {
            get
            {
                if (_NodeTypeIndex == -1)
                {
                    string strType;
                    if (Type != String.Empty)
                        strType = Type;
                    else if (InheritedType != String.Empty)
                        strType = InheritedType;
                    else return null;

                    // find the nodetype in the tree's nodetype collection
                    strType = strType.ToLower();
                    int i = 0;
                    int iCount = ParentTreeView.TreeNodeTypes.Count;
                    while (i < iCount && _NodeTypeIndex == -1)
                    {
                        if (((TreeNodeType)ParentTreeView.TreeNodeTypes[i]).Type.ToLower() == strType)
                            _NodeTypeIndex = i;
                        i++;
                    }
                    if (_NodeTypeIndex == -1)
                        return null;
                }
                return (TreeNodeType)ParentTreeView.TreeNodeTypes[_NodeTypeIndex];
            }
        }

        /// <summary>
        /// Controls how often the node determines whather or not it can be expanded
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(ExpandableValue.Auto),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeExpandable"),
        ]
        public ExpandableValue Expandable
        {
            get 
            {
                object b = ViewState["Expandable"];
                return ((b == null) ? (TreeNodeSrc == String.Empty ? ExpandableValue.Auto : ExpandableValue.CheckOnce) : (ExpandableValue)b);
            }
            set 
            {
                ViewState["Expandable"] = value;
            }
        }

        /// <summary>
        /// [TODO: to be supplied]
        /// </summary>
        private bool CheckedExpandable
        {
            get
            {
                object b = ViewState["CheckedExpandable"];
                return ((b == null) ? false : (bool)b);
            }
            set
            {
                ViewState["CheckedExpandable"] = value;
            }
        }

        /// <summary>
        /// Name of the TreeNodeType to apply to this node
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        Editor(typeof(Microsoft.Web.UI.WebControls.Design.NodeTypeEditor), typeof(UITypeEditor)),
        ResDescription("TreeType"),
        ]
        public String Type
        {
            get 
            {
                object str = ViewState["Type"];
                return ((str == null) ? String.Empty : (String)str);
            }
            set
            {
                ViewState["Type"] = value;
            }
        }

        /// <summary>
        /// Text to display
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ItemText"),
        ]
        public String Text
        {
            get
            {
                object str = ViewState["Text"];
                return ((str == null) ? String.Empty : (String)str);
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// Url to navigate to when node is selected
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeNavigateUrl"),
        ]
        public String NavigateUrl
        {
            get
            {
                object str = ViewState["NavigateUrl"];
                return ((str == null) ? String.Empty : (String)str);
            }
            set
            {
                ViewState["NavigateUrl"] = value;
            }
        }

        /// <summary>
        /// Whether or not the node is expanded
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeExpanded"),
        ]
        public bool Expanded
        {
            get
            {
                object b = ViewState["Expanded"];
                if (b == null)
                {
                    if (ParentTreeView.ExpandLevel > Level)
                        return true;
                    else
                        return false;
                }
                else
                    return (bool)b;
            }
            set
            {
                ViewState["Expanded"] = value;
            }
        }

        /// <summary>
        /// Whether or not the node is currently visible (all parents expanded)
        /// </summary>
        internal bool IsVisible
        {
            get
            {
                TreeNode prev = this;
                while (prev.Parent is TreeNode)
                {
                    prev = (TreeNode)prev.Parent;
                    if (!prev.Expanded)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Whether or not the node is checked
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeChecked"),
        ]
        public bool Checked
        {
            get
            {
                object b = ViewState["Checked"];
                if (b == null)
                    return false;
                else
                    return (bool)b;
            }
            set
            {
                ViewState["Checked"] = value;
            }
        }

        /// <summary>
        /// Custom data
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("NodeData"),
        ]
        public string NodeData
        {
            get
            {
                object str = ViewState["NodeData"];
                if (str == null)
                    return String.Empty;
                else
                    return (string)str;
            }
            set
            {
                ViewState["NodeData"] = value;
            }
        }

        /// <summary>
        /// Whether or not the node is selected
        /// </summary>
        internal bool Selected
        {
            get
            {
                object b = ViewState["Selected"];
                return ((b == null) ? false : (bool)b);
            }
            set
            {
                ViewState["Selected"] = value;
            }
        }

        /// <summary>
        /// Url of the xml file to import as the content of this node
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeNodeSrc"),
        ]
        public string TreeNodeSrc
        {
            get
            {
                object str = ViewState["TreeNodeSrc"];
                return ((str == null) ? String.Empty : (string)str);
            }
            set
            {
                ViewState["TreeNodeSrc"] = value;
                _bBound = false;
                CheckedExpandable = false;
            }
        }

        /// <summary>
        /// Url of the xsl transform file to apply to the TreeNodeSrc
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeNodeXsltSrc"),
        ]
        public string TreeNodeXsltSrc
        {
            get
            {
                object str = ViewState["TreeNodeXsltSrc"];
                return ((str == null) ? String.Empty : (string)str);
            }
            set
            {
                ViewState["TreeNodeXsltSrc"] = value;
                _bBound = false;
                CheckedExpandable = false;
            }
        }

        /// <summary>
        /// Returns a reference to the parent TreeView.
        /// </summary>
        protected internal TreeView ParentTreeView
        {
            get
            {
                if (_treeview == null)
                {
                    // Get the parent treeview
                    TreeNode prev = this;
                    while (prev.Parent is TreeNode)
                        prev = (TreeNode)prev.Parent;
                    _treeview = (TreeView)prev.Parent;
                }
                return _treeview;
            }
            set
            {
                _treeview = value;
                foreach (TreeNode node in Nodes)
                {
                    node.ParentTreeView = value;
                }
            }
        }

        /// <summary>
        /// Returns the level in the tree.
        /// </summary>
        protected int Level
        {
            get
            {
                if (_level == -1)
                {
                    Object prev = Parent;
                    while (prev is TreeNode)
                    {
                        prev = ((TreeNode)prev).Parent;
                        _level++;
                    }
                    return ++_level;
                }
                else
                    return _level;
            }
        }

        /// <summary>
        /// Returns the number of sibling TreeNodes before this one.
        /// </summary>
        internal int SibIndex
        {
            get
            {
                if (Parent != null)
                {
                    if (Parent is TreeNode)
                        return ((TreeNode)Parent).Nodes.IndexOf(this);
                    else
                        return ((TreeView)Parent).Nodes.IndexOf(this);
                }
                else
                    return -1;
            }

/*
                if (_sibIndex == -1)
                {
                    TreeNode node = this;
                    while (node != null)
                    {
                        _sibIndex++;
                        node = node.GetPreviousSibling();
                    }
                }
                return _sibIndex;
            }
            set
            {
                _sibIndex = value;
            }
*/
        }

        /// <summary>
        /// Gets the collection of nodes in the control.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        Browsable(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ResDescription("TreeNodes"),
        ]
        public virtual TreeNodeCollection Nodes
        {
            get { return _Nodes; }
        }

        /// <summary>
        /// Initializes a new instance of a TreeNode.
        /// </summary>
        public TreeNode() : base()
        {
            _level = -1;
            _treeview = null;
            _strInheritedType = null;
            _NodeTypeIndex = -1;
            _bBound = false;
            _Nodes = new TreeNodeCollection(this);
        }

        /// <summary>
        /// Event handler for the OnSelectedIndexChange event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        /// <returns>true to bubble, false to cancel</returns>
        protected virtual bool OnSelectedIndexChange(EventArgs e)
        {
            return true;
        }

        /// <summary>
        /// Event handler for the OnExpand event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        /// <returns>true to bubble, false to cancel</returns>
        protected virtual bool OnExpand(EventArgs e)
        {
            if (!Expanded && CanExpand())
            {
                Expanded = true;
                Object obj = FindNodeAttribute("Expandable");
                if (obj == null)
                {
                    obj = Expandable; // get default value if none is explicitly set
                }

                if ((ExpandableValue)obj == ExpandableValue.CheckOnce)
                    CheckedExpandable = true;

                if (!_bBound)
                    ReadXmlSrc();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Event handler for the OnCollapse event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        /// <returns>true to bubble, false to cancel</returns>
        protected virtual bool OnCollapse(EventArgs e)
        {
            if (Expanded)
            {
                Expanded = false;
                // If this node was selected, unselect it and make the parent the selected node
                // Note: The client handles this nicely uplevel; we only need to do this downlevel
                if (! ParentTreeView.IsUpLevel)
                {
                    String strNewIndex = GetNodeIndex();
                    String strOldIndex = ParentTreeView.SelectedNodeIndex;
                    if (strOldIndex.StartsWith(strNewIndex) && strOldIndex != strNewIndex)
                    {
                        TreeViewSelectEventArgs e2 = new TreeViewSelectEventArgs(strOldIndex, strNewIndex);
                        ParentTreeView.DoSelectedIndexChange(e2);
                        // Since this only gets called downlevel, we don't need to worry about other selection
                        // changes being queued-- this will be the only one, and so we can queue an event for it 
                        ParentTreeView._eventList.Add("s");
                        ParentTreeView._eventList.Add(e2);
                    }
                }
                return true;
            }
            else
                return false;
        }

        //
        // OnCheck
        //
	    /// <summary>
        /// Event handler for OnCheck event
        /// </summary>
        protected virtual bool OnCheck(EventArgs e)
        {
            Checked = !Checked;
            return true;
        }

        //
        // OnInit
        //
        /// <summary>
        /// Initializes the TreeNode.
        /// </summary>
        internal void OnInit()
        {
            if (Expanded)
            {
                Databind();
            }
        }

        /// <summary>
        /// Process the event string, calling the appropriate event handler.
        /// </summary>
        /// <param name="eventArg">Event argument.</param>
        /// <returns>true to bubble, false otherwise.</returns>
        private bool ProcessEvent(string eventArg)
        {
            int nSep = eventArg.IndexOf(',');
            if (nSep < 0)
                nSep = eventArg.Length;

            string strEvent = eventArg.Substring(0, nSep);
            EventArgs e = new EventArgs();
            if (strEvent.Equals("onexpand"))
                return OnExpand(e);
            else if (strEvent.Equals("oncollapse"))
                return OnCollapse(e);
            else if (strEvent.Equals("onselectedindexchange"))
                return OnSelectedIndexChange(e);
            else if (strEvent.Equals("oncheck"))
                return OnCheck(e);
            else
                return false;
        }

        /// <summary>
        /// Public entry point used by treeview to "bubble down" an event, thus freeing treenodes from having
        /// to include postback HTML for default treenode activity
        /// </summary>
        /// <param name="eventArg">Event argument.</param>
        internal virtual void LowerPostBackEvent(string eventArg)
        {
            ProcessEvent(eventArg);
        }

        /// <summary>
        /// Sets all items within the StateBag to be dirty
        /// </summary>
        protected internal override void SetViewStateDirty()
        {
            base.SetViewStateDirty();

            Nodes.SetViewStateDirty();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            TreeNode copy = (TreeNode)base.Clone();

            copy._strInheritedType = this._strInheritedType;
            copy._NodeTypeIndex = -1;
            copy._Nodes = (TreeNodeCollection)this._Nodes.Clone();
            
            // Fix parentage
            copy._Nodes.Parent = copy;
            if (copy._Nodes.Count > 0)
            {
                foreach (TreeNode tnode in copy._Nodes)
                {
                    tnode._Parent = copy;
                }
            }

            return copy;
        }

        /// <summary>
        /// Renders the node for uplevel browsers.
        /// </summary>
        /// <param name="output">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderUpLevelPath(HtmlTextWriter output)
        {
            if (Type != String.Empty)
                output.AddAttribute("Type", Type);

            if (Expandable == ExpandableValue.CheckOnce)
                output.AddAttribute("Expandable", "checkOnce");
            else if (Expandable == ExpandableValue.Always)
                output.AddAttribute("Expandable", "always");
            if (CheckedExpandable)
                output.AddAttribute("checkedExpandable", "true");

            // Because of ExpandLevel, we need to distinguish between explicitly-declared false and default false.
            // Only the explicit false will be stored in ViewState.
            object b = ViewState["Expanded"];
            if (Expanded || (b != null && (bool)b == false))
                output.AddAttribute("Expanded", Expanded.ToString());

            if (Selected)
                output.AddAttribute("Selected", "true");

            if (NavigateUrl != String.Empty)
                output.AddAttribute("NavigateUrl", NavigateUrl);

            if (Checked)
                output.AddAttribute("Checked", "true");

            if (NodeData != String.Empty)
                output.AddAttribute("NodeData", NodeData);
            
            AddAttributesToRender(output);

            output.RenderBeginTag("tvns:treenode");

            if (Text != String.Empty)
                output.Write(Text);
    
            base.RenderUpLevelPath(output);

            // Render contained nodes
            foreach (TreeNode node in Nodes)
                node.Render(output, RenderPathID.UpLevelPath);

            output.RenderEndTag();             
        }
        
        /// <summary>
        /// Returns the node's previous TreeNode sibling, or null
        /// </summary>
        /// <returns>The node's previous TreeNode sibling, or null.</returns>
        protected TreeNode GetPreviousSibling()
        {
            TreeNodeCollection sibs;
            if (Parent is TreeNode)
                sibs = ((TreeNode)Parent).Nodes;
            else
                sibs = ((TreeView)Parent).Nodes;

            int iIndex = sibs.IndexOf(this) - 1;
            if (iIndex >= 0)
                return sibs[iIndex];
            else
                return null;
        }

        /// <summary>
        /// Returns a x.y.z format node index string representing the node's position in the hierarchy.
        /// </summary>
        /// <returns>The x.y.z formatted index.</returns>
        public string GetNodeIndex()
        {
            string strIndex = "";
            Object node = this;
            while (node is TreeNode)
            {
                if (((TreeNode)node).SibIndex == -1)
                    return String.Empty;
                if (strIndex.Length == 0)
                    strIndex = ((TreeNode)node).SibIndex.ToString();
                else
                    strIndex = ((TreeNode)node).SibIndex.ToString() + "." + strIndex;
                node = ((TreeNode)node).Parent;
            } 
            return strIndex;
        }

        /// <summary>
        /// Determine if the current node is an L, T, or Root junction.
        /// Note: This code is essentially duplicated from the client behavior.
        /// </summary>
        /// <returns>A character defining the type of junction.</returns>
        protected char CalcJunction()
        {
            if (Parent is TreeView && GetPreviousSibling() == null)
            {
                // Get index of node and add 1 to the last value
                string strIndex = GetNodeIndex();
                int iIndexPos = strIndex.LastIndexOf('.');
                int iIndexVal = Convert.ToInt32(strIndex.Substring(iIndexPos + 1)) + 1;
                strIndex = strIndex.Substring(0,iIndexPos + 1) + iIndexVal.ToString();
                // if the node exists, we're an "F" node.
                if (ParentTreeView.GetNodeFromIndex(strIndex) != null)
                    return 'F';
                else
                    return 'R';
            }
            else
            {
                TreeNodeCollection col;
                if (Parent is TreeView)
                    col = ((TreeView)Parent).Nodes;
                else
                    col = ((TreeNode)Parent).Nodes;
                int i = col.IndexOf(this) + 1;
                if (i < col.Count)
                    return 'T';
                return 'L';
            }
        }

        /// <summary>
        /// Renders the designer version of the TreeNode, which is the downlevel version.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive that markup.</param>
        protected override void RenderDesignerPath(HtmlTextWriter output)
        {
            RenderDownLevelPath(output);
        }
            
        /// <summary>
        /// Render the downlevel version of the TreeNode.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive the markup.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter output)
        {
            string href;
            String imageSrc = "";
            int i;
            CssCollection cssStyle;
            char cJunction = CalcJunction();
            object obj; // scratch object

            output.Write("<TR><TD valign='middle'>");
            output.Write("&nbsp;");

            //
            // Walk up tree to draw lines/whitespace
            //
            Object obWalk = Parent;
            if (ParentTreeView.ShowLines == true)
            {
                int iCount = 0;
                string strLines = String.Empty;
                while (obWalk is TreeNode)
                {
                    TreeNode elWalk = (TreeNode)obWalk;
                    TreeNodeCollection kids = GetNodeCollection(elWalk.Parent);
                    i = kids.IndexOf(elWalk) + 1;
                    if (i < kids.Count)
                    {
                        if (iCount > 0)
                        {
                            strLines = "<IMG align='top' border='0' width='" + iCount * ParentTreeView.Indent + "px' height='1px' SRC='" + ParentTreeView.SystemImagesPath + "white.gif'>" + strLines;
                            iCount = 0;
                        }
                        strLines = "<IMG align='top' border='0' SRC='" + ParentTreeView.SystemImagesPath + "I.gif'>" + strLines;
                    }
                    else
                        iCount++;
                    obWalk = elWalk.Parent;
                }
                if (iCount > 0)
                    strLines = "<IMG align='top' border='0' width='" + iCount * ParentTreeView.Indent + "px' height='1px' SRC='" + ParentTreeView.SystemImagesPath + "white.gif'>" + strLines;
                output.Write(strLines);
            }
            else
            {
                if (cJunction != 'R' && cJunction != 'F')
                {
                    int iIndent = 0;
                    while (obWalk is TreeNode && ((TreeNode)obWalk).Parent != null)
                    {
                        iIndent += ParentTreeView.Indent;
                        obWalk = ((TreeNode)obWalk).Parent;
                    }
                    output.Write("<IMG align='top' border='0' SRC='" + ParentTreeView.SystemImagesPath + "white.gif' WIDTH=" + iIndent + " HEIGHT=1>");
                }
            }

            // is this a branch node?
            bool bParent = false;
            obj = FindNodeAttribute("Expandable");
            if (obj == null)
            {
                obj = Expandable; // get default value if none is explicitly set
            }
            if (Nodes.Count > 0 || (ExpandableValue)obj == ExpandableValue.Always || ((ExpandableValue)obj == ExpandableValue.CheckOnce && !CheckedExpandable))
            {
                bParent = true;
            }

            if (ParentTreeView.ShowLines == true || ParentTreeView.ShowPlus == true)
            {
                if (ParentTreeView.ShowLines == true)
                {
                    switch (cJunction)
                    {
                        case 'L':
                            imageSrc = "L";
                            break;
                        case 'T':
                            imageSrc = "T";
                            break;
                        case 'R':
                            imageSrc = "R";
                            break;
                        case 'F':
                            imageSrc = "F";
                            break;
                    }
                }
                else
                    imageSrc = "";
                imageSrc = "'" + ParentTreeView.SystemImagesPath + imageSrc;

                if (bParent && ParentTreeView.ShowPlus == true)
                {
                    if (Expanded == true)
                    {
                        imageSrc += "minus.gif'>";
                    }
                    else
                    {
                        imageSrc += "plus.gif'>";
                    }
                    if (ParentTreeView.Page != null)
                        href = "javascript:" + ParentTreeView.Page.GetPostBackEventReference(ParentTreeView, (Expanded ? "oncollapse," : "onexpand,") + GetNodeIndex());
                    else
                        href = String.Empty;
                    output.AddAttribute(HtmlTextWriterAttribute.Href, href);
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                }
                else if (ParentTreeView.ShowLines == true)
                    imageSrc += ".gif'>";
                else
                    imageSrc = "'" + ParentTreeView.SystemImagesPath + "white.gif'>";

                output.Write("<IMG align='top' border='0' class='icon' SRC=" + imageSrc);

                if (bParent && ParentTreeView.ShowPlus == true)
                    output.RenderEndTag();
            }

            // Render a checkbox
            obj = FindNodeAttribute("CheckBox");
            if (obj != null && (bool)obj == true)
            {
                string strOnclick = ParentTreeView.Page.GetPostBackEventReference(ParentTreeView, "oncheck," + GetNodeIndex());

                output.Write("<INPUT style='display:inline' align=middle type=checkbox " + (Checked ? " checked " : " ") + "onclick=\"" + strOnclick + "\" />");
            }

            // if this node is a navigating leaf, use its nav information.
            
            obj = FindNodeAttribute("NavigateUrl");
            if (obj != null && (!(ParentTreeView.SelectExpands && bParent && !Expanded)))
            {
                href = (string)obj;
                obj = FindNodeAttribute("Target");
                if (obj != null)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Target, (string)obj);
                    obj = null;
                }
            }
            else if (ParentTreeView.Page != null)
            {
                if (!Selected)
                {
                    href = "javascript:" + ParentTreeView.Page.GetPostBackEventReference(ParentTreeView, "onselectedindexchange," + ParentTreeView.SelectedNodeIndex + "," + GetNodeIndex());
                    if (bParent && ParentTreeView.SelectExpands == true)
                        href = href.Substring(0, href.Length - 2) + (";" + (Expanded ? "oncollapse," : "onexpand,") + GetNodeIndex()) + "')";
                }
                else
                    if (bParent && ParentTreeView.SelectExpands == true)
                    href = "javascript:" + ParentTreeView.Page.GetPostBackEventReference(ParentTreeView, (Expanded ? "oncollapse," : "onexpand,") + GetNodeIndex());
                else
                    href = String.Empty;
            }
            else
                href = String.Empty;

            if (href != String.Empty)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Href, href);
                output.RenderBeginTag(HtmlTextWriterTag.A);
            }

            obj = null;
            if (Selected == true)
                obj = FindNodeAttribute("SelectedImageUrl");
            if (obj == null)
            {
                if (bParent == true && Expanded == true)
                    obj = FindNodeAttribute("ExpandedImageUrl");
                if (obj == null)
                    obj = FindNodeAttribute("ImageUrl");
            }
            if (obj != null)
                output.Write("<IMG align='top' border='0' class='icon' SRC='" + (string)obj + "'>");

            cssStyle = new CssCollection("display:inline; font-size:10pt; font-face:Times; color: black; text-decoration:none; cursor: hand; overflow:hidden;");
            object o = FindNodeAttribute("DefaultStyle");
            if (o != null)
            {
                cssStyle.Merge((CssCollection)o, true);
            }
            if (Selected)
            {
                CssCollection temp = new CssCollection("color: #00FFFF; background-color: #08246B;");
                o = FindNodeAttribute("SelectedStyle");
                if (o != null)
                {
                    temp.Merge((CssCollection)o,true);
                }
                cssStyle.Merge(temp, true);
            }

            /*
                       // Links will be blue by default.  We want them to be the color specified in our style.
                       // So we have to override the link color by adding a font tag inside the A tag.
                       string curColor = cssStyle.GetColor();
                       if (!Selected && (curColor != null) && (curColor != String.Empty))
                       {
                           output.AddStyleAttribute(HtmlTextWriterStyle.Color, curColor);
                       }
           */            
            cssStyle.AddAttributesToRender(output);
            cssStyle.RenderBeginFontTag(output);
            cssStyle.RenderBeginModalTags(output);

            if (Text != null && Text != String.Empty)
                output.Write("&nbsp;" + Text);

            cssStyle.RenderEndModalTags(output);
            cssStyle.RenderEndFontTag(output);

            if (href != String.Empty)
                output.RenderEndTag();

            output.Write("</TD></TR>\n");

            // Render child treenodes
            if (Expanded)
            {
                foreach (TreeNode item in Nodes)
                {
                    item.Render(output, RenderPathID.DownLevelPath);
                }
            }
        }

        /// <summary>
        /// Returns the value of att from the node's NodeType.
        /// </summary>
        /// <param name="att">The name of the attribute to lookup.</param>
        /// <returns>The value of the attribute or null.</returns>
        protected object GetNodeTypeAttribute(string att)
        {
            if (NodeTypeObject != null)
                return NodeTypeObject.GetStateVar(att);
            return null;
        }

        /// <summary>
        /// Return the attribute from the first place it's defined among:
        /// * the node itself
        /// * the node's type (directly declared or inherited via ChildType)
        /// * the treeview
        /// </summary>
        /// <param name="strAtt">The name of the attribute to lookup.</param>
        /// <returns>The value of the attribute.</returns>
        public object FindNodeAttribute(String strAtt)
        {
            object obj = GetStateVar(strAtt);
            if (obj != null && !(obj is CssCollection && ((CssCollection)obj).CssText == String.Empty))
                return obj;

            obj = GetNodeTypeAttribute(strAtt);
            if (obj != null && !(obj is CssCollection && ((CssCollection)obj).CssText == String.Empty))
                return obj;

            return ParentTreeView.GetStateVar(strAtt);
        }

        /// <summary>
        /// [TODO: to be supplied]
        /// </summary>
        /// <returns>[TODO: to be supplied]</returns>
        private bool CanExpand()
        {
            Object exp = FindNodeAttribute("Expandable");
            if (exp == null)
                exp = Expandable;
            if ((ExpandableValue)exp == ExpandableValue.Always || ((ExpandableValue)exp == ExpandableValue.CheckOnce && !CheckedExpandable))
                return true;
            if (Nodes.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// [TODO: to be supplied]
        /// </summary>
        internal void ReadXmlSrc()
        {
            if (TreeNodeSrc != String.Empty)
            {
                TreeView tv = ParentTreeView.ReadXmlSrc(TreeNodeSrc, TreeNodeXsltSrc, "TREENODES");
                if (tv != null)
                {
                    TreeNodeCollection col = tv.Nodes;
                    ParentTreeView.CopyXmlNodesIntoTree(col, this);
                }
                _bBound = true;
            }
        }

        /// <summary>
        /// Gets the TreeNodeCollection from the given object.
        /// The object must be a TreeView or TreeNode.
        /// </summary>
        /// <param name="obj">The object on which to retrieve its Nodes collection.</param>
        /// <returns>The TreeNodeCollection.</returns>
        private TreeNodeCollection GetNodeCollection(Object obj)
        {
            if (obj is TreeView)
                return ((TreeView)obj).Nodes;
            else if (obj is TreeNode)
                return ((TreeNode)obj).Nodes;
            else
                throw new Exception(Util.GetStringResource("TreeInvalidObject"));
        }   

        /// <summary>
        /// Instructs the item to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)Nodes).TrackViewState();
        }

        /// <summary>
        /// Restores the state of this object from a saved object in ViewState.
        /// </summary>
        /// <param name="savedState">The saved object.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] state = (object[])savedState;

                base.LoadViewState(state[0]);
                ((IStateManager)Nodes).LoadViewState(state[1]);
            }
        }

        /// <summary>
        /// Saves the state of the TreeNode into the ViewState.
        /// </summary>
        /// <returns>A ViewState object representing changes to this object.</returns>
        protected override object SaveViewState()
        {
            object[] state = new object[]
            {
                base.SaveViewState(),
                ((IStateManager)Nodes).SaveViewState(),
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
        /// Remove the TreeNode and any children from its parent's Nodes collection.
        /// </summary>
        public void Remove()
        {
            TreeNodeCollection colParent = GetSiblingNodeCollection();
            if (colParent != null)
                colParent.Remove(this);
        }

        /// <summary>
        /// Retrieves the TreeNodeCollection this TreeNode is in.
        /// </summary>
        /// <returns>The TreeNodeCollection</returns>
        public TreeNodeCollection GetSiblingNodeCollection()
        {
            if (Parent is TreeView || Parent is TreeNode)
            {
                return GetNodeCollection(Parent);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Forces the node to redatabind immediately, even if it isn't expanded.
        /// Child nodes are only bound if they're expanded.
        /// </summary>
        public void Databind()
        {
            if (TreeNodeSrc != String.Empty)
            {
                ReadXmlSrc();
                foreach (TreeNode node in Nodes)
                {
                    node.OnInit();
                }
            }        
        }
    }
}
