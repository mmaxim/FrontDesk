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
	///	Automatic job tests view
	/// </summary>
	public class AutoJobTestsView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.DataGrid dgTests;

		private void Page_Load(object sender, EventArgs e) {
			
		}

		private int GetJobID() {
			return (int) ViewState["jobID"];
		}

		private void BindData() {
			
			AutoJobTest.AutoJobTestList tests = 
				new AutoJobs(Globals.CurrentIdentity).GetTests(GetJobID());

			dgTests.DataSource = tests;
			dgTests.DataBind();
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
			this.dgTests.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgTests_PageIndexChanged);
			this.dgTests.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgTests_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["jobID"] = ap.ID;
			BindData();
		}

		private void dgTests_ItemDataBound(object sender, DataGridItemEventArgs e) {
		
			Label lblSub, lblEval, lblQueue;
			System.Web.UI.WebControls.Image imgStatus;
			if (null != (lblSub = (Label) e.Item.FindControl("lblSub"))) {
				lblEval = (Label) e.Item.FindControl("lblEval");
				lblQueue = (Label) e.Item.FindControl("lblQueue");
				imgStatus = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgStatus");

				AutoJobTest test = (AutoJobTest) e.Item.DataItem;
				lblEval.Text = test.AutoEval.Name;

				Components.Submission sub = 
					new Submissions(Globals.CurrentIdentity).GetInfo(test.SubmissionID);
				lblSub.Text = new FileSystem(Globals.CurrentIdentity).GetFile(sub.LocationID).Alias;

				if (test.Status == AutoJobTest.DONE)
					imgStatus.ImageUrl = "../../attributes/subgrade.gif";
				else
					imgStatus.ImageUrl = "../../attributes/sub.gif";

				AutoJobs autojobda = new AutoJobs(Globals.CurrentIdentity);
				lblQueue.Text = String.Format("{0} out of {1}", 
					autojobda.GetQueuePosition(test), autojobda.GetAllTests().Count);
			}
		}

		private void dgTests_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgTests.CurrentPageIndex = e.NewPageIndex;
			BindData();
		}
	}
}
