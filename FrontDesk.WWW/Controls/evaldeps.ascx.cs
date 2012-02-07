using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FrontDesk.Common;
using FrontDesk.Pages;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;

namespace FrontDesk.Controls {

	/// <summary>
	///	Evaluation dependency control
	/// </summary>
	public class EvaluationDepsControl : Pagelet {
		protected System.Web.UI.WebControls.LinkButton lnkUpdate;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.CheckBoxList chkDeps;

		private void Page_Load(object sender, System.EventArgs e) {
			lnkUpdate.Visible = ShowUpdate;
			lblError.Visible = false;
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		public ListItemCollection Items {
			get { return chkDeps.Items; }
		}

		public int EvalID {
			get { return (int) ViewState["evalID"]; }
			set { ViewState["evalID"] = value; }
		}

		public bool ShowUpdate {
			get { 
				if (null != ViewState["showupdate"])
					return (bool) ViewState["showupdate"];
				else
					return false; 
			}
			set { ViewState["showupdate"] = value; }
		}

		public void Update(int editEval, int asstID) {

			Assignments assts = new Assignments(Globals.CurrentIdentity);
			Evaluations evals = new Evaluations(Globals.CurrentIdentity);

			Evaluation.EvaluationList allevals = assts.GetAutoEvals(asstID);
			foreach (ListItem item in Items) {
				int iid = Convert.ToInt32(item.Value);
				foreach (Evaluation eval in allevals) {
					if (iid == eval.ID) {
						bool dependence = evals.DependsOn(editEval, iid);
						if (dependence && !item.Selected)
							evals.DeleteDependency(editEval, iid);
						else if (!dependence && item.Selected) {
							if (!evals.CreateDependency(editEval, iid)) {
								item.Selected = false;
								throw new DataAccessException("Cannot create a circular dependency: " + 
									eval.Name);		
							}
						}
					}
				}
			}
		}

		public void BindData(AutoEvaluation eval) {

			int asstID = eval.AsstID;
			
			Assignments assts = new Assignments(Globals.CurrentIdentity);
			Evaluations evals = new Evaluations(Globals.CurrentIdentity);

			Evaluation.EvaluationList allevals = assts.GetAutoEvals(asstID);
			Evaluation.EvaluationList deps = evals.GetDependencies(eval.ID);

			chkDeps.Items.Clear();
			foreach (Evaluation e in allevals) {
				if (e.ID == eval.ID) continue;
				ListItem eitem = new ListItem(e.Name, e.ID.ToString());
				foreach (Evaluation d in deps) {
					if (d.ID == e.ID) {
						eitem.Selected = true;
						break;
					}
				}
				chkDeps.Items.Add(eitem);
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
			this.lnkUpdate.Click += new System.EventHandler(this.lnkUpdate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lnkUpdate_Click(object sender, EventArgs e) {
			Evaluation eval = new Evaluations(Globals.CurrentIdentity).GetInfo(EvalID);
			
			try {
				Update(eval.ID, eval.AsstID);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}
		
			BindData((AutoEvaluation)eval);
		}
	}
}
