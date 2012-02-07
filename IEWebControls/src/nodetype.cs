//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// TreeNodeType class
    /// </summary>
    [
    ParseChildren(true),
    ToolboxItem(false),
    ]
    public class TreeNodeType : TreeBase
    {
        /// <summary>
        /// Returns the string representation of the TreeNodeType.
        /// </summary>
        /// <returns>The string representation of the TreeNodeType.</returns>
        public override string ToString()
        {
            if (Type != String.Empty)
            {
                return Type;
            }

            return base.ToString();
        }

        /// <summary>
        /// Indicates how a tree node of this type should handle expanding.
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
                return ((b == null) ? ExpandableValue.Auto : (ExpandableValue)b);
            }
            set 
            {
                ViewState["Expandable"] = value;
            }
        }

        /// <summary>
        /// The name of this TreeNodeType.
        /// </summary>
        [
        Category("Data"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("TreeNodeTypeName"),
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
        /// The URL that TreeNodes of this type should navigate to when clicked.
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
        /// Render the uplevel version of the TreeNodeType.
        /// </summary>
        /// <param name="output">The HtmlTextWriter that will receive markup.</param>
        protected override void RenderUpLevelPath(HtmlTextWriter output)
        {
            if (Type != String.Empty)
                output.AddAttribute("Type", Type);

            if (NavigateUrl != String.Empty)
                output.AddAttribute("NavigateUrl", NavigateUrl);

            if (Expandable == ExpandableValue.Always)
                output.AddAttribute("Expandable", "always");
            else if (Expandable == ExpandableValue.CheckOnce)
                output.AddAttribute("Expandable", "checkOnce");
         
            AddAttributesToRender(output);

            output.RenderBeginTag("tvns:treenodetype");

            base.RenderUpLevelPath(output);

            output.RenderEndTag();             
        }

        /// <summary>
        /// TreeNodeTypes don't render downlevel.
        /// </summary>
        /// <param name="output">Not used.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter output)
        {
        }

        /// <summary>
        /// TreeNodeTypes don't render in the designer.
        /// </summary>
        /// <param name="output">Not used.</param>
        protected override void RenderDesignerPath(HtmlTextWriter output)
        {
        }

    }
}
