using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI.WebControls;
using System.IO;

using FrontDesk.Pages;
using FrontDesk.Data.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;
using FrontDesk.Components;
using FrontDesk.Controls.Filesys;
using FrontDesk.Controls;
using FrontDesk.Controls.Submission;
using FrontDesk.Common;

namespace FrontDesk.Controls.Matrix {

	/// <summary>
	///	Submission system pagelet
	/// </summary>
	public class SubmissionSystemView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.Image imgType;
		protected System.Web.UI.WebControls.Label lblSubName;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtNewTitle;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtNewDescription;
		protected System.Web.UI.WebControls.LinkButton DeleteLink;
		protected System.Web.UI.WebControls.DataGrid dgSubmissions;
		protected System.Web.UI.WebControls.ListBox lstPrincipal;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.LinkButton lnkFiles;
		protected System.Web.UI.WebControls.Label lblSuccess;
		protected System.Web.UI.WebControls.ListBox lstSubSys;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;

		public event RefreshEventHandler Refresh;

		private void Page_Load(object sender, System.EventArgs e) {
			lblSuccess.Visible = false;
			if (ViewState["asstID"] != null) {
				BindSubmitAs();
				SetupBrowser(GetAsstID());
				LoadSystems();
				if (!IsPostBack)
					BindSubSys();
			}
		}

		private void BindSubSys() {
			mpViews.SelectedIndex = 0;
			
			lstSubSys.DataSource = LoadSystems();
			lstSubSys.DataBind();
			lstSubSys.SelectedIndex = 0;
		}

		private ArrayList LoadSystems() {
			ArrayList syss = new ArrayList();

			//Attach IDs to controls
			int pid = Convert.ToInt32(lstPrincipal.SelectedItem.Value);
			int asstID = GetAsstID();
			foreach (Control c in mpViews.Controls) {
				ISubmissionSystemControl cson = (ISubmissionSystemControl) c.Controls[1];
				cson.AssignmentID = asstID;
				cson.SubmitterPrincipalID = pid;
				cson.SubmissionProcessed += new SubmissionEventHandler(csubsys_SubmissionProcessed);
				cson.FinishInit();
				syss.Add(cson.Name);
			}

			return syss;
		}

		private void PageError(string msg) {
			lblSuccess.Text = msg;
			lblSuccess.Visible = true;
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private void SetupBrowser(int asstID) {

			FileSystem fs = new FileSystem(Globals.CurrentIdentity);

			Components.Submission.SubmissionList subdata = 
				new Users(Globals.CurrentIdentity).GetAsstSubmissions(Globals.CurrentUserName, asstID);

			string roots="";
			foreach (Components.Submission sub in subdata) 
				roots += sub.LocationID + "|";

			lnkFiles.Attributes.Clear();
			lnkFiles.Attributes.Add("onClick", 
				@"window.open('filebrowser.aspx?Roots=" + roots +
				@"', '"+8+@"', 'width=730, height=630')");
		}

		private void csubsys_SubmissionProcessed(object sender, SubmissionEventArgs args) {
			int asstID = GetAsstID();

			BindSubGrid();	
			SetupBrowser(asstID);

			PageError("Submission Success! Scroll down to confirm the submission time " +
				"and the files you submitted. <b>If</b> the course staff has set up on-submission testing " + 
				"you will receive the results of those tests shortly through e-mail.<br>");
			Refresh(this, new RefreshEventArgs());
		}

		private void BindSubmitAs() {
			
			int pid = Convert.ToInt32(lstPrincipal.SelectedItem.Value);
			Principal principal = (new Principals(Globals.CurrentIdentity)).GetInfo(pid);

			if (principal.Type == Principal.USER)
				imgType.ImageUrl = "../../attributes/user.jpg";
			else
				imgType.ImageUrl = "../../attributes/group.jpg";

			lblSubName.Text = principal.Name;
		}

		private void BindSubGrid() {
			int asstID = GetAsstID();
			Components.Submission.SubmissionList subs =
				(new Users(Globals.CurrentIdentity)).GetAsstSubmissions(Globals.CurrentUserName, asstID);

			dgSubmissions.DataSource = subs;
			dgSubmissions.DataBind();
		}

		private void BindPListData() {
			
			int asstID = GetAsstID();
			Principal.PrincipalList plist = (new Users(Globals.CurrentIdentity)).GetPrincipals(
				Globals.CurrentUserName, asstID);

			lstPrincipal.DataSource = plist;
			lstPrincipal.DataBind();
			lstPrincipal.SelectedIndex = 0;
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
			this.lstSubSys.SelectedIndexChanged += new System.EventHandler(this.lstSubSys_SelectedIndexChanged);
			this.dgSubmissions.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgSubmissions_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgSubmissions_ItemDataBound(object sender, DataGridItemEventArgs e) {
			
			Label lblLate, lblDueDate;
			System.Web.UI.WebControls.Image imgSubber;
			if (null != (lblLate = e.Item.FindControl("lblLate") as Label)) {
				Components.Submission sub = e.Item.DataItem as Components.Submission;
				Assignment asst = (new Assignments(Globals.CurrentIdentity)).GetInfo(sub.AsstID);

				if (asst.DueDate < sub.Creation) 
					lblLate.Text = "<b><font color=\"#ff0000\">LATE</font></b>";
				else
					lblLate.Text = "<b><font color=\"#4768A3\">ON TIME</font></b>";

				lblDueDate = (Label) e.Item.FindControl("lblDueDate");
				lblDueDate.Text = asst.DueDate.ToString();

				imgSubber = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgSubber");
				int pid = Convert.ToInt32(dgSubmissions.DataKeys[e.Item.ItemIndex]);
				Principal prin = (new Principals(Globals.CurrentIdentity)).GetInfo(pid);

				if (prin.Type == Principal.USER)
					imgSubber.ImageUrl = "../../attributes/user.jpg";
				else
					imgSubber.ImageUrl = "../../attributes/group.jpg";
			}
		}

		public void Activate(ActivateParams ap) {
			ViewState["asstID"] = ap.ID;
			BindPListData();	
			BindSubmitAs();
			BindSubGrid();
			SetupBrowser(ap.ID);
			BindSubSys();
		}

		private void lstSubSys_SelectedIndexChanged(object sender, System.EventArgs e) {
			mpViews.SelectedIndex = lstSubSys.SelectedIndex;
		}
	}
}
