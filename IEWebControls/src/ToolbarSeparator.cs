//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Represents a separator.
    /// </summary>
    public class ToolbarSeparator : ToolbarItem
    {
        /// <summary>
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected override string UpLevelTag
        {
            get { return Toolbar.SeparatorTagName; }
        }

        /// <summary>
        /// Renders a pipe character.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DownLevelContent(HtmlTextWriter writer)
        {
            if (Orientation == Orientation.Horizontal)
            {
                writer.Write("<span disabled=\"true\">|</span>");
            }
            else
            {
                writer.Write("<hr>");
            }
        }

        /// <summary>
        /// Renders a vertical etched span.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DesignerContent(HtmlTextWriter writer)
        {
            if (Orientation == Orientation.Horizontal)
            {
                writer.Write("<span style=\"border-left-style:solid;border-right-style:solid;border-left-width:1px;border-right-width:1px;border-color:#999999;border-right-color:#ffffff;width:2px;overflow:hidden;margin-left:2px;margin-right:2px\">&nbsp;</span>");
            }
            else
            {
                writer.Write("<hr>");
            }
        }
    }
}
