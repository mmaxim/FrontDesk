//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls.Design
{
    using System;
    using System.Web.UI.Design;
    using System.Drawing.Design;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using Microsoft.Web.UI.WebControls;


    /// <summary>
    /// Designer for a Microsoft.Web.UI.WebControls.TreeView
    /// </summary>
    public class TreeViewDesigner : ControlDesigner
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the control can be resized.
        /// </summary>
        public override bool AllowResize
        {
            get { return true; }
        }

        /// <summary>
        /// Retrieves the HTML to display in the designer.
        /// </summary>
        /// <returns>The design-time HTML.</returns>
        public override string GetDesignTimeHtml()
        {
            TreeView tv = (TreeView)Component;

            // If the TreeView is empty, then show instructions
            if (tv.Nodes.Count == 0)
            {
                return CreatePlaceHolderDesignTimeHtml(DesignUtil.GetStringResource("TreePlaceHolder"));
            }

            return base.GetDesignTimeHtml();
        }
    }
}

