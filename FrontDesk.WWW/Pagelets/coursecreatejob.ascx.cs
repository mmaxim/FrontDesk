using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;

using FrontDesk.Controls;
using FrontDesk.Data.Access;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Common;

namespace FrontDesk.Pages.Pagelets {
	
	/// <summary>
	///	Pagelet for creating grading jobs
	/// </summary>
	public class CourseCreateJob : UserControl {
		protected System.Web.UI.WebControls.Button cmdSubmit;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.ListBox lstTests;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.ListBox lstOrder;
		protected SectionExplorer ucSectionExpl;

		private void Page_Load(object sender, System.EventArgs e) {

			int asstID = Convert.ToInt32(HttpContext.Current.Request.Params["AsstID"]);

			ucSectionExpl.Height = 402;
			ucSectionExpl.Width = 335;

			ucSectionExpl.ShowChecks = true;
			ucSectionExpl.RestrictAsst = asstID;

			lblError.Visible = false;
			if (!IsPostBack)
				BindData();
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
			this.lstTests.SelectedIndexChanged += new System.EventHandler(this.lstTests_SelectedIndexChanged);
			this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void BindData() {
			int asstID = Convert.ToInt32(HttpContext.Current.Request.Params["AsstID"]);
			Evaluation.EvaluationList evals = 
				new Assignments(Globals.CurrentIdentity).GetAutoEvals(asstID);

			lstTests.DataSource = evals;
			lstTests.DataTextField = Evaluation.NAME_FIELD;
			lstTests.DataValueField = Evaluation.ID_FIELD;
			lstTests.DataBind();
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private void cmdSubmit_Click(object sender, EventArgs e) {
		
			ArrayList prins = ucSectionExpl.Principals;
			ArrayList tests = GetTests();
			string warnings="";
			int asstID = Convert.ToInt32(HttpContext.Current.Request.Params["AsstID"]);
			AutoJobs jobs = new AutoJobs(Globals.CurrentIdentity);
			Principals aprins = new Principals(Globals.CurrentIdentity);
			foreach (int prin in prins) {
				foreach (int evalid in tests) {
					try {
					
						Components.Submission sub = aprins.GetLatestSubmission(prin, asstID);
						if (sub == null) {
							warnings += aprins.GetInfo(prin).Name + " ";
							break;
						}
						else
							jobs.Create(txtName.Text, evalid, sub.ID);

					} catch (DataAccessException er) {
						PageError(er.Message);
						return;
					}
				}
			}
		
			PageError("Job: " + txtName.Text + " created successfully. Refer to the " +
					  "job status page to monitor its progress through the testing centers. Users/Groups: " +
					  warnings + " do not have any submissions and tests will not be run on them.");
		}

		private ArrayList GetTests() {

			ArrayList tests = new ArrayList();
			foreach (ListItem item in lstTests.Items) 
				if (item.Selected)
					tests.Add(Convert.ToInt32(item.Value));
	
			return tests;
		}

		private void lstTests_SelectedIndexChanged(object sender, System.EventArgs e) {
			
			ArrayList evals = GetTests();
			Evaluations aevals = new Evaluations(Globals.CurrentIdentity);

			lstOrder.Items.Clear();
			foreach (int evalID in evals) {
				Evaluation eval = aevals.GetInfo(evalID);
				Evaluations.DependencyGraph dg = 
					new Evaluations.DependencyGraph(eval, Globals.CurrentIdentity);
				
				Evaluation.EvaluationList order = dg.GetBuildOrder();
				foreach (Evaluation oeval in order) 
					lstOrder.Items.Add(oeval.Name);
			
				lstOrder.Items.Add(eval.Name);
			}
		}
	}
}
