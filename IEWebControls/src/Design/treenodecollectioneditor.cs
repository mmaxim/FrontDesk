//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls.Design
{

    using System.Runtime.InteropServices;
    using System.ComponentModel;
    using System;
    using System.Design;
    using System.Collections;
    using Microsoft.Win32;    
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using Microsoft.Web.UI.WebControls;

    /// <summary>
    /// The TreeNodeCollectionEditor is a collection editor that is 
    /// specifically designed to edit a TreeNodeCollection.
    /// </summary>
    internal class TreeNodeCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Constructs a new instance of a TreeNodeCollectionEditor.
        /// </summary>
        public TreeNodeCollectionEditor() : base(typeof(Microsoft.Web.UI.WebControls.TreeNodeCollection))
        {
        }

        /// <summary>
        /// Creates a new form to show the current collection.
        /// You may inherit from CollectionForm to provide your own form.
        /// </summary>
        /// <returns>The form to show the currect collection.</returns>
        protected override CollectionForm CreateCollectionForm()
        {
            return new TreeNodeCollectionForm(this);
        }

        private class TreeNodeCollectionForm : CollectionForm
        {
            private int nextNode = 0;
            private Container components = new Container();
            private System.Windows.Forms.TreeView tvNodes = new System.Windows.Forms.TreeView();
            private Microsoft.Web.UI.WebControls.TreeView tvWebNodes = new Microsoft.Web.UI.WebControls.TreeView();
            private Button btnAddChild = new Button();
            private Button btnOK = new Button();
            private Button btnApply = new Button();
            private Button btnCancel = new Button();
            private Button btnDelete = new Button();
            private Button btnAddRoot = new Button();
            private Button upButton = new Button();
            private Button downButton = new Button();
            private Button leftButton = new Button();
            private Button rightButton = new Button();
            private Label lblNodes = new Label();
            private Label lblProp = new Label();
            private GroupBox groupBox1 = new GroupBox();
            private PropertyGrid propGrid = new PropertyGrid();            
            private object NextNodeKey = new object();

            public TreeNodeCollectionForm(CollectionEditor editor) : base(editor)
            {
                InitializeComponent();
            }
            
            private Microsoft.Web.UI.WebControls.TreeView TreeView
            {
                get
                {
                    if (Context != null && Context.Instance is Microsoft.Web.UI.WebControls.TreeView)
                    {
                        return (Microsoft.Web.UI.WebControls.TreeView)Context.Instance;
                    }
                    else
                    {
                        Debug.Assert(false, "TreeNodeCollectionEditor couldn't find the TreeView being designed");
                        return null;
                    }
                }
            }
            
            private System.Windows.Forms.TreeNode SelectedNode
            {
                get
                {
                    return tvNodes.SelectedNode;
                }

                set
                {
                    tvNodes.SelectedNode = value;

                    if (value == null)
                    {
                        propGrid.SelectedObject = null;
                        lblProp.Text = "Properties:";
                        btnAddChild.Enabled = false;
                        btnDelete.Enabled = false;
                        upButton.Enabled = false;
                        downButton.Enabled = false;
                        leftButton.Enabled = false;
                        rightButton.Enabled = false;
                    }
                    else
                    {
                        propGrid.SelectedObject = value.Tag;
                        lblProp.Text = value.Text + "'s Properties:";
                        btnAddChild.Enabled = true;
                        btnDelete.Enabled = true;
                        upButton.Enabled = (value.PrevNode != null);
                        downButton.Enabled = (value.NextNode != null);
                        leftButton.Enabled = (value.Parent != null);
                        rightButton.Enabled = (value.PrevNode != null);
                    }
                }
            }

            private int NextNode
            {
                get
                {
                    if (TreeView != null && TreeView.Site != null)
                    {
                        IDictionaryService ds = (IDictionaryService)TreeView.Site.GetService(typeof(IDictionaryService));
                        Debug.Assert(ds != null, "TreeNodeCollectionEditor relies on IDictionaryService, which is not available.");
                        if (ds != null)
                        {
                            object dictionaryValue = ds.GetValue(NextNodeKey);
                            if (dictionaryValue != null)
                            {
                                nextNode = (int)dictionaryValue;
                            }
                            else
                            {
                                nextNode = 0;
                                ds.SetValue(NextNodeKey, 0);
                            }
                        }    
                    }

                    return nextNode;
                }

                set 
                {
                    nextNode = value;
                    if (TreeView != null && TreeView.Site != null) 
                    {
                        IDictionaryService ds = (IDictionaryService)TreeView.Site.GetService(typeof(IDictionaryService));
                        Debug.Assert(ds != null, "TreeNodeCollectionEditor relies on IDictionaryService, which is not available.");
                        if (ds != null) 
                        {
                            ds.SetValue(NextNodeKey, nextNode);
                        }    
                    }
                }
            }

            private System.Windows.Forms.TreeNode Add(System.Windows.Forms.TreeNode parent) 
            {
                string strText = "Node" + NextNode++.ToString();
                System.Windows.Forms.TreeNode newnode;
                Microsoft.Web.UI.WebControls.TreeNodeCollection col;
                Microsoft.Web.UI.WebControls.TreeNode webNode;

                if (parent == null)
                {
                    newnode = tvNodes.Nodes.Add(strText);
                    col = tvWebNodes.Nodes;
                }
                else 
                {
                    newnode = parent.Nodes.Add(strText);
                    parent.Expand();
                    col = ((Microsoft.Web.UI.WebControls.TreeNode)parent.Tag).Nodes;
                }

                webNode = new Microsoft.Web.UI.WebControls.TreeNode();
                webNode.Text = strText;
                col.Add(webNode);
                newnode.Tag = webNode;

                btnApply.Enabled = true;

                return newnode;
            }

            private void BtnAddChild_click(object sender, EventArgs e) 
            {
                Add(SelectedNode);

                // Refresh arrow states
                SelectedNode = SelectedNode;
            }

            private void BtnAddRoot_click(object sender, EventArgs e) 
            {
                System.Windows.Forms.TreeNode rootNode = Add(null);
                if (SelectedNode == null)
                {
                    SelectedNode = rootNode;
                }
                else
                {
                    // Refresh arrow states
                    SelectedNode = SelectedNode;
                }
            }

            private void BtnDelete_click(object sender, EventArgs e) 
            {
                if (SelectedNode == null)
                {
                    return;
                }

                // Determine the new node to select
                System.Windows.Forms.TreeNode selNode = null;
                if (SelectedNode.NextNode != null)
                {
                    selNode = SelectedNode.NextNode;
                }
                else if (SelectedNode.PrevNode != null)
                {
                    selNode = SelectedNode.PrevNode;
                }
                else if (SelectedNode.Parent != null)
                {
                    selNode = SelectedNode.Parent;
                }

                ((Microsoft.Web.UI.WebControls.TreeNode)(SelectedNode.Tag)).Remove();
                SelectedNode.Remove();

                btnApply.Enabled = true;

                SelectedNode = selNode;
            }

            private void BtnOK_click(object sender, EventArgs e) 
            {
                object[] values = new object[tvWebNodes.Nodes.Count];
                for (int i = 0; i < values.Length; i++) 
                {
                    values[i] = ((ICloneable)tvWebNodes.Nodes[i]).Clone();
                }
                Items = values;
       
                btnApply.Enabled = false;
            }

            /// <summary>
            /// NOTE: The following code is required by the form
            /// designer.  It can be modified using the form editor.  Do not
            /// modify it using the code editor.
            /// </summary>
            private void InitializeComponent() 
            {
                tvNodes.Location = new Point(10, 30);
                tvNodes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
                tvNodes.Size = new Size(271, 260);
                tvNodes.TabIndex = 1;
                tvNodes.Text = "tvNodes";
                tvNodes.HideSelection = false;
                tvNodes.ImageIndex = 0;
                tvNodes.Indent = 19;
                tvNodes.LabelEdit = true;
                tvNodes.AfterLabelEdit += new NodeLabelEditEventHandler(this.TvNodes_afterLabelEdit);
                tvNodes.AfterSelect += new TreeViewEventHandler(this.TvNodes_afterSelect);

                propGrid.CommandsVisibleIfAvailable = true;
                propGrid.Location = new Point(316, 30);
                propGrid.Size = new System.Drawing.Size(215, 260);
                propGrid.TabIndex = 10;
                propGrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right;
                propGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyChanged);

                btnOK.Location = new Point(256, 352);
                btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                btnOK.Size = new Size(75, 23);
                btnOK.TabIndex = 11;
                btnOK.Text = "OK";
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Click += new EventHandler(this.BtnOK_click);

                btnCancel.Location = new Point(350, 352);
                btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                btnCancel.Size = new Size(75, 23);
                btnCancel.TabIndex = 12;
                btnCancel.Text = "Cancel";
                btnCancel.DialogResult = DialogResult.Cancel;

                btnApply.Enabled = false;
                btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                btnApply.Location = new Point(444, 352);
                btnApply.Size = new Size(80, 23);
                btnApply.TabIndex = 13;
                btnApply.Text = "Apply";
                btnApply.Click += new EventHandler(this.BtnOK_click);

                upButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                upButton.Location = new Point(286, 30);
                upButton.Size = new Size(23, 23);
                upButton.TabIndex = 2;
                upButton.Image = new Icon(typeof(CollectionEditor), "SortUp.ico").ToBitmap();
                upButton.Click += new EventHandler(this.UpButton_click);

                downButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                downButton.Location = new Point(286, 59);
                downButton.Size = new Size(23, 23);
                downButton.TabIndex = 3;
                downButton.Image = new Icon(typeof(CollectionEditor), "SortDown.ico").ToBitmap();
                downButton.Click += new EventHandler(this.DownButton_click);

                leftButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                leftButton.Location = new Point(286, 88);
                leftButton.Size = new Size(23, 23);
                leftButton.TabIndex = 4;
                leftButton.Image = new Icon(typeof(CollectionEditor), "SortDown.ico").ToBitmap();
                leftButton.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                leftButton.Click += new EventHandler(this.LeftButton_click);

                rightButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                rightButton.Location = new Point(286, 117);
                rightButton.Size = new Size(23, 23);
                rightButton.TabIndex = 5;
                rightButton.Image = new Icon(typeof(CollectionEditor), "SortDown.ico").ToBitmap();
                rightButton.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                rightButton.Click += new EventHandler(this.RightButton_click);

                this.Text = "TreeNodeEditor";
                this.AcceptButton = btnOK;
                this.AutoScaleBaseSize = new Size(5, 13);
                this.CancelButton = btnCancel;
                this.ClientSize = new Size(539, 380);
                this.MinimumSize = new Size(544, 383);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.StartPosition = FormStartPosition.CenterScreen;

                btnAddRoot.Location = new Point(14, 302);
                btnAddRoot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                btnAddRoot.Size = new Size(80, 23);
                btnAddRoot.TabIndex = 6;
                btnAddRoot.Text = "Add Root";
                btnAddRoot.ImageAlign = ContentAlignment.MiddleLeft;
                //                btnAddRoot.Image = new Icon(typeof(TreeNodeCollectionEditor), "Folder.ico").ToBitmap();
                btnAddRoot.Click += new EventHandler(this.BtnAddRoot_click);

                btnAddChild.Enabled = false;
                btnAddChild.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                btnAddChild.Location = new Point(107, 302);
                btnAddChild.Size = new Size(80, 23);
                btnAddChild.TabIndex = 7;
                btnAddChild.Text = "Add Child";
                btnAddChild.ImageAlign = ContentAlignment.MiddleLeft;
                //                btnAddChild.Image = new Icon(typeof(TreeNodeCollectionEditor), "ChildFolder.ico").ToBitmap();
                btnAddChild.Click += new EventHandler(this.BtnAddChild_click);

                btnDelete.Enabled = false;
                btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                btnDelete.Location = new Point(200, 302);
                btnDelete.Size = new Size(80, 23);
                btnDelete.TabIndex = 8;
                btnDelete.Text = "Delete";
                btnDelete.ImageAlign = ContentAlignment.MiddleLeft;
                //                btnDelete.Image = new Icon(typeof(TreeNodeCollectionEditor), "Delete.ico").ToBitmap();
                btnDelete.Click += new EventHandler(this.BtnDelete_click);

                groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                groupBox1.Location = new Point(8, 340);
                groupBox1.Size = new Size(524, 8);
                groupBox1.TabIndex = 9;
                groupBox1.TabStop = false;
                groupBox1.Text = "";

                lblNodes.Location = new Point(10, 10);
                lblNodes.Size = new Size(150, 13);
                lblNodes.TabIndex = 0;
                lblNodes.TabStop = false;
                lblNodes.Text = "Select Node To Edit:";

                lblProp.Location = new Point(314, 10);
                lblProp.Size = new Size(220, 13);
                lblProp.TabIndex = 0;
                lblProp.TabStop = false;
                lblProp.Text = "Properties:";
                lblProp.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right;

                this.Controls.Clear();                     
                this.Controls.AddRange(new Control[] {
                                                         btnDelete,
                                                         propGrid,
                                                         lblNodes,
                                                         lblProp,
                                                         btnAddRoot,
                                                         btnCancel,
                                                         btnOK,
                                                         btnAddChild,
                                                         btnApply,
                                                         groupBox1,
                                                         upButton,
                                                         downButton,
                                                         leftButton,
                                                         rightButton,
                                                         tvNodes});
            }
            
            /// <summary>
            /// This is called when the value property in the CollectionForm has changed.
            /// In it you should update your user interface to reflect the current value.
            /// </summary>
            protected override void OnEditValueChanged() 
            {
                if (EditValue != null)
                {
                    Microsoft.Web.UI.WebControls.TreeNodeCollection editCol = (Microsoft.Web.UI.WebControls.TreeNodeCollection)EditValue;
                    tvWebNodes.Site = ((Microsoft.Web.UI.WebControls.TreeView)editCol.Parent).Site;

                    object[] items = Items;
                    System.Windows.Forms.TreeNode[] nodes = new System.Windows.Forms.TreeNode[items.Length];

                    tvWebNodes.Nodes.Clear();
                    for (int i = 0; i < items.Length; i++) 
                    {
                        // We need to copy the nodes into our editor TreeView, not move them.
                        // We overwrite the passed-in array with the new roots.
                        //
                        Microsoft.Web.UI.WebControls.TreeNode webnode = (Microsoft.Web.UI.WebControls.TreeNode)((ICloneable)((Microsoft.Web.UI.WebControls.TreeNode)items[i])).Clone();
                        nodes[i] = WebNodeToFormNode(webnode);
                        tvWebNodes.Nodes.Add(webnode);
                    }

                    tvWebNodes.TreeNodeTypes.Clear();
                    foreach (TreeNodeType nodeType in ((Microsoft.Web.UI.WebControls.TreeView)editCol.Parent).TreeNodeTypes)
                    {
                        tvWebNodes.TreeNodeTypes.Add((TreeNodeType)nodeType.Clone());
                    }

                    tvNodes.Nodes.Clear();
                    tvNodes.Nodes.AddRange(nodes);
                    
                    // Update current node related UI
                    //
                    if (tvNodes.Nodes.Count > 0)
                    {
                        SelectedNode = tvNodes.Nodes[0];
                    }
                    else
                    {
                        SelectedNode = null;
                    }
                    btnApply.Enabled = false;
                }
            }

            private void TvNodes_afterSelect(object sender, TreeViewEventArgs e) 
            {
                SelectedNode = e.Node;
            }

            private void TvNodes_afterLabelEdit(object sender, NodeLabelEditEventArgs e)
            {
                ((Microsoft.Web.UI.WebControls.TreeNode)e.Node.Tag).Text = e.Label;
                propGrid.Refresh();
                btnApply.Enabled = true;
            }

            private System.Windows.Forms.TreeNode WebNodeToFormNode(Microsoft.Web.UI.WebControls.TreeNode webNode)
            {
                System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
                node.Text = webNode.Text;
                node.Tag = webNode;
                node.Expand();
                for (int i = 0; i < webNode.Nodes.Count; i++)
                {
                    node.Nodes.Add(WebNodeToFormNode(webNode.Nodes[i]));
                }
                return node;
            }

            private void PropertyChanged(Object sender, PropertyValueChangedEventArgs e)
            {
                if (e.ChangedItem.Label == "Text")
                    SelectedNode.Text = (String)e.ChangedItem.Value;
                btnApply.Enabled = true;
            }

            /// <summary>
            /// Moves the selected item down one.
            /// </summary>
            /// <param name="sender">Source object</param>
            /// <param name="e">Event arguments</param>
            private void DownButton_click(object sender, EventArgs e) 
            {
                System.Windows.Forms.TreeNodeCollection col;
                Microsoft.Web.UI.WebControls.TreeNodeCollection webCol;
                System.Windows.Forms.TreeNode movingNode = SelectedNode;
                System.Windows.Forms.TreeNode nextnode = movingNode.NextNode;

                if (movingNode.Parent != null)
                {
                    col = movingNode.Parent.Nodes;
                    webCol = ((Microsoft.Web.UI.WebControls.TreeNode)((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag).Parent).Nodes;
                }
                else
                {
                    col = tvNodes.Nodes;
                    webCol = tvWebNodes.Nodes;
                }

                ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag).Remove();
                movingNode.Remove();
                col.Insert(nextnode.Index + 1, movingNode);
                webCol.AddAt(movingNode.Index, ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag));

                SelectedNode = movingNode;
                btnApply.Enabled = true;
            }

            /// <summary>
            /// Moves the selected item up one.
            /// </summary>
            /// <param name="sender">Source object</param>
            /// <param name="e">Event arguments</param>
            private void UpButton_click(object sender, EventArgs e) 
            {
                System.Windows.Forms.TreeNodeCollection col;
                Microsoft.Web.UI.WebControls.TreeNodeCollection webCol;
                System.Windows.Forms.TreeNode movingNode = SelectedNode;
                System.Windows.Forms.TreeNode prevnode = movingNode.PrevNode;

                if (movingNode.Parent != null)
                {
                    col = movingNode.Parent.Nodes;
                    webCol = ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag).GetSiblingNodeCollection();
                }
                else
                {
                    col = tvNodes.Nodes;
                    webCol = tvWebNodes.Nodes;
                }
                ((Microsoft.Web.UI.WebControls.TreeNode)(movingNode.Tag)).Remove();
                movingNode.Remove();
                col.Insert(prevnode.Index, movingNode);
                webCol.AddAt(movingNode.Index, ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag));

                SelectedNode = movingNode;
                btnApply.Enabled = true;
            }

            /// <summary>
            /// Moves the selected item one level to the left.
            /// </summary>
            /// <param name="sender">Source object</param>
            /// <param name="e">Event arguments</param>
            private void LeftButton_click(object sender, EventArgs e) 
            {
                System.Windows.Forms.TreeNodeCollection col;
                Microsoft.Web.UI.WebControls.TreeNodeCollection webCol;
                System.Windows.Forms.TreeNode movingNode = SelectedNode;
                System.Windows.Forms.TreeNode parentnode = movingNode.Parent;

                if (parentnode.Parent != null)
                {
                    col = parentnode.Parent.Nodes;
                    webCol = ((Microsoft.Web.UI.WebControls.TreeNode)((Microsoft.Web.UI.WebControls.TreeNode)parentnode.Tag).Parent).Nodes;
                }
                else
                {
                    col = tvNodes.Nodes;
                    webCol = tvWebNodes.Nodes;
                }
                ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag).Remove();
                movingNode.Remove();
                col.Insert(parentnode.Index + 1, movingNode);
                webCol.AddAt(movingNode.Index, ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag));

                SelectedNode = movingNode;
                btnApply.Enabled = true;
            }

            /// <summary>
            /// Moves the selected item one level to the right.
            /// </summary>
            /// <param name="sender">Source object</param>
            /// <param name="e">Event arguments</param>
            private void RightButton_click(object sender, EventArgs e) 
            {
                System.Windows.Forms.TreeNodeCollection col;
                Microsoft.Web.UI.WebControls.TreeNodeCollection webCol;
                System.Windows.Forms.TreeNode movingNode = SelectedNode;

                System.Windows.Forms.TreeNode prevnode = movingNode.PrevNode;
                ((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag).Remove();
                col = prevnode.Nodes;
                webCol = ((Microsoft.Web.UI.WebControls.TreeNode)prevnode.Tag).Nodes;

                movingNode.Remove();
                col.Add(movingNode);
                webCol.Add(((Microsoft.Web.UI.WebControls.TreeNode)movingNode.Tag));
                
                SelectedNode = movingNode;
                btnApply.Enabled = true;
            }
        }
    }
}


