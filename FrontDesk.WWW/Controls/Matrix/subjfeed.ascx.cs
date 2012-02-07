using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;
using Microsoft.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Text;

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

/* 			this.lnkRefresh.Click += new EventHandler(lnkRefresh_Click);
			this.dgResults.DeleteCommand += new DataGridCommandEventHandler(dgResults_DeleteCommand);
			this.dgResults.CancelCommand += new DataGridCommandEventHandler(dgResults_CancelCommand);
			this.dgResults.UpdateCommand += new DataGridCommandEventHandler(dgResults_UpdateCommand);
			this.dgResults.EditCommand += new DataGridCommandEventHandler(dgResults_EditCommand);
			this.dgResults.ItemDataBound += new DataGridItemEventHandler(dgResults_ItemDataBound);
			this.dgResults.PageIndexChanged += new DataGridPageChangedEventHandler(dgResults_PageIndexChanged);
			this.lnkCreate.Click += new EventHandler(lnkCreate_Click);
			this.lnkComplete.Click += new System.EventHandler(this.lnkComplete_Click);
			this.lnkDefunct.Click += new System.EventHandler(this.lnkDefunct_Click);
			this.lnkRunTest.Click += new EventHandler(lnkRunTest_Click);		
			this.tsUsers.SelectedIndexChange += new System.EventHandler(this.tsUsers_SelectedIndexChange);	
*/

namespace FrontDesk.Controls.Matrix {
	
	/// <summary>
	///	Subjective feedback station
	/// </summary>
	public class SubjFeedbackView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.DataGrid dgResults;
		protected System.Web.UI.WebControls.DropDownList ddlCanned;
		protected System.Web.UI.WebControls.LinkButton lnkCreate;
		protected System.Web.UI.WebControls.LinkButton lnkFiles;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.LinkButton lnkComplete;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblRubID;
		protected System.Web.UI.WebControls.Label lblEvalID;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.WebControls.Image imgComplete;
		protected System.Web.UI.WebControls.LinkButton lnkRunTest;
		protected System.Web.UI.WebControls.Label lblRunError;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divAuto;
		protected System.Web.UI.WebControls.LinkButton lnkRefresh;
		protected System.Web.UI.HtmlControls.HtmlImage imgRefresh;
		protected System.Web.UI.WebControls.Label lblSubTime;
		protected System.Web.UI.WebControls.Label lblMarkInst;
		protected Microsoft.Web.UI.WebControls.TabStrip tsUsers;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divGrading;
		protected System.Web.UI.WebControls.DataGrid dgActivity;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divActivity;
		protected System.Web.UI.WebControls.LinkButton lnkDefunct;
		protected System.Web.UI.WebControls.Image imgDefunct;
		protected RubricViewControl ucRubric;

		private void Page_Load(object sender, System.EventArgs e) {
			ucRubric.RubricSelect += new RubricViewSelectEventHandler(ucRubric_RubricSelect);
			lblError.Visible = lblRunError.Visible = false;
		}

		private int GetSubID() {
			return (int) ViewState["subID"];
		}

		private bool IsStudent() {
			return (bool) ViewState["stumode"];
		}

		private void BindActivity() {
			Activity.ActivityList acts = new Submissions(Globals.CurrentIdentity).GetActivity(GetSubID());
			dgActivity.DataSource = acts;
			dgActivity.DataBind();
		}

		private void BindData() {
			Components.Submission sub = 
				new Submissions(Globals.CurrentIdentity).GetInfo(GetSubID());

			//Set up top controls
			if (!IsStudent()) {
				lnkComplete.Visible = true;
				imgComplete.Visible = true;
				lblMarkInst.Visible = true;
				lnkDefunct.Visible = true;
				imgDefunct.Visible = true;
				if (sub.Status == Components.Submission.GRADED) {
					lnkComplete.Text = "Mark as Incomplete";
					imgComplete.ImageUrl = "../../attributes/sub.gif";
				} else {
					lnkComplete.Text = "Mark as Completed";
					imgComplete.ImageUrl = "../../attributes/subgrade.gif";
				}

				if (sub.Status == Components.Submission.DEFUNCT)
					lnkDefunct.Text = "Mark as Non-Defunct";
				else
					lnkDefunct.Text = "Mark as Defunct";

			} else {
				lnkComplete.Visible = false;
				imgComplete.Visible = false;
				lnkDefunct.Visible = false;
				imgDefunct.Visible = false;
				lblMarkInst.Visible = false;
			}

			//Set up sub time label
			string latestr;
			Assignment asst = new Assignments(Globals.CurrentIdentity).GetInfo(sub.AsstID);
			if (sub.Creation <= asst.DueDate)
				latestr = "<font color=\"#4768A3\"><b>ON TIME</b></font>";
			else
				latestr = "<font color=\"#ff0000\"><b>LATE</b></font>";
			lblSubTime.Text = String.Format("<b>Time:</b> {0} <b>Status:</b> {1}", sub.Creation.ToString(),
				latestr);

			//Set up rubric view
			Rubric arub = new Assignments(Globals.CurrentIdentity).GetRubric(sub.AsstID);	
			ucRubric.ExpandSubj = false;
			ucRubric.InitRubric(arub, GetSubID(), "");

			//Set up view files link
			BindFileLink();

			if (!IsStudent())
				mpViews.SelectedIndex = 1;
			else
				mpViews.SelectedIndex = 4;
		}

		private void BindFileLink() {
			
			Components.Submission sub = new Submissions(Globals.CurrentIdentity).GetInfo(GetSubID());
			if (sub == null) return;

			string roots = sub.LocationID.ToString();
			lnkFiles.Attributes.Clear();
			if (!IsStudent()) 
				lnkFiles.Attributes.Add("onClick", 
					@"window.open('filebrowser.aspx?SubID=" + GetSubID()+"&Roots=" + roots +
					@"', '"+8+@"', 'width=730, height=630')");
			else 
				lnkFiles.Attributes.Add("onClick", 
					@"window.open('filebrowser.aspx?Roots=" + roots +
					@"', '"+8+@"', 'width=730, height=630')");
		}

		private void BindAuto(Rubric rub) {
			
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			Result.ResultList ress = rubda.GetResults(rub.ID, GetSubID());

			lblEvalID.Text = rub.EvalID.ToString();
			if (ress.Count == 0)
				if (!IsStudent())
					mpViews.SelectedIndex = 2;
				else
					mpViews.SelectedIndex = 5;
			else {
				AutoResult res = ress[0] as AutoResult;		
					
				XmlWizard xmlwiz = new XmlWizard();
				xmlwiz.DisplayDivXslt(res.XmlResult, Path.Combine(Globals.WWWDirectory, "Xml/reshtml.xslt"), divAuto);
				
				mpViews.SelectedIndex = 3;
			}
		}

		private void BindSubj(Rubric rub) {
	
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			Result.ResultList ress = rubda.GetResults(rub.ID, GetSubID());

			//Update the rubric
			ucRubric.UpdateRubric();

			//Reset to 0 if page index greater than numpages
			if (dgResults.CurrentPageIndex >= dgResults.PageCount)
				dgResults.CurrentPageIndex = 0;
			
			dgResults.DataSource = ress;
			dgResults.DataBind();

			if (!IsStudent()) {

				ddlCanned.Visible = true;
				lnkCreate.Visible = true;
				dgResults.Columns[0].Visible = true;
				dgResults.Columns[6].Visible = true;

				dgResults.Style["TOP"] = "58px";

				CannedResponse.CannedResponseList cans = rubda.GetCannedResponses(rub.ID);
				ddlCanned.Items.Clear();

				foreach (CannedResponse can in cans) {
					string canstr = can.Comment.Substring(0, Math.Min(80, can.Comment.Length));
					if (canstr.Length == 80)
						canstr += " ...";
					ListItem item = new ListItem(canstr, can.ID.ToString());

					ddlCanned.Items.Add(item);
				}
				ddlCanned.Items.Add(new ListItem("Custom", "-1"));
			} else {
				ddlCanned.Visible = false;
				lnkCreate.Visible = false;
				dgResults.Columns[0].Visible = false;
				dgResults.Columns[6].Visible = false;

				lnkRefresh.Style["TOP"] = "0px";
				imgRefresh.Style["TOP"] = "0px";
				dgResults.Style["TOP"] = "20px";
			}

			lblRubID.Text = rub.ID.ToString();
			mpViews.SelectedIndex = 0;
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
			this.lnkRefresh.Click += new EventHandler(lnkRefresh_Click);
			this.dgResults.DeleteCommand += new DataGridCommandEventHandler(dgResults_DeleteCommand);
			this.dgResults.CancelCommand += new DataGridCommandEventHandler(dgResults_CancelCommand);
			this.dgResults.UpdateCommand += new DataGridCommandEventHandler(dgResults_UpdateCommand);
			this.dgResults.EditCommand += new DataGridCommandEventHandler(dgResults_EditCommand);
			this.dgResults.ItemDataBound += new DataGridItemEventHandler(dgResults_ItemDataBound);
			this.dgResults.PageIndexChanged += new DataGridPageChangedEventHandler(dgResults_PageIndexChanged);
			this.lnkCreate.Click += new EventHandler(lnkCreate_Click);
			this.lnkComplete.Click += new System.EventHandler(this.lnkComplete_Click);
			this.lnkDefunct.Click += new System.EventHandler(this.lnkDefunct_Click);
			this.lnkRunTest.Click += new EventHandler(lnkRunTest_Click);		
			this.tsUsers.SelectedIndexChange += new System.EventHandler(this.tsUsers_SelectedIndexChange);	
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public event FrontDesk.Controls.Matrix.RefreshEventHandler Refresh;

		public void Activate(ActivateParams ap) {

			Components.Submission sub;

			sub = new Submissions(Globals.CurrentIdentity).GetInfo(ap.ID);

			ViewState["subID"] = sub.ID;	
			ViewState["stumode"] = ap.StudentMode;
			tsUsers.SelectedIndex = 0;
			divGrading.Visible = true;
			divActivity.Visible = false;
			BindData();
		}

		private void ucRubric_RubricSelect(object sender, RubricViewSelectEventArgs e) {
		
			Rubric rub = e.SelectedRubric;	
			if (rub.EvalID >= 0) {
				Evaluation eval = new Evaluations(Globals.CurrentIdentity).GetInfo(rub.EvalID);
				if (eval.ResultType == Result.AUTO_TYPE)
					BindAuto(rub);
				else if (eval.ResultType == Result.SUBJ_TYPE)
					BindSubj(rub);
			}
			else if (!new Rubrics(Globals.CurrentIdentity).IsHeading(rub))
				BindSubj(rub);
			else {
				if (!IsStudent())
					mpViews.SelectedIndex = 4;
				else
					mpViews.SelectedIndex = 1;
			}
		}

		private void dgResults_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblType;
			System.Web.UI.WebControls.Image imgType;
			LinkButton lnkFile;
			DropDownList ddlTypes;

			SubjResult res = e.Item.DataItem as SubjResult;
			if (null != (lblType = (Label) e.Item.FindControl("lblType"))) {
				imgType = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgType");

				switch (res.SubjType) {
					case Rubric.GOOD:
						imgType.ImageUrl = "../../attributes/good.gif";
						lblType.Text = "Good";
						break;
					case Rubric.WARNING:
						imgType.ImageUrl = "../../attributes/warning.gif";
						lblType.Text = "Warning";
						break;
					case Rubric.ERROR:
						imgType.ImageUrl = "../../attributes/error.gif";
						lblType.Text = "Error";
						break;
				}
				if (res.FileID >= 0) {
					CFile file = new FileSystem(Globals.CurrentIdentity).GetFile(res.FileID);
					lnkFile = (LinkButton) e.Item.FindControl("lnkFile");
					lnkFile.Attributes.Add("onClick", 
						@"window.open('Controls/Filesys/viewfile.aspx?FileID=" + file.ID + 
						@"', '"+file.ID+@"', 'width=770, height=580')");
					lnkFile.Text = file.Name + ":" + res.Line;
				}
			}
			if (null != (ddlTypes = (DropDownList) e.Item.FindControl("ddlTypes"))) {
				ddlTypes.Items.Add(new ListItem("Error", Rubric.ERROR.ToString()));
				ddlTypes.Items.Add(new ListItem("Warning", Rubric.WARNING.ToString()));
				ddlTypes.Items.Add(new ListItem("Good", Rubric.GOOD.ToString()));

				switch (res.SubjType) {
					case Rubric.ERROR:
						ddlTypes.SelectedIndex = 0; break;
					case Rubric.WARNING:
						ddlTypes.SelectedIndex = 1; break;
					case Rubric.GOOD:
						ddlTypes.SelectedIndex = 2; break;
				}
			}
		}

		private void PageRunError(string msg) {
			lblRunError.Text = msg;
			lblRunError.Visible = true;
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private CannedResponse GetCannedResponse() {
			int canID = Convert.ToInt32(ddlCanned.SelectedItem.Value);

			if (canID < 0) {
				CannedResponse can = new CannedResponse();
				can.Comment = "Custom Comment";
				can.Points = 0.0;
				can.Type = Rubric.WARNING;
				return can;
			} else
				return new Rubrics(Globals.CurrentIdentity).GetCannedInfo(canID);
		}

		private void lnkCreate_Click(object sender, System.EventArgs e) {
			
			int rubID = Convert.ToInt32(lblRubID.Text);
			CannedResponse can = GetCannedResponse();
			try {
				new Results(Globals.CurrentIdentity).CreateSubj(GetSubID(), rubID, can.Comment, 
					-1, -1, can.Points, new ArrayList(), can.Type);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}

			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(rubID));
			ucRubric.UpdateRubric();
		}

		private void dgResults_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {
			dgResults.CurrentPageIndex = e.NewPageIndex;
			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblRubID.Text)));
		}

		private void lnkComplete_Click(object sender, EventArgs e) {
			Submissions subda = new Submissions(Globals.CurrentIdentity);
			Components.Submission sub = subda.GetInfo(GetSubID());

			//Change status
			if (sub.Status == Components.Submission.INPROGRESS ||
				sub.Status == Components.Submission.UNGRADED)
				sub.Status = Components.Submission.GRADED;
			else
				sub.Status = Components.Submission.UNGRADED;

			try {
				subda.Update(sub, new EmptySource());
			} catch (DataAccessException er) {
				PageError(er.Message);
			} catch (FileOperationException er) {
				PageError(er.Message);
			}

			BindData();
			Refresh(this, new RefreshEventArgs("", true, false));
		}

		private void lnkDefunct_Click(object sender, EventArgs e) {
			Submissions subda = new Submissions(Globals.CurrentIdentity);
			Components.Submission sub = subda.GetInfo(GetSubID());

			//Change status
			if (sub.Status != Components.Submission.DEFUNCT)
				sub.Status = Components.Submission.DEFUNCT;
			else
				sub.Status = Components.Submission.UNGRADED;

			try {
				subda.Update(sub, new EmptySource());
			} catch (DataAccessException er) {
				PageError(er.Message);
			} catch (FileOperationException er) {
				PageError(er.Message);
			}

			BindData();
			Refresh(this, new RefreshEventArgs("", true, false));
		}

		private void dgResults_DeleteCommand(object source, DataGridCommandEventArgs e) {

			try {
				new Results(Globals.CurrentIdentity).Delete(
					Convert.ToInt32(dgResults.DataKeys[e.Item.ItemIndex]));
			} catch (DataAccessException er) {
				PageError(er.Message);
			}

			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblRubID.Text)));
			ucRubric.UpdateRubric();
		}

		private void dgResults_EditCommand(object source, DataGridCommandEventArgs e) {
			dgResults.EditItemIndex = e.Item.ItemIndex;
			dgResults.Columns[2].Visible = dgResults.Columns[5].Visible = false;

			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblRubID.Text)));

		}

		private void dgResults_CancelCommand(object source, DataGridCommandEventArgs e) {
			dgResults.EditItemIndex = -1;
			dgResults.Columns[2].Visible = dgResults.Columns[5].Visible = true;

			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblRubID.Text)));
		}

		private void dgResults_UpdateCommand(object source, DataGridCommandEventArgs e) {
			
			TextBox txtGridComment, txtPoints;
			DropDownList ddlTypes;

			if (null != (txtGridComment = (TextBox) e.Item.FindControl("txtGridComment"))) {
				txtPoints = (TextBox) e.Item.FindControl("txtPoints");
				ddlTypes = (DropDownList) e.Item.FindControl("ddlTypes");

				Results resda = new Results(Globals.CurrentIdentity);
				SubjResult res = new SubjResult();
				res.ID = Convert.ToInt32(dgResults.DataKeys[e.Item.ItemIndex]);
				res = (SubjResult) resda.GetInfo(res.ID);
				res.Points = Convert.ToDouble(txtPoints.Text);
				res.Comment = txtGridComment.Text;
				res.SubjType = Convert.ToInt32(ddlTypes.SelectedIndex);
				
				try {
					resda.UpdateSubj(res);
				} catch (DataAccessException er) {
					PageError(er.Message);
				}
			}

			dgResults.Columns[2].Visible = dgResults.Columns[5].Visible = true;
			dgResults.EditItemIndex = -1;
			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblRubID.Text)));
			ucRubric.UpdateRubric();
		}

		private void lnkRunTest_Click(object sender, EventArgs e) {

			AutoJobs ajobda = new AutoJobs(Globals.CurrentIdentity);
			
			Components.Submission sub = 
				new Submissions(Globals.CurrentIdentity).GetInfo(GetSubID());

			Evaluation eval = 
				new Evaluations(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblEvalID.Text));
			string name = new FileSystem(Globals.CurrentIdentity).GetFile(sub.LocationID).Alias + 
				" (" + eval.Name + ")";
			try {
				AutoJob job = 
					ajobda.Create(name, sub.AsstID);
				ajobda.CreateTest(job.ID, sub.ID, eval.ID, false);
			} catch (DataAccessException er) {
				PageRunError(er.Message);
				return;
			}

			PageRunError("Automatic job created successfully!");
		}

		private void lnkRefresh_Click(object sender, EventArgs e) {
			BindSubj(new Rubrics(Globals.CurrentIdentity).GetInfo(Convert.ToInt32(lblRubID.Text)));
		}

		private void tsUsers_SelectedIndexChange(object sender, EventArgs e) {
			switch (tsUsers.SelectedIndex) {
			case 0:
				divGrading.Visible = true;
				divActivity.Visible = false;
				BindData();
				break;
			case 1:
				divGrading.Visible = false;
				divActivity.Visible = true;
				BindActivity();
				break;
			}
		}
	}
}
