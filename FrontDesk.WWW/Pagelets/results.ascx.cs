using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Text;

using FrontDesk.Common;
using FrontDesk.Components;
using FrontDesk.Components.Evaluation;
using FrontDesk.Components.Filesys;
using FrontDesk.Data.Access;
using FrontDesk.Data.Filesys;
using FrontDesk.Controls.Matrix;

namespace FrontDesk.Pages.Pagelets {
	
	/// <summary>
	///	Student result summary pagelet
	/// </summary>
	public class StudentResultsPagelet : UserControl {
		protected System.Web.UI.HtmlControls.HtmlGenericControl divSubj;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divAuto;
		protected System.Web.UI.WebControls.DataGrid dgSubj;
		protected System.Web.UI.WebControls.DropDownList ddlSubs;
		protected System.Web.UI.WebControls.Label lblNoSubs;
		protected System.Web.UI.WebControls.Label lblSubs; 

		private void Page_Load(object sender, System.EventArgs e) {
			int asstID = Convert.ToInt32(Request.Params["AsstID"]);

			if (!IsPostBack) {
				divSubj.Visible = divAuto.Visible = false;
				BindSubs();
			}
			if (ddlSubs.Items.Count > 0) {
			//	ucRubric.Visible = true;
				ddlSubs.Visible = true;
				lblSubs.Visible = true;
				lblNoSubs.Visible = false;
				if (!IsPostBack) {
				//	ucRubric.AddRoot(new Assignments(Globals.CurrentIdentity).GetRubric(asstID));
				//	ucRubric.ResultSub = Convert.ToInt32(ddlSubs.Items[0].Value);
				}
			}
			else {
			//	ucRubric.Visible = false;
				ddlSubs.Visible = false;
				lblSubs.Visible = false;
				lblNoSubs.Visible = true;
			}
		}

		private int GetCurrentSub() {
			return Convert.ToInt32(ddlSubs.SelectedItem.Value);
		}

		private void BindSubs() {
			int asstID = Convert.ToInt32(Request.Params["AsstID"]);
			Components.Submission.SubmissionList subs = 
				new Users(Globals.CurrentIdentity).GetAsstSubmissions(Globals.CurrentUserName, asstID);

			ddlSubs.Items.Clear();
			foreach (Components.Submission sub in subs) {
				string name = 
					new Principals(Globals.CurrentIdentity).GetInfo(sub.PrincipalID).Name + 
					" (" + sub.Creation.ToString() + ")";
				ddlSubs.Items.Add(new ListItem(name, sub.ID.ToString()));
			}
		}

		private void BindAuto(Rubric rub) {

			Result.ResultList ress = 
				new Rubrics(Globals.CurrentIdentity).GetResults(rub.ID, GetCurrentSub());
			
			if (ress.Count == 0) { 
				divAuto.InnerHtml = "<i>There are no results for this evaluation item</i>";
				return;
			}

			AutoResult res = ress[0] as AutoResult;		
			MemoryStream xmlstr = new MemoryStream(Encoding.ASCII.GetBytes(res.XmlResult));
				
			XslTransform xslt = new XslTransform();

			XPathDocument xpathdoc = new XPathDocument(xmlstr);
			XPathNavigator nav = xpathdoc.CreateNavigator();
				
			XPathDocument xsldoc = new XPathDocument(
				Path.Combine(Globals.WWWDirectory, "Xml/reshtml.xslt"));
				
			StringBuilder strb = new StringBuilder();
			xslt.Load(xsldoc, null, null);
			xslt.Transform(xpathdoc, null, new XmlTextWriter(new StringWriter(strb)) , (XmlResolver)null);
		
			divAuto.InnerHtml = strb.ToString();
		}

		private void BindSubj(Rubric rub) {
				
			Result.ResultList ress = 
				new Rubrics(Globals.CurrentIdentity).GetResults(rub.ID, GetCurrentSub());

			dgSubj.DataSource = ress;
			dgSubj.DataBind();
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
			this.ddlSubs.SelectedIndexChanged += new System.EventHandler(this.ddlSubs_SelectedIndexChanged);
			this.dgSubj.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgSubj_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ucRubric_ItemSelect(object sender, EventArgs args) {
		/*	if (args.ItemType == CourseMatrixControl.AUTOMATIC) {
				divSubj.Visible = false;
				divAuto.Visible = true;
				BindAuto(args.Rubric);
			}
			else if (args.ItemType == CourseMatrixControl.CANNED) {
				divSubj.Visible = true;
				divAuto.Visible = false;
				BindSubj(args.Rubric);
			}*/
		}

		private void dgSubj_ItemDataBound(object sender, DataGridItemEventArgs e) {
			Label lblPoints, lblType;
			System.Web.UI.WebControls.Image imgType;
			LinkButton lnkFile;

			if (null != (lblPoints = (Label) e.Item.FindControl("lblPoints"))) {
				lblType = (Label) e.Item.FindControl("lblType");
				imgType = (System.Web.UI.WebControls.Image) e.Item.FindControl("imgType");

				SubjResult res = e.Item.DataItem as SubjResult;

				switch (res.SubjType) {
					case Rubric.GOOD:
						imgType.ImageUrl = "../attributes/good.gif";
						lblType.Text = "Good";
						break;
					case Rubric.WARNING:
						imgType.ImageUrl = "../attributes/warning.gif";
						lblType.Text = "Warning";
						break;
					case Rubric.ERROR:
						imgType.ImageUrl = "../attributes/error.gif";
						lblType.Text = "Error";
						break;
				}
				lblPoints.Text = res.Points.ToString();

				if (res.FileID >= 0) {
					CFile file = FileSystem.GetInstance().GetFile(res.FileID);
					lnkFile = (LinkButton) e.Item.FindControl("lnkFile");
					lnkFile.Attributes.Add("onClick", 
						@"window.open('Controls/Filesys/viewfile.aspx?FileID=" + file.ID + 
						@"', '"+file.ID+@"', 'width=770, height=580')");
					lnkFile.Text = file.Name + ":" + res.Line;
				}
			}
		}

		private void ddlSubs_SelectedIndexChanged(object sender, System.EventArgs e) {
		//	ucRubric.ResultSub = Convert.ToInt32(ddlSubs.SelectedItem.Value);
		}
	}
}
