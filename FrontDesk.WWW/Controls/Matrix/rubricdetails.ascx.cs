using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.IO;
using System.Collections;

using FrontDesk.Pages;
using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Tools;
using FrontDesk.Controls;

/* Events
			dgCanned.EditCommand += new DataGridCommandEventHandler(dgCanned_EditCommand);
			dgCanned.UpdateCommand += new DataGridCommandEventHandler(dgCanned_UpdateCommand);
			dgCanned.CancelCommand += new DataGridCommandEventHandler(dgCanned_CancelCommand);
			dgCanned.ItemDataBound += new DataGridItemEventHandler(dgCanned_ItemDataBound);
			dgCanned.DeleteCommand += new DataGridCommandEventHandler(dgCanned_DeleteCommand);
			lnkCanCreate.Click += new EventHandler(lnkCanCreate_Click);
			cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
			cmdJUnitUpload.Click += new System.EventHandler(this.cmdJUnitUpload_Click);
			this.cmdCSUpload.Click += new System.EventHandler(this.cmdCSUpload_Click);
			this.cmdJUnitUpdate.Click += new System.EventHandler(this.cmdJUnitUpdate_Click);
			*/

namespace FrontDesk.Controls.Matrix {
	
	/// <summary>
	///	Rubric details matrix control
	/// </summary>
	public class RubricDetailsView : Pagelet, IMatrixControl {
		protected System.Web.UI.WebControls.TextBox txtDescription;
		protected System.Web.UI.WebControls.Button cmdSave;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtPoints;
		protected System.Web.UI.WebControls.Label lblError;
		protected Microsoft.Web.UI.WebControls.MultiPage mpViews;
		protected System.Web.UI.WebControls.LinkButton lnkCanCreate;
		protected System.Web.UI.WebControls.DataGrid dgCanned;
		protected System.Web.UI.WebControls.Label lblCanError;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Div1;
		protected System.Web.UI.WebControls.CheckBox chkPretest;
		protected System.Web.UI.WebControls.CheckBox chkBuild;
		protected System.Web.UI.WebControls.TextBox txtTimeLimit;
		protected System.Web.UI.WebControls.DropDownList ddlRunTool;
		protected System.Web.UI.WebControls.TextBox txtRunArguments;
		protected System.Web.UI.WebControls.Button cmdUpdate;
		protected System.Web.UI.WebControls.CheckBox chkCompete;
		protected System.Web.UI.WebControls.Label lblAutoError;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.LinkButton lnkFiles;
		protected System.Web.UI.WebControls.Label lblEvalID;
		protected System.Web.UI.HtmlControls.HtmlGenericControl Div2;
		protected System.Web.UI.HtmlControls.HtmlInputFile fuTester;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divRubDetails;
		protected EvaluationDepsControl ucEvalDeps, ucJUnitDeps, ucCSDeps;

		public const int SUBJVIEW = 0, AUTOVIEW = 1, JUNITVIEW = 2, 
			CHECKSTYLEVIEW=3, NOVIEW = 4;
		protected System.Web.UI.WebControls.LinkButton lnkJUnitView;
		protected System.Web.UI.WebControls.Image Image2;
		protected System.Web.UI.WebControls.Label lblJUnitTime;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divJUnitTest;
		protected System.Web.UI.WebControls.Button cmdJUnitUpload;
		protected System.Web.UI.HtmlControls.HtmlInputFile fiJUnit;
		protected System.Web.UI.WebControls.DropDownList ddlVersioning;
		protected System.Web.UI.WebControls.TextBox txtVersion;
		protected System.Web.UI.WebControls.Label lblJUnitError;
		protected System.Web.UI.WebControls.Label lblJUnitCount;
		protected System.Web.UI.WebControls.Button cmdCSUpload;
		protected System.Web.UI.WebControls.LinkButton lnkCSEval;
		protected System.Web.UI.WebControls.Image Image3;
		protected System.Web.UI.HtmlControls.HtmlInputFile fuCS;
		protected System.Web.UI.WebControls.Label lblCSError;
		protected System.Web.UI.WebControls.CheckBox chkJUnitPreTest;
		protected System.Web.UI.WebControls.Button cmdJUnitUpdate;
		protected System.Web.UI.WebControls.CheckBox chkAllowNeg;
		protected System.Web.UI.WebControls.CheckBox chkJUnitCompete;
		protected System.Web.UI.WebControls.Label Label1;

		public event RefreshEventHandler Refresh; 

		private void Page_Load(object sender, System.EventArgs e) {
			lblError.Visible = lblCanError.Visible = lblAutoError.Visible = false;
			lblJUnitError.Visible = lblCSError.Visible = false;
			ucJUnitDeps.ShowUpdate = true;
			ucCSDeps.ShowUpdate = true;
		}

		private void PageCanError(string msg) {
			lblCanError.Text = msg;
			lblCanError.Visible = true;
		}

		private void PageJUnitError(string msg) {
			lblJUnitError.Text = msg;
			lblJUnitError.Visible = true;
		}

		private void PageAutoError(string msg) {
			lblAutoError.Text = msg;
			lblAutoError.Visible = true;
		}

		private void PageCSError(string msg) {
			lblCSError.Text = msg;
			lblCSError.Visible = true;
		}

		private int GetCurrentID() {
			return (int) ViewState["rubid"];
		}	

		private void BindCSView(AutoEvaluation eval) {
			ucCSDeps.BindData(eval);
			ucCSDeps.ShowUpdate = true;
			ucCSDeps.EvalID = eval.ID;

			AddBrowserAttr(lnkCSEval, eval);
			lblEvalID.Text = eval.ID.ToString();
		}

		private void BindJUnitView(AutoEvaluation eval) {
			
			string xmltests=null;
			double points=0;
			int time=0, count=0;
			
			//Attempt to load suite info
			try {
				xmltests = new JUnitTool().ReDiscover(eval, out points, out time, out count);
			} catch (JUnitToolException) {
				points = 0; time = 0; count = 0; xmltests = null;
			} catch (CustomException er) {
				PageJUnitError(er.Message);
			}
	
			//Display test info
			XmlWizard xmlwiz = new XmlWizard();
			lblJUnitTime.Text = "Total Time Limit: <b>" + time + "s</b>";	
			lblJUnitCount.Text = "Total Test Count: <b>" + count + "</b>";
			if (xmltests != null)
				xmlwiz.DisplayDivXslt(xmltests, 
					Path.Combine(Globals.WWWDirectory, "Xml/testhtml.xslt"), divJUnitTest);
			else
				divJUnitTest.InnerHtml = "<br><i>No JUnit Test Suite Present</i>";

			ucJUnitDeps.BindData(eval);
			ucJUnitDeps.ShowUpdate = true;
			ucJUnitDeps.EvalID = eval.ID;

			AddBrowserAttr(lnkJUnitView, eval);
			lblEvalID.Text = eval.ID.ToString();
			chkJUnitPreTest.Checked = eval.RunOnSubmit;
			chkJUnitCompete.Checked = eval.Competitive;
		}

		private void AddBrowserAttr(LinkButton link, AutoEvaluation eval) {
			
			Evaluation.EvaluationList evals = 
				(new Assignments(Globals.CurrentIdentity)).GetAutoEvals(eval.AsstID);

			int i;
			string roots="";
			for (i = 0; i < evals.Count; i++) {
				AutoEvaluation aeval = evals[i] as AutoEvaluation;
				if (aeval.ZoneID > 0) 
					roots += aeval.ZoneID+"|";
			}

			link.Attributes.Add("onClick", 
				@"window.open('filebrowser.aspx?Roots=" + roots +
				@"', '"+8+@"', 'width=730, height=630')");
		}

		private void BindAutoView(AutoEvaluation eval) {
			
			//Add file browser link
			AddBrowserAttr(lnkFiles, eval);

			lblEvalID.Text = eval.ID.ToString();
			chkBuild.Checked = eval.IsBuild;
			chkPretest.Checked = eval.RunOnSubmit;
			chkCompete.Checked = eval.Competitive;
			txtVersion.Text = eval.ToolVersion;

			//Set up versioning
			ddlVersioning.Items.Clear();
			ddlVersioning.Items.Add(new ListItem("At Least", ((int)ExternalToolFactory.VersionCompare.ATLEAST).ToString()));
			ddlVersioning.Items.Add(new ListItem("Exact Match", ((int)ExternalToolFactory.VersionCompare.EQUAL).ToString()));
			ddlVersioning.Items.Add(new ListItem("No Versioning", ((int)ExternalToolFactory.VersionCompare.NONE).ToString()));
			ddlVersioning.SelectedIndex = eval.ToolVersioning;

			BindAutoTools(eval.RunTool);
			txtRunArguments.Text = eval.RunToolArgs;
			txtTimeLimit.Text = eval.TimeLimit.ToString();

			ucEvalDeps.BindData(eval);
		}

		private void BindAutoTools(string seltool) {
			ExternalToolFactory extfact = ExternalToolFactory.GetInstance();
		
			ddlRunTool.Items.Clear();
			foreach (string key in extfact.ListKeys()) {
				ListItem item = new ListItem(key);
				item.Selected = (seltool == key);
				ddlRunTool.Items.Add(item);
			}
		}

		private void BindCannedResponses(int rubID) {
			
			CannedResponse.CannedResponseList cans = 
				new Rubrics(Globals.CurrentIdentity).GetCannedResponses(rubID);

			dgCanned.DataSource = cans;
			dgCanned.DataBind();
		}

		private void PageError(string msg) {
			lblError.Text = msg;
			lblError.Visible = true;
		}

		private bool IsHeading(Rubric rub) {
			return (rub.Points < 0 || rub.ParentID == -1);
		}

		private void lnkCanCreate_Click(object sender, System.EventArgs e) {
			int rubID = GetCurrentID();

			try {
				new Rubrics(Globals.CurrentIdentity).AddCannedResponse(rubID, "New Suggested Comment",
					0, Rubric.WARNING);
			} catch (DataAccessException er) {
				PageCanError(er.Message);
			}

			BindCannedResponses(rubID);
		}

		private void dgCanned_EditCommand(object source, DataGridCommandEventArgs e) {
			dgCanned.EditItemIndex = e.Item.ItemIndex;
			BindCannedResponses(GetCurrentID());
		}

		private void dgCanned_DeleteCommand(object source, DataGridCommandEventArgs e) {
			int canID = Convert.ToInt32(dgCanned.DataKeys[e.Item.ItemIndex]);

			try {
				new Rubrics(Globals.CurrentIdentity).RemoveCannedResponse(canID);
			} catch (DataAccessException er) {
				PageCanError(er.Message);
			}

			BindCannedResponses(GetCurrentID());
		}

		private void dgCanned_CancelCommand(object source, DataGridCommandEventArgs e) {
			dgCanned.EditItemIndex = -1;
			BindCannedResponses(GetCurrentID());
		}

		private void dgCanned_UpdateCommand(object source, DataGridCommandEventArgs e) {
			
			int canID = Convert.ToInt32(dgCanned.DataKeys[e.Item.ItemIndex]);
			CannedResponse can = new CannedResponse();

			can.ID = canID; can.RubricID = GetCurrentID();
			can.Comment = (e.Item.FindControl("txtGridComment") as TextBox).Text;
			string strpoints = (e.Item.FindControl("txtGridPoints") as TextBox).Text;
			double points;
			try {
				points = Convert.ToDouble(strpoints);
			} catch (Exception) { points=0; }
			can.Points = points;
			can.Type = Convert.ToInt32((e.Item.FindControl("ddlTypes") as DropDownList).SelectedValue);

			try {
				new Rubrics(Globals.CurrentIdentity).UpdateCannedResponse(can);
			} catch (DataAccessException er) {
				PageCanError(er.Message);
			}

			dgCanned.EditItemIndex = -1;
			BindCannedResponses(can.RubricID);
		}

		private void dgCanned_ItemDataBound(object sender, DataGridItemEventArgs e) {
		
			System.Web.UI.WebControls.Image imgType;
			DropDownList ddlTypes;
			Label lblType;
			if (null != (imgType = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgType"))) {
				lblType = (Label) e.Item.FindControl("lblType");
				CannedResponse can = e.Item.DataItem as CannedResponse;
				switch (can.Type) {
					case Rubric.ERROR:
						imgType.ImageUrl = "../../attributes/error.gif";
						lblType.Text = "Error";
						break;
					case Rubric.WARNING:
						imgType.ImageUrl = "../../attributes/warning.gif";
						lblType.Text = "Warning";
						break;
					case Rubric.GOOD:
						imgType.ImageUrl = "../../attributes/good.gif";
						lblType.Text = "Good";
						break;
				};
			}
			if (null != (ddlTypes = (DropDownList) e.Item.FindControl("ddlTypes"))) {
				CannedResponse can = e.Item.DataItem as CannedResponse;
				ListItem gooditem = new ListItem("Good", Rubric.GOOD.ToString());
				ListItem warnitem = new ListItem("Warning", Rubric.WARNING.ToString());
				ListItem eritem = new ListItem("Error", Rubric.ERROR.ToString());

				switch (can.Type) {
					case Rubric.ERROR:
						eritem.Selected = true;
						break;
					case Rubric.WARNING:
						warnitem.Selected = true;
						break;
					case Rubric.GOOD:
						gooditem.Selected = true;
						break;
				};

				ddlTypes.Items.Add(gooditem);
				ddlTypes.Items.Add(warnitem);
				ddlTypes.Items.Add(eritem);
			}
		}

		private void UpdateRightSide() {
		
			int rubID = GetCurrentID();
			Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(rubID);

			txtDescription.Text = rub.Description;
			txtName.Text = rub.Name;
			txtPoints.Enabled = true;
			chkAllowNeg.Checked = rub.AllowNegativePoints;
			if (!IsHeading(rub)) {
				txtPoints.Visible = true;
				txtPoints.Text = rub.Points.ToString();
				if (rub.EvalID >= 0) {
					Evaluation eval = new Evaluations(Globals.CurrentIdentity).GetInfo(rub.EvalID);
					if (eval.Manager == Evaluation.JUNIT_MANAGER) {
						BindJUnitView((AutoEvaluation)eval);
					//	txtPoints.Enabled = false;
						mpViews.SelectedIndex = JUNITVIEW;
					} else if (eval.Manager == Evaluation.CHECKSTYLE_MANAGER) {
						BindCSView((AutoEvaluation)eval);
						mpViews.SelectedIndex = CHECKSTYLEVIEW;
					} else {
						BindAutoView((AutoEvaluation)eval);
						mpViews.SelectedIndex = AUTOVIEW;
					}
				}
				else {
					BindCannedResponses(rubID);
					mpViews.SelectedIndex = SUBJVIEW;
				}
			}
			else {
				txtPoints.Visible = false;
				mpViews.SelectedIndex = NOVIEW;
			}
		}

		private void cmdSave_Click(object sender, System.EventArgs e) {	
	
			Rubric rub = new Rubrics(Globals.CurrentIdentity).GetInfo(GetCurrentID());

			rub.Name = txtName.Text; rub.Description = txtDescription.Text;
			rub.AllowNegativePoints = chkAllowNeg.Checked;
			if (!txtPoints.Visible)
				rub.Points = -1;
			else 
				rub.Points = Convert.ToDouble(txtPoints.Text);
			
			try {
				new Rubrics(Globals.CurrentIdentity).Update(rub);
			} catch (DataAccessException er) {
				PageError(er.Message);
			}

			Refresh(this, new RefreshEventArgs("", true, true));
			UpdateRightSide();
		}



		private IExternalSource CreateSource(HttpPostedFile tarchive) {

			ArchiveToolFactory afact = ArchiveToolFactory.GetInstance();
			IExternalSource ztool = afact.CreateArchiveTool(".zip") as IExternalSource;

			ztool.CreateSource(tarchive.InputStream);

			return ztool;
		}

		private void cmdUpdate_Click(object source, EventArgs e) {
			
			AutoEvaluation eval = new AutoEvaluation();

			eval.ID = Convert.ToInt32(lblEvalID.Text);
			eval = (AutoEvaluation) new Evaluations(Globals.CurrentIdentity).GetInfo(eval.ID);
			eval.TimeLimit = Convert.ToInt32(txtTimeLimit.Text);
			eval.RunToolArgs = txtRunArguments.Text;
			eval.RunTool = ddlRunTool.SelectedItem.Text;

			eval.IsBuild = chkBuild.Checked;
			eval.RunOnSubmit = chkPretest.Checked;
			eval.Competitive = chkCompete.Checked;
			eval.ToolVersion = txtVersion.Text;
			eval.ToolVersioning = Convert.ToInt32(ddlVersioning.SelectedItem.Value);

			//Create deps if possible
			try {
				ucEvalDeps.Update(eval.ID, eval.AsstID);
			} catch (CustomException er) {
				PageJUnitError(er.Message);
				return;
			}

			//Create xtern source
			IExternalSource esrc;		
			if (fuTester.PostedFile.ContentLength == 0)
				esrc = new EmptySource();
			else
				esrc = CreateSource(fuTester.PostedFile);

			//Upload files
			try {
				new Evaluations(Globals.CurrentIdentity).UpdateAuto(eval, esrc);
			} catch (CustomException er) {
				PageAutoError(er.Message);
			}

			BindAutoView(eval);
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
			dgCanned.EditCommand += new DataGridCommandEventHandler(dgCanned_EditCommand);
			dgCanned.UpdateCommand += new DataGridCommandEventHandler(dgCanned_UpdateCommand);
			dgCanned.CancelCommand += new DataGridCommandEventHandler(dgCanned_CancelCommand);
			dgCanned.ItemDataBound += new DataGridItemEventHandler(dgCanned_ItemDataBound);
			dgCanned.DeleteCommand += new DataGridCommandEventHandler(dgCanned_DeleteCommand);
			lnkCanCreate.Click += new EventHandler(lnkCanCreate_Click);
			cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
			cmdJUnitUpload.Click += new System.EventHandler(this.cmdJUnitUpload_Click);
			this.cmdCSUpload.Click += new System.EventHandler(this.cmdCSUpload_Click);
			this.cmdJUnitUpdate.Click += new System.EventHandler(this.cmdJUnitUpdate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void Activate(ActivateParams ap) {
			ViewState["rubid"] = ap.ID;
			UpdateRightSide();
		}

		private void cmdJUnitUpload_Click(object sender, System.EventArgs e) {
			
			//Import tester data
			AutoEvaluation eval;
			Evaluations evalda = new Evaluations(Globals.CurrentIdentity);
			Rubrics rubda = new Rubrics(Globals.CurrentIdentity);
			IExternalSource esrc;	
	
			if (fiJUnit.PostedFile.ContentLength == 0) {
				PageJUnitError("You must specify a tester suite to upload");
				return;
			}
			else {
				esrc = CreateSource(fiJUnit.PostedFile);
				eval = (AutoEvaluation) new Evaluations(Globals.CurrentIdentity).GetInfo(
					Convert.ToInt32(lblEvalID.Text));
			}

			//Load files
			try {
				evalda.UpdateAuto(eval, esrc);
			} catch (CustomException er) {
				PageJUnitError(er.Message);
				return;
			}

			//Discover JUnit test
			double points=0;
			int time=0, count=0;
			try {
				new JUnitTool().Discover(eval, out points, out time, out count);
			} catch (CustomException er) {
				PageJUnitError(er.Message);
			}

			//Update points and time
			Rubric rub = rubda.GetInfo(GetCurrentID());
			eval.TimeLimit = time;
			rub.Points = points;
			try {
				evalda.UpdateAuto(eval, new EmptySource());
				rubda.Update(rub);
				PageJUnitError("Upload successful!");
			} catch (CustomException er) {
				PageJUnitError(er.Message);
			}
		
			UpdateRightSide();
		}

		private void cmdCSUpload_Click(object sender, System.EventArgs e) {
		
			//Import tester data
			AutoEvaluation eval;
			Evaluations evalda = new Evaluations(Globals.CurrentIdentity);
			IExternalSource esrc;	
	
			//Get all necessary info
			if (fuCS.PostedFile.ContentLength == 0) {
				PageCSError("You must specify a tester suite to upload");
				return;
			}
			else {
				esrc = CreateSource(fuCS.PostedFile);
				eval = (AutoEvaluation) new Evaluations(Globals.CurrentIdentity).GetInfo(
					Convert.ToInt32(lblEvalID.Text));
			}

			//Load files
			try {
				evalda.UpdateAuto(eval, esrc);
				new CheckStyleTool().CopySupportFiles(eval);
				PageCSError("Upload successful!");
			} catch (CustomException er) {
				PageCSError(er.Message);
			}
			
			UpdateRightSide();
		}

		private void cmdJUnitUpdate_Click(object sender, System.EventArgs e) {
			int evalID = Convert.ToInt32(lblEvalID.Text);
			Evaluations evalda = new Evaluations(Globals.CurrentIdentity);
			AutoEvaluation eval = (AutoEvaluation) evalda.GetInfo(evalID);
			try {
				eval.RunOnSubmit = chkJUnitPreTest.Checked;
				eval.Competitive = chkJUnitCompete.Checked;
				evalda.UpdateAuto(eval, new EmptySource());
			} catch (CustomException er) {
				PageJUnitError(er.Message);
			}

			UpdateRightSide();
		}
	}
}
