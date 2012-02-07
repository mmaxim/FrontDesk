using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FrontDesk.Components;
using FrontDesk.Data.Access;
using FrontDesk.Components.Evaluation;
using FrontDesk.Common;
using FrontDesk.Pages;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Aggregate view for the feedback system
	/// </summary>
	public class AggregateView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.LinkButton lnkLoadAll;
		protected System.Web.UI.WebControls.Image Image1;

		protected System.Web.UI.WebControls.DataGrid dgAggregate;

		private void Page_Load(object sender, System.EventArgs e) {
			
		}

		private string GetUserList() {
			return (string) ViewState["users"];
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private bool GetFileRoots(out string roots) {
			roots = "";
			if (ViewState["fileroots"] == null)
				return false;
			else {
				roots = (string) ViewState["fileroots"];
				return true;
			}
		}

		private void AddRubricColumn(string name, string bind) {
			BoundColumn col = new BoundColumn();
			col.HeaderText = name;
			col.DataField = bind;
			dgAggregate.Columns.Add(col);
		}

		private void BindFileLink() {

			string roots="";

			//Check to see if we already did this, if not then calculate sub root string for FB
			if (!GetFileRoots(out roots)) {
				Users userda = new Users(Globals.CurrentIdentity);
				User.UserList users = CourseMatrixControl.GetFeedbackUsers(GetUserList());
				foreach (User user in users) {
					Components.Submission.SubmissionList subs = userda.GetAsstSubmissions(user.UserName, GetAsstID());
					foreach (Components.Submission sub in subs) 
						roots += sub.LocationID.ToString() + "|";
				}

				ViewState["fileroots"] = roots;
			}

			lnkLoadAll.Attributes.Add("onClick", 
				@"window.open('filebrowser.aspx?Roots=" + roots +
				@"', '"+8+@"', 'width=730, height=630')");
		}

		private string GetRubricPoints(Rubric rub, int subID) {
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			Result.ResultList ress = rubda.GetResults(rub.ID, subID);
			if (ress.Count == 0 && rub.EvalID >= 0)
				return "??";
			else
				return rubda.GetPoints(rub.ID, subID).ToString();
		}

		private DataTable TabulateUsers(User.UserList users) {

			DataTable resulttab = new DataTable();
			int asstID = GetAsstID();
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			Principals prinda = new Principals(Globals.CurrentIdentity);
			Rubric asstrub = new Assignments(Globals.CurrentIdentity).GetRubric(asstID);

			//Add rubric columns to data grid
			Rubric.RubricList flatrub = rubda.Flatten(asstrub);
			resulttab.Columns.Add("UserName");
			resulttab.Columns.Add("Status");
			resulttab.Columns.Add("Total");
			foreach (Rubric rub in flatrub) {
				AddRubricColumn(rub.Name, rub.Name);
				resulttab.Columns.Add(rub.Name);
			}

			//Add user data to the datatable
			foreach (User user in users) {
				Components.Submission sub = prinda.GetLatestSubmission(user.PrincipalID, asstID);
				DataRow row = resulttab.NewRow();

				if (sub == null) continue;

				row["UserName"] = user.UserName;
				row["Status"] = sub.Status;
				row["Total"] = rubda.GetPoints(asstrub.ID, sub.ID).ToString() + "/" + asstrub.Points.ToString();
				foreach (Rubric rub in flatrub) 
					row[rub.Name] = GetRubricPoints(rub, sub.ID) + "/" + rub.Points.ToString();

				resulttab.Rows.Add(row);
			}

			return resulttab;
		}

		private void BindData() {

			string userstr = GetUserList();
			User.UserList users = CourseMatrixControl.GetFeedbackUsers(userstr);
		
			DataTable userset = TabulateUsers(users);

			dgAggregate.DataSource = new DataView(userset);
			dgAggregate.DataBind();
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
			this.lnkLoadAll.Click += new System.EventHandler(this.lnkLoadAll_Click);
			this.dgAggregate.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgAggregate_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		public event FrontDesk.Controls.Matrix.RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["users"] = ap.Auxiliary;
			ViewState["asstID"] = ap.ID;

			BindData();
			BindFileLink();
		}

		private void dgAggregate_ItemDataBound(object sender, DataGridItemEventArgs e) {
			System.Web.UI.WebControls.Image imgStatus;
			if (null != (imgStatus = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgStatus"))) {
				DataRowView row = (DataRowView) e.Item.DataItem;
				imgStatus.ImageUrl = (Convert.ToInt32((string)row["Status"]) == Components.Submission.GRADED)
					? "../../attributes/subgrade.gif" 
					: "../../attributes/sub.gif";
			}
		}

		private void lnkLoadAll_Click(object sender, System.EventArgs e) {
			BindData();
			BindFileLink();
		}

	}
}
