using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Data.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Components.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Controls.Filesys;
using FrontDesk.Controls;
using FrontDesk.Controls.Submission;
using FrontDesk.Common;
using FrontDesk.Tools.Export;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Group course report
	/// </summary>
	public class GroupCourseReport : GroupReport, IMatrixControl {
		protected System.Web.UI.WebControls.DataGrid dgCourse;
		protected System.Web.UI.WebControls.DropDownList ddlExportFormat;
		protected System.Web.UI.HtmlControls.HtmlInputFile ufUserList;
		protected CustomButton.ClickOnceButton cmdExport;
		protected System.Web.UI.WebControls.Label lblExportError;
		protected System.Web.UI.WebControls.LinkButton lnkExport;
		protected System.Web.UI.WebControls.DataGrid dgReport;

		private void Page_Load(object sender, System.EventArgs e) {
			lblExportError.Visible = false;
		}

		private string GetUsername() {
			return (string) ViewState["username"];
		}

		private int GetSectionID() {
			return (int) ViewState["sectionID"];
		}

		private int GetCourseID() {
			return (int) ViewState["courseID"];
		}

		private void PageExportError(string msg) {
			lblExportError.Text = msg;
			lblExportError.Visible = true;
		}

		private void BindData() {
			
			User.UserList users = GetUserSet(GetSectionID(), GetCourseID());
			
			dgCourse.DataSource = users;
			dgCourse.DataBind();

			BindExporters();
		}

		private void BindDetailsData(string username) {

			ViewState["username"] = username;

			dgReport.DataSource = 
				new Courses(Globals.CurrentIdentity).GetAssignments(GetCourseID());
			dgReport.DataBind();
		}

		private void BindExporters() {
			string[] exporters = ExporterFactory.GetInstance().ListKeys();

			ddlExportFormat.Items.Clear();
			foreach (string exporter in exporters)
				ddlExportFormat.Items.Add(new ListItem(exporter, exporter));
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
			this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
			this.dgCourse.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgCourse_ItemCommand);
			this.dgCourse.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgCourse_PageIndexChanged);
			this.dgCourse.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCourse_ItemDataBound);
			this.dgReport.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgReport_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["sectionID"] = ap.ID;
			ViewState["courseID"] = Convert.ToInt32(ap.Auxiliary);

			BindData();
		}

		private void dgCourse_ItemDataBound(object sender, DataGridItemEventArgs e) {
			
			Label lblStudent, lblPoints;
			
			if (null != (lblStudent = (Label) e.Item.FindControl("lblStudent"))) {
				lblPoints = (Label) e.Item.FindControl("lblPoints");

				User user = e.Item.DataItem as User;

				int courseID = GetCourseID();
				double total = new Courses(Globals.CurrentIdentity).GetTotalPoints(courseID);
				double userp = 
					new Users(Globals.CurrentIdentity).GetCoursePoints(user.UserName, courseID);
				
				lblPoints.Text = String.Format("{0} / {1} ({2}%)",
					userp, total, Math.Round((userp/total)*100.0, 2));

				lblStudent.Text = user.FullName + " (" + user.UserName + ")";
			}
		}

		private void dgCourse_ItemCommand(object source, DataGridCommandEventArgs e) {
			if (e.CommandName == "Details") {
				string username = (string) dgCourse.DataKeys[e.Item.ItemIndex];
				BindDetailsData(username);
			}
		}

		private void dgCourse_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgCourse.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblPoints;
			if (null != (lblPoints = (Label) e.Item.FindControl("lblPoints"))) {
				
				int asstID = Convert.ToInt32(dgReport.DataKeys[e.Item.ItemIndex]);
				User user = new Users(Globals.CurrentIdentity).GetInfo(GetUsername(), null);

				lblPoints.Text = GetAsstPoints(user, asstID);
			}
		}

		private void cmdExport_Click(object sender, System.EventArgs e) {
			
			CFile export;
			ExportRow.ExportRowList rows = new ExportRow.ExportRowList();
			
			int courseID = GetCourseID();
			Users userda = new Users(Globals.CurrentIdentity);
			User.UserList users = GetUserSet(GetSectionID(), GetCourseID());
			
			//Add rows
			rows.Add(userda.GetCourseExportHeading(courseID));
			foreach (User user in users) 
				rows.Add(userda.GetCourseExport(user.UserName, courseID));	
	
			//Do export
			try {
				IExporter exporter = 
					(IExporter) ExporterFactory.GetInstance().CreateItem(ddlExportFormat.SelectedItem.Value);
				export = exporter.Export(new FileSystem(Globals.CurrentIdentity), rows);
			} catch (CustomException er) {
				PageExportError(er.Message);
				return;
			}
			
			//Setup d/l link
			lnkExport.Attributes.Clear();
			lnkExport.Attributes.Add("onClick", 
				@"window.open('Controls/Filesys/dlfile.aspx?FileID=" + export.ID + 
				@"', '"+export.ID+@"', 'width=770, height=580')");
			lnkExport.Visible = true;
		}

	}
}
