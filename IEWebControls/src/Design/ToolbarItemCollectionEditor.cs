//------------------------------------------------------------------------------
// Copyright (c) 2000-2003 Microsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Microsoft.Web.UI.WebControls.Design
{
    using System;
    using System.ComponentModel.Design;

    /// <summary>
    /// Designer editor for the ToolbarItem collection.
    /// </summary>
    public class ToolbarItemCollectionEditor : ItemCollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the ToolbarItemCollectionEditor class.
        /// </summary>
        /// <param name="type">The type of collection this object is to edit.</param>
        public ToolbarItemCollectionEditor(Type type) : base(type)
        {
        }

        /// <summary>
        /// Returns the type of objects the editor can create.
        /// </summary>
        /// <returns>An array of types.</returns>
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
            {
                typeof(ToolbarButton),
                typeof(ToolbarCheckButton),
                typeof(ToolbarCheckGroup),
                typeof(ToolbarLabel),
                typeof(ToolbarSeparator),
                typeof(ToolbarTextBox),
                typeof(ToolbarDropDownList),
            };
        }
    }
}
