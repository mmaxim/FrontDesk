using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Pages;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Components.Filesys;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Controls.Filesys;
using FrontDesk.Controls;
using FrontDesk.Controls.Submission;
using FrontDesk.Common;

namespace FrontDesk.Controls.Matrix {
	
	/// <summary>
	///	User course report
	/// </summary>
	public class UserCourseReportView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.TextBox txtFirst;
		protected System.Web.UI.WebControls.TextBox txtLast;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.DataGrid dgReport;
		protected System.Web.UI.WebControls.Label lblTotal;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DropDownList ddlRoles;
		protected System.Web.UI.WebControls.Button cmdUpdate;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private int GetCourseID() {
			return (int) ViewState["courseID"];
		}

		private string GetUsername() {
			return (string) ViewState["username"];
		}

		private void BindData() {
			
			Users userda = new Users(Globals.CurrentIdentity);
			Courses courseda = new Courses(Globals.CurrentIdentity);
			int courseID = GetCourseID();

			User user = userda.GetInfo(GetUsername(), null);
			txtFirst.Text = user.FirstName; txtLast.Text = user.LastName;
			txtEmail.Text = user.Email;

			double total = courseda.GetTotalPoints(courseID);
			double userp = userda.GetCoursePoints(user.UserName, courseID);
			lblTotal.Text = String.Format("{0} / {1} ({2}%)",
				userp, total, Math.Round((userp/total)*100.0, 2));

			dgReport.DataSource = courseda.GetAssignments(courseID);
			dgReport.DataBind();

			CourseRole.CourseRoleList roles = courseda.GetRoles(courseID, null);
			CourseRole urole = courseda.GetRole(user.UserName, courseID, null);
			ddlRoles.Items.Clear();
			foreach (CourseRole role in roles) {
				ListItem item = new ListItem(role.Name, role.PrincipalID.ToString());
				if (role.PrincipalID == urole.PrincipalID)
					item.Selected = true;
				ddlRoles.Items.Add(item);
			}
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
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			this.dgReport.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgReport_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event FrontDesk.Controls.Matrix.RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["courseID"] = ap.ID;
			ViewState["username"] = ap.Auxiliary;

			BindData();
		}

		private void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e) {
			
			Label lblPoints;
			if (null != (lblPoints = (Label) e.Item.FindControl("lblPoints"))) {
				
				int asstID = Convert.ToInt32(dgReport.DataKeys[e.Item.ItemIndex]);
				User user = new Users(Globals.CurrentIdentity).GetInfo(GetUsername(), null);
				Components.Submission latsub = 
					new Principals(Globals.CurrentIdentity).GetLatestGradedSubmission(user.PrincipalID, asstID);
				double total = new Assignments(Globals.CurrentIdentity).GetRubric(asstID).Points;

				if (latsub != null) {	
					double userp = new Users(Globals.CurrentIdentity).GetAsstPoints(user.UserName, asstID);
					lblPoints.Text = 
						String.Format("{0} / {1} ({2}%)", userp, total, Math.Round((userp/total)*100.0, 2));
				} else {
					lblPoints.Text = 
						String.Format("?? / {0}", total);
				}
			}
		}

		private void cmdUpdate_Click(object sender, System.EventArgs e) {
		
			User user = new User();
			
			user.UserName = GetUsername(); user.FirstName = txtFirst.Text;
			user.LastName = txtLast.Text; user.Email = txtEmail.Text;
			string role = ddlRoles.SelectedItem.Text;
			try {
				new Users(Globals.CurrentIdentity).Update(user, null);
				new Courses(Globals.CurrentIdentity).UpdateRole(user.UserName, GetCourseID(), role, null);
			} catch (CustomException er) {
				PageError(er.Message);
			}

			BindData();
			if (Refresh != null)
				Refresh(this, new RefreshEventArgs());
		}

	}
}
