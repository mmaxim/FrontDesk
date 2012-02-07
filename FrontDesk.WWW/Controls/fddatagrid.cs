using System;
using System.Web.UI.WebControls;

namespace FrontDesk.Controls {

	/// <summary>
	/// Extended enhanced grid for common FrontDesk features
	/// </summary>
	public class FDDataGrid : DataGrid {
		
		public FDDataGrid() {
			m_delmsg = "Are you sure you wish to delete this item?";
		}

		protected string m_delmsg;
		
		protected override void OnItemDataBound(DataGridItemEventArgs e) {
			
			if(e.Item.FindControl("DeleteLink") != null) 
				((LinkButton) e.Item.FindControl("DeleteLink")).Attributes.Add("onClick", 
					@"return confirm('"+m_delmsg+@"');");

			base.OnItemDataBound (e);
		}
		
		public string DeleteMessage {
			get { return m_delmsg; }
			set { m_delmsg = value; }
		}
	}
}
