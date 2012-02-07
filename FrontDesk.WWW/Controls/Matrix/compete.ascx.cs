using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;

namespace FrontDesk.Controls.Matrix {
	
	/// <summary>
	///	Competition Matrix view
	/// </summary>
	public class CompetitionView : Pagelet, IMatrixControl {

		protected System.Web.UI.WebControls.DropDownList ddlCompete;
		protected System.Web.UI.WebControls.DataGrid dgCompete;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMain;
		protected System.Web.UI.WebControls.Label lblNone;

		protected Hashtable m_subs;

		private void Page_Load(object sender, System.EventArgs e) {
			
		}

		private int GetAsstID() {
			return (int) ViewState["asstID"];
		}

		private void BindComps() {
			//Bind list
			Evaluation.EvaluationList comps = new Assignments(Globals.CurrentIdentity).GetCompetitions(GetAsstID());
			ddlCompete.Items.Clear();
			foreach (Evaluation comp in comps)
				ddlCompete.Items.Add(new ListItem(comp.Name, comp.ID.ToString()));

			divMain.Visible = !(lblNone.Visible = (ddlCompete.Items.Count == 0));
		}

		private void BindData() {

			if (ddlCompete.Items.Count == 0) return;

			m_subs = new Hashtable();
			//Get results
			Result.ResultList ress = 
				new Evaluations(Globals.CurrentIdentity).GetCompetitionResults(Convert.ToInt32(ddlCompete.SelectedValue), 
																			   out m_subs);	
			dgCompete.DataSource = ress;
			dgCompete.DataBind();
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
			this.ddlCompete.SelectedIndexChanged += new System.EventHandler(this.ddlCompete_SelectedIndexChanged);
			this.dgCompete.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCompete_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlCompete_SelectedIndexChanged(object sender, System.EventArgs e) {
			BindData();
		}

		public event RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {
			ViewState["asstID"] = ap.ID;

			BindComps();
			BindData();
		}

		private void dgCompete_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblIndex, lblSub;
			if (null != (lblIndex = (Label) e.Item.FindControl("lblIndex"))) {
				lblSub = (Label) e.Item.FindControl("lblSub");

				lblIndex.Text = (e.Item.ItemIndex+1).ToString();
				lblSub.Text = ((Components.Submission)m_subs[((AutoResult)e.Item.DataItem).SubmissionID]).Name;
			}
		}

	}
}
