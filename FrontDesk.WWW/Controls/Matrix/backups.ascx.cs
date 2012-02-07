using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	BAckups Viwe
	/// </summary>
	public class BackupsView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.DataGrid dgBackups;

		private void Page_Load(object sender, EventArgs e) {
			
		}

		private int GetCourseID() {
			return (int) ViewState["courseID"];
		}

		public void BindData() {
			dgBackups.DataSource = 
				new Courses(Globals.CurrentIdentity).GetBackups(GetCourseID());
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

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["courseID"] = ap.ID;
			BindData();
		}

		private void dgBackups_ItemDataBound(object sender, DataGridItemEventArgs e) {
			HyperLink hypName;
			if (null != (hypName = e.Item.FindControl("hypName") as HyperLink)) {
				Backup bak = e.Item.DataItem as Backup;
				hypName.Text = bak.BackedUp;
				hypName.NavigateUrl = "../../" + bak.FileLocation;
			}
		}

	}
}
