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
using System.Web.Security;
using Microsoft.Web.UI.WebControls;

using FrontDesk.Common;
using FrontDesk.Controls;
using FrontDesk.Controls.Matrix;

namespace FrontDesk.Pages {
	
	/// <summary>
	/// Master page controller
	/// </summary>
	public class MasterPage : Page {

		private Control ucMatrix;
		private LongTaskControl ucLongTask;
		private Guid m_test;

		public MasterPage() { m_test = Guid.NewGuid(); }

		private bool m_haslogout=true, m_hastabs=false;

		public bool HasLogout {
			get { return m_haslogout; }
			set { m_haslogout = true; }
		}

		public bool HasTabs {
			get { return m_hastabs; }
			set { m_hastabs = value; }
		}

		public LongTaskControl GetLongTaskControl() {
			return ucLongTask;
		}

		public void ExecuteLongTask(ICommand task) {
			
			ucLongTask.Visible = true;
			ucMatrix.Visible = false;

			//Start the task
			ucLongTask.Start(task);
		}

		private void Page_Load(object sender, System.EventArgs e) {

			HtmlAnchor CourseMain=null;
			LinkButton cmdLogout=null;
			Label lblID=null;
			TabStrip tsVert=null;

			//Attempt to find the controls
			CourseMain = (HtmlAnchor) FindControl("CourseMain");
			lblID = (Label) FindControl("lblID");

			if (HasLogout)
				cmdLogout = (LinkButton) FindControl("cmdLogout");
			if (HasTabs)
				tsVert = (TabStrip) FindControl("tsVert");	

			//Set up commonality
			int courseID = Convert.ToInt32(Request.Params["CourseID"]);
			if (CourseMain != null)
				CourseMain.HRef = "course.aspx?CourseID=" + courseID;

			if (cmdLogout != null) {
				cmdLogout.CausesValidation = false;
				cmdLogout.Click += new System.EventHandler(cmdLogout_Click);
			}

			if (lblID != null) 
				lblID.Text = Globals.CurrentUserName;

			if (tsVert != null)
				SetupTabStyle(tsVert);

			Pagelet.FixControlDims(Controls[1]);

			ucMatrix = FindControl("ucMatrix");
			ucLongTask = (LongTaskControl) FindControl("ucLongTask");

			if (ucLongTask != null) {	
				ucLongTask.Visible = ucLongTask.IsBusy();	
				ucMatrix.Visible = !ucLongTask.Visible;
			}
		}

		protected void SetupTabStyle(TabStrip tsVert) {

			tsVert.TabDefaultStyle.AppendCssText("font-family: Verdana;" +
												 "font-size: 11pt;" + 
												 "border:solid 1px black;" + 
												 "background:#dddddd;" + 
												 "padding-left:5px;" +
												 "padding-right:5px;");

			tsVert.TabHoverStyle.AppendCssText("color:#4768A3;");
			tsVert.TabSelectedStyle.AppendCssText("font-family: Verdana;" +
												  "font-size: 11pt;" +
												  "border:solid 1px black;" +
												  "border-bottom:none;" +
												  "background:white;" +
												  "padding-left:5px;" +
												  "padding-right:1px;");

			tsVert.SepDefaultStyle.AppendCssText("border-bottom:solid 1px #000000;");
		}

		private void cmdLogout_Click(object sender, EventArgs ev) {
			FormsAuthentication.SignOut();
			Page.Response.Redirect(Globals.DefaultUrl);
		}

		override protected void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);
		}

	}
}
