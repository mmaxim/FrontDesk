using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;

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
	///	Group assignment report
	/// </summary>
	public class GroupAsstReportView : GroupReport, IMatrixControl {

		protected System.Web.UI.WebControls.DataGrid dgReport;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spnNothing;
		protected System.Web.UI.WebControls.Label lblDetailsName;
		protected RubricViewControl ucRubric;

		private void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack) {
				spnNothing.Visible = true;
				ucRubric.Visible = false;
			}
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private int GetSectionID() {
			return (int) ViewState["sectionID"];
		}

		private string GetGroups() {
			return (string) ViewState["Groups"];
		}

		private void BindGroups() {
			int courseID = new Assignments(Globals.CurrentIdentity).GetInfo(GetAsstID()).CourseID;
			int sectionID = GetSectionID();

			//Load the groups into viewstate
			User.UserList users = GetUserSet(sectionID, courseID);
			ArrayList groups = new ArrayList();
			new UserGrouper().Group(users, groups, 8);
			string vsrecord="";
			foreach (UserGrouper.UserGroup group in groups) 
				vsrecord += group.LowerBound + " " + group.UpperBound + "\n";
			ViewState["Groups"] = vsrecord;

			dgReport.VirtualItemCount = groups.Count*8;
			dgReport.CurrentPageIndex = 0;
		}

		public void BindData() {

			int courseID = new Assignments(Globals.CurrentIdentity).GetInfo(GetAsstID()).CourseID;
			int sectionID = GetSectionID();

			//Load the groups into viewstate
			User.UserList users = GetUserSet(sectionID, courseID);
			string[] bounds = GetGroups().Split(
					"\n".ToCharArray())[dgReport.CurrentPageIndex].Split(" ".ToCharArray());
			new UserGrouper().Regroup(bounds[1], bounds[0], users);

			dgReport.DataSource = users;
			dgReport.DataBind();
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
			this.dgReport.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgReport_ItemCreated);
			this.dgReport.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgReport_ItemCommand);
			this.dgReport.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgReport_PageIndexChanged);
			this.dgReport.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgReport_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
		
			dgReport.CurrentPageIndex = 0;
			ViewState["asstID"] = Convert.ToInt32(ap.Auxiliary);
			ViewState["sectionID"] = ap.ID;
			spnNothing.Visible = true;
			ucRubric.Visible = false;
			lblDetailsName.Text = "";

			BindGroups();
			BindData();
		}

		private void dgReport_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblStudent, lblPoints;
			LinkButton lnkDetails;
			System.Web.UI.WebControls.Image imgSub;

			if (null != (lblStudent = (Label) e.Item.FindControl("lblStudent"))) {
				lblPoints = (Label) e.Item.FindControl("lblPoints");
				imgSub = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgSub");
				lnkDetails = (LinkButton) e.Item.FindControl("lnkDetails");

				User user = e.Item.DataItem as User;

				lblPoints.Text = GetAsstPoints(user, GetAsstID());
				lblStudent.Text = user.FullName + " (" + user.UserName + ")";
				imgSub.ImageUrl = GetSubImage(user);
				lnkDetails.Enabled	= (imgSub.ImageUrl != "../../attributes/nosub.gif");
			}		
		}

		private Components.Submission GetLatestSub(int principalID) {
			Components.Submission gsub, rsub;

			gsub = new Principals(Globals.CurrentIdentity).GetLatestGradedSubmission(
				principalID, GetAsstID());
			if (gsub != null) return gsub;

			rsub = new Principals(Globals.CurrentIdentity).GetLatestSubmission(
				principalID, GetAsstID());
			return rsub;
		}

		private string GetSubImage(User user) {

			Components.Submission gsub = 
				new Principals(Globals.CurrentIdentity).GetLatestGradedSubmission(
				user.PrincipalID, GetAsstID());
			Components.Submission rsub =
				new Principals(Globals.CurrentIdentity).GetLatestSubmission(
				user.PrincipalID, GetAsstID());

			if (gsub != null)
				return "../../attributes/subgrade.gif";
			else if (rsub != null)
				return "../../attributes/sub.gif";
			else
				return "../../attributes/nosub.gif";
		}

		private void dgReport_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgReport.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}

		private void dgReport_ItemCommand(object source, DataGridCommandEventArgs e) {	
			if (e.CommandName == "Details") {
				int pid = (int) dgReport.DataKeys[e.Item.ItemIndex];
				Rubric rub = new Assignments(Globals.CurrentIdentity).GetRubric(GetAsstID());

				Components.Submission sub = GetLatestSub(pid);
				if (sub != null) {
					ucRubric.Visible = true;
					spnNothing.Visible = false;
					lblDetailsName.Text = "for " + new Users(Globals.CurrentIdentity).GetInfo(
						new Principals(Globals.CurrentIdentity).GetInfo(pid).Name, null).FullName;
					ucRubric.InitRubric(rub, sub.ID, "");
				}
			}
		}

		private void dgReport_ItemCreated(object sender, DataGridItemEventArgs e) {
			//Take the pager over
			if (e.Item.ItemType == ListItemType.Pager) {
				TableCell pager = e.Item.Cells[0];
				string[] groups = ((string)ViewState["Groups"]).Split("\n".ToCharArray());

				//Change the names of the numbers to group descs
				foreach (Control c in pager.Controls) {
					if (c is LinkButton) {
						LinkButton lc = (LinkButton) c;
						if (lc.Text != "...") {
							string[] bounds = groups[Convert.ToInt32(lc.Text)-1].Split(" ".ToCharArray());
							lc.Text = bounds[1].ToUpper() + "-" + bounds[0].ToUpper();
						}
					} else if (c is Label) {
						Label lc = (Label) c;
						if (lc.Text != "...") {
							string[] bounds = groups[Convert.ToInt32(lc.Text)-1].Split(" ".ToCharArray());
							lc.Text = bounds[1].ToUpper() + "-" + bounds[0].ToUpper();
						}
					}
				}
			}
		}
	}
}
