using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Pages;

namespace FrontDesk.Pages.Pagelets {
	

	/// <summary>
	///	Auto job status pagelet
	/// </summary>
	public class AutoJobStatusPagelet : Pagelet {

		protected System.Web.UI.WebControls.DataGrid dgActive;
		protected System.Web.UI.WebControls.TextBox txtDetails;
		protected System.Web.UI.WebControls.Label lblDetails;
		protected System.Web.UI.WebControls.DataGrid dgQueued;

		private void Page_Load(object sender, System.EventArgs e) {
		//	if (!IsPostBack)
		//		BindData();
		}

		private void BindData() {

			AutoJobs acjobs = new AutoJobs(Globals.CurrentIdentity);

			AutoJob.AutoJobList jobs = acjobs.GetAll();
			
			dgActive.DataSource = jobs;
			dgActive.DataBind();

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
			this.dgActive.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgActive_ItemDataBound);
			this.dgQueued.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgQueued_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgActive_ItemDataBound(object sender, DataGridItemEventArgs e) {
		
		//	Label lblEvaluation, lblCourse;
		/*	if (null != (lblEvaluation = (Label) e.Item.FindControl("lblEvaluation"))) {
				
				lblCourse = (Label) e.Item.FindControl("lblCourse");
				
				AutoJob aj = (AutoJob) e.Item.DataItem;
				lblEvaluation.Text = aj.AutoEval.Name;

				Course course = (new Courses(Globals.CurrentIdentity)).GetInfo(aj.AutoEval.CourseID);
				lblCourse.Text = course.Number;
			}*/
		}

		private void dgQueued_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
		//	Label lblEvaluation, lblCourse;
		/*	if (null != (lblEvaluation = (Label) e.Item.FindControl("lblEvaluation"))) {
				
				lblCourse = (Label) e.Item.FindControl("lblCourse");
				
				AutoJob aj = (AutoJob) e.Item.DataItem;
				lblEvaluation.Text = aj.AutoEval.Name;

				Course course = (new Courses(Globals.CurrentIdentity)).GetInfo(aj.AutoEval.CourseID);
				lblCourse.Text = course.Number;
			}*/
		}
	}
}
