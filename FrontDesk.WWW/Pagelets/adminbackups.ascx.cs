using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Components;
using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Data.Access;

namespace FrontDesk.Pages.Admin.Pagelets {

	/// <summary>
	///	Admin backups pagelet
	/// </summary>
	public class AdminBackups : Pagelet {

		protected System.Web.UI.WebControls.DataGrid dgBackups;

		private void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack)
				BindData();
		}

		private void BindData() {
			dgBackups.DataSource = new Backups(Globals.CurrentIdentity).GetAll();
			dgBackups.DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dgBackups.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgBackups_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgBackups_ItemDataBound(object sender, DataGridItemEventArgs e) {
			HyperLink hypName;
			if (null != (hypName = e.Item.FindControl("hypName") as HyperLink)) {
				Backup bak = e.Item.DataItem as Backup;
				hypName.Text = bak.BackedUp;
				hypName.NavigateUrl = "../" + bak.FileLocation;
			}
		}
	}
}
