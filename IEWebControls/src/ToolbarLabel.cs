//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Globalization;

    /// <summary>
    /// Represents a label within a toolbar.
    /// </summary>
    public class ToolbarLabel : ToolbarItem
    {
        /// <summary>
        /// Text to display within the label.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ItemText"),
        ]
        public string Text
        {
            get
            {
                Object obj = ViewState["Text"];
                return (obj == null) ? String.Empty : (string)obj;
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// The URL of an image to display within the label.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(""),
        Editor(typeof(Microsoft.Web.UI.WebControls.Design.ObjectImageUrlEditor), typeof(UITypeEditor)),
        PersistenceMode(PersistenceMode.Attribute),
        ResDescription("ItemImageUrl"),
        ]
        public string ImageUrl
        {
            get
            {
                Object obj = ViewState["Image"];
                return (obj == null) ? String.Empty : (string)obj;
            }

            set
            {
                ViewState["Image"] = value;
            }
        }

        /// <summary>
        /// The uplevel ToolbarTag ID for the toolbar item.
        /// </summary>
        protected override string UpLevelTag
        {
            get { return Toolbar.LabelTagName; }
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

            if (ImageUrl != String.Empty)
            {
                writer.WriteAttribute("imageUrl", ImageUrl);
            }
        }

        /// <summary>
        /// Render the item's content.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void UpLevelContent(HtmlTextWriter writer)
        {
            if (Text != String.Empty)
            {
                writer.Write(Text);
            }
            else if (ImageUrl == String.Empty)
            {
                // There is no content, so write out a non-breaking space
                writer.Write("&nbsp;");
            }
        }

        /// <summary>
        /// Renders the item for downlevel browsers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void RenderDownLevelPath(HtmlTextWriter writer)
        {
            HtmlInlineWriter inlineWriter = (writer is HtmlInlineWriter) ? (HtmlInlineWriter)writer : new HtmlInlineWriter(writer);

            if (!Enabled)
            {
                inlineWriter.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
            }

            if (ToolTip != String.Empty)
            {
                inlineWriter.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);
            }

            base.RenderDownLevelPath(inlineWriter);

            writer.WriteLine();
        }

        /// <summary>
        /// Renders the item's contents.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DownLevelContent(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

        /// <summary>
        /// Renders the item's contents.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        protected override void DesignerContent(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

        /// <summary>
        /// Renders contents for downlevel and visual designers.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        private void RenderContents(HtmlTextWriter writer)
        {
            bool renderTable = ((ImageUrl != String.Empty) && (Text != String.Empty));

            if (renderTable)
            {
                FontInfo font = ParentToolbar.Font;

                if (CurrentStyle["font"] == null)
                {
                    string[] names = font.Names;
                    if ((names.Length > 0) && (CurrentStyle["font-family"] == null))
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
                    if (!fu.IsEmpty && (CurrentStyle["font-size"] == null))
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, font.Size.ToString(CultureInfo.InvariantCulture));
                    }

                    if (font.Bold && (CurrentStyle["font-weight"] == null))
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
                    }
                    if (font.Italic && (CurrentStyle["font-style"] == null))
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.FontStyle, "italic");
                    }
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
                if ((td.Length > 0) && (CurrentStyle["text-decoration"] == null))
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextDecoration, td);

                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }

            if (ImageUrl != String.Empty)
            {
                RenderImage(writer, ImageUrl);
            }

            if (renderTable)
            {
                writer.RenderEndTag();
                writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, null);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }

            if (Text != String.Empty)
            {
                RenderText(writer, Text);
            }
            else if (ImageUrl == String.Empty)
            {
                // Neither text nor image was rendered, so render a blank
                RenderText(writer, "&nbsp;");
            }

            if (renderTable)
            {
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Renders the image tag.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        /// <param name="imageUrl">The url of the image.</param>
        protected virtual void RenderImage(HtmlTextWriter writer, string imageUrl)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders the text property.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object that receives the content.</param>
        /// <param name="text">The text to render.</param>
        protected virtual void RenderText(HtmlTextWriter writer, string text)
        {
            writer.Write(text);
        }
    }
}
