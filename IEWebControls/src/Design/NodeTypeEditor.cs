//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;
    using System.Web.UI.Design;
    using Microsoft.Web.UI.WebControls;
    using System.Windows.Forms;

    /// <summary>
    /// Provides an editor for visually selecting or editting a NodeType property.
    /// </summary>
    public class NodeTypeEditor : UITypeEditor 
    {
        /// <summary>
        /// Gets the editting style of the Edit method.
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>The style</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Edits the specified object value using the editor style provided by GetEditorStyle.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="provider">Provider</param>
        /// <param name="value">The value to edit</param>
        /// <returns>The editted value</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null) 
                {
                    object node = null;

                    if ((context.Instance is BaseChildNode) ||
                        (context.Instance is Microsoft.Web.UI.WebControls.TreeView))
                    {
                        node = context.Instance;
                    }
                    else if (context.Instance is object[])
                    {
                        object[] components = (object[])context.Instance;
                        if (components[0] is BaseChildNode)
                        {
                            node = components[0];
                        }
                    }

                    while ((node != null) && (node is BaseChildNode))
                    {
                        node = ((BaseChildNode)node).Parent;
                    }

                    if ((node != null) && (node is Microsoft.Web.UI.WebControls.TreeView))
                    {
                        TreeNodeTypeCollection nodeTypes = ((Microsoft.Web.UI.WebControls.TreeView)node).TreeNodeTypes;
                        ListBox list = new ListBox();
                        TreeNodeType origVal = null;

                        foreach (TreeNodeType nodeType in nodeTypes)
                        {
                            if ((nodeType.Type != String.Empty) && (nodeType.Type == (string)value))
                            {
                                origVal = nodeType;
                            }
                            list.Items.Add(nodeType);
                        }

                        // Set the initial SelectedIndex
                        if (origVal != null)
                        {
                            int selectedIndex = list.Items.IndexOf(origVal);
                            if (selectedIndex >= 0)
                            {
                                list.SelectedIndex = selectedIndex;
                            }
                        }

                        // Set the style
                        list.BorderStyle = BorderStyle.None;

                        // Respond to changes
                        list.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);

                        // Run the control
                        _edSvc = edSvc;
                        edSvc.DropDownControl(list);
                        _edSvc = null;

                        if ((list.SelectedItem != null) && 
                            (list.SelectedItem != origVal))
                        {
                            value = ((TreeNodeType)list.SelectedItem).Type;
                        }
                    }
                }
            }

            return value;
        }

        private IWindowsFormsEditorService _edSvc = null;

        private void OnSelectedIndexChanged(Object source, EventArgs e)
        {
            _edSvc.CloseDropDown();
        }
    }

}
