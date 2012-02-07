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
	///	Autojob status viewer
	/// </summary>
	public class AutoJobsView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgJobs;

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = false;
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private void BindData() {
			AutoJob.AutoJobList jobs = 
				new AutoJobs(Globals.CurrentIdentity).GetUserAsstJobs(GetAsstID());

			dgJobs.DataSource = jobs;
			dgJobs.DataBind();
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
			this.dgJobs.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgJobs_DeleteCommand);
			this.dgJobs.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgJobs_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["asstID"] = ap.ID;
			BindData();
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private string GetProgress(AutoJob job) {
			AutoJobTest.AutoJobTestList tests = 
				new AutoJobs(Globals.CurrentIdentity).GetTests(job.ID);
			int done=0;
			foreach (AutoJobTest test in tests)
				if (test.Status == AutoJobTest.DONE)
					done++;

			return String.Format("{0}/{1}", done, tests.Count);
		}

		private void dgJobs_ItemDataBound(object sender, DataGridItemEventArgs e) {
			
			Label lblProgress;
			if (null != (lblProgress = (Label) e.Item.FindControl("lblProgress"))) {
				lblProgress.Text = GetProgress((AutoJob)e.Item.DataItem);
			}
		}

		private void dgJobs_DeleteCommand(object source, DataGridCommandEventArgs e) {
			try {
				new AutoJobs(Globals.CurrentIdentity).Finish(
					Convert.ToInt32(dgJobs.DataKeys[e.Item.ItemIndex]), null);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}

			BindData();
			Refresh(this, new RefreshEventArgs());
		}

	}
}
