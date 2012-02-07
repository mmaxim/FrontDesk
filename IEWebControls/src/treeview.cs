//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    /// <summary>
    /// Event arguments for the OnSelectedIndexChange event
    /// </summary>
    public class TreeViewSelectEventArgs : EventArgs
    {
        private string _oldnodeindex;
        private string _newnodeindex;

        /// <summary>
        /// The previously selected node.
        /// </summary>
        public string OldNode
        {
            get {return _oldnodeindex;}
        }
       
        /// <summary>
        /// The newly selected node.
        /// </summary>
        public string NewNode
        {
            get {return _newnodeindex;}
        }

        /// <summary>
        /// Initializes a new instance of a TreeViewSelectEventArgs object.
        /// </summary>
        /// <param name="strOldNodeIndex">The old node.</param>
        /// <param name="strNewNodeIndex">The new node.</param>
        public TreeViewSelectEventArgs(string strOldNodeIndex, string strNewNodeIndex)
        {
            _oldnodeindex = strOldNodeIndex;
            _newnodeindex = strNewNodeIndex;
        }
    }
    
    /// <summary>
    /// Event arguments for the OnClick event 
    /// </summary>
    public class TreeViewClickEventArgs : EventArgs
    {
        private string _nodeid;

        /// <summary>
        /// The ID of the node that was clicked.
        /// </summary>
        public string Node
        {
            get { return _nodeid; }
        }

        /// <summary>
        /// Initializes a new instance of a TreeViewClickEventArgs object.
        /// </summary>
        /// <param name="node">The ID of the node that was clicked</param>
        public TreeViewClickEventArgs(string node)
        {
            _nodeid = node;
        }
    }

    /// <summary>
    /// Class to eliminate runat="server" requirement and restrict content to approved tags
    /// </summary>
    internal class TreeViewControlBuilder : FilterControlBuilder
    {
        protected override void FillTagTypeTable()
        {
            Add("treenode", typeof(TreeNode));
            Add("treenodetype", typeof(TreeNodeType));
        }
    }

    /// <summary>
    /// Delegate to handle click events on the TreeView.
    /// </summary>
    public delegate void ClickEventHandler(object sender, TreeViewClickEventArgs e);

    /// <summary>
    /// Delegate to handle select events on the TreeView.
    /// </summary>
    public delegate void SelectEventHandler(object sender, TreeViewSelectEventArgs e);

    /// <summary>
    /// TreeView class: Represents a tree.
    /// </summary>
    [
    ParseChildren(false),
    ControlBuilderAttribute(typeof(TreeViewControlBuilder)),
    Designer(typeof(Microsoft.Web.UI.WebControls.Design.TreeViewDesigner)),
    ToolboxBitmap(typeof(Microsoft.Web.UI.WebControls.TreeView)),
    ]
    public class TreeView : BasePostBackControl
    {
        /// <summary>
        /// Event fired when a TreeNode is expanded.
        /// </summary>
        [ResDescription("TreeExpand")]
        public event ClickEventHandler Expand;

        /// <summary>
        /// Event fired when a TreeNode is collapsed.
        /// </summary>
        [ResDescription("TreeCollapse")]
        public event ClickEventHandler Collapse;

        /// <summary>
        /// Event fired when a TreeNode's checkbox is clicked.
        /// </summary>
        [ResDescription("TreeCheck")]
        public event ClickEventHandler Check;

        /// <summary>
        /// Event fired when the selected TreeNode changes.
        /// </summary>
        [ResDescription("TreeSelectedIndexChanged")]
        public event SelectEventHandler SelectedIndexChange;

        private TreeNodeTypeCollection _TreeNodeTypes;
        private TreeNodeCollection _Nodes;
        private bool _bFocused;
        private int _scrollTop;
        private int _scrollLeft;
        private int _parentTop;
        private int _parentLeft;
        private CssCollection _HoverStyle;
        private CssCollection _DefaultStyle;
        private CssCollection _SelectedStyle;
        private bool _bCreated;
        internal ArrayList _eventList;

        /// <summary>
        /// Initializes a new instance of a TreeView.
        /// </summary>
        public TreeView() : base()
        {
            _TreeNodeTypes = new TreeNodeTypeCollection(this);
            _Nodes = new TreeNodeCollection(this);
            _bFocused = false;
            _bCreated = false;
            _HoverStyle = new CssCollection();
            _DefaultStyle = new CssCollection();
            _SelectedStyle = new CssCollection();
            _scrollTop = _scrollLeft = -1;
            _parentTop = _parentLeft = -1;
            _eventList = new ArrayList();
        }

        /// <summary>
        /// Called when a TreeNode expands.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnExpand(TreeViewClickEventArgs e)
        {
            if (Expand != null)
                Expand(this, e);
        }

        /// <summary>
        /// Called when a TreeNode collapses.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCollapse(TreeViewClickEventArgs e)
        {
            if (Collapse != null)
                Collapse(this, e);
        }

        /// <summary>
        /// Called when a TreeNode's checkbox is clicked.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCheck(TreeViewClickEventArgs e)
        {
            if (Check != null)
                Check(this, e);
        }
     
        /// <summary>
        /// Called when the selected TreeNode changes.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected internal virtual void DoSelectedIndexChange(TreeViewSelectEventArgs e)
        {
            // select/deselect nodes
            TreeNode node = GetNodeFromIndex(e.OldNode);
            if (node != null)
                node.Selected = false;

            node = GetNodeFromIndex(e.NewNode);
            if (node != null)
            {
                node.Selected = true;
                SelectedNodeIndex = e.NewNode;
            }
        }

        /// <summary>
        /// Event handler for selection changes.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected internal virtual void OnSelectedIndexChange(TreeViewSelectEventArgs e)
        {
            if (SelectedIndexChange != null)
                SelectedIndexChange(this, e);
        }  

        /// <summary>
        /// Style to use when hovered
        /// </summary>
        [
        Category("Styles"),
        DefaultValue(typeof(CssCollection), ""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ParentHoverStyle"),
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
                if (IsTrackingViewState)
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
        ResDescription("ParentDefaultStyle"),
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
                if (IsTrackingViewState)
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
        ResDescription("ParentSelectedStyle"),
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
                if (IsTrackingViewState)
                {
                    ((IStateManager)_SelectedStyle).TrackViewState();
                    _SelectedStyle.Dirty = true;
                }
            }
        }

        /// <summary>
        /// Url of the image to display when not selected, expanded, or hovered
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor)),
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
        Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor)),
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
        Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor)),
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
        /// Url of the xml file to import as the TreeNode content of the tree
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
            }
        }

        /// <summary>
        /// Gets the collection of TreeNodeTypes in the control.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ResDescription("TreeNodeTypes"),
        ]
        public TreeNodeTypeCollection TreeNodeTypes
        {
            get 
            {
                return _TreeNodeTypes;
            }
        }

        /// <summary>
        /// Gets the collection of nodes in the control.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerDefaultProperty),
        ResDescription("TreeNodes"),
        ]
        public TreeNodeCollection Nodes
        {
            get
            {
                return _Nodes;
            }
        }
 
        /// <summary>
        /// Whether or not to post back to the server on each interaction
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
                object b = ViewState["AutoPostBack"];
                return ((b == null) ? false : (bool)b);
            }
            set 
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        /// <summary>
        /// Show dotted lines representing tree hierarchy?
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(true),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeShowLines"),
        ]
        public bool ShowLines
        {
            get 
            {
                object b = ViewState["ShowLines"];
                return ((b == null) ? true : (bool)b);
            }
            set
            {
                ViewState["ShowLines"] = value;
            }
        }

        /// <summary>
        /// Show tooltips on parent nodes?
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(true),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeToolTip"),
        ]
        public bool ShowToolTip
        {
            get 
            {
                object b = ViewState["ShowToolTip"];
                return ((b == null) ? true : (bool)b);
            }
            set
            {
                ViewState["ShowToolTip"] = value;
            }
        }

        /// <summary>
        /// Show +/- symbols on expandable nodes?
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(true),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeShowPlus"),
        ]
        public bool ShowPlus
        {
            get 
            {
                object b = ViewState["ShowPlus"];
                return ((b == null) ? true : (bool)b);
            }
            set
            {
                ViewState["ShowPlus"] = value;
            }
        }

        /// <summary>
        /// Expand/collapse a node by clicking on it?
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeSelectExpands"),
        ]
        public bool SelectExpands
        {
            get 
            {
                object b = ViewState["SelectExpands"];
                return ((b == null) ? !ShowPlus : (bool)b);
            }
            set
            {
                ViewState["SelectExpands"] = value;
            }
        }

        /// <summary>
        /// Automatically select a node when hovered with the keyboard?
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(false),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeAutoSelect"),
        ]
        public bool AutoSelect
        {
            get 
            {
                object b = ViewState["AutoSelect"];
                return ((b == null) ? false : (bool)b);
            }
            set
            {
                ViewState["AutoSelect"] = value;
            }
        }

        /// <summary>
        /// If ShowLines=false, number of pixels to indent each level of tree
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(19),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeIndent"),
        ]
        public int Indent
        {
            get
            {
                object i = ViewState["Indent"];
                return ((i == null) ? 19 : (int)i);
            }
            set 
            {
                ViewState["Indent"] = value;
            }
        }

        /// <summary>
        /// Path to the directory holding images (lines, +, -, etc) required by the control
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeSystemImagesPath"),
        ]
        public string SystemImagesPath
        {
            get
            {
                object str = ViewState["SystemImagesPath"];
                return ((str == null) ? AddPathToFilename("treeimages/") : (string)str);
            }
            set
            {
                String str = value;
                if (str.Length > 0 && str[str.Length - 1] != '/')
                    str = str + '/';
                ViewState["SystemImagesPath"] = str;
            }
        }

        /// <summary>
        /// Url of the xml file to import as the TreeNodeTypes content of this node
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeNodeTypeSrc"),
        ]
        public string TreeNodeTypeSrc
        {
            get
            {
                object str = ViewState["TreeNodeTypeSrc"];
                return ((str == null) ? String.Empty : (string)str);
            }
            set
            {
                ViewState["TreeNodeTypeSrc"] = value;
            }
        }

        /// <summary>
        /// Index of the selected node
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeSelectedNodeIndex"),
        ]
        public string SelectedNodeIndex
        {
            get
            {
                object str = ViewState["SelectedNodeIndex"];

                if (str == null)
                    return (Nodes.Count > 0) ? "0" : String.Empty;
                else
                    return (string)str;
            } 
            set
            {
                if (IsValidIndex(value))
                {
                    TreeNode node = GetNodeFromIndex(value);
                    if (!_bCreated || (value == null) || (value == String.Empty) || ((node != null) && node.IsVisible))
                    {
                        if (value != (string)ViewState["SelectedNodeIndex"])
                            ViewState["SelectedNodeIndex"] = value;
                    }
                    else
                        throw new Exception(String.Format(Util.GetStringResource("TreeInvisibleSelectedNode"), value));
                }
                else
                    throw new Exception(String.Format(Util.GetStringResource("TreeInvalidIndexFormat"), value));
            }
        }

        /// <summary>
        /// Index of the hovered node
        /// </summary>
        internal string HoverNodeIndex
        {
            get
            {
                object str = ViewState["HoverNodeIndex"];
                return ((str == null) ? String.Empty : (string)str);
            }
            set
            {
                if ((string)ViewState["HoverNodeIndex"] != value)
                    ViewState["HoverNodeIndex"] = value;
            }
        }

        /// <summary>
        /// Number of levels deep to expand the tree by default
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(0),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeExpandLevel"),
        ]
        public int ExpandLevel
        {
            get
            {
                object i = ViewState["ExpandLevel"];
                return ((i == null) ? 0 : (int)i);
            }
            set
            {
                ViewState["ExpandLevel"] = value;
            }
        }

        /// <summary>
        /// Indicates that the SelectedNodeIndex can be modified on insertions.
        /// </summary>
        internal bool IsInitialized
        {
            get { return IsTrackingViewState; }
        }

        /// <summary>
        /// Adds parsed child objects to the TreeView.
        /// </summary>
        /// <param name="obj">Child object to add, must be either a TreeNode or TreeNodeType.</param>
        protected override void AddParsedSubObject(Object obj)
        {
            if (obj is TreeNode)
            {
                _Nodes.Add((TreeNode)obj);
            }
            else if (obj is TreeNodeType)
            {
                _TreeNodeTypes.Add((TreeNodeType)obj);
            }
        }

        /// <summary>
        /// Overridden. Verifies certain properties.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Controls.Clear();
            // Databind from XML source
            if (!Page.IsPostBack)
            {
                ReadTreeNodeTypeXmlSrc();
                ReadTreeNodeXmlSrc();

                foreach (TreeNode node in Nodes)
                {
                    node.OnInit();
                }
            }
        }
        
        /// <summary>
        /// On a postback, review state information so we can expand nodes as needed
        /// </summary>
        /// <param name="e"> </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _bCreated = true; // first chance to do this on non-postback

            // initialize SelectedNodeIndex, if needed
            if ((SelectedNodeIndex == "" || SelectedNodeIndex == String.Empty) && Nodes.Count > 0)
                SelectedNodeIndex = "0";

            TreeNode node = GetNodeFromIndex(SelectedNodeIndex);
            if (node != null)
                node.Selected = true;
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
        /// Determines whether the FORMAT of the given index string is valid
        /// (this is, containing only digits and periods).  No validation of the
        /// actual index being referenced is made.
        /// </summary>
        /// <param name="strIndex">Index to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool IsValidIndex(string strIndex)
        {
            if (strIndex == null || strIndex == String.Empty)
                return true;

            Regex r = new Regex("[^0-9.]");
            if (r.IsMatch(strIndex))    // a character other than a digit or .
                return false;

            if (strIndex[0] == '.')     // mustn't begin with a .
                return false;

            if (strIndex.IndexOf("..") != -1)   // mustn't have two consecutive periods
                return false;

            return true;
        }

        /// <summary>
        /// Returns the TreeNode at the given index location
        /// </summary>
        /// <param name="strIndex">string of dot-separated indices (e.g. "1.0.3") where 
        /// each index is the 0-based position of a node at the next deeper level of the tree</param>
        /// <returns>The TreeNode, if found, or null</returns>
        public TreeNode GetNodeFromIndex(string strIndex)
        {
            if (strIndex != null && strIndex.Length != 0)
            {
                // convert index string into array of strings
                string[] a = strIndex.Split(new Char[] {'.'});
                int i = 0;
                int index;
                TreeNodeCollection colNodes = Nodes;
                while (i < a.GetLength(0) - 1)
                {
                    index = Convert.ToInt32(a[i]);

                    if (index >= colNodes.Count)
                        return null;
                    colNodes = colNodes[index].Nodes;
                    i++;
                }
                index = Convert.ToInt32(a[i]);
                if (index >= colNodes.Count)
                    return null;
                else
                    return colNodes[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Process data being posted back.
        /// </summary>
        /// <param name="strData">The data that was posted.</param>
        /// <returns>true if data changed, false otherwise.</returns>
        protected override bool ProcessData(String strData)
        {
            if (strData == null || strData == String.Empty)
                return false;

            // Split data using the pipe character as a delimeter
            String[] astrData = strData.Split(new Char[] {'|'});

            // Extract whether the tree was focused before the postback
            if (astrData[0] == "1")
                _bFocused = true;

            // Extract the hovered node index
            HoverNodeIndex = astrData[1];

            // Extract the scroll top value
            if (astrData[2] != String.Empty)
            {
                try
                {
                    _scrollTop = Convert.ToInt32(astrData[2]);
                }
                catch
                {
                    // Ignore
                }
            }

            // Extract the scroll left value
            if (astrData[3] != String.Empty)
            {
                try
                {
                    _scrollLeft = Convert.ToInt32(astrData[3]);
                }
                catch
                {
                    // Ignore
                }
            }

            // Extract the parent's scroll top value
            if (astrData[4] != String.Empty)
            {
                try
                {
                    _parentTop = Convert.ToInt32(astrData[4]);
                }
                catch
                {
                    // Ignore
                }
            }

            // Extract the parent's scroll left value
            if (astrData[5] != String.Empty)
            {
                try
                {
                    _parentLeft = Convert.ToInt32(astrData[5]);
                }
                catch
                {
                    // Ignore
                }
            }

            // Extract the queued events
            if (astrData[6] != null && astrData[6] != String.Empty)
                return ProcessEvents(astrData[6]);

            return false;
        }

        /// <summary>
        /// Called when a downlevel browser submits the form
        /// </summary>
        /// <param name="eventArg">Event argument.</param>
        protected override void RaisePostBackEvent(string eventArg)
        {
            ProcessEvents(eventArg);
            RaisePostDataChangedEvent();
        }

        /// <summary>
        /// Called when the TreeView on the client-side submitted the form.
        /// </summary>
        /// <param name="eventArg">Event argument.</param>
        protected bool ProcessEvents(string eventArg)
        {
            if (eventArg == null || eventArg == String.Empty || eventArg == " ") // Don't know why, but the framework is giving a " " eventArg instead of null
                return false;

            TreeNode tn = null;
            String[] events = eventArg.Split(new Char[] {';'});
            foreach (string strWholeEvent in events)
            {
                String[] parms = strWholeEvent.Split(new Char[] {','});
                if (parms[0].Length > 0)
                {
                    if (parms[0].Equals("onselectedindexchange") && parms.GetLength(0) == 3)
                    {
                        TreeViewSelectEventArgs e = new TreeViewSelectEventArgs(parms[1], parms[2]);
                        tn = GetNodeFromIndex(parms[2]);
                        if (tn != null)
                            tn.LowerPostBackEvent(parms[0]);
                        DoSelectedIndexChange(e);
                        _eventList.Add("s");
                        _eventList.Add(e);
                    }
                    else if ((parms[0].Equals("onexpand") || parms[0].Equals("oncollapse") || parms[0].Equals("oncheck")) && parms.GetLength(0) == 2)
                    {
                        TreeViewClickEventArgs e = new TreeViewClickEventArgs(parms[1]);
                        if (parms[0].Equals("onexpand"))
                            _eventList.Add("e");
                        else if (parms[0].Equals("oncollapse"))
                            _eventList.Add("c");
                        else
                            _eventList.Add("k");
                        _eventList.Add(e);
                        tn = GetNodeFromIndex(parms[1]);
                        if (tn != null)
                            tn.LowerPostBackEvent(parms[0]);
                    }
                }
            }
            if (_eventList.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Fires pending events from a postback
        /// </summary>
        protected override void RaisePostDataChangedEvent()
        {
            for (int i = 0; i < _eventList.Count; i += 2)
            {
                String str = (String)_eventList[i];
                if (str == "s")
                    OnSelectedIndexChange((TreeViewSelectEventArgs)_eventList[i + 1]);
                else if (str == "e")
                    OnExpand((TreeViewClickEventArgs)_eventList[i + 1]);
                else if (str == "c")
                    OnCollapse((TreeViewClickEventArgs)_eventList[i + 1]);
                else
                    OnCheck((TreeViewClickEventArgs)_eventList[i + 1]);
            }
            _eventList.Clear();
        }

        /// <summary>
        /// Reads the XML file specified in XmlSrc and creates TreeNodes accordingly.  The
        /// file is assumed to be valid XML with a TREENODES outer container and TREENODE
        /// inner containers.
        /// </summary>
        /// <param name="TreeNodeSrc">The XML source.</param>
        /// <param name="TreeNodeXsltSrc">The XSL source.</param>
        /// <param name="strOuter">Outer element name.</param>
        /// <returns>A new TreeView or null.</returns>
        internal TreeView ReadXmlSrc(string TreeNodeSrc, string TreeNodeXsltSrc, string strOuter)
        {
            XmlTextReader reader;
            bool bReading = false;

            if (TreeNodeSrc != String.Empty)
            {
                try
                {
                    reader = GetXmlReaderFromUri(TreeNodeSrc, TreeNodeXsltSrc);
                    bReading = reader.Read();
                }
                catch
                {
                    // couldn't read.  Try TreeNodeSrc as a string.
                    reader = GetXmlReaderFromString(TreeNodeSrc, TreeNodeXsltSrc);
                    if (reader != null)
                        bReading = reader.Read();
                }

                if (reader != null && bReading)
                {
                    if (!reader.IsStartElement(strOuter))
                    {
                        throw new Exception(String.Format(Util.GetStringResource("TreeMissingOuterContainer"), TreeNodeSrc, TreeNodeXsltSrc));
                    }

                    // ParseDesignerModeControl returns a single control (and its children).  This is
                    // a problem if there are multiple top-level treenodes in the XML.  Parsing the string
                    // and finding a matching end tag to each treenode tag is non-trivial. 
                    // For each node, we'd have to handle both
                    // <TREENODE></TREENODE> and <TREENODE /> cases, and make sure we've got a matched set.
                    //
                    // Plan B would be to use XmlTextReader to parse things for us, but it can't handle
                    // the namespace prefix without a register directive in the page itself.
                    // *** NOTE: Investigate that possibility.
                    //
                    // So, plan C: Add a dummy container treenode, then loop through its children and add
                    // them to our tree.  Simple, effective, and the creation of one extra treenode
                    // should involve less overhead than lots of text parsing.

                    string xml = reader.ReadInnerXml();
                    xml = "<%@ Register TagPrefix='_TreeViewPrefix' Assembly='" + typeof(TreeView).Assembly.ToString() + "' Namespace='Microsoft.Web.UI.WebControls' %><%@ import Namespace='Microsoft.Web.UI.WebControls' %><_TreeViewPrefix:TREEVIEW runat='server'>" + xml + "</_TreeViewPrefix:TREEVIEW>";
                    Control c = Page.ParseControl(xml);
                    TreeView tv = (TreeView)c.Controls[0];
        
                    return (tv);
                }
            }
            return (null);
        }

        /// <summary>
        /// [TODO: to be supplied]
        /// </summary>
        /// <param name="TreeNodeSrc">[TODO: to be supplied]</param>
        /// <param name="TreeNodeXsltSrc">[TODO: to be supplied]</param>
        /// <returns>[TODO: to be supplied]</returns>
        internal XmlTextReader GetXmlReaderFromUri(string TreeNodeSrc, string TreeNodeXsltSrc)
        {
            Uri uri = new Uri(Page.Request.Url, TreeNodeSrc);
            if (TreeNodeXsltSrc != String.Empty)
            {
                // Load TreeNodeSrc into an XmlDocument
                XPathDocument xmldoc = new XPathDocument(uri.AbsoluteUri);

                return GetXmlReaderFromXPathDoc(xmldoc, TreeNodeXsltSrc);
            }
            else
            {
                return new XmlTextReader(uri.AbsoluteUri);
            }
        }

        /// <summary>
        /// [TODO: to be supplied]
        /// </summary>
        /// <param name="TreeNodeSrc">[TODO: to be supplied]</param>
        /// <param name="TreeNodeXsltSrc">[TODO: to be supplied]</param>
        /// <returns>[TODO: to be supplied]</returns>
        internal XmlTextReader GetXmlReaderFromString(string TreeNodeSrc, string TreeNodeXsltSrc)
        {
            StringReader reader = new StringReader(TreeNodeSrc);
            if (TreeNodeXsltSrc != String.Empty)
            {
                // Load TreeNodeSrc into an XmlDocument
                XPathDocument xmldoc = new XPathDocument(reader);

                return GetXmlReaderFromXPathDoc(xmldoc, TreeNodeXsltSrc);
            }
            else
            {
                return new XmlTextReader(reader);
            }
        }

        /// <summary>
        /// [TODO: to be supplied]
        /// </summary>
        /// <param name="xmldoc">[TODO: to be supplied]</param>
        /// <param name="TreeNodeXsltSrc">[TODO: to be supplied]</param>
        /// <returns>[TODO: to be supplied]</returns>
        internal XmlTextReader GetXmlReaderFromXPathDoc(XPathDocument xmldoc, string TreeNodeXsltSrc)
        {
            // Load TreeNodeXsltSrc into XslTransform
            XslTransform xslTrans = new XslTransform();
            Uri uri = new Uri(Page.Request.Url, TreeNodeXsltSrc);
            xslTrans.Load(uri.AbsoluteUri);

            // Do transform
            MemoryStream stream = new MemoryStream();
            xslTrans.Transform(((IXPathNavigable)xmldoc).CreateNavigator(), null, stream);
            stream.Position = 0;
            return new XmlTextReader(stream);
        }

        //
        // ReadTreeNodeTypeXmlSrc()
        //
        // Reads the XML file specified in TreeNodeTypeSrc and creates TreeNodeTypes accordingly.  The
        // file is assumed to be valid XML with a <TREENODETYPES> outer container and <TREENODETYPE>
        // inner containers.
        //
        internal void ReadTreeNodeTypeXmlSrc()
        {
            TreeView tv = ReadXmlSrc(TreeNodeTypeSrc, String.Empty, "TREENODETYPES");
            if (tv != null)
            {
                TreeNodeTypeCollection newTypes = new TreeNodeTypeCollection(this);
                ((IStateManager)newTypes).TrackViewState();
                newTypes.Clear();
                if (tv.TreeNodeTypes != null)
                {
                    for (int i = 0; i < tv.TreeNodeTypes.Count; i++)
                    {
                        newTypes.Add(tv.TreeNodeTypes[i]);
                        tv.TreeNodeTypes[i].SetViewStateDirty();
                    }
                }
                _TreeNodeTypes = newTypes;
            }
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if ((SelectedNodeIndex == "" || SelectedNodeIndex == String.Empty) && Nodes.Count > 0)
                SelectedNodeIndex = "0";
            else
                SelectedNodeIndex = SelectedNodeIndex; // verify current index

            // We only check HoverNodeIndex for validity here, because it's possible to have had many events
            // stacked up on the client if AutoPostBack=false.  The HoverNodeIndex might not validate until
            // after these events are processed, so we have to wait until now to check.
            if (!IsValidIndex(HoverNodeIndex))
                throw new Exception(String.Format(Util.GetStringResource("TreeInvalidIndexFormat"), HoverNodeIndex));

            base.OnPreRender(e);
        }

        /// <summary>
        /// Renders the uplevel version of the TreeView.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive the output.</param>
        protected override void RenderUpLevelPath(HtmlTextWriter output)
        {
            // render uplevel
            output.Write("<?XML:NAMESPACE PREFIX=TVNS />\n<?IMPORT NAMESPACE=TVNS IMPLEMENTATION=\"" 
                + AddPathToFilename("treeview.htc") + "\" />");
            output.WriteLine();
            
            AddAttributesToRender(output);

            string cssText = DefaultStyle.CssText;
            if (cssText != String.Empty)
                output.AddAttribute("defaultStyle", cssText);

            cssText = HoverStyle.CssText;
            if (cssText != String.Empty)
                output.AddAttribute("hoverStyle", cssText);

            cssText = SelectedStyle.CssText;
            if (cssText != String.Empty)
                output.AddAttribute("selectedStyle", cssText);

            if (ImageUrl != String.Empty)
                output.AddAttribute("imageUrl", ImageUrl);

            if (SelectedImageUrl != String.Empty)
                output.AddAttribute("selectedImageUrl", SelectedImageUrl);

            if (ExpandedImageUrl != String.Empty)
                output.AddAttribute("expandedImageUrl", ExpandedImageUrl);

            if (ChildType != String.Empty)
                output.AddAttribute("childType", ChildType);

            if (Target != String.Empty)
                output.AddAttribute("target", Target);

            output.AddAttribute("selectedNodeIndex", SelectedNodeIndex);
            output.AddAttribute("HelperID", HelperID);
            output.AddAttribute("systemImagesPath", SystemImagesPath);
 
            if (HoverNodeIndex != String.Empty)
                output.AddAttribute("HoverNodeIndex", HoverNodeIndex);
            
            if (ShowLines == false)
            {
                output.AddAttribute("showLines", "false");
                output.AddAttribute("indent", Indent.ToString());
            }

            if (ShowPlus == false)
                output.AddAttribute("showPlus", "false");

            if (ShowToolTip == false)
                output.AddAttribute("showToolTip", "false");

            if (SelectExpands == true)
                output.AddAttribute("selectExpands", "true");

            if (AutoSelect == true)
                output.AddAttribute("autoSelect", "true");

            if (_bFocused)
                output.AddAttribute("Focused", "true");

            if (_scrollTop >= 0)
            {
                output.AddAttribute("__scrollTop", _scrollTop.ToString());
            }
            if (_scrollLeft >= 0)
            {
                output.AddAttribute("__scrollLeft", _scrollLeft.ToString());
            }
            if (_bFocused)
            {
                if (_parentTop >= 0)
                {
                    output.AddAttribute("__parentTop", _parentTop.ToString());
                }
                if (_parentLeft >= 0)
                {
                    output.AddAttribute("__parentLeft", _parentLeft.ToString());
                }
            }

            if (Page != null)
            {
                output.AddAttribute("onexpand", "javascript:" + " if (this.clickedNodeIndex != null) this.queueEvent('onexpand', this.clickedNodeIndex)");
                output.AddAttribute("oncollapse", "javascript:" + " if (this.clickedNodeIndex != null) this.queueEvent('oncollapse', this.clickedNodeIndex)");
                output.AddAttribute("oncheck", "javascript:" + " if (this.clickedNodeIndex != null) this.queueEvent('oncheck', this.clickedNodeIndex)");
                output.AddAttribute("onselectedindexchange", "javascript:" + " if (event.oldTreeNodeIndex != event.newTreeNodeIndex) this.queueEvent('onselectedindexchange', event.oldTreeNodeIndex + ',' + event.newTreeNodeIndex)");
                if (AutoPostBack == true)
                {
                    string str = Page.GetPostBackEventReference(this, "");
                    str = str.Replace("'", "\\'");
                    str = "javascript: window.setTimeout('" + str + "', 0, 'JavaScript')";
                    output.AddAttribute("onfirequeuedevents", str);
                }
            }
                        
            output.RenderBeginTag("tvns:treeview");

            base.RenderUpLevelPath(output);

            output.RenderEndTag();
        }

        /// <summary>
        /// Renders the downlevel version of the TreeView.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive the output.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter output)
        {
            HtmlTextWriter newWriter = new HtmlTextWriter(output);
            ControlStyle.AddAttributesToRender(newWriter);
            AddAttributesToRender(newWriter);
            newWriter.RenderBeginTag("DIV");

            output.AddAttribute("CELLSPACING", "0");
            output.AddAttribute("CELLPADDING", "0");
            output.AddAttribute("BORDER", "0");
//            AddAttributesToRender(output);
            output.RenderBeginTag("TABLE");

            base.RenderDownLevelPath(output);
            
            output.RenderEndTag();             
            newWriter.RenderEndTag();             
        }

        /// <summary>
        /// Renders the TreeView in the designer (the downlevel version).
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive the output.</param>
        protected override void RenderDesignerPath(HtmlTextWriter output)
        {
            RenderDownLevelPath(output);
        }

        private void ReadTreeNodeXmlSrc()
        {
            if (TreeNodeSrc != String.Empty)
            {
                TreeView tv = ReadXmlSrc(TreeNodeSrc, TreeNodeXsltSrc, "TREENODES");
                if (tv != null)
                {
                    TreeNodeCollection col = tv.Nodes;
                    CopyXmlNodesIntoTree(col, this);
                }
            }
        }

        //
        // CopyXmlNodesIntoTree
        //
        // To make sure databound nodes are recreated on postback with redatabinding, the
        // TreeNodeCollections they're in must be recreated dynamically so the actions of
        // adding each node can be stored.
        //
        internal void CopyXmlNodesIntoTree(TreeNodeCollection col, Object dest)
        {
            TreeNodeCollection destNodes = (dest is TreeView ? ((TreeView)dest)._Nodes : ((TreeNode)dest)._Nodes);
            destNodes.Clear();
            if (col != null)
            {
                while (col.Count > 0)
                {
                    TreeNode node = col[0];
                    col.RemoveAt(0);
                    destNodes.Add(node);
                }
            }
            destNodes.SetViewStateDirty();
        }

        private TreeNodeCollection GetNodeCollection(Object obj)
        {
            if (obj is TreeView)
                return ((TreeView)obj).Nodes;
            else if (obj is TreeNode)
                return ((TreeNode)obj).Nodes;
            else
                throw new Exception(Util.GetStringResource("TreeInvalidObject"));
        }   

        internal object GetStateVar(String att)
        {
            if (att.EndsWith("Style"))
                return typeof(TreeView).InvokeMember (att, BindingFlags.Default | BindingFlags.GetProperty, null, this, new object [] {});
            else
                return ViewState[att];
        }

        /// <summary>
        /// Renders the contents of the TreeView.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive the output.</param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (IsUpLevelBrowser)
            {
                foreach (TreeNodeType tnt in TreeNodeTypes)
                {
                    tnt.Render(output, RenderPathID.UpLevelPath);
                }
            }

            foreach (TreeNode node in Nodes)
            {
                node.Render(output, RenderPath);
            }
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
                ((IStateManager)DefaultStyle).LoadViewState(state[1]);
                ((IStateManager)HoverStyle).LoadViewState(state[2]);
                ((IStateManager)SelectedStyle).LoadViewState(state[3]);
                ((IStateManager)TreeNodeTypes).LoadViewState(state[4]);
                ((IStateManager)Nodes).LoadViewState(state[5]);
            }
            _bCreated = true;
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
                ((IStateManager)DefaultStyle).SaveViewState(),
                ((IStateManager)HoverStyle).SaveViewState(),
                ((IStateManager)SelectedStyle).SaveViewState(),
                ((IStateManager)TreeNodeTypes).SaveViewState(),
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
        /// Instructs the control to track changes to its view state.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)Nodes).TrackViewState();
            ((IStateManager)TreeNodeTypes).TrackViewState();
            ((IStateManager)DefaultStyle).TrackViewState();
            ((IStateManager)HoverStyle).TrackViewState();
            ((IStateManager)SelectedStyle).TrackViewState();
        }

        /// <summary>
        /// Color of the text within the control.
        /// </summary>
        [Browsable(false)]
        public override System.Drawing.Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        /// <summary>
        /// The font used for text within the control.
        /// </summary>
        [Browsable(false)]
        public override FontInfo Font
        {
            get { return base.Font; }
        }

        /// <summary>
        /// The tooltip displayed when the mouse is over the control.
        /// </summary>
        [Browsable(false)]
        public override string ToolTip
        {
            get { return base.ToolTip; }
            set { base.ToolTip = value; }
        }

        /// <summary>
        /// Value to indicate whether child values need to recalculate
        /// </summary>
        internal bool AlwaysCalcValues
        {
            get { return IsDesignMode; }
        }

        /// <summary>
        /// Value to indicate whether we're running in an up-level browser
        /// </summary>
        internal bool IsUpLevel
        {
            get { return IsUpLevelBrowser; }
        }

        /// <summary>
        /// Forces the node to redatabind immediately, even if it isn't expanded.
        /// Child nodes are only bound if they're expanded.
        /// </summary>
        public override void DataBind()
        {
            if (TreeNodeTypeSrc != String.Empty)
                ReadTreeNodeTypeXmlSrc();

            if (TreeNodeSrc != String.Empty)
            {
                ReadTreeNodeXmlSrc();
                foreach (TreeNode node in Nodes)
                {
                    node.OnInit();
                }
            }        
        }

    }    
}
