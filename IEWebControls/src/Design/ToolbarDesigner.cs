//------------------------------------------------------------------------------
// Copyright (c) 2000-2001 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls.Design
{
    using System;
    using System.Web.UI.Design;
    using System.Drawing.Design;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using Microsoft.Web.UI.WebControls;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Designer class for Microsoft.Web.UI.WebControls.Toolbar
    /// </summary>
    public class ToolbarDesigner : ControlDesigner
    {
        private DesignerVerbCollection _Verbs;

        /// <summary>
        /// Gets or sets a value indicating whether or not the control can be resized.
        /// </summary>
        public override bool AllowResize
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the design time verbs supported by the component associated
        /// with the designer.
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_Verbs == null)
                {
                    string addButton = DesignUtil.GetStringResource("ToolbarAddButton");

                    _Verbs = new DesignerVerbCollection(
                        new DesignerVerb[]
                        {
                            new DesignerVerb(addButton, new EventHandler(OnAddButton)),
                        });
                }

                return _Verbs;
            }
        }

        /// <summary>
        /// Called when the Add Button menu item is clicked.
        /// </summary>
        /// <param name="sender">The source object</param>
        /// <param name="e">Event arguments</param>
        private void OnAddButton(object sender, EventArgs e)
        {
            Toolbar tbar = (Toolbar)Component;
            PropertyDescriptor itemsDesc = DesignUtil.GetPropertyDescriptor(tbar, "Items");
            if (itemsDesc != null)
            {
                // Tell the designer that we're changing the property
                RaiseComponentChanging(itemsDesc);

                // Do the change
                ToolbarButton btn = new ToolbarButton();
                btn.Text = "Button";
                tbar.Items.Add(btn);

                // Tell the designer that we've changed the property
                RaiseComponentChanged(itemsDesc, null, null);
                UpdateDesignTimeHtml();
            }
        }

        /// <summary>
        /// Retrieves the HTML to display in the designer.
        /// </summary>
        /// <returns>The design-time HTML.</returns>
        public override string GetDesignTimeHtml()
        {
            string html;
            Toolbar tbar = (Toolbar)Component;

            // If the toolbar is empty, then add a label with instructions
            if (tbar.Items.Count == 0)
            {
                return CreatePlaceHolderDesignTimeHtml(DesignUtil.GetStringResource("ToolbarNoItems"));
            }

            bool madeBlock = false;
            Unit oldUnit = Unit.Empty;
            object obj = Behavior.GetStyleAttribute("position", false, true);
            bool notAbsolute = true;
            if ((obj != null) && (obj is string))
            {
                notAbsolute = (String.Compare((string)obj, "absolute", true) != 0);
            }
            if ((tbar.Width == Unit.Empty) && notAbsolute)
            {
                madeBlock = true;
                oldUnit = tbar.Width;
                tbar.Width = Unit.Percentage(100.0);
            }

            try
            {
                html = base.GetDesignTimeHtml();
            }
            finally
            {
                if (madeBlock)
                {
                    tbar.Width = oldUnit;
                }
            }

            return html;
        }
    }
}
